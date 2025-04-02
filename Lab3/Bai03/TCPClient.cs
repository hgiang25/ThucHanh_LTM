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
        private TcpClient tcpClient = null;
        private NetworkStream ns;
        public TCPClient()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {                                 
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("Hello server\n");
            ns.Write(data, 0, data.Length);
            //tcpClient.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            tcpClient = new TcpClient();
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPonit = new IPEndPoint(ipAddress, 8080);
            tcpClient.Connect(ipEndPonit);
            ns = tcpClient.GetStream();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("quit\n");
            ns.Write(data);
            ns.Close();
            tcpClient.Close();
        }
    }
}
