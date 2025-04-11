using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Lab3.Bai04
{
    public partial class ChatServer : Form
    {
        public ChatServer()
        {
            InitializeComponent();
        }
        private Dictionary<TcpClient, string> clientUsernames = new Dictionary<TcpClient, string>();

        private TcpListener tcpListener;
        private bool isServerRunning = false;
        private Thread listenerThread;
        private List<TcpClient> clientList = new List<TcpClient>();

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (isServerRunning)
            {
                MessageBox.Show("Server is already running.");
                return;
            }

            int port = 8080;
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            tcpListener.Start();
            isServerRunning = true;

            listenerThread = new Thread(ListenForClients);
            listenerThread.IsBackground = true;
            listenerThread.Start();

            lvMessage.Invoke(new Action(() =>
            {
                lvMessage.Items.Add(new ListViewItem("Server running on 127.0.0.1:" + port));
            }));

            MessageBox.Show("Server started on port " + port);
        }

        private void ListenForClients()
        {
            while (isServerRunning)
            {
                try
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    // Nhận username
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        client.Close();
                        continue;
                    }

                    string username = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                    lock (clientUsernames)
                    {
                        if (clientUsernames.ContainsValue(username))
                        {
                            byte[] rejectMsg = Encoding.UTF8.GetBytes("USERNAME_EXISTS\n");
                            stream.Write(rejectMsg, 0, rejectMsg.Length);
                            client.Close();
                            continue;
                        }
                        else
                        {
                            clientUsernames[client] = username;
                            clientList.Add(client);

                            byte[] okMsg = Encoding.UTF8.GetBytes("OK\n");
                            stream.Write(okMsg, 0, okMsg.Length);

                            // Hiển thị log
                            lvMessage.Invoke(new Action(() =>
                            {
                                lvMessage.Items.Add(new ListViewItem($"New client connected from: {client.Client.RemoteEndPoint}"));
                            }));

                            // ✅ Truyền username vào thread xử lý client
                            Thread clientThread = new Thread(() => HandleClientComm(client, username));
                            clientThread.IsBackground = true;
                            clientThread.Start();
                        }
                    }
                }
                catch (SocketException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    lvMessage.Invoke(new Action(() =>
                    {
                        lvMessage.Items.Add(new ListViewItem($"Error while accepting client: {ex.Message}"));
                    }));
                }
            }
        }


        private void HandleClientComm(TcpClient client, string username)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;
                StringBuilder messageBuilder = new StringBuilder();

                // Không nhận lại username ở đây nữa!
                // Bắt đầu nhận tin nhắn luôn

                while (isServerRunning && client.Connected)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    while (messageBuilder.ToString().Contains("\n"))
                    {
                        int newlineIndex = messageBuilder.ToString().IndexOf("\n");
                        string message = messageBuilder.ToString().Substring(0, newlineIndex).Trim();
                        messageBuilder.Remove(0, newlineIndex + 1);

                        if (string.IsNullOrEmpty(message)) continue;

                        string fullMessage = $"{client.Client.RemoteEndPoint}: {message}";

                        // Hiển thị trên server
                        lvMessage.Invoke(new Action(() =>
                        {
                            lvMessage.Items.Add(new ListViewItem(fullMessage));
                        }));

                        // Gửi đến các client
                        BroadcastMessage(fullMessage);
                    }
                }
            }
            catch { }
            finally
            {
                try { stream?.Close(); client?.Close(); } catch { }
                lock (clientList) { clientList.Remove(client); }
                lock (clientUsernames) { clientUsernames.Remove(client); }
            }
        }


        private void BroadcastMessage(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message + "\n");

            lock (clientList)
            {
                foreach (var client in clientList.ToList())
                {
                    if (client.Connected)
                    {
                        try
                        {
                            NetworkStream stream = client.GetStream();
                            stream.Write(messageBytes, 0, messageBytes.Length);
                        }
                        catch
                        {
                            // Bỏ qua client bị lỗi
                        }
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            isServerRunning = false;

            // Gửi tin nhắn đóng kết nối đến tất cả client trước khi đóng
            string shutdownMessage = "SERVER_CLOSING\n";
            byte[] shutdownBytes = Encoding.UTF8.GetBytes(shutdownMessage);

            foreach (var client in clientList)
            {
                if (client.Connected)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(shutdownBytes, 0, shutdownBytes.Length);
                    }
                    catch { }
                }

                client.Close();
            }

            clientList.Clear();
            tcpListener?.Stop();

            MessageBox.Show("Server stopped");
        }

    }
}
