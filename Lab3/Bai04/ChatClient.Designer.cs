namespace Lab3.Bai04
{
    partial class ChatClient
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView lvMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblMessages;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lvMessage = new System.Windows.Forms.ListView();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblMessages = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lvMessage - Hiển thị tin nhắn
            this.lvMessage.Location = new System.Drawing.Point(12, 30);
            this.lvMessage.Size = new System.Drawing.Size(460, 250);
            this.lvMessage.View = System.Windows.Forms.View.List;

            // txtMessage - Nhập tin nhắn
            this.txtMessage.Location = new System.Drawing.Point(12, 290);
            this.txtMessage.Size = new System.Drawing.Size(370, 22);

            // btnSend - Gửi tin nhắn
            this.btnSend.Location = new System.Drawing.Point(390, 288);
            this.btnSend.Size = new System.Drawing.Size(75, 25);
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // btnConnect - Kết nối đến server
            this.btnConnect.Location = new System.Drawing.Point(12, 320);
            this.btnConnect.Size = new System.Drawing.Size(75, 30);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // btnClose - Đóng kết nối
            this.btnClose.Location = new System.Drawing.Point(105, 320);
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // lblMessages
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(12, 10);
            this.lblMessages.Text = "Chat Messages:";

            // ChatClient Form
            this.ClientSize = new System.Drawing.Size(500, 370);
            this.Controls.Add(this.lvMessage);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblMessages);
            this.Text = "Chat Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatClient_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
