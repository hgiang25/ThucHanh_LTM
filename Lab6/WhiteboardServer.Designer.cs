namespace Lab6
{
    partial class WhiteboardServer
    {
        private Button btnStart;
        private Button btnStop;
        private ListBox lstLog;
        private Label lblClients;

        private void InitializeComponent()
        {
            this.btnStart = new Button();
            this.btnStop = new Button();
            this.lstLog = new ListBox();
            this.lblClients = new Label();
            this.SuspendLayout();

            // btnStart
            this.btnStart.Location = new Point(12, 12);
            this.btnStart.Size = new Size(75, 23);
            this.btnStart.Text = "Start";
            this.btnStart.Click += new EventHandler(this.btnStart_Click);

            // btnStop
            this.btnStop.Location = new Point(93, 12);
            this.btnStop.Size = new Size(75, 23);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new EventHandler(this.btnStop_Click);

            // lstLog
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Location = new Point(12, 41);
            this.lstLog.Size = new Size(360, 160);

            // lblClients
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new Point(174, 17);
            this.lblClients.Text = "Clients: 0";

            // Form
            this.ClientSize = new Size(384, 211);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.lblClients);
            this.Text = "Whiteboard Server";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}