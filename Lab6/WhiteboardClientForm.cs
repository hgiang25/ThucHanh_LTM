using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace Lab6
{
    public partial class WhiteboardClientForm : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private Graphics g;
        private Pen currentPen = new Pen(Color.Black, 2);
        private Point prevPoint;
        private bool isDrawing = false;
        private Bitmap canvas;
        private bool isReceiving = false;

        private List<DrawAction> actions = new List<DrawAction>();
        private DrawAction? selectedImageAction = null;
        private bool isResizing = false;
        private Point resizeStartPoint;
        private enum ResizeHandle { None, BottomRight }
        private ResizeHandle currentResizeHandle = ResizeHandle.None;

        public WhiteboardClientForm()
        {
            InitializeComponent();

            canvas = new Bitmap(800, 600);
            g = Graphics.FromImage(canvas);
            g.Clear(Color.White);
            pictureBox1.Image = canvas;

            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 5000);
                stream = client.GetStream();

                var thread = new Thread(() => Listen());
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to server: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void Listen()
        {
            byte[] buffer = new byte[8192];

            while (true)
            {
                try
                {
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    if (bytes == 0)
                    {
                        Console.WriteLine("Disconnected from server (read 0 bytes)");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytes);
                    Console.WriteLine("Received message: " + message);
                    var json = JsonDocument.Parse(message);
                    string type = json.RootElement.GetProperty("type").GetString();

                    if (type == "draw")
                    {
                        int x1 = json.RootElement.GetProperty("x1").GetInt32();
                        int y1 = json.RootElement.GetProperty("y1").GetInt32();
                        int x2 = json.RootElement.GetProperty("x2").GetInt32();
                        int y2 = json.RootElement.GetProperty("y2").GetInt32();
                        string color = json.RootElement.GetProperty("color").GetString();
                        int thickness = json.RootElement.GetProperty("thickness").GetInt32();

                        Invoke(new Action(() =>
                        {
                            var action = new DrawAction
                            {
                                Type = ActionType.Line,
                                StartPoint = new Point(x1, y1),
                                EndPoint = new Point(x2, y2),
                                PenColor = ColorTranslator.FromHtml(color),
                                PenWidth = thickness
                            };
                            actions.Add(action);
                            RedrawAll();
                        }));
                    }
                    else if (type == "count")
                    {
                        int count = json.RootElement.GetProperty("count").GetInt32();
                        Console.WriteLine($"Client count update received: {count}");
                        Invoke(new Action(() =>
                        {
                            this.Text = $"Whiteboard - Clients: {count}";
                            lblClientCount.Text = $"Clients connected: {count}";
                        }));
                    }
                    else if (type == "image")
                    {
                        string url = json.RootElement.GetProperty("url").GetString();
                        int width = json.RootElement.GetProperty("width").GetInt32();
                        int height = json.RootElement.GetProperty("height").GetInt32();
                        Invoke(new Action(() =>
                        {
                            try
                            {
                                using (var wc = new WebClient())
                                {
                                    wc.Headers.Add("User-Agent", "Mozilla/5.0");
                                    byte[] imageData = wc.DownloadData(url);
                                    using (var ms = new MemoryStream(imageData))
                                    {
                                        Image img = Image.FromStream(ms);
                                        if (img.Width == 0 || img.Height == 0)
                                        {
                                            throw new Exception("Invalid image data");
                                        }
                                        var action = new DrawAction
                                        {
                                            Type = ActionType.Image,
                                            ImageUrl = url,
                                            ImageRect = new Rectangle(100, 100, width, height),
                                            Image = (Image)img.Clone()
                                        };
                                        actions.Add(action);
                                        RedrawAll();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Failed to load image from {url}: {ex.Message}");
                                MessageBox.Show($"Failed to load image from {url}: {ex.Message}", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }));
                    }
                    else if (type == "whiteboard_image")
                    {
                        string base64 = json.RootElement.GetProperty("imageBase64").GetString();
                        byte[] imgBytes = Convert.FromBase64String(base64);
                        Invoke(new Action(() =>
                        {
                            actions.Clear();
                            using (MemoryStream ms = new MemoryStream(imgBytes))
                            {
                                Image img = Image.FromStream(ms);
                                var action = new DrawAction
                                {
                                    Type = ActionType.Image,
                                    ImageRect = new Rectangle(0, 0, canvas.Width, canvas.Height),
                                    Image = (Image)img.Clone()
                                };
                                actions.Add(action);
                            }
                            RedrawAll();
                            pictureBox1.Invalidate(); // Ensure canvas is refreshed
                            pictureBox1.Refresh();
                        }));
                    }
                    else if (type == "undo")
                    {
                        Invoke(new Action(() =>
                        {
                            if (actions.Count > 0)
                            {
                                actions.RemoveAt(actions.Count - 1);
                                RedrawAll();
                            }
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Listen: {ex.Message}");
                }
            }
        }

        private void RedrawAll()
        {
            g.Clear(Color.White);
            Console.WriteLine($"Redrawing {actions.Count} actions");
            foreach (var action in actions)
            {
                Console.WriteLine($"Drawing action: {action.Type}");
                action.Draw(g);
            }
            if (selectedImageAction != null && selectedImageAction.Type == ActionType.Image)
            {
                using (Pen pen = new Pen(Color.Blue, 2))
                {
                    g.DrawRectangle(pen, selectedImageAction.ImageRect!.Value);
                }
                Rectangle handleRect = new Rectangle(
                    selectedImageAction.ImageRect!.Value.Right - HANDLE_SIZE,
                    selectedImageAction.ImageRect!.Value.Bottom - HANDLE_SIZE,
                    HANDLE_SIZE,
                    HANDLE_SIZE);
                g.FillRectangle(Brushes.Blue, handleRect);
            }
            pictureBox1.Image = canvas;
            pictureBox1.Invalidate();
            pictureBox1.Refresh();
        }

        private const int HANDLE_SIZE = 10;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selectedImageAction = null;
                foreach (var action in actions)
                {
                    if (action.Type == ActionType.Image && action.ImageRect.HasValue && action.ImageRect.Value.Contains(e.Location))
                    {
                        selectedImageAction = action;
                        var handleRect = new Rectangle(
                            action.ImageRect!.Value.Right - HANDLE_SIZE,
                            action.ImageRect!.Value.Bottom - HANDLE_SIZE,
                            HANDLE_SIZE,
                            HANDLE_SIZE);
                        if (handleRect.Contains(e.Location))
                        {
                            isResizing = true;
                            resizeStartPoint = e.Location;
                            currentResizeHandle = ResizeHandle.BottomRight;
                        }
                        else
                        {
                            isResizing = false;
                            currentResizeHandle = ResizeHandle.None;
                        }
                        break;
                    }
                }

                if (selectedImageAction == null)
                {
                    isDrawing = true;
                    prevPoint = e.Location;
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing && selectedImageAction != null && selectedImageAction.Type == ActionType.Image)
            {
                int newWidth = selectedImageAction.ImageRect!.Value.Width + (e.X - resizeStartPoint.X);
                int newHeight = selectedImageAction.ImageRect!.Value.Height + (e.Y - resizeStartPoint.Y);

                if (newWidth < 10) newWidth = 10;
                if (newHeight < 10) newHeight = 10;

                selectedImageAction.ImageRect = new Rectangle(
                    selectedImageAction.ImageRect!.Value.X,
                    selectedImageAction.ImageRect!.Value.Y,
                    newWidth,
                    newHeight);
                resizeStartPoint = e.Location;
                RedrawAll();
            }
            else if (isDrawing)
            {
                var action = new DrawAction
                {
                    Type = ActionType.Line,
                    StartPoint = prevPoint,
                    EndPoint = e.Location,
                    PenColor = currentPen.Color,
                    PenWidth = currentPen.Width
                };
                actions.Add(action);
                action.Draw(g);
                pictureBox1.Invalidate();
                pictureBox1.Refresh();

                try
                {
                    var message = JsonSerializer.Serialize(new
                    {
                        type = "draw",
                        x1 = prevPoint.X,
                        y1 = prevPoint.Y,
                        x2 = e.X,
                        y2 = e.Y,
                        color = ColorTranslator.ToHtml(currentPen.Color),
                        thickness = (int)currentPen.Width
                    });
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    stream.Flush(); // Ensure data is sent immediately
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending draw message: {ex.Message}");
                }

                prevPoint = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                isResizing = false;
                currentResizeHandle = ResizeHandle.None;
            }
            else
            {
                isDrawing = false;
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (actions.Count == 0) return;

            actions.RemoveAt(actions.Count - 1);
            RedrawAll();

            try
            {
                var message = JsonSerializer.Serialize(new { type = "undo" });
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending undo message: {ex.Message}");
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn lưu lại hình ảnh trước khi thoát?", "Xác nhận", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PNG Image|*.png";
                    saveDialog.Title = "Chọn nơi lưu và đặt tên file";
                    saveDialog.FileName = "whiteboard.png";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            canvas.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            MessageBox.Show("Đã lưu hình ảnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else if (result == DialogResult.No)
            {
                this.Close();
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    currentPen.Color = dlg.Color;
                }
            }
        }

        private void btnThickness_ValueChanged(object sender, EventArgs e)
        {
            currentPen.Width = (float)((NumericUpDown)sender).Value;
        }

        private void btnInsertImage_Click(object sender, EventArgs e)
        {
            string imageUrl = Microsoft.VisualBasic.Interaction.InputBox("Enter image URL:", "Insert Image", "http://example.com/image.jpg");
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                if (!Regex.IsMatch(imageUrl, @"^(http|https)://([\w-]+.)+[\w-]+(/[\w-./?%&=])?$"))
                {
                    MessageBox.Show("Invalid URL format. Please enter a valid URL starting with http:// or https://", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string widthInput = Microsoft.VisualBasic.Interaction.InputBox("Enter image width (pixels):", "Image Width", "200");
                string heightInput = Microsoft.VisualBasic.Interaction.InputBox("Enter image height (pixels):", "Image Height", "200");

                if (int.TryParse(widthInput, out int width) && int.TryParse(heightInput, out int height) && width > 0 && height > 0)
                {
                    try
                    {
                        using (var wc = new WebClient())
                        {
                            wc.Headers.Add("User-Agent", "Mozilla/5.0");
                            byte[] imageData = wc.DownloadData(imageUrl);
                            using (var ms = new MemoryStream(imageData))
                            {
                                Image img = Image.FromStream(ms);
                                if (img.Width == 0 || img.Height == 0)
                                    throw new Exception("Invalid image data");
                            }
                            var message = JsonSerializer.Serialize(new
                            {
                                type = "image",
                                url = imageUrl,
                                width,
                                height
                            });
                            byte[] data = Encoding.UTF8.GetBytes(message);
                            stream.Write(data, 0, data.Length);
                            stream.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to validate image URL {imageUrl}: {ex.Message}");
                        MessageBox.Show($"Cannot access image at {imageUrl}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid width or height. Please enter positive numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void WhiteboardClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                stream?.Close();
                client?.Close();
            }
            catch { }
        }
    }
}
