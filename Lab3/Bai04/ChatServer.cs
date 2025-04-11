using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Lab3.Bai04
{
    public partial class ChatServer : Form
    {
        public ChatServer()
        {
            InitializeComponent();
        }

        private TcpListener tcpListener;
        private bool isServerRunning = false;
        private Thread listenerThread;
        private List<TcpClient> clientList = new List<TcpClient>();
        private int clientCounter = 0; // Biến để đếm số thứ tự client

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (isServerRunning)
            {
                MessageBox.Show("Server is already running.");
                return; // Dừng lại và không tiếp tục khởi động lại server
            }

            int port = 8080;
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            tcpListener.Start();
            isServerRunning = true;

            listenerThread = new Thread(ListenForClients);
            listenerThread.IsBackground = true;
            listenerThread.Start();
            lvMessage.Invoke(new Action(() =>
            {
                lvMessage.Items.Add(new ListViewItem("Server running on 127.0.0.1:" + port));
            }));
            MessageBox.Show("Server started on port " + port);
        }

        private void ListenForClients()
        {
            while (isServerRunning)
            {
                try
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    if (client.Connected)
                    {
                        clientList.Add(client);

                        // Lấy thông tin Host và Port của client
                        IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                        string clientHost = clientEndPoint.Address.ToString();
                        int clientPort = clientEndPoint.Port;

                        // Hiển thị thông tin client mới kết nối lên ListView
                        lvMessage.Invoke(new Action(() =>
                        {
                            lvMessage.Items.Add(new ListViewItem($"New client connected from: {clientHost}:{clientPort}"));
                        }));

                        clientCounter++; // Tăng số thứ tự client mỗi khi có client mới

                        Thread clientThread = new Thread(() => HandleClientComm(client));
                        clientThread.IsBackground = true;
                        clientThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    lvMessage.Invoke(new Action(() =>
                    {
                        lvMessage.Items.Add(new ListViewItem($"Error while accepting client: {ex.Message}"));
                    }));
                    break;
                }
            }
        }

        private void BroadcastMessage(string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message + "\n"); // Đảm bảo có ký tự newline ở cuối, nhưng không dư thừa

            foreach (var client in clientList)
            {
                if (client.Connected)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(messageBytes, 0, messageBytes.Length);
                    }
                    catch
                    {
                        continue; // Nếu gặp lỗi khi gửi tin nhắn, có thể bỏ qua client này
                    }
                }
            }
        }


        private void HandleClientComm(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                while (client.Connected)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Nếu không còn dữ liệu thì kết thúc

                    messageBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

                    // Nếu nhận được một tin nhắn hoàn chỉnh (kết thúc bằng \n)
                    if (messageBuilder.ToString().Contains("\n"))
                    {
                        string completeMessage = messageBuilder.ToString().Trim(); // Xóa dấu \n ở cuối tin nhắn
                        messageBuilder.Clear(); // Xóa StringBuilder để nhận tin nhắn mới

                        // Hiển thị tin nhắn nhận được vào ListView
                        lvMessage.Invoke(new Action(() =>
                        {
                            lvMessage.Items.Add(new ListViewItem($"{client.Client.RemoteEndPoint}: {completeMessage}"));
                        }));

                        // Phát lại tin nhắn đến tất cả các client
                        BroadcastMessage(completeMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nhận dữ liệu: " + ex.Message);
            }
            finally
            {
                client.Close();
                clientList.Remove(client);
            }
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            isServerRunning = false;
            tcpListener?.Stop();

            foreach (var client in clientList)
            {
                client.Close();
            }

            clientList.Clear();
            MessageBox.Show("Server stopped");
        }
    }
}