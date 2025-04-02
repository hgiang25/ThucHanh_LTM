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
            btnListen = new Button();
            lvMessage = new ListView();
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
            // TCPServer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lvMessage);
            Controls.Add(btnListen);
            Name = "TCPServer";
            Text = "TCPServer";
            ResumeLayout(false);
        }

        #endregion

        private Button btnListen;
        private ListView lvMessage;
    }
}