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
        private bool isRunning = false;

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
                isRunning = true;

                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                //InfoMessage($"Server đang lắng nghe tại cổng {((IPEndPoint)udpClient.Client.LocalEndPoint).Port}");

                while (isRunning)
                {
                    if (udpClient.Available > 0) // Kiểm tra có dữ liệu trước khi nhận
                    {
                        byte[] receivedBytes = udpClient.Receive(ref remoteEndPoint);
                        string returnData = Encoding.UTF8.GetString(receivedBytes);
                        string mess = $"{remoteEndPoint.Address}:{returnData}";
                        InfoMessage(mess);
                    }
                    else
                    {
                        Thread.Sleep(100); // Tránh vòng lặp chạy liên tục gây tốn CPU
                    }
                }
            }
            catch (SocketException se)
            {
                InfoMessage($"Socket đã đóng: {se.Message}");
            }
            catch (Exception ex)
            {
                InfoMessage($"Lỗi: {ex.Message}");
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

            if (IsPortInUse(port))
            {
                MessageBox.Show($"Cổng {port} đang được sử dụng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (serverThread != null && serverThread.IsAlive)
            {
                MessageBox.Show("Server đang lắng nghe!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            serverThread = new Thread(new ThreadStart(ServerThread));
            serverThread.IsBackground = true;
            serverThread.Start();

            MessageBox.Show($"Server đang lắng nghe trên cổng {port}...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void UDPServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                isRunning = false;

                if (udpClient != null)
                {
                    udpClient.Close(); // Giải phóng socket để break khỏi Receive()
                    udpClient = null;
                }

                if (serverThread != null && serverThread.IsAlive)
                {
                    serverThread.Join(500); // Chờ thread dừng trong tối đa 0.5s
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đóng server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
