using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab3.Bai01;
using Lab3.Bai02;
using Lab3.Bai03;

namespace Lab3
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TCPServer form = new TCPServer();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Lab03_Bai03 lab03_Bai03 = new Lab03_Bai03();    
            lab03_Bai03.Show();
        }
    }
}
