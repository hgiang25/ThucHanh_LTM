using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace Lab3.Bai03
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }
        private bool isServerRunning = false;
        private Socket serverSocket = null;
        private Thread serverThread;
        private Socket currentClient = null;
        private readonly object clientLock = new object(); // đảm bảo thread-safe

        //private Socket currentClientSocket = null;


        private void btnListen_Click(object sender, EventArgs e)
        {
            // Đảm bảo chỉ tạo server nếu chưa chạy
            if (serverSocket != null && serverSocket.IsBound)
            {
                MessageBox.Show("Server đang chạy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Đảm bảo thread cũ đã kết thúc
            if (serverThread != null && serverThread.IsAlive)
            {
                serverThread.Join(); // Đợi thread cũ chết hẳn
                serverThread = null;
            }

            isServerRunning = true;
            CheckForIllegalCrossThreadCalls = false;

            serverThread = new Thread(new ThreadStart(StartUnsafeThread));
            serverThread.IsBackground = true;
            serverThread.Start();
        }


        void StartUnsafeThread()
        {
            int port = 8080;

            try
            {
                serverSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);

                serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
                serverSocket.Listen(5);

                Invoke(new Action(() => MessageBox.Show("Server đang lắng nghe trên cổng " + port)));
                lvMessage.Invoke(new Action(() =>
                {
                    lvMessage.Items.Add(new ListViewItem("Server running on 127.0.0.1:8080"));
                }));
                while (isServerRunning)
                {
                    Socket clientSocket = null;

                    try
                    {
                        clientSocket = serverSocket.Accept();
                    }
                    catch
                    {
                        break;
                    }

                    lock (clientLock)
                    {
                        if (currentClient != null && currentClient.Connected)
                        {
                            // Đã có client đang kết nối, từ chối client mới
                            byte[] rejectMsg = Encoding.UTF8.GetBytes("BUSY\n");
                            try { clientSocket.Send(rejectMsg); } catch { }
                            clientSocket.Close();
                            continue;
                        }

                        currentClient = clientSocket;
                    }

                    // Gửi phản hồi "OK"
                    try
                    {
                        byte[] welcomeMsg = Encoding.UTF8.GetBytes("OK\n");
                        currentClient.Send(welcomeMsg);
                    }
                    catch
                    {
                        currentClient.Close();
                        currentClient = null;
                        continue;
                    }

                    lvMessage.Invoke(new Action(() =>
                    {
                        lvMessage.Items.Add(new ListViewItem("New client connected"));
                    }));

                    // Xử lý client trong thread con
                    Thread clientThread = new Thread(() => HandleClient(currentClient));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => MessageBox.Show("Lỗi socket: " + ex.Message)));
            }
        }
        private void HandleClient(Socket clientSocket)
        {
            byte[] recv = new byte[1024];

            try
            {
                while (isServerRunning && clientSocket.Connected)
                {
                    int bytesReceived = 0;

                    try
                    {
                        bytesReceived = clientSocket.Receive(recv);
                    }
                    catch (SocketException se)
                    {
                        // Có thể là lỗi do telnet đóng đột ngột
                        Invoke(new Action(() => lvMessage.Items.Add(new ListViewItem("Client socket error: " + se.Message))));
                        break;
                    }

                    if (bytesReceived == 0)
                    {
                        // Telnet/client đã đóng kết nối
                        Invoke(new Action(() => lvMessage.Items.Add(new ListViewItem("Client closed the connection"))));
                        break;
                    }

                    string message = Encoding.UTF8.GetString(recv, 0, bytesReceived).Trim();

                    if (string.Equals(message, "quit", StringComparison.OrdinalIgnoreCase))
                        break;

                    if (string.Equals(message, "hello", StringComparison.OrdinalIgnoreCase))
                        continue;

                    Invoke(new Action(() =>
                    {
                        lvMessage.Items.Add(new ListViewItem(message));
                    }));
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => lvMessage.Items.Add(new ListViewItem("Exception: " + ex.Message))));
            }
            finally
            {
                try { clientSocket.Shutdown(SocketShutdown.Both); } catch { }
                clientSocket.Close();

                lock (clientLock)
                {
                    if (currentClient == clientSocket)
                        currentClient = null;
                }

                Invoke(new Action(() =>
                {
                    lvMessage.Items.Add(new ListViewItem("Client disconnected"));
                }));
            }
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            StopServer();
            MessageBox.Show("Server đã dừng!");
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }

        private void StopServer()
        {
            isServerRunning = false;

            try
            {
                // Đóng socket sẽ khiến Accept()/Receive() bị lỗi và thoát thread
                if (serverSocket != null)
                {
                    serverSocket.Close();
                    serverSocket = null;
                }
            }
            catch { }

            // KHÔNG Join() ở đây, thread sẽ tự thoát sau khi bị lỗi ở Accept()

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lvMessage.Clear();
        }
    }
}