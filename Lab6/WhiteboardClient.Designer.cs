namespace Lab6
{
    partial class WhiteboardClient
    {
        private Button btnConnect;
        private PictureBox canvas;
        private Button btnColor;
        private TrackBar penWidth;
        private Button btnEnd;
        private ColorDialog colorDialog1;

        private void InitializeComponent()
        {
            this.btnConnect = new Button();
            this.canvas = new PictureBox();
            this.btnColor = new Button();
            this.penWidth = new TrackBar();
            this.btnEnd = new Button();
            this.colorDialog1 = new ColorDialog();

            this.SuspendLayout();

            // btnConnect
            this.btnConnect.Location = new Point(10, 10);
            this.btnConnect.Size = new Size(75, 23);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new EventHandler(this.btnConnect_Click);

            // canvas
            this.canvas.Location = new Point(10, 40);
            this.canvas.Size = new Size(800, 600);
            this.canvas.BackColor = Color.White;
            this.canvas.MouseDown += new MouseEventHandler(this.canvas_MouseDown);
            this.canvas.MouseMove += new MouseEventHandler(this.canvas_MouseMove);
            this.canvas.MouseUp += new MouseEventHandler(this.canvas_MouseUp);

            // btnColor
            this.btnColor.Location = new Point(90, 10);
            this.btnColor.Size = new Size(75, 23);
            this.btnColor.Text = "Color";
            this.btnColor.Click += new EventHandler(this.btnColor_Click);

            // penWidth
            this.penWidth.Location = new Point(170, 10);
            this.penWidth.Minimum = 1;
            this.penWidth.Maximum = 10;
            this.penWidth.Value = 2;
            this.penWidth.Scroll += new EventHandler(this.penWidth_Scroll);

            // btnEnd
            this.btnEnd.Location = new Point(250, 10);
            this.btnEnd.Size = new Size(75, 23);
            this.btnEnd.Text = "End";
            this.btnEnd.Click += new EventHandler(this.btnEnd_Click);

            // Form
            this.ClientSize = new Size(830, 660);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.btnColor);
            this.Controls.Add(this.penWidth);
            this.Controls.Add(this.btnEnd);
            this.Text = "Whiteboard Client";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}