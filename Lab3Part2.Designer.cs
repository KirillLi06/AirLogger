namespace WinFormsApp1
{
    partial class Lab3Part2
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboBoxFractal;
        private NumericUpDown numericUpDownDepth;
        private Button buttonBuild;
        private Panel panelCanvas;
        private Label label1;
        private Label label2;
        private Label labelStatus;
        private Button buttonResetView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            comboBoxFractal = new ComboBox();
            numericUpDownDepth = new NumericUpDown();
            buttonBuild = new Button();
            panelCanvas = new Panel();
            label1 = new Label();
            label2 = new Label();
            labelStatus = new Label();
            buttonResetView = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDepth).BeginInit();
            SuspendLayout();
            // 
            // comboBoxFractal
            // 
            comboBoxFractal.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFractal.Items.AddRange(new object[] { "Дерево Пифагора", "Салфетка Серпинского" });
            comboBoxFractal.Location = new Point(72, 12);
            comboBoxFractal.Name = "comboBoxFractal";
            comboBoxFractal.Size = new Size(180, 23);
            comboBoxFractal.TabIndex = 7;
            comboBoxFractal.SelectedIndexChanged += comboBoxFractal_SelectedIndexChanged;
            // 
            // numericUpDownDepth
            // 
            numericUpDownDepth.Location = new Point(330, 12);
            numericUpDownDepth.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numericUpDownDepth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownDepth.Name = "numericUpDownDepth";
            numericUpDownDepth.Size = new Size(70, 23);
            numericUpDownDepth.TabIndex = 6;
            numericUpDownDepth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownDepth.ValueChanged += numericUpDownDepth_ValueChanged;
            // 
            // buttonBuild
            // 
            buttonBuild.Location = new Point(420, 11);
            buttonBuild.Name = "buttonBuild";
            buttonBuild.Size = new Size(130, 25);
            buttonBuild.TabIndex = 5;
            buttonBuild.Text = "Построить";
            buttonBuild.Click += buttonBuild_Click;
            // 
            // panelCanvas
            // 
            panelCanvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelCanvas.BackColor = Color.White;
            panelCanvas.BorderStyle = BorderStyle.FixedSingle;
            panelCanvas.Location = new Point(12, 50);
            panelCanvas.Name = "panelCanvas";
            panelCanvas.Size = new Size(976, 588);
            panelCanvas.TabIndex = 4;
            panelCanvas.TabStop = true;
            panelCanvas.Paint += panelCanvas_Paint;
            panelCanvas.MouseDown += panelCanvas_MouseDown;
            panelCanvas.MouseMove += panelCanvas_MouseMove;
            panelCanvas.MouseUp += panelCanvas_MouseUp;
            panelCanvas.MouseWheel += panelCanvas_MouseWheel;
            panelCanvas.Resize += panelCanvas_Resize;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(53, 15);
            label1.TabIndex = 3;
            label1.Text = "Фрактал";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(270, 15);
            label2.Name = "label2";
            label2.Size = new Size(53, 15);
            label2.TabIndex = 2;
            label2.Text = "Глубина";
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Location = new Point(570, 16);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(45, 15);
            labelStatus.TabIndex = 1;
            labelStatus.Text = "Готово";
            // 
            // buttonResetView
            // 
            buttonResetView.Location = new Point(858, 12);
            buttonResetView.Name = "buttonResetView";
            buttonResetView.Size = new Size(130, 25);
            buttonResetView.TabIndex = 0;
            buttonResetView.Text = "Сброс вида";
            buttonResetView.Click += buttonResetView_Click;
            // 
            // Lab3Part2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 650);
            Controls.Add(buttonResetView);
            Controls.Add(labelStatus);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panelCanvas);
            Controls.Add(buttonBuild);
            Controls.Add(numericUpDownDepth);
            Controls.Add(comboBoxFractal);
            Name = "Lab3Part2";
            Text = "ЛР 3 - Часть 2. Фракталы";
            ((System.ComponentModel.ISupportInitialize)numericUpDownDepth).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
