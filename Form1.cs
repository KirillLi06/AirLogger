using System;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly System.Windows.Forms.Timer clockTimer;

        public Form1()
        {
            InitializeComponent();
            DetailedActionLogger.Attach(this);

            AppLogger.LogAdded += AppLogger_LogAdded;
            ProcessStatusService.StatusChanged += ProcessStatusService_StatusChanged;

            clockTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
            UpdateSystemDateTime();

            ProcessStatusService.Reset();
            AppLogger.Log("Приложение запущено.");
        }

        private void AppLogger_LogAdded(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppLogger_LogAdded), message);
                return;
            }

            if (!string.IsNullOrWhiteSpace(textBoxFileContent.Text))
                textBoxFileContent.AppendText(Environment.NewLine);

            textBoxFileContent.AppendText(message);
            textBoxFileContent.SelectionStart = textBoxFileContent.TextLength;
            textBoxFileContent.ScrollToCaret();
        }

        private void ProcessStatusService_StatusChanged(object? sender, ProcessStatusEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object?, ProcessStatusEventArgs>(ProcessStatusService_StatusChanged), sender, e);
                return;
            }

            processStatusLabel.Text = e.ProcessName == "Готово"
                ? "Процесс: Готово"
                : $"Процесс: {e.ProcessName} — {e.Progress}%";
            processProgressBar.Value = e.Progress;
            processProgressBar.ToolTipText = $"Выполнение: {e.Progress}%";
        }

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            UpdateSystemDateTime();
        }

        private void UpdateSystemDateTime()
        {
            dateTimeStatusLabel.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        private void часть1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                lab1_part1 form = new lab1_part1();
                form.Show();
                AppLogger.Log("Открыта форма ЛР1 - Часть 1.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.часть1ToolStripMenuItem_Click");
            }
        }

        private void часть2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Lab1_part2 form = new Lab1_part2();
                form.Show();
                AppLogger.Log("Открыта форма ЛР1 - Часть 2.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.часть2ToolStripMenuItem_Click");
            }
        }

        private void лр2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Lab2 form = new Lab2();
                form.Show();
                AppLogger.Log("Открыта форма ЛР2 - Вариант 2.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.лр2ToolStripMenuItem_Click");
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBoxFileContent.Text = File.ReadAllText(ofd.FileName);
                    AppLogger.Log($"Открыт файл: {ofd.FileName}");
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.открытьToolStripMenuItem_Click");
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";
                sfd.FileName = "new_file.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, textBoxFileContent.Text);
                    AppLogger.Log($"Сохранён файл: {sfd.FileName}");
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.сохранитьКакToolStripMenuItem_Click");
            }
        }

        private void лр3Часть1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Lab3Part1 form = new Lab3Part1();
                form.Show();
                AppLogger.Log("Открыта форма ЛР3 - Часть 1.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.лр3Часть1ToolStripMenuItem_Click");
            }
        }

        private void лр3Часть2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Lab3Part2 form = new Lab3Part2();
                form.Show();
                AppLogger.Log("Открыта форма ЛР3 - Часть 2.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.лр3Часть2ToolStripMenuItem_Click");
            }
        }

        private void лр3Часть3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Lab3Part3 form = new Lab3Part3();
                form.Show();
                AppLogger.Log("Открыта форма ЛР3 - Часть 3.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.лр3Часть3ToolStripMenuItem_Click");
            }
        }

        private void очиститьTextboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxFileContent.Clear();
            AppLogger.Log("TextBox главной формы очищен.");
        }

        public string GetMainLogText()
        {
            return textBoxFileContent.Text;
        }

        private void лР4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Lab4Form form = new Lab4Form(this);
                form.Show();
                AppLogger.Log("Открыта форма ЛР4 - Вариант 2.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.лР4ToolStripMenuItem_Click");
            }
        }

        private void лР5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Lab5Form form = new Lab5Form();
                form.Show();
                AppLogger.Log("Открыта форма ЛР5 - Вариант 2.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Form1.лР5ToolStripMenuItem_Click");
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            clockTimer.Stop();
            clockTimer.Dispose();
            AppLogger.LogAdded -= AppLogger_LogAdded;
            ProcessStatusService.StatusChanged -= ProcessStatusService_StatusChanged;
            base.OnFormClosed(e);
        }
    }
}
