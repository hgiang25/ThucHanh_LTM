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

namespace Lab3.Bai03
{
    public partial class TCPClient : Form
    {
        public TCPClient()
        {
            InitializeComponent();
        }

        private TcpClient tcpClient = null;
        private NetworkStream ns;
        

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tcpClient != null && tcpClient.Connected && ns != null)
            {
                try
                {
                    string message = "Hello server\n";
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    ns.Write(data, 0, data.Length);                    
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

                    // Gửi thông điệp kiểm tra
                    byte[] checkMsg = Encoding.UTF8.GetBytes("HELLO\n");
                    ns.Write(checkMsg, 0, checkMsg.Length);

                    // Đọc phản hồi từ server
                    byte[] buffer = new byte[1024];
                    ns.ReadTimeout = 2000; // 2 giây timeout
                    int bytesRead = ns.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                    if (response == "OK") // Giả sử server phản hồi "OK" nếu chấp nhận
                    {
                        MessageBox.Show("✅ Kết nối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        throw new Exception("Server từ chối kết nối.");
                    }
                }
                else
                {
                    MessageBox.Show("⚠️ Đã kết nối với server!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("🚫 Lỗi khi kiểm tra kết nối: " + ex.Message,
                                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Đóng lại kết nối nếu có lỗi
                if (tcpClient != null)
                {
                    try
                    {
                        tcpClient.Close();
                        tcpClient = null;
                        ns = null;
                    }
                    catch { }
                }
            }
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                try
                {
                    byte[] data = Encoding.ASCII.GetBytes("quit\n");
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
