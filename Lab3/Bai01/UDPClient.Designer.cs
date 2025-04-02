namespace Lab3.Bai01
{
    partial class UDPClient
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
            txtHost = new TextBox();
            txtPort = new TextBox();
            rtbMess = new RichTextBox();
            btnSend = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // txtHost
            // 
            txtHost.Location = new Point(90, 77);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(372, 27);
            txtHost.TabIndex = 0;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(550, 77);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(191, 27);
            txtPort.TabIndex = 1;
            // 
            // rtbMess
            // 
            rtbMess.Location = new Point(90, 162);
            rtbMess.Name = "rtbMess";
            rtbMess.Size = new Size(651, 169);
            rtbMess.TabIndex = 2;
            rtbMess.Text = "";
            // 
            // btnSend
            // 
            btnSend.Location = new Point(90, 381);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 3;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(90, 139);
            label1.Name = "label1";
            label1.Size = new Size(67, 20);
            label1.TabIndex = 4;
            label1.Text = "Message";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(90, 54);
            label2.Name = "label2";
            label2.Size = new Size(112, 20);
            label2.TabIndex = 5;
            label2.Text = "IP Remote Host";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(550, 54);
            label3.Name = "label3";
            label3.Size = new Size(35, 20);
            label3.TabIndex = 6;
            label3.Text = "Port";
            // 
            // UDPClient
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSend);
            Controls.Add(rtbMess);
            Controls.Add(txtPort);
            Controls.Add(txtHost);
            Name = "UDPClient";
            Text = "UDPClient";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtHost;
        private TextBox txtPort;
        private RichTextBox rtbMess;
        private Button btnSend;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}