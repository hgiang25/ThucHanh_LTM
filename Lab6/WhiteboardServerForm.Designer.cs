namespace Lab6
{
    partial class WhiteboardServerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnOpenClient;
        private System.Windows.Forms.TextBox txtLog;

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
            btnOpenClient = new Button();
            txtLog = new TextBox();
            SuspendLayout();
            // 
            // btnOpenClient
            // 
            btnOpenClient.Location = new Point(15, 22);
            btnOpenClient.Margin = new Padding(3, 4, 3, 4);
            btnOpenClient.Name = "btnOpenClient";
            btnOpenClient.Size = new Size(130, 38);
            btnOpenClient.TabIndex = 1;
            btnOpenClient.Text = "Open New Client";
            btnOpenClient.UseVisualStyleBackColor = true;
            btnOpenClient.Click += btnOpenClient_Click;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(15, 68);
            txtLog.Margin = new Padding(3, 4, 3, 4);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(600, 436);
            txtLog.TabIndex = 2;
            // 
            // WhiteboardServerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(630, 562);
            Controls.Add(txtLog);
            Controls.Add(btnOpenClient);
            Margin = new Padding(3, 4, 3, 4);
            Name = "WhiteboardServerForm";
            Text = "Whiteboard Server - Clients connected: 0";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
