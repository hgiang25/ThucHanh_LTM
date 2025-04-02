using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3.Bai01
{
    public partial class UDPClient : Form
    {
        public UDPClient()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int port;
            try
            {
                string host = txtHost.Text.Trim();
                if (string.IsNullOrWhiteSpace(host))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ host hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtPort.Text, out port) || port < 1 || port > 65535)
                {
                    MessageBox.Show("Vui lòng nhập một số hợp lệ cho cổng (1 - 65535).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (UdpClient udpClient = new UdpClient())
                {
                    //udpClient.Connect(txtHost.Text,port);
                    Byte[] sendBytes = Encoding.ASCII.GetBytes("Hello UITer!");
                    Byte[] message = Encoding.ASCII.GetBytes(rtbMess.Text);

                    udpClient.Send(sendBytes, sendBytes.Length, host, port);
                    udpClient.Send(message, message.Length, host, port);

                    MessageBox.Show("Gửi tin nhắn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Định dạng số cổng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
