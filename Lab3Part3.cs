using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Lab3Part3 : Form
    {
        private readonly List<string> recursionTrace = new List<string>();
        private bool isPanning;
        private float zoom = 1f;
        private PointF viewOffset = PointF.Empty;
        private Point panStartPoint;
        private PointF panStartOffset;

        public Lab3Part3()
        {
            InitializeComponent();
            DetailedActionLogger.Attach(this);
            numericUpDownDepth.Value = 4;
            numericUpDownSatellites.Value = 4;
            numericUpDownOrbit.Value = 3;
            numericUpDownSatelliteRatio.Value = 45;
            ResetView();
            AppLogger.Log("ЛР3 Часть 3: форма варианта 1 открыта.");
        }

        private void ResetView()
        {
            zoom = 1f;
            if (panelPlanetSystem.ClientSize.Width <= 0 || panelPlanetSystem.ClientSize.Height <= 0)
            {
                viewOffset = PointF.Empty;
                return;
            }

            viewOffset = new PointF(panelPlanetSystem.ClientSize.Width / 2f, panelPlanetSystem.ClientSize.Height / 2f);
            panelPlanetSystem.Invalidate();
        }

        private void ZoomAt(Point location, float factor)
        {
            float oldZoom = zoom;
            float newZoom = Math.Max(0.05f, Math.Min(20f, zoom * factor));
            if (Math.Abs(newZoom - oldZoom) < 0.0001f)
                return;

            PointF worldPoint = new PointF((location.X - viewOffset.X) / oldZoom, (location.Y - viewOffset.Y) / oldZoom);
            zoom = newZoom;
            viewOffset = new PointF(location.X - worldPoint.X * zoom, location.Y - worldPoint.Y * zoom);
            panelPlanetSystem.Invalidate();
        }

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            recursionTrace.Clear();
            panelPlanetSystem.Invalidate();
            AppLogger.Log("ЛР3 Часть 3: планетарная система перестроена в векторном режиме.");
        }

        private void panelPlanetSystem_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.Black);
            recursionTrace.Clear();

            int n = (int)numericUpDownDepth.Value;
            int satellites = Math.Max(1, (int)numericUpDownSatellites.Value);
            float orbitCoeff = (float)numericUpDownOrbit.Value;
            float satelliteCoeff = (float)numericUpDownSatelliteRatio.Value / 100f;
            float radius = Math.Min(panelPlanetSystem.Width, panelPlanetSystem.Height) / 10f;

            using Matrix transform = new Matrix();
            transform.Translate(viewOffset.X, viewOffset.Y);
            transform.Scale(zoom, zoom);
            g.Transform = transform;

            DrawPlanet(g,
                0f,
                0f,
                radius,
                n,
                satellites,
                orbitCoeff,
                satelliteCoeff,
                0,
                n);

            g.ResetTransform();
            textBoxTrace.Lines = recursionTrace.ToArray();
        }

        private void DrawPlanet(Graphics g, float x, float y, float radius, int level, int satellites, float orbitCoeff, float satelliteCoeff, int depthIndex, int maxDepth)
        {
            if (level <= 0 || radius < 2f)
                return;

            int colorValue = 255 - depthIndex * Math.Max(15, 150 / Math.Max(1, maxDepth));
            using SolidBrush brush = new SolidBrush(Color.FromArgb(255, Math.Max(40, colorValue / 4), Math.Max(80, colorValue / 2), Math.Min(255, colorValue)));
            using Pen orbitPen = new Pen(Color.FromArgb(70, Color.White), 1f / zoom);
            using Pen borderPen = new Pen(Color.White, 1.3f / zoom);

            g.FillEllipse(brush, x - radius, y - radius, radius * 2f, radius * 2f);
            g.DrawEllipse(borderPen, x - radius, y - radius, radius * 2f, radius * 2f);

            recursionTrace.Add($"Уровень {depthIndex + 1}: планета ({(int)x}; {(int)y}), R={radius:F1}");

            float orbitRadius = radius * orbitCoeff;
            g.DrawEllipse(orbitPen, x - orbitRadius, y - orbitRadius, orbitRadius * 2f, orbitRadius * 2f);

            double step = 2 * Math.PI / satellites;
            for (int i = 1; i <= satellites; i++)
            {
                float childX = x + (float)(orbitRadius * Math.Cos(step * i));
                float childY = y + (float)(orbitRadius * Math.Sin(step * i));
                DrawPlanet(g, childX, childY, radius * satelliteCoeff, level - 1, satellites, orbitCoeff, satelliteCoeff, depthIndex + 1, maxDepth);
            }
        }

        private void ParamsChanged(object sender, EventArgs e)
        {
            panelPlanetSystem.Invalidate();
        }

        private void panelPlanetSystem_MouseWheel(object sender, MouseEventArgs e)
        {
            ZoomAt(e.Location, e.Delta > 0 ? 1.15f : 1f / 1.15f);
        }

        private void panelPlanetSystem_MouseDown(object sender, MouseEventArgs e)
        {
            panelPlanetSystem.Focus();
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
            {
                isPanning = true;
                panStartPoint = e.Location;
                panStartOffset = viewOffset;
                panelPlanetSystem.Cursor = Cursors.Hand;
            }
        }

        private void panelPlanetSystem_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPanning)
                return;

            viewOffset = new PointF(
                panStartOffset.X + (e.X - panStartPoint.X),
                panStartOffset.Y + (e.Y - panStartPoint.Y));
            panelPlanetSystem.Invalidate();
        }

        private void panelPlanetSystem_MouseUp(object sender, MouseEventArgs e)
        {
            isPanning = false;
            panelPlanetSystem.Cursor = Cursors.Default;
        }

        private void panelPlanetSystem_Resize(object sender, EventArgs e)
        {
            if (zoom <= 1.01f)
                ResetView();
            else
                panelPlanetSystem.Invalidate();
        }

        private void buttonResetView_Click(object sender, EventArgs e)
        {
            ResetView();
            AppLogger.Log("ЛР3 Часть 3: масштаб и позиция планетарной системы сброшены.");
        }

        private void Lab3Part3_Load(object sender, EventArgs e)
        {

        }
    }
}
