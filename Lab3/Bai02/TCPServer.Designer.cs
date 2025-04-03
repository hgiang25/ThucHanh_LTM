namespace Lab3.Bai02
{
    partial class TCPServer
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
            components = new System.ComponentModel.Container();
            btnListen = new Button();
            lvMessage = new ListView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            btnClose = new Button();
            SuspendLayout();
            // 
            // btnListen
            // 
            btnListen.Location = new Point(605, 52);
            btnListen.Name = "btnListen";
            btnListen.Size = new Size(94, 29);
            btnListen.TabIndex = 0;
            btnListen.Text = "Listen";
            btnListen.UseVisualStyleBackColor = true;
            btnListen.Click += btnListen_Click;
            // 
            // lvMessage
            // 
            lvMessage.Location = new Point(137, 123);
            lvMessage.Name = "lvMessage";
            lvMessage.Size = new Size(562, 264);
            lvMessage.TabIndex = 1;
            lvMessage.UseCompatibleStateImageBehavior = false;
            lvMessage.View = View.List;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // btnClose
            // 
            btnClose.Location = new Point(147, 51);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 29);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // TCPServer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnClose);
            Controls.Add(lvMessage);
            Controls.Add(btnListen);
            Name = "TCPServer";
            Text = "TCPServer";
            FormClosing += TCPServer_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private Button btnListen;
        private ListView lvMessage;
        private ContextMenuStrip contextMenuStrip1;
        private Button btnClose;
    }
}