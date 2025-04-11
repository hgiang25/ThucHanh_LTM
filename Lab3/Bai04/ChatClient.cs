//using System;
//using System.Net.Sockets;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Windows.Forms;

//namespace Lab3.Bai04
//{
//    public partial class ChatClient : Form
//    {
//        private TcpClient tcpClient = null;
//        private NetworkStream ns;
//        private Thread listenThread;
//        private bool isConnected = false;

//        public ChatClient()
//        {
//            InitializeComponent();
//        }

//        private void btnConnect_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (!isConnected)
//                {
//                    tcpClient = new TcpClient();
//                    tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 8080);
//                    ns = tcpClient.GetStream();

//                    isConnected = true;
//                    MessageBox.Show("Kết nối thành công!");

//                    listenThread = new Thread(ListenForMessages);
//                    listenThread.IsBackground = true;
//                    listenThread.Start();
//                }
//                else
//                {
//                    MessageBox.Show("Đã kết nối với server!");
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi kết nối: " + ex.Message);
//            }
//        }

//        private void btnSend_Click(object sender, EventArgs e)
//        {
//            if (isConnected && ns != null)
//            {
//                try
//                {
//                    string message = txtMessage.Text.Trim();
//                    if (string.IsNullOrEmpty(message)) return;

//                    byte[] data = Encoding.UTF8.GetBytes(message + "\n");
//                    ns.Write(data, 0, data.Length);
//                    AppendMessage("Me: " + message);
//                    txtMessage.Clear();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Lỗi gửi dữ liệu: " + ex.Message);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Chưa kết nối đến Server!");
//            }
//        }

//        private void ListenForMessages()
//        {
//            byte[] buffer = new byte[1024];
//            int bytesRead;

//            try
//            {
//                while (isConnected && (bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
//                {
//                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//                    AppendMessage("Server: " + message);
//                }
//            }
//            catch
//            {
//                AppendMessage("Mất kết nối đến server.");
//            }
//        }

//        private void AppendMessage(string message)
//        {
//            if (lvMessage.InvokeRequired)
//            {
//                lvMessage.Invoke(new Action(() =>
//                {
//                    lvMessage.Items.Add(new ListViewItem(message));
//                    lvMessage.EnsureVisible(lvMessage.Items.Count - 1);
//                }));
//            }
//            else
//            {
//                lvMessage.Items.Add(new ListViewItem(message));
//                lvMessage.EnsureVisible(lvMessage.Items.Count - 1);
//            }
//        }

//        private void btnClose_Click(object sender, EventArgs e)
//        {
//            if (isConnected)
//            {
//                isConnected = false;
//                listenThread?.Abort();

//                ns?.Close();
//                tcpClient?.Close();

//                ns = null;
//                tcpClient = null;
//                MessageBox.Show("Đã ngắt kết nối!");
//            }
//            else
//            {
//                MessageBox.Show("Chưa có kết nối nào để đóng!");
//            }
//        }

//        private void ChatClient_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            btnClose_Click(sender, e);
//        }
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Net.Sockets;

//namespace Lab3.Bai03
//{
//    public partial class TCPClient : Form
//    {
//        public TCPClient()
//        {
//            InitializeComponent();
//        }

//        private TcpClient tcpClient = null;
//        private NetworkStream ns;


//        private void btnSend_Click(object sender, EventArgs e)
//        {
//            if (tcpClient != null && tcpClient.Connected && ns != null)
//            {
//                try
//                {                
//                    string message = $"{txtUsername.Text}: {txtMessage.Text}\n";

//                    byte[] data = Encoding.ASCII.GetBytes(message);
//                    ns.Write(data, 0, data.Length);
//                    ns.Flush();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Lỗi gửi dữ liệu: " + ex.Message);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Chưa kết nối đến Server!");
//            }
//        }

//        private void btnConnect_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (tcpClient == null || !tcpClient.Connected)
//                {
//                    tcpClient = new TcpClient();
//                    IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
//                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8080);

//                    tcpClient.Connect(ipEndPoint);
//                    ns = tcpClient.GetStream();
//                    MessageBox.Show("Kết nối thành công!");
//                }
//                else
//                {
//                    MessageBox.Show("Đã kết nối với server!");
//                    return;
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void btnClose_Click(object sender, EventArgs e)
//        {
//            if (tcpClient != null && tcpClient.Connected)
//            {
//                try
//                {
//                    byte[] data = Encoding.ASCII.GetBytes("quit\n");
//                    ns.Write(data, 0, data.Length);
//                    ns.Close();
//                    tcpClient.Close();
//                    tcpClient = null;
//                    ns = null;
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Lỗi khi đóng kết nối: " + ex.Message);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Chưa có kết nối nào để đóng!");
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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace Lab3.Bai04
{
    public partial class ChatClient : Form
    {
        public ChatClient()
        {
            InitializeComponent();
        }

        private TcpClient tcpClient = null;
        private NetworkStream ns;
        private Thread receiveThread;

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tcpClient != null && tcpClient.Connected && ns != null)
            {
                try
                {
                    // Đóng gói tin nhắn theo định dạng "username:message"
                    string message = $"{txtUsername.Text}: {txtMessage.Text}\n";
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    // Gửi tin nhắn đến server
                    ns.Write(data, 0, data.Length);
                    ns.Flush();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi gửi dữ liệu: " + ex.Message);
                }
                txtMessage.Clear();
            }
            else
            {
                MessageBox.Show("Chưa kết nối đến Server!");
            }
        }



        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient == null || !tcpClient.Connected)
                {
                    tcpClient = new TcpClient();
                    IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8080);

                    tcpClient.Connect(ipEndPoint);
                    ns = tcpClient.GetStream();

                    // Bắt đầu nhận tin nhắn trong một luồng nền
                    Task.Run(() => ReceiveMessages());

                    MessageBox.Show("Kết nối thành công!");
                }
                else
                {
                    MessageBox.Show("Đã kết nối với server!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }



        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            int bytesReceived;
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                while (tcpClient.Connected)
                {
                    bytesReceived = ns.Read(buffer, 0, buffer.Length);

                    if (bytesReceived > 0)
                    {
                        // Kiểm tra tin nhắn đã nhận được
                        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                        // MessageBox.Show($"Received Message: {receivedMessage}"); // Kiểm tra xem client có nhận tin nhắn không

                        messageBuilder.Append(receivedMessage);

                        // Kiểm tra nếu có một hoặc nhiều tin nhắn hoàn chỉnh
                        while (messageBuilder.ToString().Contains("\n"))
                        {
                            int newlineIndex = messageBuilder.ToString().IndexOf("\n");
                            string message = messageBuilder.ToString().Substring(0, newlineIndex).Trim();
                            messageBuilder.Remove(0, newlineIndex + 1); // Xóa tin nhắn đã lấy ra khỏi StringBuilder

                            // Sử dụng Invoke để cập nhật ListView từ luồng khác
                            lvMessages.Invoke(new Action(() =>
                            {
                                lvMessages.Items.Add(new ListViewItem(message)); // Hiển thị tin nhắn lên ListView
                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhận dữ liệu: " + ex.Message);
            }
        }







        private void btnClose_Click(object sender, EventArgs e)
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes("quit\n");
                    ns.Write(data, 0, data.Length);
                    ns.Close();
                    tcpClient.Close();
                    tcpClient = null;
                    ns = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đóng kết nối: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Chưa có kết nối nào để đóng!");
            }
        }
    }
}
