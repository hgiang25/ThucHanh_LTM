//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Net.Sockets;
//using System.Net;

//namespace Lab3.Bai02
//{
//    public partial class TCPServer : Form
//    {
//        Thread serverThread;
//        private bool isServerRunning = false;
//        public TCPServer()
//        {
//            InitializeComponent();
//        }

//        private Socket serverSocket = null;

//        private void btnListen_Click(object sender, EventArgs e)
//        {
//            // Đảm bảo thread không gây lỗi
//            CheckForIllegalCrossThreadCalls = false;

//            // Tạo và bắt đầu thread server chỉ khi server chưa chạy
//            serverThread = new Thread(new ThreadStart(StartUnsafeThread));
//            serverThread.Start();
//            isServerRunning = true;
//        }

//        void StartUnsafeThread()
//        {
//            int bytesReceived = 0;
//            byte[] recv = new byte[1024];
//            Socket clientSocket;

//            int port = 8080;

//            // Kiểm tra xem serverSocket đã được khởi tạo chưa
//            if (serverSocket != null && serverSocket.IsBound)
//            {
//                Invoke(new Action(() => MessageBox.Show("Server đang chạy, không thể mở lại")));
//                return;
//            }

//            try
//            {
//                // Khởi tạo socket để lắng nghe kết nối TCP
//                serverSocket = new Socket(
//                    AddressFamily.InterNetwork,
//                    SocketType.Stream,
//                    ProtocolType.Tcp);

//                serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

//                serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
//                serverSocket.Listen(-1);


//                // Hiển thị thông báo khi server bắt đầu lắng nghe
//                Invoke(new Action(() => MessageBox.Show("Server đang lắng nghe trên cổng " + port)));

//                // Chấp nhận kết nối từ client
//                while (isServerRunning)
//                {
//                    clientSocket = serverSocket.Accept();
//                    lvMessage.Items.Add(new ListViewItem("Telnet running on 127.0.0.1:8080"));
//                    lvMessage.Items.Add(new ListViewItem("New client connected"));

//                    while (clientSocket.Connected)
//                    {
//                        StringBuilder text = new StringBuilder();
//                        bytesReceived = 0;

//                        // Nhận dữ liệu từ client
//                        while ((bytesReceived = clientSocket.Receive(recv, 0, recv.Length, SocketFlags.None)) > 0)
//                        {
//                            text.Append(Encoding.ASCII.GetString(recv, 0, bytesReceived));

//                            if (text.ToString().Contains("\n"))
//                            {
//                                break;
//                            }
//                        }

//                        // Hiển thị thông điệp nhận được
//                        if (text.Length > 0)
//                        {
//                            Invoke(new Action(() =>
//                            {
//                                lvMessage.Items.Add(new ListViewItem(text.ToString()));
//                                lvMessage.EnsureVisible(lvMessage.Items.Count - 1);
//                            }));
//                        }
//                        else
//                        {
//                            Invoke(new Action(() =>
//                            {
//                                lvMessage.Items.Add(new ListViewItem("Client disconnected from " + clientSocket.RemoteEndPoint.ToString()));
//                                lvMessage.EnsureVisible(lvMessage.Items.Count - 1);
//                            }));
//                            break;
//                        }
//                    }
//                    // Đóng kết nối cũ, chuẩn bị chờ client mới
//                    clientSocket.Shutdown(SocketShutdown.Both);
//                    clientSocket.Close();
//                }

//            }
//            catch (Exception ex)
//            {
//                Invoke(new Action(() => MessageBox.Show("Lỗi: " + ex.Message)));
//            }
//        }

//        private void TCPServer_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            //isServerRunning = false;
//            //if (serverSocket != null && serverSocket.Connected)
//            //{
//            //    try
//            //    {
//            //        // Đảm bảo đóng kết nối đúng cách
//            //        serverSocket.Shutdown(SocketShutdown.Both); // Đóng cả hai chiều gửi và nhận
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        // Xử lý ngoại lệ nếu có khi gọi Shutdown
//            //        Console.WriteLine("Error during socket shutdown: " + ex.Message);
//            //    }
//            //}

//            //// Đảm bảo đóng socket đúng cách
//            //if (serverSocket != null)
//            //{
//            //    serverSocket.Close();  // Đóng socket
//            //    serverSocket = null;   // Giải phóng tài nguyên                
//            //}

//            //try
//            //{
//            //    // Kiểm tra nếu socket đang mở
//            //    if (serverSocket != null)
//            //    {
//            //        serverSocket.Close();  // Đóng socket để ngừng lắng nghe
//            //        serverSocket = null;   // Giải phóng tài nguyên
//            //    }

//            //    // Nếu có thread chạy, dừng nó an toàn trước khi đóng form
//            //    if (serverThread != null && serverThread.IsAlive)
//            //    {
//            //        serverThread.Abort(); // Dừng thread tránh crash ứng dụng
//            //        serverThread = null;
//            //    }
//            //}
//            //catch (Exception ex)
//            //{
//            //    MessageBox.Show("Lỗi khi đóng server: " + ex.Message);
//            //}

//            isServerRunning = false;
//            try
//            {
//                if (serverSocket != null)
//                {
//                    // Đảm bảo đóng socket và giải phóng tài nguyên
//                    if (serverSocket.Connected)
//                    {
//                        serverSocket.Shutdown(SocketShutdown.Both);  // Đóng cả hai chiều gửi và nhận
//                    }
//                    serverSocket.Close();  // Đóng socket
//                    serverSocket = null;   // Giải phóng tài nguyên
//                }

//                // Dừng thread nếu cần
//                if (serverThread != null && serverThread.IsAlive)
//                {
//                    serverThread.Abort(); // Dừng thread tránh crash ứng dụng
//                    serverThread = null;
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi đóng server: " + ex.Message);
//            }
//        }
//    }
//}


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

namespace Lab3.Bai02
{
    public partial class TCPServer : Form
    {
        private bool isServerRunning = false;        
        public TCPServer()
        {
            InitializeComponent();
        }

        private Socket serverSocket = null;

        private void btnListen_Click(object sender, EventArgs e)
        {
            // Đảm bảo thread không gây lỗi
            CheckForIllegalCrossThreadCalls = false;

            // Tạo và bắt đầu thread server chỉ khi server chưa chạy
            Thread serverThread = new Thread(new ThreadStart(StartUnsafeThread));
            serverThread.Start();
            isServerRunning = true;
        }

        void StartUnsafeThread()
        {
            int bytesReceived = 0;
            byte[] recv = new byte[1024];
            Socket clientSocket;

            int port = 8080;

            // Kiểm tra xem serverSocket đã được khởi tạo chưa
            if (serverSocket != null && serverSocket.IsBound)
            {
                Invoke(new Action(() => MessageBox.Show("Server đang chạy, không thể mở lại")));
                return;
            }

            try
            {
                // Khởi tạo socket để lắng nghe kết nối TCP
                serverSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);

                serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
                serverSocket.Listen(-1);


                // Hiển thị thông báo khi server bắt đầu lắng nghe
                Invoke(new Action(() => MessageBox.Show("Server đang lắng nghe trên cổng " + port)));
                while (isServerRunning)
                {            
                    clientSocket = serverSocket.Accept();
                    lvMessage.Items.Add(new ListViewItem("Telnet running on 127.0.0.1:8080"));
                    lvMessage.Items.Add(new ListViewItem("New client connected"));
                    while (clientSocket.Connected)
                    {            
                        StringBuilder text = new StringBuilder();
                        bytesReceived = 0;

                        // Nhận dữ liệu từ client
                        while ((bytesReceived = clientSocket.Receive(recv, 0, recv.Length, SocketFlags.None)) > 0)
                        {
                            text.Append(Encoding.ASCII.GetString(recv, 0, bytesReceived));

                            if (text.ToString().Contains("\n"))
                            {
                                break;
                            }
                        }

                        // Hiển thị thông điệp nhận được
                        if (text.Length > 0)
                        {
                            Invoke(new Action(() =>
                            {
                                lvMessage.Items.Add(new ListViewItem(text.ToString()));
                                lvMessage.EnsureVisible(lvMessage.Items.Count - 1);
                            }));
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                lvMessage.Items.Add(new ListViewItem("Client disconnected from " + clientSocket.RemoteEndPoint.ToString()));
                                lvMessage.EnsureVisible(lvMessage.Items.Count - 1);
                            }));
                            break;
                        }
                    }
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }

                // Chấp nhận kết nối từ client
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => MessageBox.Show("Lỗi: " + ex.Message)));
            }
        }

        private void TCPServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serverSocket != null)
            {
                serverSocket.Close();  // Đóng socket
                serverSocket = null;   // Giải phóng tài nguyên                    
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            isServerRunning = false;             
            if (serverSocket != null && serverSocket.Connected)
            {
                try
                {
                    // Đảm bảo đóng kết nối đúng cách
                    serverSocket.Shutdown(SocketShutdown.Both); // Đóng cả hai chiều gửi và nhận
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu có khi gọi Shutdown
                    Console.WriteLine("Error during socket shutdown: " + ex.Message);
                }
            }

            // Đảm bảo đóng socket đúng cách
            if (serverSocket != null)
            {
                serverSocket.Close();  // Đóng socket
                serverSocket = null;   // Giải phóng tài nguyên                    
            }
        }
    }
}