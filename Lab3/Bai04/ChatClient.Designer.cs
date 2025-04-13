//namespace Lab3.Bai04
//{
//    partial class ChatClient
//    {
//        private System.ComponentModel.IContainer components = null;
//        private System.Windows.Forms.ListView lvMessage;
//        private System.Windows.Forms.TextBox txtMessage;
//        private System.Windows.Forms.Button btnSend;
//        private System.Windows.Forms.Button btnConnect;
//        private System.Windows.Forms.Button btnClose;
//        private System.Windows.Forms.Label lblMessages;

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private void InitializeComponent()
//        {
//            this.lvMessage = new System.Windows.Forms.ListView();
//            this.txtMessage = new System.Windows.Forms.TextBox();
//            this.btnSend = new System.Windows.Forms.Button();
//            this.btnConnect = new System.Windows.Forms.Button();
//            this.btnClose = new System.Windows.Forms.Button();
//            this.lblMessages = new System.Windows.Forms.Label();
//            this.SuspendLayout();

//            // lvMessage - Hiển thị tin nhắn
//            this.lvMessage.Location = new System.Drawing.Point(12, 30);
//            this.lvMessage.Size = new System.Drawing.Size(460, 250);
//            this.lvMessage.View = System.Windows.Forms.View.List;

//            // txtMessage - Nhập tin nhắn
//            this.txtMessage.Location = new System.Drawing.Point(12, 290);
//            this.txtMessage.Size = new System.Drawing.Size(370, 22);

//            // btnSend - Gửi tin nhắn
//            this.btnSend.Location = new System.Drawing.Point(390, 288);
//            this.btnSend.Size = new System.Drawing.Size(75, 25);
//            this.btnSend.Text = "Send";
//            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

//            // btnConnect - Kết nối đến server
//            this.btnConnect.Location = new System.Drawing.Point(12, 320);
//            this.btnConnect.Size = new System.Drawing.Size(75, 30);
//            this.btnConnect.Text = "Connect";
//            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

//            // btnClose - Đóng kết nối
//            this.btnClose.Location = new System.Drawing.Point(105, 320);
//            this.btnClose.Size = new System.Drawing.Size(75, 30);
//            this.btnClose.Text = "Close";
//            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

//            // lblMessages
//            this.lblMessages.AutoSize = true;
//            this.lblMessages.Location = new System.Drawing.Point(12, 10);
//            this.lblMessages.Text = "Chat Messages:";

//            // ChatClient Form
//            this.ClientSize = new System.Drawing.Size(500, 370);
//            this.Controls.Add(this.lvMessage);
//            this.Controls.Add(this.txtMessage);
//            this.Controls.Add(this.btnSend);
//            this.Controls.Add(this.btnConnect);
//            this.Controls.Add(this.btnClose);
//            this.Controls.Add(this.lblMessages);
//            this.Text = "Chat Client";
//            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatClient_FormClosing);
//            this.ResumeLayout(false);
//            this.PerformLayout();
//        }
//    }
//}


using System.Windows.Forms;

namespace Lab3.Bai04
{
    partial class ChatClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSend = new Button();
            btnConnect = new Button();
            btnClose = new Button();
            txtUsername = new TextBox();
            txtMessage = new TextBox();
            lvMessages = new ListView();
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.AutoSize = true;
            btnSend.Location = new Point(509, 302);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(129, 50);
            btnSend.TabIndex = 0;
            btnSend.Text = "Send message";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(293, 255);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(112, 31);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(546, 255);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(92, 31);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(46, 259);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(125, 27);
            txtUsername.TabIndex = 4;
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(46, 314);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(412, 27);
            txtMessage.TabIndex = 5;
            // 
            // lvMessages
            // 
            lvMessages.Location = new Point(46, 30);
            lvMessages.Name = "lvMessages";
            lvMessages.Size = new Size(641, 156);
            lvMessages.TabIndex = 6;
            lvMessages.UseCompatibleStateImageBehavior = false;
            lvMessages.View = View.List;
            // 
            // ChatClient
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(699, 372);
            Controls.Add(lvMessages);
            Controls.Add(txtMessage);
            Controls.Add(txtUsername);
            Controls.Add(btnClose);
            Controls.Add(btnConnect);
            Controls.Add(btnSend);
            Name = "ChatClient";
            Text = "TCPClientForm";
            FormClosing += ChatClient_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private Button btnConnect;
        private Button btnClose;
        private TextBox txtUsername;
        private TextBox txtMessage;
        private ListView lvMessages;
    }
}