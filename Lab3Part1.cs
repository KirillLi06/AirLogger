using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Lab3Part1 : Form
    {
        private Bitmap? canvasBitmap;
        private bool isDrawing;
        private bool isPanning;
        private Point lastPoint;
        private Point shapeStartPoint;
        private Point currentPreviewPoint;
        private Point panStartPoint;
        private PointF panStartOffset;
        private readonly System.Windows.Forms.Timer fourierTimer = new System.Windows.Forms.Timer();
        private float phase;
        private float canvasZoom = 1f;
        private PointF canvasOffset = PointF.Empty;

        public Lab3Part1()
        {
            InitializeComponent();
            DetailedActionLogger.Attach(this);

            comboBoxTool.SelectedIndex = 0;
            comboBoxDashStyle.SelectedIndex = 0;
            comboBoxFourierType.SelectedIndex = 0;
            numericUpDownPenWidth.Value = 2;
            panelLineColor.BackColor = Color.DarkBlue;
            panelFillColor.BackColor = Color.LightGoldenrodYellow;
            panelBackColor.BackColor = Color.White;

            fourierTimer.Interval = 40;
            fourierTimer.Tick += (_, _) =>
            {
                phase += 0.18f;
                panelFourier.Invalidate();
            };

            CreateCanvasBitmap();
            ResetCanvasView();
            AppLogger.Log("ЛР3 Часть 1: форма редактора открыта.");
        }

        private void CreateCanvasBitmap()
        {
            int width = Math.Max(1, pictureBoxCanvas.ClientSize.Width);
            int height = Math.Max(1, pictureBoxCanvas.ClientSize.Height);

            Bitmap newBitmap = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(newBitmap);
            g.Clear(panelBackColor.BackColor);

            if (canvasBitmap != null)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(canvasBitmap, new Rectangle(0, 0, width, height));
                canvasBitmap.Dispose();
            }

            canvasBitmap = newBitmap;
            pictureBoxCanvas.Invalidate();
        }

        private void ResetCanvasView()
        {
            if (canvasBitmap == null || pictureBoxCanvas.ClientSize.Width <= 0 || pictureBoxCanvas.ClientSize.Height <= 0)
                return;

            float zoomX = pictureBoxCanvas.ClientSize.Width / (float)canvasBitmap.Width;
            float zoomY = pictureBoxCanvas.ClientSize.Height / (float)canvasBitmap.Height;
            canvasZoom = Math.Max(0.05f, Math.Min(zoomX, zoomY));

            float viewWidth = canvasBitmap.Width * canvasZoom;
            float viewHeight = canvasBitmap.Height * canvasZoom;
            canvasOffset = new PointF(
                (pictureBoxCanvas.ClientSize.Width - viewWidth) / 2f,
                (pictureBoxCanvas.ClientSize.Height - viewHeight) / 2f);

            pictureBoxCanvas.Invalidate();
        }

        private Pen CreatePen()
        {
            DashStyle dashStyle = DashStyle.Solid;
            switch (comboBoxDashStyle.SelectedIndex)
            {
                case 1: dashStyle = DashStyle.Dash; break;
                case 2: dashStyle = DashStyle.Dot; break;
                case 3: dashStyle = DashStyle.DashDot; break;
            }

            return new Pen(panelLineColor.BackColor, (float)numericUpDownPenWidth.Value)
            {
                DashStyle = dashStyle,
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };
        }

        private Point ToImagePoint(Point screenPoint)
        {
            int x = (int)Math.Round((screenPoint.X - canvasOffset.X) / canvasZoom);
            int y = (int)Math.Round((screenPoint.Y - canvasOffset.Y) / canvasZoom);
            if (canvasBitmap == null)
                return new Point(x, y);

            x = Math.Max(0, Math.Min(canvasBitmap.Width - 1, x));
            y = Math.Max(0, Math.Min(canvasBitmap.Height - 1, y));
            return new Point(x, y);
        }

        private bool IsPointInsideCanvas(Point screenPoint)
        {
            if (canvasBitmap == null)
                return false;

            RectangleF rect = new RectangleF(
                canvasOffset.X,
                canvasOffset.Y,
                canvasBitmap.Width * canvasZoom,
                canvasBitmap.Height * canvasZoom);
            return rect.Contains(screenPoint);
        }

        private void ZoomCanvasAt(Point location, float factor)
        {
            if (canvasBitmap == null)
                return;

            float oldZoom = canvasZoom;
            float newZoom = Math.Max(0.05f, Math.Min(20f, canvasZoom * factor));
            if (Math.Abs(newZoom - oldZoom) < 0.0001f)
                return;

            PointF imagePoint = new PointF(
                (location.X - canvasOffset.X) / oldZoom,
                (location.Y - canvasOffset.Y) / oldZoom);

            canvasZoom = newZoom;
            canvasOffset = new PointF(
                location.X - imagePoint.X * canvasZoom,
                location.Y - imagePoint.Y * canvasZoom);

            pictureBoxCanvas.Invalidate();
        }

        private void pictureBoxCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.Clear(Color.Gainsboro);

            if (canvasBitmap == null)
                return;

            e.Graphics.TranslateTransform(canvasOffset.X, canvasOffset.Y);
            e.Graphics.ScaleTransform(canvasZoom, canvasZoom);
            e.Graphics.DrawImage(canvasBitmap, 0, 0, canvasBitmap.Width, canvasBitmap.Height);

            if (isDrawing && comboBoxTool.SelectedIndex != 3)
            {
                using Pen previewPen = CreatePen();
                using SolidBrush previewBrush = new SolidBrush(Color.FromArgb(90, panelFillColor.BackColor));

                if (comboBoxTool.SelectedIndex == 0)
                    e.Graphics.DrawLine(previewPen, shapeStartPoint, currentPreviewPoint);
                else if (comboBoxTool.SelectedIndex == 1)
                    DrawPreviewShape(e.Graphics, previewPen, previewBrush, shapeStartPoint, currentPreviewPoint, true);
                else if (comboBoxTool.SelectedIndex == 2)
                    DrawPreviewShape(e.Graphics, previewPen, previewBrush, shapeStartPoint, currentPreviewPoint, false);
            }

            using Pen borderPen = new Pen(Color.DimGray, 1f / canvasZoom);
            e.Graphics.DrawRectangle(borderPen, 0, 0, canvasBitmap.Width - 1, canvasBitmap.Height - 1);
            e.Graphics.ResetTransform();
        }

        private void pictureBoxCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBoxCanvas.Focus();

            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
            {
                isPanning = true;
                panStartPoint = e.Location;
                panStartOffset = canvasOffset;
                pictureBoxCanvas.Cursor = Cursors.Hand;
                return;
            }

            if (canvasBitmap == null || e.Button != MouseButtons.Left || !IsPointInsideCanvas(e.Location))
                return;

            isDrawing = true;
            lastPoint = ToImagePoint(e.Location);
            shapeStartPoint = lastPoint;
            currentPreviewPoint = lastPoint;

            if (comboBoxTool.SelectedIndex == 3)
            {
                using Graphics g = Graphics.FromImage(canvasBitmap);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using SolidBrush brush = new SolidBrush(panelFillColor.BackColor);
                g.FillEllipse(brush, lastPoint.X - 4, lastPoint.Y - 4, 8, 8);
                pictureBoxCanvas.Invalidate();
            }
        }

        private void pictureBoxCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                canvasOffset = new PointF(
                    panStartOffset.X + (e.X - panStartPoint.X),
                    panStartOffset.Y + (e.Y - panStartPoint.Y));
                pictureBoxCanvas.Invalidate();
                return;
            }

            if (!isDrawing || canvasBitmap == null)
                return;

            Point currentPoint = ToImagePoint(e.Location);
            currentPreviewPoint = currentPoint;

            if (comboBoxTool.SelectedIndex == 3)
            {
                using Graphics g = Graphics.FromImage(canvasBitmap);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using SolidBrush brush = new SolidBrush(panelFillColor.BackColor);
                g.FillEllipse(brush, currentPoint.X - 4, currentPoint.Y - 4, 8, 8);
                lastPoint = currentPoint;
            }

            pictureBoxCanvas.Invalidate();
        }

        private void DrawPreviewShape(Graphics g, Pen pen, Brush brush, Point p1, Point p2, bool ellipse)
        {
            Rectangle rect = NormalizeRect(p1, p2);
            bool fill = checkBoxFill.Checked;

            if (fill)
            {
                if (ellipse)
                    g.FillEllipse(brush, rect);
                else
                    g.FillRectangle(brush, rect);
            }

            if (ellipse)
                g.DrawEllipse(pen, rect);
            else
                g.DrawRectangle(pen, rect);
        }

        private Rectangle NormalizeRect(Point p1, Point p2)
        {
            int x = Math.Min(p1.X, p2.X);
            int y = Math.Min(p1.Y, p2.Y);
            int w = Math.Max(1, Math.Abs(p2.X - p1.X));
            int h = Math.Max(1, Math.Abs(p2.Y - p1.Y));
            return new Rectangle(x, y, w, h);
        }

        private void pictureBoxCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing && canvasBitmap != null && comboBoxTool.SelectedIndex != 3)
            {
                Point endPoint = ToImagePoint(e.Location);
                using Graphics g = Graphics.FromImage(canvasBitmap);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using Pen pen = CreatePen();
                using SolidBrush brush = new SolidBrush(panelFillColor.BackColor);

                switch (comboBoxTool.SelectedIndex)
                {
                    case 0:
                        g.DrawLine(pen, shapeStartPoint, endPoint);
                        break;
                    case 1:
                        DrawPreviewShape(g, pen, brush, shapeStartPoint, endPoint, true);
                        break;
                    case 2:
                        DrawPreviewShape(g, pen, brush, shapeStartPoint, endPoint, false);
                        break;
                }
            }

            isDrawing = false;
            isPanning = false;
            pictureBoxCanvas.Cursor = Cursors.Default;
            pictureBoxCanvas.Invalidate();
        }

        private void pictureBoxCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            ZoomCanvasAt(e.Location, e.Delta > 0 ? 1.15f : 1f / 1.15f);
        }

        private void buttonResetView_Click(object sender, EventArgs e)
        {
            ResetCanvasView();
            AppLogger.Log("ЛР3 Часть 1: масштаб и позиция холста сброшены.");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (canvasBitmap == null) return;
            using Graphics g = Graphics.FromImage(canvasBitmap);
            g.Clear(panelBackColor.BackColor);
            pictureBoxCanvas.Invalidate();
            AppLogger.Log("ЛР3 Часть 1: холст очищен.");
        }

        private void buttonLineColor_Click(object sender, EventArgs e)
        {
            using ColorDialog dialog = new ColorDialog();
            dialog.Color = panelLineColor.BackColor;
            if (dialog.ShowDialog() == DialogResult.OK)
                panelLineColor.BackColor = dialog.Color;
        }

        private void buttonFillColor_Click(object sender, EventArgs e)
        {
            using ColorDialog dialog = new ColorDialog();
            dialog.Color = panelFillColor.BackColor;
            if (dialog.ShowDialog() == DialogResult.OK)
                panelFillColor.BackColor = dialog.Color;
        }

        private void buttonBackColor_Click(object sender, EventArgs e)
        {
            using ColorDialog dialog = new ColorDialog();
            dialog.Color = panelBackColor.BackColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                panelBackColor.BackColor = dialog.Color;
                buttonClear_Click(sender, e);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (canvasBitmap == null) return;

            using SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp";
            dialog.FileName = "lab3_editor.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                canvasBitmap.Save(dialog.FileName);
                AppLogger.Log($"ЛР3 Часть 1: изображение сохранено ({dialog.FileName}).");
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using Bitmap loaded = new Bitmap(dialog.FileName);
                canvasBitmap?.Dispose();
                canvasBitmap = new Bitmap(loaded, Math.Max(1, pictureBoxCanvas.ClientSize.Width), Math.Max(1, pictureBoxCanvas.ClientSize.Height));
                ResetCanvasView();
                pictureBoxCanvas.Invalidate();
                AppLogger.Log($"ЛР3 Часть 1: изображение открыто ({dialog.FileName}).");
            }
        }

        private void pictureBoxCanvas_Resize(object sender, EventArgs e)
        {
            if (pictureBoxCanvas.Width > 0 && pictureBoxCanvas.Height > 0)
            {
                CreateCanvasBitmap();
                ResetCanvasView();
            }
        }

        private void buttonStartFourier_Click(object sender, EventArgs e)
        {
            fourierTimer.Enabled = !fourierTimer.Enabled;
            buttonStartFourier.Text = fourierTimer.Enabled ? "Стоп" : "Пуск";
            panelFourier.Invalidate();
        }

        private void panelFourier_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            int w = panelFourier.ClientSize.Width;
            int h = panelFourier.ClientSize.Height;
            if (w < 80 || h < 60)
                return;

            int left = 40;
            int right = w - 15;
            int top = 15;
            int bottom = h - 15;
            int midY = h / 2;
            float amplitude = Math.Max(20, (bottom - top) / 2f - 10f);

            using Pen axisPen = new Pen(Color.Gray, 1);
            using Pen wavePen = new Pen(Color.RoyalBlue, 2);
            using Font font = new Font("Segoe UI", 9);

            g.DrawLine(axisPen, left, midY, right, midY);
            g.DrawLine(axisPen, left, top, left, bottom);

            int harmonics = (int)numericUpDownHarmonics.Value;
            List<PointF> points = new List<PointF>();
            double width = Math.Max(1, right - left);

            for (int px = left; px < right; px++)
            {
                double t = (px - left) / width * Math.PI * 4 + phase;
                double y = 0;
                for (int n = 1; n <= harmonics; n++)
                {
                    double coeff = GetHarmonicCoefficient(n, comboBoxFourierType.SelectedIndex);
                    y += coeff * Math.Sin(n * t);
                }

                points.Add(new PointF(px, midY - (float)(y * amplitude * 0.75)));
            }

            if (points.Count > 1)
                g.DrawLines(wavePen, points.ToArray());

            g.DrawString("Сигнал Фурье", font, Brushes.Black, 45, 12);
            g.DrawString($"Гармоник: {harmonics}", font, Brushes.DimGray, right - 110, 12);
        }

        private double GetHarmonicCoefficient(int n, int typeIndex)
        {
            return typeIndex switch
            {
                0 => n % 2 == 1 ? 4.0 / (Math.PI * n) : 0.0,
                1 => n % 2 == 1 ? (8.0 / (Math.PI * Math.PI)) * (((n - 1) / 2) % 2 == 0 ? 1 : -1) / (n * n) : 0.0,
                _ => 2.0 / Math.PI * (((n + 1) % 2 == 0) ? 1 : -1) / n,
            };
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            fourierTimer.Stop();
            canvasBitmap?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
