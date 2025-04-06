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
                    MessageBox.Show("Kết nối thành công!");
                }
                else
                {
                    MessageBox.Show("Đã kết nối với server!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message,"Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
