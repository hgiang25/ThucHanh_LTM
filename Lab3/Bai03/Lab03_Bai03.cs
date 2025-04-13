using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab3.Bai02;

namespace Lab3.Bai03
{
    public partial class Lab03_Bai03 : Form
    {
        public Lab03_Bai03()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Server server = new Server();
            server.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TCPClient form = new TCPClient();
            form.Show();
        }
    }
}
