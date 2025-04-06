using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3.Bai04
{
    public partial class ChatRoom : Form
    {
        public ChatRoom()
        {
            InitializeComponent();
        }

        private ChatServer chatServer = null; // Biến để lưu instance của ChatServer
        private void button1_Click(object sender, EventArgs e)
        {
            if (chatServer == null || chatServer.IsDisposed) // Kiểm tra nếu instance không tồn tại hoặc đã bị đóng
            {
                chatServer = new ChatServer();
                chatServer.Show();
            }
            else
            {
                MessageBox.Show("Chat server is already running.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChatClient form = new ChatClient();
            form.Show();
        }
    }
}
