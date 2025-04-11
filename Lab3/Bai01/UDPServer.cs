using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;


namespace Lab3.Bai01
{
    public partial class UDPServer : Form
    {
        delegate void InfoMessageDel(String info);
        private Thread serverThread;
        private UdpClient udpClient;

        public UDPServer()
        {
        InitializeComponent();
        }

        private bool IsPortInUse(int port)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var udpListeners = ipGlobalProperties.GetActiveUdpListeners();
            return udpListeners.Any(p => p.Port == port);
        }

        public void InfoMessage(string message)
        {
            if (rtbMess.InvokeRequired)
            {
                rtbMess.Invoke(new Action(() => rtbMess.AppendText(message + Environment.NewLine)));
            }
            else
            {
                rtbMess.AppendText(message + Environment.NewLine);
            }           
        }
        public void ServerThread()
        {
            try
            {
                udpClient = new UdpClient(Convert.ToInt32(txtPort.Text));                
                while (true)
                {
                    IPEndPoint RemoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] receivedBytes = udpClient.Receive(ref RemoteIPEndPoint);
                    string returnData = Encoding.UTF8.GetString(receivedBytes);
                    string mess = RemoteIPEndPoint.Address.ToString() + ":" +
                        returnData.ToString();
                    InfoMessage(mess);
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Lỗi:" + ex.Message);
            }                       
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            int port;
            if (!int.TryParse(txtPort.Text, out port) || port < 1024 || port > 65535)
            {
                MessageBox.Show("Vui lòng nhập một số cổng hợp lệ (1024 - 65535).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra nếu cổng đã được dùng
            if (IsPortInUse(port))
            {
                MessageBox.Show($"Cổng {port} đang được sử dụng bởi một ứng dụng khác. Vui lòng chọn cổng khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra nếu server đã chạy
            if (serverThread != null && serverThread.IsAlive)
            {
                MessageBox.Show("Server đang hoạt động!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            serverThread = new Thread(new ThreadStart(ServerThread));
            serverThread.IsBackground = true;
            serverThread.Start();
            MessageBox.Show($"Server đang lắng nghe trên cổng {port}...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //private void UDPServer_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    try
        //    {
        //        if (serverThread != null && serverThread.IsAlive)
        //        {
        //            serverThread.Abort(); // Hoặc dùng cancellation token nếu muốn an toàn hơn
        //        }

        //        if (udpClient != null)
        //        {
        //            udpClient.Close();
        //            udpClient = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi đóng server: " + ex.Message);
        //    }
        //}

    }
}
