using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using System.Net.Http;

namespace Lab6
{
    public partial class WhiteboardServerForm : Form
    {
        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();
        private readonly int maxClients = 5;
        private readonly string alertEmail = "hoanggiangtran239@gmail.com";
        private List<DrawAction> actions = new List<DrawAction>();
        private Thread listenerThread;
        private Bitmap whiteboardBitmap = new Bitmap(800, 600);
        private readonly object bitmapLock = new object();

        public WhiteboardServerForm()
        {
            InitializeComponent();

            using (Graphics g = Graphics.FromImage(whiteboardBitmap))
            {
                g.Clear(Color.White);
            }

            StartServer();
        }

        private void StartServer()
        {
            listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();

            Console.WriteLine("Server started, listening on port 5000...");

            listenerThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var tcpClient = listener.AcceptTcpClient();
                        Console.WriteLine($"New client connected: {tcpClient.Client.RemoteEndPoint}");

                        lock (clients)
                        {
                            clients.Add(tcpClient);
                            Console.WriteLine($"Client added. Total clients: {clients.Count}");
                        }

                        if (clients.Count > maxClients)
                        {
                            SendAlertEmail();
                            Console.WriteLine("Warning: Client limit exceeded!");

                            using (NetworkStream ns = tcpClient.GetStream())
                            {
                                string rejectMsg = JsonSerializer.Serialize(new { type = "error", message = "Server is full." });
                                byte[] rejectData = Encoding.UTF8.GetBytes(rejectMsg);
                                ns.Write(rejectData, 0, rejectData.Length);
                            }

                            tcpClient.Close();
                            continue;
                        }

                        BroadcastClientCount();
                        SendCurrentWhiteboardToClient(tcpClient);

                        Thread clientThread = new Thread(() => HandleClient(tcpClient));
                        clientThread.IsBackground = true;
                        clientThread.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Listener stopped or error: " + ex.Message);
                        break;
                    }
                }
            });
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        private void SendAlertEmail()
        {
            try
            {
                var mail = new MailMessage("hoanggiangtran239@gmail.com", alertEmail);
                mail.Subject = "Whiteboard Server Alert";
                mail.Body = $"Client limit of {maxClients} exceeded at {DateTime.Now}";

                using (var smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential("hoanggiangtran239@gmail.com", "kclu cgih dkrv lqrw");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                Console.WriteLine("Alert email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send alert email: {ex.Message}");
                Invoke(new Action(() =>
                {
                    MessageBox.Show($"Failed to send alert email: {ex.Message}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[8192];

            try
            {
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        Console.WriteLine($"Client disconnected gracefully: {client.Client.RemoteEndPoint}");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received from {client.Client.RemoteEndPoint}: {message}");

                    UpdateWhiteboardState(message);
                    BroadcastMessage(message, client);
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Client {client.Client.RemoteEndPoint} disconnected (IO error): {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client {client.Client.RemoteEndPoint} disconnected (unexpected error): {ex.Message}");
            }
            finally
            {
                lock (clients)
                {
                    if (clients.Remove(client))
                    {
                        Console.WriteLine($"Client removed. Total clients: {clients.Count}");
                        BroadcastClientCount();
                        if (clients.Count == 0)
                        {
                            // Reset server state when all clients disconnect
                            lock (bitmapLock)
                            {
                                actions.Clear();
                                whiteboardBitmap.Dispose();
                                whiteboardBitmap = new Bitmap(800, 600);
                                using (Graphics g = Graphics.FromImage(whiteboardBitmap))
                                {
                                    g.Clear(Color.White);
                                }
                                Console.WriteLine("All clients disconnected. Server state reset.");
                            }
                        }
                    }
                }

                try { client.Close(); } catch { }
            }
        }

        private Image DownloadImageSync(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                    var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    using (var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
                    {
                        Image img = Image.FromStream(stream);
                        if (img.Width == 0 || img.Height == 0)
                        {
                            throw new Exception("Invalid image data");
                        }
                        return img;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download image from {url}: {ex.Message}");
                return null;
            }
        }

        private void UpdateWhiteboardState(string message)
        {
            try
            {
                var json = JsonDocument.Parse(message);
                string type = json.RootElement.GetProperty("type").GetString();

                lock (bitmapLock)
                {
                    using (Graphics g = Graphics.FromImage(whiteboardBitmap))
                    {
                        if (type == "draw")
                        {
                            int x1 = json.RootElement.GetProperty("x1").GetInt32();
                            int y1 = json.RootElement.GetProperty("y1").GetInt32();
                            int x2 = json.RootElement.GetProperty("x2").GetInt32();
                            int y2 = json.RootElement.GetProperty("y2").GetInt32();
                            string color = json.RootElement.GetProperty("color").GetString();
                            int thickness = json.RootElement.GetProperty("thickness").GetInt32();

                            var action = new DrawAction
                            {
                                Type = ActionType.Line,
                                StartPoint = new Point(x1, y1),
                                EndPoint = new Point(x2, y2),
                                PenColor = ColorTranslator.FromHtml(color),
                                PenWidth = thickness
                            };
                            actions.Add(action);
                            action.Draw(g);
                            BroadcastMessage(message);
                        }
                        else if (type == "image")
                        {
                            string url = json.RootElement.GetProperty("url").GetString();
                            int width = json.RootElement.GetProperty("width").GetInt32();
                            int height = json.RootElement.GetProperty("height").GetInt32();

                            Image img = DownloadImageSync(url);
                            if (img != null)
                            {
                                var action = new DrawAction
                                {
                                    Type = ActionType.Image,
                                    ImageUrl = url,
                                    ImageRect = new Rectangle(100, 100, width, height),
                                    Image = (Image)img.Clone()
                                };
                                actions.Add(action);
                                action.Draw(g);
                                BroadcastMessage(message);
                            }
                            else
                            {
                                Console.WriteLine($"Failed to load image from {url}");
                            }
                        }
                        else if (type == "undo")
                        {
                            if (actions.Count > 0)
                            {
                                actions.RemoveAt(actions.Count - 1);
                                whiteboardBitmap.Dispose();
                                whiteboardBitmap = new Bitmap(800, 600);
                                g.Clear(Color.White);
                                foreach (var action in actions)
                                {
                                    action.Draw(g);
                                }
                            }
                            BroadcastMessage(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating whiteboard state: {ex.Message}");
            }
        }

        private void BroadcastMessage(string message, TcpClient excludeClient = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            lock (clients)
            {
                foreach (var client in clients)
                {
                    if (client == excludeClient) continue;

                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);
                        stream.Flush();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send message to {client.Client.RemoteEndPoint}: {ex.Message}");
                    }
                }
            }
        }

        private void BroadcastClientCount()
        {
            var msg = JsonSerializer.Serialize(new { type = "count", count = clients.Count });
            BroadcastMessage(msg);

            Console.WriteLine($"Broadcasting client count: {clients.Count}");

            Invoke(new Action(() =>
            {
                this.Text = $"Whiteboard Server - Clients connected: {clients.Count}";
            }));
        }

        private void SendCurrentWhiteboardToClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();

                // Gửi ảnh bảng vẽ hiện tại
                byte[] imgBytes;
                lock (bitmapLock)
                {
                    using (var ms = new MemoryStream())
                    {
                        whiteboardBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imgBytes = ms.ToArray();
                    }
                }
                string base64 = Convert.ToBase64String(imgBytes);
                var whiteboardMessage = JsonSerializer.Serialize(new { type = "whiteboard_image", imageBase64 = base64 });
                byte[] whiteboardData = Encoding.UTF8.GetBytes(whiteboardMessage);
                stream.Write(whiteboardData, 0, whiteboardData.Length);
                stream.Flush();
                Console.WriteLine("Sent whiteboard image to new client");

                // Gửi danh sách các hành động (actions)
                lock (bitmapLock)
                {
                    foreach (var action in actions)
                    {
                        if (action.Type == ActionType.Line)
                        {
                            var message = JsonSerializer.Serialize(new
                            {
                                type = "draw",
                                x1 = action.StartPoint.X,
                                y1 = action.StartPoint.Y,
                                x2 = action.EndPoint.X,
                                y2 = action.EndPoint.Y,
                                color = ColorTranslator.ToHtml(action.PenColor),
                                thickness = (int)action.PenWidth
                            });
                            byte[] data = Encoding.UTF8.GetBytes(message);
                            stream.Write(data, 0, data.Length);
                            stream.Flush();
                        }
                        else if (action.Type == ActionType.Image)
                        {
                            var message = JsonSerializer.Serialize(new
                            {
                                type = "image",
                                url = action.ImageUrl,
                                width = action.ImageRect!.Value.Width,
                                height = action.ImageRect!.Value.Height
                            });
                            byte[] data = Encoding.UTF8.GetBytes(message);
                            stream.Write(data, 0, data.Length);
                            stream.Flush();
                        }
                    }
                }
                Console.WriteLine($"Sent {actions.Count} actions to new client");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send whiteboard to client: {ex.Message}");
            }
        }

        private void WhiteboardServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener?.Stop();
            lock (clients)
            {
                foreach (var c in clients)
                {
                    c.Close();
                }
            }
        }

        private void btnOpenClient_Click(object sender, EventArgs e)
        {
            WhiteboardClientForm clientForm = new WhiteboardClientForm();
            clientForm.Show();
        }
    }
}