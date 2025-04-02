namespace Lab3.Bai03
{
    partial class TCPClient
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
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.AutoSize = true;
            btnSend.Location = new Point(274, 114);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(114, 111);
            btnSend.TabIndex = 0;
            btnSend.Text = "Send message";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(91, 114);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(114, 111);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(454, 114);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(113, 111);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // TCPClient
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(653, 325);
            Controls.Add(btnClose);
            Controls.Add(btnConnect);
            Controls.Add(btnSend);
            Name = "TCPClient";
            Text = "TCPClientForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private Button btnConnect;
        private Button btnClose;
    }
}