namespace WinFormsApp1
{
    partial class Lab3Part1
    {
        private System.ComponentModel.IContainer components = null;
        private SplitContainer splitContainer1;
        private FlowLayoutPanel flowLayoutPanel1;
        private ComboBox comboBoxTool;
        private ComboBox comboBoxDashStyle;
        private NumericUpDown numericUpDownPenWidth;
        private CheckBox checkBoxFill;
        private Button buttonLineColor;
        private Button buttonFillColor;
        private Button buttonBackColor;
        private Button buttonOpen;
        private Button buttonSave;
        private Button buttonClear;
        private Button buttonResetView;
        private PictureBox pictureBoxCanvas;
        private Panel panelLineColor;
        private Panel panelFillColor;
        private Panel panelBackColor;
        private Panel panelFourier;
        private ComboBox comboBoxFourierType;
        private NumericUpDown numericUpDownHarmonics;
        private Button buttonStartFourier;
        private Label label1;
        private Label label2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            pictureBoxCanvas = new PictureBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label1 = new Label();
            comboBoxTool = new ComboBox();
            comboBoxDashStyle = new ComboBox();
            numericUpDownPenWidth = new NumericUpDown();
            checkBoxFill = new CheckBox();
            buttonLineColor = new Button();
            panelLineColor = new Panel();
            buttonFillColor = new Button();
            panelFillColor = new Panel();
            buttonBackColor = new Button();
            panelBackColor = new Panel();
            buttonOpen = new Button();
            buttonSave = new Button();
            buttonClear = new Button();
            buttonResetView = new Button();
            panelFourier = new Panel();
            buttonStartFourier = new Button();
            numericUpDownHarmonics = new NumericUpDown();
            comboBoxFourierType = new ComboBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCanvas).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPenWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHarmonics).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pictureBoxCanvas);
            splitContainer1.Panel1.Controls.Add(flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panelFourier);
            splitContainer1.Panel2.Controls.Add(buttonStartFourier);
            splitContainer1.Panel2.Controls.Add(numericUpDownHarmonics);
            splitContainer1.Panel2.Controls.Add(comboBoxFourierType);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Size = new Size(1100, 700);
            splitContainer1.SplitterDistance = 430;
            splitContainer1.TabIndex = 0;
            // 
            // pictureBoxCanvas
            // 
            pictureBoxCanvas.BackColor = Color.White;
            pictureBoxCanvas.Dock = DockStyle.Fill;
            pictureBoxCanvas.Location = new Point(0, 44);
            pictureBoxCanvas.Name = "pictureBoxCanvas";
            pictureBoxCanvas.Size = new Size(1100, 386);
            pictureBoxCanvas.TabIndex = 0;
            pictureBoxCanvas.Paint += pictureBoxCanvas_Paint;
            pictureBoxCanvas.MouseDown += pictureBoxCanvas_MouseDown;
            pictureBoxCanvas.MouseMove += pictureBoxCanvas_MouseMove;
            pictureBoxCanvas.MouseUp += pictureBoxCanvas_MouseUp;
            pictureBoxCanvas.MouseWheel += pictureBoxCanvas_MouseWheel;
            pictureBoxCanvas.Resize += pictureBoxCanvas_Resize;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(comboBoxTool);
            flowLayoutPanel1.Controls.Add(comboBoxDashStyle);
            flowLayoutPanel1.Controls.Add(numericUpDownPenWidth);
            flowLayoutPanel1.Controls.Add(checkBoxFill);
            flowLayoutPanel1.Controls.Add(buttonLineColor);
            flowLayoutPanel1.Controls.Add(panelLineColor);
            flowLayoutPanel1.Controls.Add(buttonFillColor);
            flowLayoutPanel1.Controls.Add(panelFillColor);
            flowLayoutPanel1.Controls.Add(buttonBackColor);
            flowLayoutPanel1.Controls.Add(panelBackColor);
            flowLayoutPanel1.Controls.Add(buttonOpen);
            flowLayoutPanel1.Controls.Add(buttonSave);
            flowLayoutPanel1.Controls.Add(buttonClear);
            flowLayoutPanel1.Controls.Add(buttonResetView);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(8);
            flowLayoutPanel1.Size = new Size(1100, 44);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 16);
            label1.Margin = new Padding(3, 8, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 0;
            label1.Text = "Редактор:";
            // 
            // comboBoxTool
            // 
            comboBoxTool.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTool.Items.AddRange(new object[] { "Прямая", "Эллипс", "Прямоугольник", "Кисть" });
            comboBoxTool.Location = new Point(77, 11);
            comboBoxTool.Name = "comboBoxTool";
            comboBoxTool.Size = new Size(120, 23);
            comboBoxTool.TabIndex = 1;
            // 
            // comboBoxDashStyle
            // 
            comboBoxDashStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDashStyle.Items.AddRange(new object[] { "Сплошная", "Штрих", "Точки", "Штрих-пунктир" });
            comboBoxDashStyle.Location = new Point(203, 11);
            comboBoxDashStyle.Name = "comboBoxDashStyle";
            comboBoxDashStyle.Size = new Size(120, 23);
            comboBoxDashStyle.TabIndex = 2;
            // 
            // numericUpDownPenWidth
            // 
            numericUpDownPenWidth.Location = new Point(329, 11);
            numericUpDownPenWidth.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numericUpDownPenWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownPenWidth.Name = "numericUpDownPenWidth";
            numericUpDownPenWidth.Size = new Size(60, 23);
            numericUpDownPenWidth.TabIndex = 3;
            numericUpDownPenWidth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // checkBoxFill
            // 
            checkBoxFill.AutoSize = true;
            checkBoxFill.Location = new Point(395, 16);
            checkBoxFill.Margin = new Padding(3, 8, 3, 0);
            checkBoxFill.Name = "checkBoxFill";
            checkBoxFill.Size = new Size(71, 19);
            checkBoxFill.TabIndex = 4;
            checkBoxFill.Text = "Заливка";
            // 
            // buttonLineColor
            // 
            buttonLineColor.Location = new Point(472, 11);
            buttonLineColor.Name = "buttonLineColor";
            buttonLineColor.Size = new Size(75, 23);
            buttonLineColor.TabIndex = 5;
            buttonLineColor.Text = "Линия";
            buttonLineColor.Click += buttonLineColor_Click;
            // 
            // panelLineColor
            // 
            panelLineColor.BorderStyle = BorderStyle.FixedSingle;
            panelLineColor.Location = new Point(553, 11);
            panelLineColor.Margin = new Padding(3, 3, 8, 3);
            panelLineColor.Name = "panelLineColor";
            panelLineColor.Size = new Size(24, 24);
            panelLineColor.TabIndex = 6;
            // 
            // buttonFillColor
            // 
            buttonFillColor.Location = new Point(588, 11);
            buttonFillColor.Name = "buttonFillColor";
            buttonFillColor.Size = new Size(75, 23);
            buttonFillColor.TabIndex = 7;
            buttonFillColor.Text = "Заливка";
            buttonFillColor.Click += buttonFillColor_Click;
            // 
            // panelFillColor
            // 
            panelFillColor.BorderStyle = BorderStyle.FixedSingle;
            panelFillColor.Location = new Point(669, 11);
            panelFillColor.Margin = new Padding(3, 3, 8, 3);
            panelFillColor.Name = "panelFillColor";
            panelFillColor.Size = new Size(24, 24);
            panelFillColor.TabIndex = 8;
            // 
            // buttonBackColor
            // 
            buttonBackColor.Location = new Point(704, 11);
            buttonBackColor.Name = "buttonBackColor";
            buttonBackColor.Size = new Size(75, 23);
            buttonBackColor.TabIndex = 9;
            buttonBackColor.Text = "Фон";
            buttonBackColor.Click += buttonBackColor_Click;
            // 
            // panelBackColor
            // 
            panelBackColor.BorderStyle = BorderStyle.FixedSingle;
            panelBackColor.Location = new Point(785, 11);
            panelBackColor.Margin = new Padding(3, 3, 8, 3);
            panelBackColor.Name = "panelBackColor";
            panelBackColor.Size = new Size(24, 24);
            panelBackColor.TabIndex = 10;
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(820, 11);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(75, 23);
            buttonOpen.TabIndex = 11;
            buttonOpen.Text = "Открыть";
            buttonOpen.Click += buttonOpen_Click;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(901, 11);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 12;
            buttonSave.Text = "Сохранить";
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonClear
            // 
            buttonClear.Location = new Point(982, 11);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(75, 23);
            buttonClear.TabIndex = 13;
            buttonClear.Text = "Очистить";
            buttonClear.Click += buttonClear_Click;
            // 
            // buttonResetView
            // 
            buttonResetView.Location = new Point(11, 41);
            buttonResetView.Name = "buttonResetView";
            buttonResetView.Size = new Size(75, 23);
            buttonResetView.TabIndex = 14;
            buttonResetView.Text = "Сброс вида";
            buttonResetView.Click += buttonResetView_Click;
            // 
            // panelFourier
            // 
            panelFourier.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelFourier.BackColor = Color.White;
            panelFourier.BorderStyle = BorderStyle.FixedSingle;
            panelFourier.Location = new Point(12, 50);
            panelFourier.Name = "panelFourier";
            panelFourier.Size = new Size(1076, 197);
            panelFourier.TabIndex = 0;
            panelFourier.Paint += panelFourier_Paint;
            // 
            // buttonStartFourier
            // 
            buttonStartFourier.Location = new Point(360, 12);
            buttonStartFourier.Name = "buttonStartFourier";
            buttonStartFourier.Size = new Size(100, 23);
            buttonStartFourier.TabIndex = 1;
            buttonStartFourier.Text = "Пуск";
            buttonStartFourier.Click += buttonStartFourier_Click;
            // 
            // numericUpDownHarmonics
            // 
            numericUpDownHarmonics.Location = new Point(270, 12);
            numericUpDownHarmonics.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numericUpDownHarmonics.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownHarmonics.Name = "numericUpDownHarmonics";
            numericUpDownHarmonics.Size = new Size(75, 23);
            numericUpDownHarmonics.TabIndex = 2;
            numericUpDownHarmonics.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // comboBoxFourierType
            // 
            comboBoxFourierType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFourierType.Items.AddRange(new object[] { "Прямоугольный", "Треугольный", "Пилообразный" });
            comboBoxFourierType.Location = new Point(105, 12);
            comboBoxFourierType.Name = "comboBoxFourierType";
            comboBoxFourierType.Size = new Size(150, 23);
            comboBoxFourierType.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 15);
            label2.Name = "label2";
            label2.Size = new Size(87, 15);
            label2.TabIndex = 4;
            label2.Text = "Сигнал Фурье:";
            // 
            // Lab3Part1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(splitContainer1);
            Name = "Lab3Part1";
            Text = "ЛР 3 - Часть 1. Редактор и сигналы Фурье";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxCanvas).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPenWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHarmonics).EndInit();
            ResumeLayout(false);
        }
    }
}
