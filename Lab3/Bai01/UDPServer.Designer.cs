namespace Lab3.Bai01
{
    partial class UDPServer
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
            txtPort = new TextBox();
            rtbMess = new RichTextBox();
            btnListen = new Button();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // txtPort
            // 
            txtPort.Location = new Point(162, 62);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(125, 27);
            txtPort.TabIndex = 0;
            // 
            // rtbMess
            // 
            rtbMess.Location = new Point(95, 189);
            rtbMess.Name = "rtbMess";
            rtbMess.Size = new Size(633, 230);
            rtbMess.TabIndex = 1;
            rtbMess.Text = "";
            // 
            // btnListen
            // 
            btnListen.Location = new Point(634, 60);
            btnListen.Name = "btnListen";
            btnListen.Size = new Size(94, 29);
            btnListen.TabIndex = 2;
            btnListen.Text = "Listen";
            btnListen.UseVisualStyleBackColor = true;
            btnListen.Click += btnListen_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(95, 141);
            label1.Name = "label1";
            label1.Size = new Size(137, 20);
            label1.TabIndex = 3;
            label1.Text = "Received messages";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(95, 69);
            label2.Name = "label2";
            label2.Size = new Size(35, 20);
            label2.TabIndex = 4;
            label2.Text = "Port";
            // 
            // UDPServer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnListen);
            Controls.Add(rtbMess);
            Controls.Add(txtPort);
            Name = "UDPServer";
            Text = "UDPServer";
            //FormClosing += UDPServer_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPort;
        private RichTextBox rtbMess;
        private Button btnListen;
        private Label label1;
        private Label label2;
    }
}