namespace SimplePaint
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Windows.Forms;
    using System.IO;

    public partial class Form1 : Form
    {
        enum ToolType { Line, Rectangle, Circle } // 사용할 도형 타입
        private Bitmap canvasBitmap; // 실제 그림이 저장되는 비트맵
        private Graphics canvasGraphics; // 비트맵 위에 그리기 위한객체
        private bool isDrawing = false; // 현재 드래그 중인지 여부
        private Point startPoint; // 드래그 시작점 (control coords)
        private Point endPoint; // 드래그 끝점 (control coords)
        private Point startPointImage; // 드래그 시작점 (image/bitmap coords)
        private Point endPointImage; // 드래그 끝점 (image/bitmap coords)
        private ToolType currentTool = ToolType.Line; // 현재 선택된 도형
        private Color currentColor = Color.Black; // 현재 색상
        private int currentLineWidth = 2; // 현재 선 두께
        private float currentZoom = 1.0f; // 확대/축소 배율



        public Form1()
        {
            InitializeComponent();

            // 캔버스 초기화
            canvasBitmap = new Bitmap(picCanvas.Width, picCanvas.Height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.Clear(Color.White); // 캔버스를 흰색으로 초기화

            // We'll handle painting the bitmap scaled in the Paint event
            picCanvas.Image = null;
            picCanvas.SizeMode = PictureBoxSizeMode.Normal;
            picCanvas.Width = (int)(canvasBitmap.Width * currentZoom);
            picCanvas.Height = (int)(canvasBitmap.Height * currentZoom);
            pnlCanvas.AutoScrollMinSize = new Size(picCanvas.Width, picCanvas.Height);

            // 마우스 이벤트 연결
            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.MouseMove += PicCanvas_MouseMove;
            picCanvas.MouseUp += PicCanvas_MouseUp;

            // put picturebox inside scrollable panel
            pnlCanvas.Controls.Add(picCanvas);

            // ensure mouse wheel events reach our handlers
            pnlCanvas.MouseWheel += Panel_MouseWheel;
            picCanvas.MouseWheel += PicCanvas_MouseWheel;
            pnlCanvas.MouseEnter += (s, e) => pnlCanvas.Focus();
            picCanvas.MouseEnter += (s, e) => picCanvas.Focus();

            // We will draw the image scaled inside Paint handler
            picCanvas.SizeMode = PictureBoxSizeMode.Normal;

            // open button
            btnOpenFile.Click += btnOpenFile_Click;

            // picCanvas가 다시 그려질 때 PicCanvas_Paint 함수를 실행하도록 연결
            picCanvas.Paint += PicCanvas_Paint;

            // 도형 선택 버튼 이벤트 연결
            btnLine.Click += btnLine_Click;
            btnRectangle.Click += btnRectangle_Click;
            btnCircle.Click += btnCircle_Click;

            // 색상 콤보박스 이벤트 연결
            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            cmbColor.SelectedIndex = 1; // 기본값: Black

            // 선 두께 트랙바 이벤트 연결
            trbLineWidth.Minimum = 1; // 최소값
            trbLineWidth.Maximum = 10; // 최대값
            trbLineWidth.Value = 5;
            trbLineWidth.ValueChanged += trbLineWidth_ValueChanged;

            // 저장 버튼 이벤트 연결
            btnSaveFile.Click += btnSaveFile_Click;

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "이미지 열기";
                ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp|All Files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // load image from file
                        Image img = Image.FromFile(ofd.FileName);

                        // dispose existing canvas bitmap/graphics
                        canvasGraphics?.Dispose();
                        canvasBitmap?.Dispose();

                        // create bitmap the same size as image
                        canvasBitmap = new Bitmap(img.Width, img.Height);
                        canvasGraphics = Graphics.FromImage(canvasBitmap);
                        canvasGraphics.Clear(Color.White);
                        canvasGraphics.DrawImage(img, 0, 0, img.Width, img.Height);

                        // set picturebox image and adjust sizes
                        picCanvas.Image = canvasBitmap;
                        currentZoom = 1.0f;
                        picCanvas.Width = (int)(canvasBitmap.Width * currentZoom);
                        picCanvas.Height = (int)(canvasBitmap.Height * currentZoom);
                        picCanvas.SizeMode = PictureBoxSizeMode.StretchImage;

                        // ensure panel scrollbars appear when needed
                        pnlCanvas.AutoScrollMinSize = new Size(picCanvas.Width, picCanvas.Height);

                        img.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"이미지 열기 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            // zoom with Ctrl+wheel
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                ZoomAt(e.Delta, Cursor.Position);
            }
        }

        private void PicCanvas_MouseWheel(object? sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
                ZoomAt(e.Delta, Cursor.Position);
        }

        private void Panel_MouseWheel(object? sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
                ZoomAt(e.Delta, Cursor.Position);
        }

        private void ZoomAt(int delta, Point screenCursor)
        {
            if (canvasBitmap == null) return;
            float oldZoom = currentZoom;
            if (delta > 0) currentZoom *= 1.1f;
            else currentZoom /= 1.1f;
            if (currentZoom <= 1e-8f) currentZoom = 1e-8f;

            // mouse position relative to panel
            Point mousePanel = pnlCanvas.PointToClient(screenCursor);
            int offsetX = -pnlCanvas.AutoScrollPosition.X;
            int offsetY = -pnlCanvas.AutoScrollPosition.Y;
            int mouseXInImage = mousePanel.X + offsetX;
            int mouseYInImage = mousePanel.Y + offsetY;

            // new size
            picCanvas.Width = (int)(canvasBitmap.Width * currentZoom);
            picCanvas.Height = (int)(canvasBitmap.Height * currentZoom);
            pnlCanvas.AutoScrollMinSize = new Size(picCanvas.Width, picCanvas.Height);

            // adjust scroll to keep mouse position over same image location
            float relX = (float)mouseXInImage / (canvasBitmap.Width * oldZoom);
            float relY = (float)mouseYInImage / (canvasBitmap.Height * oldZoom);
            int newScrollX = (int)(relX * picCanvas.Width) - mousePanel.X;
            int newScrollY = (int)(relY * picCanvas.Height) - mousePanel.Y;
            pnlCanvas.AutoScrollPosition = new Point(newScrollX, newScrollY);

            picCanvas.Invalidate();
        }

        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true; // 드래그 시작
            startPoint = e.Location; // 시작점 저장 (control coords)
            if (canvasBitmap != null)
            {
                int ix = (int)(e.X / currentZoom);
                int iy = (int)(e.Y / currentZoom);
                if (ix < 0) ix = 0; if (iy < 0) iy = 0;
                if (ix >= canvasBitmap.Width) ix = canvasBitmap.Width - 1;
                if (iy >= canvasBitmap.Height) iy = canvasBitmap.Height - 1;
                startPointImage = new Point(ix, iy);
            }
        }
        // 과제2
        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return; // 그림 그리기와 상관 없는 마우스 움직임은무시
            endPoint = e.Location; // 현재 위치 갱신 (control coords)
            if (canvasBitmap != null)
            {
                int ix = (int)(e.X / currentZoom);
                int iy = (int)(e.Y / currentZoom);
                if (ix < 0) ix = 0; if (iy < 0) iy = 0;
                if (ix >= canvasBitmap.Width) ix = canvasBitmap.Width - 1;
                if (iy >= canvasBitmap.Height) iy = canvasBitmap.Height - 1;
                endPointImage = new Point(ix, iy);
            }
                                   // picCanvas를 다시 그리라 (Paint 이벤트를 발생시킨다)
            picCanvas.Invalidate(); // 화면 다시 그리기 (미리보기)
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return; // 그림 그리기와 상관 없는 마우스 움직임은무시
            isDrawing = false; // 드래그 종료
            endPoint = e.Location;
            if (canvasBitmap != null)
            {
                int ix = (int)(e.X / currentZoom);
                int iy = (int)(e.Y / currentZoom);
                if (ix < 0) ix = 0; if (iy < 0) iy = 0;
                if (ix >= canvasBitmap.Width) ix = canvasBitmap.Width - 1;
                if (iy >= canvasBitmap.Height) iy = canvasBitmap.Height - 1;
                endPointImage = new Point(ix, iy);
            }
            // 실제 비트맵에 도형 그리기 (확정) - 이미지(비트맵) 좌표로 변환
            using (Pen pen = new Pen(currentColor, currentLineWidth))
            {
                DrawShape(canvasGraphics, pen, startPointImage, endPointImage);
            }
            // ensure drawing is flushed and PictureBox displays the updated bitmap
            canvasGraphics?.Flush();
            picCanvas.Image = canvasBitmap;
            picCanvas.Refresh();
            picCanvas.Invalidate();
        }// 다시 그려서 결과 반영, Paint 이벤트 발생}

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            // draw the canvasBitmap scaled by currentZoom
            if (canvasBitmap != null)
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                var destRect = new Rectangle(0, 0, (int)(canvasBitmap.Width * currentZoom), (int)(canvasBitmap.Height * currentZoom));
                e.Graphics.DrawImage(canvasBitmap, destRect, new Rectangle(0, 0, canvasBitmap.Width, canvasBitmap.Height), GraphicsUnit.Pixel);
            }

            if (!isDrawing) return;

            // Draw preview (dashed) scaled from image coords to control coords
            using (Pen previewPen = new Pen(currentColor, Math.Max(1f, currentLineWidth * currentZoom)))
            {
                previewPen.DashStyle = DashStyle.Dash;
                // convert image-space points to control-space for preview
                Point p1 = new Point((int)(startPointImage.X * currentZoom), (int)(startPointImage.Y * currentZoom));
                Point p2 = new Point((int)(endPointImage.X * currentZoom), (int)(endPointImage.Y * currentZoom));
                DrawShape(e.Graphics, previewPen, p1, p2);
            }
        }

        private void DrawShape(Graphics g, Pen pen, Point p1, Point p2)
        {
            Rectangle rect = GetRectangle(p1, p2);
            switch (currentTool)
            {
                case ToolType.Line:
                    g.DrawLine(pen, p1, p2);
                    break;
                case ToolType.Rectangle:
                    g.DrawRectangle(pen, rect);
                    break;
                case ToolType.Circle:
                    g.DrawEllipse(pen, rect);
                    break;
            }
        }

        private Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
            Math.Min(p1.X, p2.X),
            Math.Min(p1.Y, p2.Y),
            Math.Abs(p1.X - p2.X),
            Math.Abs(p1.Y - p2.Y)
            );
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            currentTool = ToolType.Line;
        }
        private void btnRectangle_Click(object sender, EventArgs e)
        {
            currentTool = ToolType.Rectangle;
        }
        private void btnCircle_Click(object sender, EventArgs e)
        {
            currentTool = ToolType.Circle;
        }

        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbColor.SelectedIndex)
            {
                case 0: // Black 검정
                    currentColor = Color.Black;
                    break;
                case 1: // Red 빨강
                    currentColor = Color.Red;
                    break;
                case 2: // Blue 파랑
                    currentColor = Color.Blue;
                    break;
                case 3: // Green 녹색
                    currentColor = Color.Green;
                    break;
                default:
                    currentColor = Color.Black;
                    break;
            }
        }

        private void trbLineWidth_ValueChanged(object sender, EventArgs e)
        {
            currentLineWidth = trbLineWidth.Value;
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            if (canvasBitmap == null)
            {
                MessageBox.Show("저장할 이미지가 없습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "이미지 저장";
                sfd.Filter = "PNG (*.png)|*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Bitmap (*.bmp)|*.bmp";
                sfd.FilterIndex = 1;
                sfd.AddExtension = true;
                sfd.DefaultExt = "png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string file = sfd.FileName;
                        string ext = Path.GetExtension(file).ToLowerInvariant();
                        ImageFormat fmt = ImageFormat.Png;
                        if (ext == ".jpg" || ext == ".jpeg") fmt = ImageFormat.Jpeg;
                        else if (ext == ".bmp") fmt = ImageFormat.Bmp;

                        canvasBitmap.Save(file, fmt);
                        MessageBox.Show("이미지를 저장했습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"저장 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


      
    }
}
