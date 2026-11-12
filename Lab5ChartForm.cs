using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public sealed record BenchmarkPoint(string FileName, int SizeKb, long SyncMs, long AsyncMs);

    public class Lab5ChartForm : Form
    {
        private readonly List<BenchmarkPoint> points;

        public Lab5ChartForm(List<BenchmarkPoint> points)
        {
            this.points = points;
            Text = "ЛР 5 — сравнительный график времени парсинга";
            Width = 900;
            Height = 560;
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            DetailedActionLogger.Attach(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            if (points.Count == 0)
            {
                g.DrawString("Нет данных для построения графика.", Font, Brushes.Black, 20, 20);
                return;
            }

            int left = 80;
            int right = 40;
            int top = 45;
            int bottom = 90;
            Rectangle plot = new Rectangle(left, top, ClientSize.Width - left - right, ClientSize.Height - top - bottom);
            if (plot.Width <= 0 || plot.Height <= 0)
                return;

            long maxMs = Math.Max(1, points.Max(p => Math.Max(p.SyncMs, p.AsyncMs)));
            int maxSize = Math.Max(1, points.Max(p => p.SizeKb));

            using Pen axisPen = new Pen(Color.Black, 1.5f);
            using Pen syncPen = new Pen(Color.RoyalBlue, 2.5f);
            using Pen asyncPen = new Pen(Color.Firebrick, 2.5f);
            using Brush syncBrush = new SolidBrush(Color.RoyalBlue);
            using Brush asyncBrush = new SolidBrush(Color.Firebrick);

            g.DrawLine(axisPen, plot.Left, plot.Bottom, plot.Right, plot.Bottom);
            g.DrawLine(axisPen, plot.Left, plot.Bottom, plot.Left, plot.Top);
            g.DrawString("Размер журнала, КБ", Font, Brushes.Black, plot.Left + plot.Width / 2 - 65, ClientSize.Height - 35);
            g.DrawString("Время, мс", Font, Brushes.Black, 10, 18);
            g.DrawString("Синхронный режим", Font, syncBrush, plot.Right - 230, 18);
            g.DrawString("Асинхронный режим", Font, asyncBrush, plot.Right - 230, 38);

            PointF? prevSync = null;
            PointF? prevAsync = null;
            foreach (BenchmarkPoint p in points.OrderBy(x => x.SizeKb))
            {
                float x = plot.Left + (p.SizeKb / (float)maxSize) * plot.Width;
                float ySync = plot.Bottom - (p.SyncMs / (float)maxMs) * plot.Height;
                float yAsync = plot.Bottom - (p.AsyncMs / (float)maxMs) * plot.Height;
                PointF ps = new PointF(x, ySync);
                PointF pa = new PointF(x, yAsync);

                if (prevSync.HasValue)
                    g.DrawLine(syncPen, prevSync.Value, ps);
                if (prevAsync.HasValue)
                    g.DrawLine(asyncPen, prevAsync.Value, pa);

                g.FillEllipse(syncBrush, x - 4, ySync - 4, 8, 8);
                g.FillEllipse(asyncBrush, x - 4, yAsync - 4, 8, 8);
                g.DrawString(p.SizeKb.ToString(), Font, Brushes.Black, x - 12, plot.Bottom + 8);

                prevSync = ps;
                prevAsync = pa;
            }

            for (int i = 0; i <= 5; i++)
            {
                float y = plot.Bottom - i * plot.Height / 5f;
                long label = maxMs * i / 5;
                g.DrawLine(Pens.LightGray, plot.Left, y, plot.Right, y);
                g.DrawString(label.ToString(), Font, Brushes.Black, 28, y - 8);
            }
        }
    }
}
