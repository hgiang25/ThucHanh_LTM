namespace Lab3.Bai04
{
    partial class ChatServer
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView lvMessage;
        private System.Windows.Forms.ListView lvClients;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Label lblClients;

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
            this.lvClients = new System.Windows.Forms.ListView();
            this.btnListen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblMessages = new System.Windows.Forms.Label();
            this.lblClients = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lvMessage - Hiển thị tin nhắn
            this.lvMessage.Location = new System.Drawing.Point(12, 30);
            this.lvMessage.Size = new System.Drawing.Size(460, 250);
            this.lvMessage.View = System.Windows.Forms.View.List;

            // lvClients - Hiển thị danh sách client
            this.lvClients.Location = new System.Drawing.Point(480, 30);
            this.lvClients.Size = new System.Drawing.Size(180, 250);
            this.lvClients.View = System.Windows.Forms.View.List;

            // btnListen - Bắt đầu server
            this.btnListen.Location = new System.Drawing.Point(12, 290);
            this.btnListen.Size = new System.Drawing.Size(75, 30);
            this.btnListen.Text = "Listen";
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);

            // btnClose - Đóng server
            this.btnClose.Location = new System.Drawing.Point(105, 290);
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // lblMessages
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(12, 10);
            this.lblMessages.Text = "Server Messages:";

            // lblClients
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new System.Drawing.Point(480, 10);
            this.lblClients.Text = "Connected Clients:";

            // ChatServer Form
            this.ClientSize = new System.Drawing.Size(680, 340);
            this.Controls.Add(this.lvMessage);
            this.Controls.Add(this.lvClients);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.lblClients);
            this.Text = "Chat Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatServer_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
