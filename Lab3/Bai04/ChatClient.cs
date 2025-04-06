using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Lab3.Bai04
{
    public partial class ChatClient : Form
    {
        private TcpClient tcpClient = null;
        private NetworkStream ns;
        private Thread listenThread;
        private bool isConnected = false;

        public ChatClient()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isConnected)
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 8080);
                    ns = tcpClient.GetStream();

                    isConnected = true;
                    MessageBox.Show("Kết nối thành công!");

                    listenThread = new Thread(ListenForMessages);
                    listenThread.IsBackground = true;
                    listenThread.Start();
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

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (isConnected && ns != null)
            {
                try
                {
                    string message = txtMessage.Text.Trim();
                    if (string.IsNullOrEmpty(message)) return;

                    byte[] data = Encoding.UTF8.GetBytes(message + "\n");
                    ns.Write(data, 0, data.Length);
                    AppendMessage("Me: " + message);
                    txtMessage.Clear();
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

        private void ListenForMessages()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while (isConnected && (bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AppendMessage("Server: " + message);
                }
            }
            catch
            {
                AppendMessage("Mất kết nối đến server.");
            }
        }

        private void AppendMessage(string message)
        {
            if (lvMessage.InvokeRequired)
            {
                lvMessage.Invoke(new Action(() =>
                {
                    lvMessage.Items.Add(new ListViewItem(message));
                    lvMessage.EnsureVisible(lvMessage.Items.Count - 1);
                }));
            }
            else
            {
                lvMessage.Items.Add(new ListViewItem(message));
                lvMessage.EnsureVisible(lvMessage.Items.Count - 1);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                isConnected = false;
                listenThread?.Abort();

                ns?.Close();
                tcpClient?.Close();

                ns = null;
                tcpClient = null;
                MessageBox.Show("Đã ngắt kết nối!");
            }
            else
            {
                MessageBox.Show("Chưa có kết nối nào để đóng!");
            }
        }

        private void ChatClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnClose_Click(sender, e);
        }
    }
}
