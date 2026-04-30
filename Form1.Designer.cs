namespace SimplePaint
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblAppName = new Label();
            btnLine = new Button();
            btnCircle = new Button();
            btnRectangle = new Button();
            cmbColor = new ComboBox();
            grpShape = new GroupBox();
            grpColor = new GroupBox();
            grpWidth = new GroupBox();
            trbLineWidth = new TrackBar();
            btnSaveFile = new Button();
            btnOpenFile = new Button();
            picCanvas = new PictureBox();
            grpShape.SuspendLayout();
            grpColor.SuspendLayout();
            grpWidth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trbLineWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCanvas).BeginInit();
            SuspendLayout();
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("한컴 말랑말랑 Bold", 28F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblAppName.Location = new Point(31, 50);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(355, 72);
            lblAppName.TabIndex = 0;
            lblAppName.Text = "Simple Paint";
            // 
            // btnLine
            // 
            btnLine.Font = new Font("한컴 말랑말랑 Bold", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnLine.Image = Properties.Resources.jiksun;
            btnLine.ImageAlign = ContentAlignment.TopCenter;
            btnLine.Location = new Point(20, 47);
            btnLine.Name = "btnLine";
            btnLine.Size = new Size(112, 97);
            btnLine.TabIndex = 1;
            btnLine.Text = "직선";
            btnLine.TextAlign = ContentAlignment.BottomCenter;
            btnLine.UseVisualStyleBackColor = true;
            // 
            // btnCircle
            // 
            btnCircle.Font = new Font("한컴 말랑말랑 Bold", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnCircle.Image = Properties.Resources.one;
            btnCircle.ImageAlign = ContentAlignment.TopCenter;
            btnCircle.Location = new Point(303, 47);
            btnCircle.Name = "btnCircle";
            btnCircle.Size = new Size(112, 97);
            btnCircle.TabIndex = 2;
            btnCircle.Text = "원";
            btnCircle.TextAlign = ContentAlignment.BottomCenter;
            btnCircle.UseVisualStyleBackColor = true;
            // 
            // btnRectangle
            // 
            btnRectangle.Font = new Font("한컴 말랑말랑 Bold", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnRectangle.Image = Properties.Resources.square;
            btnRectangle.ImageAlign = ContentAlignment.TopCenter;
            btnRectangle.Location = new Point(162, 47);
            btnRectangle.Name = "btnRectangle";
            btnRectangle.Size = new Size(112, 97);
            btnRectangle.TabIndex = 3;
            btnRectangle.Text = "사각형";
            btnRectangle.TextAlign = ContentAlignment.BottomCenter;
            btnRectangle.UseVisualStyleBackColor = true;
            // 
            // cmbColor
            // 
            cmbColor.FormattingEnabled = true;
            cmbColor.Items.AddRange(new object[] { "Black 검정", "Red 빨강", "Blue 파랑", "Green 녹색" });
            cmbColor.Location = new Point(16, 75);
            cmbColor.Name = "cmbColor";
            cmbColor.Size = new Size(182, 33);
            cmbColor.TabIndex = 4;
            // 
            // grpShape
            // 
            grpShape.Controls.Add(btnRectangle);
            grpShape.Controls.Add(btnCircle);
            grpShape.Controls.Add(btnLine);
            grpShape.Location = new Point(23, 149);
            grpShape.Name = "grpShape";
            grpShape.Size = new Size(439, 167);
            grpShape.TabIndex = 5;
            grpShape.TabStop = false;
            grpShape.Text = "도형 선택";
            // 
            // grpColor
            // 
            grpColor.Controls.Add(cmbColor);
            grpColor.Location = new Point(508, 149);
            grpColor.Name = "grpColor";
            grpColor.Size = new Size(216, 167);
            grpColor.TabIndex = 6;
            grpColor.TabStop = false;
            grpColor.Text = "색 선택";
            // 
            // grpWidth
            // 
            grpWidth.Controls.Add(trbLineWidth);
            grpWidth.Location = new Point(770, 149);
            grpWidth.Name = "grpWidth";
            grpWidth.Size = new Size(216, 167);
            grpWidth.TabIndex = 7;
            grpWidth.TabStop = false;
            grpWidth.Text = "굵기 선택";
            // 
            // trbLineWidth
            // 
            trbLineWidth.Location = new Point(6, 75);
            trbLineWidth.Name = "trbLineWidth";
            trbLineWidth.Size = new Size(195, 69);
            trbLineWidth.TabIndex = 0;
            // 
            // btnSaveFile
            // 
            btnSaveFile.BackColor = SystemColors.ActiveBorder;
            btnSaveFile.Font = new Font("한컴 말랑말랑 Bold", 16F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnSaveFile.Location = new Point(1121, 224);
            btnSaveFile.Name = "btnSaveFile";
            btnSaveFile.Size = new Size(93, 92);
            btnSaveFile.TabIndex = 8;
            btnSaveFile.Text = "저장";
            btnSaveFile.UseVisualStyleBackColor = false;
            
            // 
            // btnOpenFile
            // 
            btnOpenFile.BackColor = Color.Gainsboro;
            btnOpenFile.Font = new Font("한컴 말랑말랑 Bold", 16F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnOpenFile.ForeColor = SystemColors.ControlText;
            btnOpenFile.Location = new Point(1012, 224);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(93, 92);
            btnOpenFile.TabIndex = 9;
            btnOpenFile.Text = "열기";
            btnOpenFile.UseVisualStyleBackColor = false;
            // 
            // picCanvas
            // 
            picCanvas.BackColor = Color.White;
            picCanvas.BorderStyle = BorderStyle.FixedSingle;
            picCanvas.Location = new Point(23, 342);
            picCanvas.Name = "picCanvas";
            picCanvas.Size = new Size(1191, 542);
            picCanvas.TabIndex = 10;
            picCanvas.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1250, 910);
            Controls.Add(picCanvas);
            Controls.Add(btnOpenFile);
            Controls.Add(btnSaveFile);
            Controls.Add(grpWidth);
            Controls.Add(grpColor);
            Controls.Add(grpShape);
            Controls.Add(lblAppName);
            Name = "Form1";
            Text = "Simple Paint v1.0";
            grpShape.ResumeLayout(false);
            grpColor.ResumeLayout(false);
            grpWidth.ResumeLayout(false);
            grpWidth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trbLineWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCanvas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblAppName;
        private Button btnLine;
        private Button btnCircle;
        private Button btnRectangle;
        private ComboBox cmbColor;
        private GroupBox grpShape;
        private GroupBox grpColor;
        private GroupBox grpWidth;
        private TrackBar trbLineWidth;
        private Button btnSaveFile;
        private Button btnOpenFile;
        private PictureBox picCanvas;
    }
}
