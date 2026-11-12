using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Lab3Part2 : Form
    {
        private bool isPanning;
        private float zoom = 1f;
        private PointF viewOffset = PointF.Empty;
        private Point panStartPoint;
        private PointF panStartOffset;

        public Lab3Part2()
        {
            InitializeComponent();
            DetailedActionLogger.Attach(this);
            comboBoxFractal.SelectedIndex = 0;
            numericUpDownDepth.Value = 6;
            ResetView();
            labelStatus.Text = "Векторная отрисовка";
            AppLogger.Log("ЛР3 Часть 2: форма фракталов открыта.");
        }

        private void ResetView()
        {
            zoom = 1f;
            viewOffset = PointF.Empty;
            panelCanvas.Invalidate();
        }

        private void ZoomAt(Point location, float factor)
        {
            float oldZoom = zoom;
            float newZoom = Math.Max(0.05f, Math.Min(20f, zoom * factor));
            if (Math.Abs(newZoom - oldZoom) < 0.0001f)
                return;

            PointF worldPoint = ScreenToWorld(location);
            zoom = newZoom;
            viewOffset = new PointF(location.X - worldPoint.X * zoom, location.Y - worldPoint.Y * zoom);
            panelCanvas.Invalidate();
        }

        private PointF ScreenToWorld(Point screenPoint)
        {
            return new PointF(
                (screenPoint.X - viewOffset.X) / zoom,
                (screenPoint.Y - viewOffset.Y) / zoom);
        }

        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.Gainsboro);

            using Matrix transform = new Matrix();
            transform.Translate(viewOffset.X, viewOffset.Y);
            transform.Scale(zoom, zoom);
            g.Transform = transform;

            g.Clear(Color.White);

            int width = Math.Max(1, panelCanvas.ClientSize.Width);
            int height = Math.Max(1, panelCanvas.ClientSize.Height);
            int depth = (int)numericUpDownDepth.Value;
            int type = comboBoxFractal.SelectedIndex;

            if (type == 0)
            {
                DrawPythagorasTree(g, width / 2f, height - 25f, height / 4.2f, 90f, depth, depth);
            }
            else
            {
                PointF a = new PointF(width / 2f, 20f);
                PointF b = new PointF(20f, height - 20f);
                PointF c = new PointF(width - 20f, height - 20f);
                DrawSierpinski(g, a, b, c, depth, depth);
            }

            g.ResetTransform();
            labelStatus.Text = "Векторная отрисовка";
        }

        private void DrawPythagorasTree(Graphics g, float x, float y, float length, float angle, int depth, int maxDepth)
        {
            if (depth <= 0 || length < 2f)
                return;

            float x2 = x + (float)(length * Math.Cos(angle * Math.PI / 180.0));
            float y2 = y - (float)(length * Math.Sin(angle * Math.PI / 180.0));

            int green = 255 - (maxDepth - depth) * Math.Max(10, 180 / Math.Max(1, maxDepth));
            green = Math.Max(70, Math.Min(255, green));

            using Pen pen = new Pen(Color.FromArgb(30, green / 2, 30), Math.Max(1f, depth / 2f));
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                g.DrawLine(pen, x, y, x2, y2);
            }

            DrawPythagorasTree(g, x2, y2, length * 0.72f, angle - 30f, depth - 1, maxDepth);
            DrawPythagorasTree(g, x2, y2, length * 0.72f, angle + 30f, depth - 1, maxDepth);
        }

        private void DrawSierpinski(Graphics g, PointF a, PointF b, PointF c, int depth, int maxDepth)
        {
            if (depth <= 0)
            {
                int tint = 255 - (maxDepth * 10);
                using SolidBrush brush = new SolidBrush(Color.FromArgb(255, Math.Max(30, tint / 3), Math.Max(50, tint / 2), Math.Min(230, tint)));
                using Pen pen = new Pen(Color.Navy, 1f);
                g.FillPolygon(brush, new[] { a, b, c });
                g.DrawPolygon(pen, new[] { a, b, c });
                return;
            }

            PointF ab = Mid(a, b);
            PointF bc = Mid(b, c);
            PointF ca = Mid(c, a);

            DrawSierpinski(g, a, ab, ca, depth - 1, maxDepth);
            DrawSierpinski(g, ab, b, bc, depth - 1, maxDepth);
            DrawSierpinski(g, ca, bc, c, depth - 1, maxDepth);
        }

        private PointF Mid(PointF p1, PointF p2)
        {
            return new PointF((p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);
        }

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            panelCanvas.Invalidate();
            AppLogger.Log("ЛР3 Часть 2: фрактал перестроен в векторном режиме.");
        }

        private void numericUpDownDepth_ValueChanged(object sender, EventArgs e)
        {
            panelCanvas.Invalidate();
        }

        private void comboBoxFractal_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelCanvas.Invalidate();
        }

        private void panelCanvas_Resize(object sender, EventArgs e)
        {
            panelCanvas.Invalidate();
        }

        private void panelCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            ZoomAt(e.Location, e.Delta > 0 ? 1.15f : 1f / 1.15f);
        }

        private void panelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            panelCanvas.Focus();
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
            {
                isPanning = true;
                panStartPoint = e.Location;
                panStartOffset = viewOffset;
                panelCanvas.Cursor = Cursors.Hand;
            }
        }

        private void panelCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPanning)
                return;

            viewOffset = new PointF(
                panStartOffset.X + (e.X - panStartPoint.X),
                panStartOffset.Y + (e.Y - panStartPoint.Y));
            panelCanvas.Invalidate();
        }

        private void panelCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            isPanning = false;
            panelCanvas.Cursor = Cursors.Default;
        }

        private void buttonResetView_Click(object sender, EventArgs e)
        {
            ResetView();
            AppLogger.Log("ЛР3 Часть 2: масштаб и позиция фрактала сброшены.");
        }
    }
}
