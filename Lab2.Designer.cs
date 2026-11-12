namespace WinFormsApp1
{
    partial class Lab2
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
            numericUpDownSize = new NumericUpDown();
            numericUpDownDisplayCount = new NumericUpDown();
            radioButtonManual = new RadioButton();
            radioButtonRandom = new RadioButton();
            radioButtonFrequency = new RadioButton();
            textBoxMin = new TextBox();
            textBoxMax = new TextBox();
            textBoxFreqValue = new TextBox();
            textBoxFreqPercent = new TextBox();
            buttonGenerate = new Button();
            buttonSequential = new Button();
            buttonParallel = new Button();
            buttonCompare = new Button();
            dataGridViewSource = new DataGridView();
            dataGridViewA = new DataGridView();
            dataGridViewModified = new DataGridView();
            labelTimeSequential = new Label();
            labelTimeParallel = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            labelSource = new Label();
            labelA = new Label();
            labelModified = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDisplayCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewModified).BeginInit();
            SuspendLayout();
            // 
            // numericUpDownSize
            // 
            numericUpDownSize.Location = new Point(12, 31);
            numericUpDownSize.Name = "numericUpDownSize";
            numericUpDownSize.Size = new Size(120, 23);
            numericUpDownSize.TabIndex = 0;
            // 
            // numericUpDownDisplayCount
            // 
            numericUpDownDisplayCount.Location = new Point(159, 31);
            numericUpDownDisplayCount.Name = "numericUpDownDisplayCount";
            numericUpDownDisplayCount.Size = new Size(145, 23);
            numericUpDownDisplayCount.TabIndex = 1;
            // 
            // radioButtonManual
            // 
            radioButtonManual.AutoSize = true;
            radioButtonManual.Location = new Point(579, 18);
            radioButtonManual.Name = "radioButtonManual";
            radioButtonManual.Size = new Size(70, 19);
            radioButtonManual.TabIndex = 2;
            radioButtonManual.TabStop = true;
            radioButtonManual.Text = "Вручную";
            radioButtonManual.UseVisualStyleBackColor = true;
            // 
            // radioButtonRandom
            // 
            radioButtonRandom.AutoSize = true;
            radioButtonRandom.Location = new Point(579, 49);
            radioButtonRandom.Name = "radioButtonRandom";
            radioButtonRandom.Size = new Size(80, 19);
            radioButtonRandom.TabIndex = 3;
            radioButtonRandom.TabStop = true;
            radioButtonRandom.Text = "Случайно";
            radioButtonRandom.UseVisualStyleBackColor = true;
            // 
            // radioButtonFrequency
            // 
            radioButtonFrequency.AutoSize = true;
            radioButtonFrequency.Location = new Point(579, 80);
            radioButtonFrequency.Name = "radioButtonFrequency";
            radioButtonFrequency.Size = new Size(201, 19);
            radioButtonFrequency.TabIndex = 4;
            radioButtonFrequency.TabStop = true;
            radioButtonFrequency.Text = "Случайно с заданной частотой";
            radioButtonFrequency.UseVisualStyleBackColor = true;
            // 
            // textBoxMin
            // 
            textBoxMin.Location = new Point(12, 88);
            textBoxMin.Name = "textBoxMin";
            textBoxMin.Size = new Size(141, 23);
            textBoxMin.TabIndex = 5;
            // 
            // textBoxMax
            // 
            textBoxMax.Location = new Point(159, 88);
            textBoxMax.Name = "textBoxMax";
            textBoxMax.Size = new Size(145, 23);
            textBoxMax.TabIndex = 6;
            // 
            // textBoxFreqValue
            // 
            textBoxFreqValue.Location = new Point(579, 126);
            textBoxFreqValue.Name = "textBoxFreqValue";
            textBoxFreqValue.Size = new Size(100, 23);
            textBoxFreqValue.TabIndex = 7;
            // 
            // textBoxFreqPercent
            // 
            textBoxFreqPercent.Location = new Point(691, 126);
            textBoxFreqPercent.Name = "textBoxFreqPercent";
            textBoxFreqPercent.Size = new Size(100, 23);
            textBoxFreqPercent.TabIndex = 8;
            // 
            // buttonGenerate
            // 
            buttonGenerate.Location = new Point(12, 126);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new Size(141, 28);
            buttonGenerate.TabIndex = 9;
            buttonGenerate.Text = "Сформировать массив";
            buttonGenerate.UseVisualStyleBackColor = true;
            buttonGenerate.Click += buttonGenerate_Click;
            // 
            // buttonSequential
            // 
            buttonSequential.Location = new Point(12, 160);
            buttonSequential.Name = "buttonSequential";
            buttonSequential.Size = new Size(141, 28);
            buttonSequential.TabIndex = 10;
            buttonSequential.Text = "Обработка без потоков";
            buttonSequential.UseVisualStyleBackColor = true;
            buttonSequential.Click += buttonSequential_Click;
            // 
            // buttonParallel
            // 
            buttonParallel.Location = new Point(159, 160);
            buttonParallel.Name = "buttonParallel";
            buttonParallel.Size = new Size(145, 28);
            buttonParallel.TabIndex = 11;
            buttonParallel.Text = "Обработка с потоками";
            buttonParallel.UseVisualStyleBackColor = true;
            buttonParallel.Click += buttonParallel_Click;
            // 
            // buttonCompare
            // 
            buttonCompare.Location = new Point(159, 126);
            buttonCompare.Name = "buttonCompare";
            buttonCompare.Size = new Size(145, 28);
            buttonCompare.TabIndex = 12;
            buttonCompare.Text = "Сравнить время";
            buttonCompare.UseVisualStyleBackColor = true;
            buttonCompare.Click += buttonCompare_Click;
            // 
            // dataGridViewSource
            // 
            dataGridViewSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewSource.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSource.Location = new Point(12, 221);
            dataGridViewSource.Name = "dataGridViewSource";
            dataGridViewSource.Size = new Size(876, 72);
            dataGridViewSource.TabIndex = 13;
            // 
            // dataGridViewA
            // 
            dataGridViewA.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewA.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewA.Location = new Point(12, 320);
            dataGridViewA.Name = "dataGridViewA";
            dataGridViewA.ReadOnly = true;
            dataGridViewA.Size = new Size(876, 72);
            dataGridViewA.TabIndex = 14;
            // 
            // dataGridViewModified
            // 
            dataGridViewModified.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewModified.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewModified.Location = new Point(12, 419);
            dataGridViewModified.Name = "dataGridViewModified";
            dataGridViewModified.ReadOnly = true;
            dataGridViewModified.Size = new Size(876, 72);
            dataGridViewModified.TabIndex = 15;
            // 
            // labelTimeSequential
            // 
            labelTimeSequential.AutoSize = true;
            labelTimeSequential.Location = new Point(12, 508);
            labelTimeSequential.Name = "labelTimeSequential";
            labelTimeSequential.Size = new Size(114, 15);
            labelTimeSequential.TabIndex = 16;
            labelTimeSequential.Text = "Время без потоков:";
            // 
            // labelTimeParallel
            // 
            labelTimeParallel.AutoSize = true;
            labelTimeParallel.Location = new Point(362, 508);
            labelTimeParallel.Name = "labelTimeParallel";
            labelTimeParallel.Size = new Size(111, 15);
            labelTimeParallel.TabIndex = 17;
            labelTimeParallel.Text = "Время с потоками:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 13);
            label1.Name = "label1";
            label1.Size = new Size(91, 15);
            label1.TabIndex = 18;
            label1.Text = "Длина массива";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 70);
            label2.Name = "label2";
            label2.Size = new Size(141, 15);
            label2.TabIndex = 19;
            label2.Text = "Минимальное значение";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(159, 70);
            label3.Name = "label3";
            label3.Size = new Size(145, 15);
            label3.TabIndex = 20;
            label3.Text = "Максимальное значение";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(579, 108);
            label4.Name = "label4";
            label4.Size = new Size(60, 15);
            label4.TabIndex = 21;
            label4.Text = "Значение";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(691, 108);
            label5.Name = "label5";
            label5.Size = new Size(68, 15);
            label5.TabIndex = 22;
            label5.Text = "Частота (%)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(159, 13);
            label6.Name = "label6";
            label6.Size = new Size(128, 15);
            label6.TabIndex = 23;
            label6.Text = "Показать первых N";
            // 
            // labelSource
            // 
            labelSource.AutoSize = true;
            labelSource.Location = new Point(12, 203);
            labelSource.Name = "labelSource";
            labelSource.Size = new Size(113, 15);
            labelSource.TabIndex = 24;
            labelSource.Text = "Исходный массив C";
            // 
            // labelA
            // 
            labelA.AutoSize = true;
            labelA.Location = new Point(12, 302);
            labelA.Name = "labelA";
            labelA.Size = new Size(144, 15);
            labelA.TabIndex = 25;
            labelA.Text = "Сформированный массив A";
            // 
            // labelModified
            // 
            labelModified.AutoSize = true;
            labelModified.Location = new Point(12, 401);
            labelModified.Name = "labelModified";
            labelModified.Size = new Size(111, 15);
            labelModified.TabIndex = 26;
            labelModified.Text = "Изменённый массив";
            // 
            // Lab2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 542);
            Controls.Add(labelModified);
            Controls.Add(labelA);
            Controls.Add(labelSource);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(labelTimeParallel);
            Controls.Add(labelTimeSequential);
            Controls.Add(dataGridViewModified);
            Controls.Add(dataGridViewA);
            Controls.Add(dataGridViewSource);
            Controls.Add(buttonCompare);
            Controls.Add(buttonParallel);
            Controls.Add(buttonSequential);
            Controls.Add(buttonGenerate);
            Controls.Add(textBoxFreqPercent);
            Controls.Add(textBoxFreqValue);
            Controls.Add(textBoxMax);
            Controls.Add(textBoxMin);
            Controls.Add(radioButtonFrequency);
            Controls.Add(radioButtonRandom);
            Controls.Add(radioButtonManual);
            Controls.Add(numericUpDownDisplayCount);
            Controls.Add(numericUpDownSize);
            MinimumSize = new Size(830, 580);
            Name = "Lab2";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ЛР2 — обработка массивов, вариант 2";
            ((System.ComponentModel.ISupportInitialize)numericUpDownSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDisplayCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewA).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewModified).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown numericUpDownSize;
        private NumericUpDown numericUpDownDisplayCount;
        private RadioButton radioButtonManual;
        private RadioButton radioButtonRandom;
        private RadioButton radioButtonFrequency;
        private TextBox textBoxMin;
        private TextBox textBoxMax;
        private TextBox textBoxFreqValue;
        private TextBox textBoxFreqPercent;
        private Button buttonGenerate;
        private Button buttonSequential;
        private Button buttonParallel;
        private Button buttonCompare;
        private DataGridView dataGridViewSource;
        private DataGridView dataGridViewA;
        private DataGridView dataGridViewModified;
        private Label labelTimeSequential;
        private Label labelTimeParallel;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label labelSource;
        private Label labelA;
        private Label labelModified;
    }
}
