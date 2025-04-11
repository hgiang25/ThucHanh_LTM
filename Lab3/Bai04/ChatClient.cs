using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private bool isReceiving = false;

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                MessageBox.Show("Đã kết nối với server!");
                return;
            }

            string username = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập Username trước khi kết nối.");
                return;
            }

            btnConnect.Enabled = false;

            await Task.Run(() =>
            {
                try
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect("127.0.0.1", 8080);
                    ns = tcpClient.GetStream();

                    // Gửi username
                    byte[] usernameBytes = Encoding.UTF8.GetBytes(username + "\n");
                    ns.Write(usernameBytes, 0, usernameBytes.Length);

                    // Nhận phản hồi
                    byte[] responseBuffer = new byte[1024];
                    int bytes = ns.Read(responseBuffer, 0, responseBuffer.Length);
                    string response = Encoding.UTF8.GetString(responseBuffer, 0, bytes).Trim();

                    if (response == "USERNAME_EXISTS")
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("Username đã tồn tại. Vui lòng chọn tên khác.");
                            ns.Close();
                            tcpClient.Close();
                            ns = null;
                            tcpClient = null;
                        }));
                    }
                    else if (response == "OK")
                    {
                        isReceiving = true;
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("Kết nối thành công!");
                            btnSend.Enabled = true;
                            txtMessage.Enabled = true;
                        }));
                        Task.Run(() => ReceiveMessages());
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("Phản hồi không hợp lệ từ server.");
                            ns.Close();
                            tcpClient.Close();
                            ns = null;
                            tcpClient = null;
                        }));
                    }
                }
                catch
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show("Không thể kết nối với Server");
                    }));
                }
                finally
                {
                    this.Invoke(new Action(() => { btnConnect.Enabled = true; }));
                }
            });
        }




        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tcpClient != null && tcpClient.Connected && ns != null)
            {
                try
                {
                    string message = $"{txtUsername.Text}: {txtMessage.Text.Trim()}";
                    if (!string.IsNullOrEmpty(txtMessage.Text.Trim()))
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message + "\n");
                        ns.Write(data, 0, data.Length);
                        ns.Flush();

                        txtMessage.Clear(); // Chỉ xóa khi gửi thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi gửi dữ liệu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Chưa kết nối đến Server!");
            }
        }



        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            int bytesReceived;
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                while (isReceiving && tcpClient != null && tcpClient.Connected)
                {
                    bytesReceived = ns.Read(buffer, 0, buffer.Length);
                    if (bytesReceived <= 0) break;

                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    messageBuilder.Append(receivedMessage);

                    while (messageBuilder.ToString().Contains("\n"))
                    {
                        int newlineIndex = messageBuilder.ToString().IndexOf("\n");
                        string message = messageBuilder.ToString().Substring(0, newlineIndex).Trim();
                        messageBuilder.Remove(0, newlineIndex + 1);

                        // Bỏ qua phản hồi OK hoặc xác thực
                        if (message == "OK" || message == "USERNAME_EXISTS") continue;

                        if (message == "SERVER_CLOSING")
                        {
                            isReceiving = false;
                            ns.Close();
                            tcpClient.Close();
                            ns = null;
                            tcpClient = null;
                            return;
                        }

                        // Hiển thị tin nhắn bình thường
                        lvMessages.Invoke(new Action(() =>
                        {
                            lvMessages.Items.Add(new ListViewItem(message));
                        }));
                    }
                }
            }
            catch
            {
                // Có thể ghi log lỗi nếu cần
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                try
                {
                    isReceiving = false; // Ngừng vòng lặp nhận tin nhắn

                    byte[] data = Encoding.UTF8.GetBytes("quit\n");
                    ns.Write(data, 0, data.Length);

                    ns.Close();
                    tcpClient.Close();

                    ns = null;
                    tcpClient = null;

                    MessageBox.Show("Đã đóng kết nối.");
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

        private void TCPClient_Load(object sender, EventArgs e)
        {
            //btnSend.Enabled = false;
            //txtMessage.Enabled = false;
        }
    }
}
