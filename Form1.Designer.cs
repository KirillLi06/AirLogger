namespace WinFormsApp1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            файлToolStripMenuItem = new ToolStripMenuItem();
            открытьToolStripMenuItem = new ToolStripMenuItem();
            сохранитьКакToolStripMenuItem = new ToolStripMenuItem();
            очиститьTextBoxToolStripMenuItem = new ToolStripMenuItem();
            лР1ToolStripMenuItem = new ToolStripMenuItem();
            часть1ToolStripMenuItem = new ToolStripMenuItem();
            часть2ToolStripMenuItem = new ToolStripMenuItem();
            лР2ToolStripMenuItem = new ToolStripMenuItem();
            лР3ToolStripMenuItem = new ToolStripMenuItem();
            лр3Часть1ToolStripMenuItem = new ToolStripMenuItem();
            лр3Часть2ToolStripMenuItem = new ToolStripMenuItem();
            лр3Часть3ToolStripMenuItem = new ToolStripMenuItem();
            лР4ToolStripMenuItem = new ToolStripMenuItem();
            лР5ToolStripMenuItem = new ToolStripMenuItem();
            textBoxFileContent = new TextBox();
            statusStrip1 = new StatusStrip();
            processStatusLabel = new ToolStripStatusLabel();
            processProgressBar = new ToolStripProgressBar();
            spacerStatusLabel = new ToolStripStatusLabel();
            dateTimeStatusLabel = new ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { файлToolStripMenuItem, лР1ToolStripMenuItem, лР2ToolStripMenuItem, лР3ToolStripMenuItem, лР4ToolStripMenuItem, лР5ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(3, 1, 0, 1);
            menuStrip1.Size = new Size(820, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            файлToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { открытьToolStripMenuItem, сохранитьКакToolStripMenuItem, очиститьTextBoxToolStripMenuItem });
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            файлToolStripMenuItem.Size = new Size(48, 22);
            файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            открытьToolStripMenuItem.Size = new Size(169, 22);
            открытьToolStripMenuItem.Text = "Открыть";
            открытьToolStripMenuItem.Click += открытьToolStripMenuItem_Click;
            // 
            // сохранитьКакToolStripMenuItem
            // 
            сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            сохранитьКакToolStripMenuItem.Size = new Size(169, 22);
            сохранитьКакToolStripMenuItem.Text = "Сохранить как";
            сохранитьКакToolStripMenuItem.Click += сохранитьКакToolStripMenuItem_Click;
            // 
            // очиститьTextBoxToolStripMenuItem
            // 
            очиститьTextBoxToolStripMenuItem.Name = "очиститьTextBoxToolStripMenuItem";
            очиститьTextBoxToolStripMenuItem.Size = new Size(169, 22);
            очиститьTextBoxToolStripMenuItem.Text = "Очистить TextBox";
            очиститьTextBoxToolStripMenuItem.Click += очиститьTextboxToolStripMenuItem_Click;
            // 
            // лР1ToolStripMenuItem
            // 
            лР1ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { часть1ToolStripMenuItem, часть2ToolStripMenuItem });
            лР1ToolStripMenuItem.Name = "лР1ToolStripMenuItem";
            лР1ToolStripMenuItem.Size = new Size(43, 22);
            лР1ToolStripMenuItem.Text = "ЛР 1";
            // 
            // часть1ToolStripMenuItem
            // 
            часть1ToolStripMenuItem.Name = "часть1ToolStripMenuItem";
            часть1ToolStripMenuItem.Size = new Size(114, 22);
            часть1ToolStripMenuItem.Text = "Часть 1";
            часть1ToolStripMenuItem.Click += часть1ToolStripMenuItem_Click;
            // 
            // часть2ToolStripMenuItem
            // 
            часть2ToolStripMenuItem.Name = "часть2ToolStripMenuItem";
            часть2ToolStripMenuItem.Size = new Size(114, 22);
            часть2ToolStripMenuItem.Text = "Часть 2";
            часть2ToolStripMenuItem.Click += часть2ToolStripMenuItem_Click;
            // 
            // лР2ToolStripMenuItem
            // 
            лР2ToolStripMenuItem.Name = "лР2ToolStripMenuItem";
            лР2ToolStripMenuItem.Size = new Size(40, 22);
            лР2ToolStripMenuItem.Text = "ЛР2";
            лР2ToolStripMenuItem.Click += лр2ToolStripMenuItem_Click;
            // 
            // лР3ToolStripMenuItem
            // 
            лР3ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { лр3Часть1ToolStripMenuItem, лр3Часть2ToolStripMenuItem, лр3Часть3ToolStripMenuItem });
            лР3ToolStripMenuItem.Name = "лР3ToolStripMenuItem";
            лР3ToolStripMenuItem.Size = new Size(43, 22);
            лР3ToolStripMenuItem.Text = "ЛР 3";
            // 
            // лр3Часть1ToolStripMenuItem
            // 
            лр3Часть1ToolStripMenuItem.Name = "лр3Часть1ToolStripMenuItem";
            лр3Часть1ToolStripMenuItem.Size = new Size(178, 22);
            лр3Часть1ToolStripMenuItem.Text = "Часть 1";
            лр3Часть1ToolStripMenuItem.Click += лр3Часть1ToolStripMenuItem_Click;
            // 
            // лр3Часть2ToolStripMenuItem
            // 
            лр3Часть2ToolStripMenuItem.Name = "лр3Часть2ToolStripMenuItem";
            лр3Часть2ToolStripMenuItem.Size = new Size(178, 22);
            лр3Часть2ToolStripMenuItem.Text = "Часть 2";
            лр3Часть2ToolStripMenuItem.Click += лр3Часть2ToolStripMenuItem_Click;
            // 
            // лр3Часть3ToolStripMenuItem
            // 
            лр3Часть3ToolStripMenuItem.Name = "лр3Часть3ToolStripMenuItem";
            лр3Часть3ToolStripMenuItem.Size = new Size(178, 22);
            лр3Часть3ToolStripMenuItem.Text = "Часть 3 (вариант 1)";
            лр3Часть3ToolStripMenuItem.Click += лр3Часть3ToolStripMenuItem_Click;
            // 
            // лР4ToolStripMenuItem
            // 
            лР4ToolStripMenuItem.Name = "лР4ToolStripMenuItem";
            лР4ToolStripMenuItem.Size = new Size(43, 22);
            лР4ToolStripMenuItem.Text = "ЛР 4";
            лР4ToolStripMenuItem.Click += лР4ToolStripMenuItem_Click;
            // 
            // лР5ToolStripMenuItem
            // 
            лР5ToolStripMenuItem.Name = "лР5ToolStripMenuItem";
            лР5ToolStripMenuItem.Size = new Size(40, 22);
            лР5ToolStripMenuItem.Text = "ЛР5";
            лР5ToolStripMenuItem.Click += лР5ToolStripMenuItem_Click;
            // 
            // textBoxFileContent
            // 
            textBoxFileContent.Dock = DockStyle.Fill;
            textBoxFileContent.Location = new Point(0, 24);
            textBoxFileContent.Multiline = true;
            textBoxFileContent.Name = "textBoxFileContent";
            textBoxFileContent.ReadOnly = true;
            textBoxFileContent.ScrollBars = ScrollBars.Both;
            textBoxFileContent.Size = new Size(820, 374);
            textBoxFileContent.TabIndex = 1;
            textBoxFileContent.WordWrap = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { processStatusLabel, processProgressBar, spacerStatusLabel, dateTimeStatusLabel });
            statusStrip1.Location = new Point(0, 398);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(820, 25);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // processStatusLabel
            // 
            processStatusLabel.Name = "processStatusLabel";
            processStatusLabel.Size = new Size(89, 20);
            processStatusLabel.Text = "Процесс: Готово";
            // 
            // processProgressBar
            // 
            processProgressBar.Name = "processProgressBar";
            processProgressBar.Size = new Size(180, 19);
            processProgressBar.Step = 1;
            // 
            // spacerStatusLabel
            // 
            spacerStatusLabel.Name = "spacerStatusLabel";
            spacerStatusLabel.Size = new Size(377, 20);
            spacerStatusLabel.Spring = true;
            // 
            // dateTimeStatusLabel
            // 
            dateTimeStatusLabel.Name = "dateTimeStatusLabel";
            dateTimeStatusLabel.Size = new Size(151, 20);
            dateTimeStatusLabel.Text = "00.00.0000 00:00:00";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 423);
            Controls.Add(textBoxFileContent);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(700, 380);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ли К.А. — консолидированный проект";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem лР1ToolStripMenuItem;
        private ToolStripMenuItem часть1ToolStripMenuItem;
        private ToolStripMenuItem часть2ToolStripMenuItem;
        private ToolStripMenuItem лР2ToolStripMenuItem;
        private ToolStripMenuItem лР3ToolStripMenuItem;
        private ToolStripMenuItem лр3Часть1ToolStripMenuItem;
        private ToolStripMenuItem лр3Часть2ToolStripMenuItem;
        private ToolStripMenuItem лр3Часть3ToolStripMenuItem;
        private ToolStripMenuItem лР4ToolStripMenuItem;
        private ToolStripMenuItem лР5ToolStripMenuItem;
        private ToolStripMenuItem файлToolStripMenuItem;
        private ToolStripMenuItem открытьToolStripMenuItem;
        private ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private ToolStripMenuItem очиститьTextBoxToolStripMenuItem;
        private TextBox textBoxFileContent;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel processStatusLabel;
        private ToolStripProgressBar processProgressBar;
        private ToolStripStatusLabel spacerStatusLabel;
        private ToolStripStatusLabel dateTimeStatusLabel;
    }
}
