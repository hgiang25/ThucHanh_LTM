using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace Lab6
{
    public partial class WhiteboardServer : Form
    {
        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();
        private Thread listenThread;
        private bool isRunning = false;

        public WhiteboardServer()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, 9000);
            listener.Start();
            isRunning = true;

            listenThread = new Thread(() =>
            {
                while (isRunning)
                {
                    var client = listener.AcceptTcpClient();
                    lock (clients) clients.Add(client);
                    UpdateClientCount();
                    Thread thread = new Thread(() => HandleClient(client));
                    thread.Start();
                }
            });
            listenThread.IsBackground = true;
            listenThread.Start();

            lstLog.Items.Add("Server started...");
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                using var ns = client.GetStream();
                using var reader = new StreamReader(ns, Encoding.UTF8);
                while (isRunning && client.Connected)
                {
                    string json = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(json))
                        break;

                    var action = JsonSerializer.Deserialize<DrawAction>(json, new JsonSerializerOptions
                    {
                        Converters = { new JsonColorConverter() }
                    });

                    if (action != null)
                    {
                        Broadcast(action, client);
                    }
                }
            }
            catch (IOException) { /* client ngắt kết nối */ }
            catch (Exception ex)
            {
                // Có thể ghi log nếu cần:
                Invoke(new Action(() => lstLog.Items.Add("Client error: " + ex.Message)));
            }
            finally
            {
                lock (clients) clients.Remove(client);
                UpdateClientCount();
                client.Close();
                Invoke(new Action(() => lstLog.Items.Add("Client disconnected")));
            }
        }


        private void Broadcast(DrawAction action, TcpClient excludeClient)
        {
            lock (clients)
            {
                string json = JsonSerializer.Serialize(action);
                byte[] buffer = Encoding.UTF8.GetBytes(json + "\n");

                foreach (var c in clients)
                {
                    if (c == excludeClient || !c.Connected) continue;
                    try
                    {
                        NetworkStream ns = c.GetStream();
                        ns.Write(buffer, 0, buffer.Length);
                    }
                    catch { }
                }
            }
        }

        private void UpdateClientCount()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateClientCount));
            }
            else
            {
                lblClients.Text = "Clients: " + clients.Count;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isRunning = false;
            listener.Stop();
            lock (clients)
            {
                foreach (var c in clients)
                {
                    c.Close();
                }
                clients.Clear();
            }
            lstLog.Items.Add("Server stopped.");
            UpdateClientCount();
        }
    }
}