using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3.Bai01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            UDPClient form = new UDPClient();
            form.Show();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            UDPServer form = new UDPServer();
            form.Show();
        }
    }
}
