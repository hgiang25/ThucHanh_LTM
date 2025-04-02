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

namespace Lab3.Bai01
{
    public partial class UDPServer : Form
    {
        delegate void InfoMessageDel(String info);
        private Thread serverThread;
        public UDPServer()
        {
        InitializeComponent();
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
                UdpClient udpClient = new UdpClient(Convert.ToInt32(txtPort.Text));                
                while (true)
                {
                    IPEndPoint RemoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] receivedBytes = udpClient.Receive(ref RemoteIPEndPoint);
                    string returnData = Encoding.ASCII.GetString(receivedBytes);
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
            // Kiểm tra nếu server đã chạy
            if (serverThread != null && serverThread.IsAlive)
            {
                MessageBox.Show("Server đang hoạt động!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //Thread thrUDPServer = new Thread(new ThreadStart(ServerThread));
            //thrUDPServer.IsBackground = true;
            //thrUDPServer.Start();

            // Gán cho biến serverThread
            serverThread = new Thread(new ThreadStart(ServerThread)); 
            serverThread.IsBackground = true;
            serverThread.Start();
            MessageBox.Show($"Server đang lắng nghe trên cổng " +
                $"{port}...", "Thông báo", MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }
    }
}
