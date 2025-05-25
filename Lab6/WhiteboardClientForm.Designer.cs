namespace Lab6
{
    partial class WhiteboardClientForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Label lblClientCount;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.NumericUpDown nudThickness;
        private System.Windows.Forms.Button btnInsertImage;

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
            pictureBox1 = new PictureBox();
            btnUndo = new Button();
            btnEnd = new Button();
            lblClientCount = new Label();
            btnColor = new Button();
            nudThickness = new NumericUpDown();
            btnInsertImage = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudThickness).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Location = new Point(12, 50);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(800, 750);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // btnUndo
            // 
            btnUndo.Location = new Point(820, 50);
            btnUndo.Margin = new Padding(3, 4, 3, 4);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(100, 38);
            btnUndo.TabIndex = 1;
            btnUndo.Text = "Undo";
            btnUndo.UseVisualStyleBackColor = true;
            btnUndo.Click += btnUndo_Click;
            // 
            // btnEnd
            // 
            btnEnd.Location = new Point(820, 725);
            btnEnd.Margin = new Padding(3, 4, 3, 4);
            btnEnd.Name = "btnEnd";
            btnEnd.Size = new Size(100, 38);
            btnEnd.TabIndex = 2;
            btnEnd.Text = "End";
            btnEnd.UseVisualStyleBackColor = true;
            btnEnd.Click += btnEnd_Click;
            // 
            // lblClientCount
            // 
            lblClientCount.AutoSize = true;
            lblClientCount.Location = new Point(12, 19);
            lblClientCount.Name = "lblClientCount";
            lblClientCount.Size = new Size(141, 20);
            lblClientCount.TabIndex = 3;
            lblClientCount.Text = "Clients connected: 0";
            // 
            // btnColor
            // 
            btnColor.Location = new Point(820, 125);
            btnColor.Margin = new Padding(3, 4, 3, 4);
            btnColor.Name = "btnColor";
            btnColor.Size = new Size(100, 38);
            btnColor.TabIndex = 4;
            btnColor.Text = "Choose Color";
            btnColor.UseVisualStyleBackColor = true;
            btnColor.Click += btnColor_Click;
            // 
            // nudThickness
            // 
            nudThickness.Location = new Point(820, 188);
            nudThickness.Margin = new Padding(3, 4, 3, 4);
            nudThickness.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudThickness.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudThickness.Name = "nudThickness";
            nudThickness.Size = new Size(100, 27);
            nudThickness.TabIndex = 5;
            nudThickness.Value = new decimal(new int[] { 2, 0, 0, 0 });
            nudThickness.ValueChanged += btnThickness_ValueChanged;
            // 
            // btnInsertImage
            // 
            btnInsertImage.Location = new Point(820, 250);
            btnInsertImage.Margin = new Padding(3, 4, 3, 4);
            btnInsertImage.Name = "btnInsertImage";
            btnInsertImage.Size = new Size(100, 38);
            btnInsertImage.TabIndex = 6;
            btnInsertImage.Text = "Insert Image";
            btnInsertImage.UseVisualStyleBackColor = true;
            btnInsertImage.Click += btnInsertImage_Click;
            // 
            // WhiteboardClientForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(940, 825);
            Controls.Add(btnInsertImage);
            Controls.Add(nudThickness);
            Controls.Add(btnColor);
            Controls.Add(lblClientCount);
            Controls.Add(btnEnd);
            Controls.Add(btnUndo);
            Controls.Add(pictureBox1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "WhiteboardClientForm";
            Text = "Whiteboard - Clients: 0";
            this.FormClosing += WhiteboardClientForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudThickness).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
