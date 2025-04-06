using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Lab3.Bai04
{
    public partial class ChatServer : Form
    {
        private bool isServerRunning = false;
        private Socket serverSocket;
        private List<Socket> clientSockets = new List<Socket>();
        private object locker = new object(); // Đảm bảo thread an toàn

        public ChatServer()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            if (!isServerRunning)
            {
                Thread serverThread = new Thread(StartServer);
                serverThread.Start();
                isServerRunning = true;
            }
        }

        void StartServer()
        {
            int port = 8080;

            try
            {
                serverSocket = new Socket(
                    AddressFamily.InterNetwork, 
                    SocketType.Stream, 
                    ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
                serverSocket.Listen(10);

                Invoke(new Action(() => lvMessage.Items.Add(new ListViewItem($"Server đang lắng nghe trên cổng {port}..."))));

                while (isServerRunning)
                {
                    Socket clientSocket = serverSocket.Accept();
                    lock (locker)
                    {
                        clientSockets.Add(clientSocket);
                    }

                    Invoke(new Action(() =>
                    {
                        lvMessage.Items.Add(new ListViewItem($"Client kết nối: {clientSocket.RemoteEndPoint}"));
                        UpdateClientList();
                    }));

                    Thread clientThread = new Thread(() => HandleClient(clientSocket));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => MessageBox.Show("Lỗi: " + ex.Message)));
            }
        }

        void HandleClient(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = clientSocket.Receive(buffer)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Invoke(new Action(() =>
                    {
                        lvMessage.Items.Add(new ListViewItem($"Client: {message}"));
                    }));

                    BroadcastMessage($"Client {clientSocket.RemoteEndPoint}: {message}", clientSocket);
                }
            }
            catch
            {
                // Client ngắt kết nối
                lock (locker)
                {
                    clientSockets.Remove(clientSocket);
                }
                Invoke(new Action(() =>
                {
                    lvMessage.Items.Add(new ListViewItem($"Client {clientSocket.RemoteEndPoint} đã ngắt kết nối."));
                    UpdateClientList();
                }));
            }
            finally
            {
                clientSocket.Close();
            }
        }

        void BroadcastMessage(string message, Socket senderSocket)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            lock (locker)
            {
                foreach (var client in clientSockets)
                {
                    if (client != senderSocket && client.Connected)
                    {
                        try
                        {
                            client.Send(data);
                        }
                        catch
                        {
                            // Nếu có lỗi khi gửi, xóa client khỏi danh sách
                            clientSockets.Remove(client);
                        }
                    }
                }
            }
        }

        void UpdateClientList()
        {
            lvClients.Items.Clear();
            foreach (var client in clientSockets)
            {
                lvClients.Items.Add(new ListViewItem(client.RemoteEndPoint.ToString()));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            isServerRunning = false;
            lock (locker)
            {
                foreach (var client in clientSockets)
                {
                    client.Close();
                }
                clientSockets.Clear();
            }

            if (serverSocket != null)
            {
                serverSocket.Close();
                serverSocket = null;
            }
        }

        private void ChatServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnClose_Click(sender, e);
        }
    }
}
