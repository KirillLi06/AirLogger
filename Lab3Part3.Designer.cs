namespace WinFormsApp1
{
    partial class Lab3Part3
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelPlanetSystem;
        private NumericUpDown numericUpDownDepth;
        private NumericUpDown numericUpDownSatellites;
        private NumericUpDown numericUpDownOrbit;
        private NumericUpDown numericUpDownSatelliteRatio;
        private Button buttonBuild;
        private TextBox textBoxTrace;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button buttonResetView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelPlanetSystem = new Panel();
            numericUpDownDepth = new NumericUpDown();
            numericUpDownSatellites = new NumericUpDown();
            numericUpDownOrbit = new NumericUpDown();
            numericUpDownSatelliteRatio = new NumericUpDown();
            buttonBuild = new Button();
            textBoxTrace = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            buttonResetView = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDepth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSatellites).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownOrbit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSatelliteRatio).BeginInit();
            SuspendLayout();
            // 
            // panelPlanetSystem
            // 
            panelPlanetSystem.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panelPlanetSystem.BackColor = Color.Black;
            panelPlanetSystem.BorderStyle = BorderStyle.FixedSingle;
            panelPlanetSystem.Location = new Point(12, 50);
            panelPlanetSystem.Name = "panelPlanetSystem";
            panelPlanetSystem.Size = new Size(800, 588);
            panelPlanetSystem.TabIndex = 6;
            panelPlanetSystem.TabStop = true;
            panelPlanetSystem.Paint += panelPlanetSystem_Paint;
            panelPlanetSystem.MouseDown += panelPlanetSystem_MouseDown;
            panelPlanetSystem.MouseMove += panelPlanetSystem_MouseMove;
            panelPlanetSystem.MouseUp += panelPlanetSystem_MouseUp;
            panelPlanetSystem.MouseWheel += panelPlanetSystem_MouseWheel;
            panelPlanetSystem.Resize += panelPlanetSystem_Resize;
            // 
            // numericUpDownDepth
            // 
            numericUpDownDepth.Location = new Point(80, 12);
            numericUpDownDepth.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numericUpDownDepth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownDepth.Name = "numericUpDownDepth";
            numericUpDownDepth.Size = new Size(120, 23);
            numericUpDownDepth.TabIndex = 11;
            numericUpDownDepth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownDepth.ValueChanged += ParamsChanged;
            // 
            // numericUpDownSatellites
            // 
            numericUpDownSatellites.Location = new Point(281, 12);
            numericUpDownSatellites.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            numericUpDownSatellites.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownSatellites.Name = "numericUpDownSatellites";
            numericUpDownSatellites.Size = new Size(120, 23);
            numericUpDownSatellites.TabIndex = 10;
            numericUpDownSatellites.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownSatellites.ValueChanged += ParamsChanged;
            // 
            // numericUpDownOrbit
            // 
            numericUpDownOrbit.DecimalPlaces = 1;
            numericUpDownOrbit.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numericUpDownOrbit.Location = new Point(472, 12);
            numericUpDownOrbit.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
            numericUpDownOrbit.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numericUpDownOrbit.Name = "numericUpDownOrbit";
            numericUpDownOrbit.Size = new Size(120, 23);
            numericUpDownOrbit.TabIndex = 9;
            numericUpDownOrbit.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numericUpDownOrbit.ValueChanged += ParamsChanged;
            // 
            // numericUpDownSatelliteRatio
            // 
            numericUpDownSatelliteRatio.Location = new Point(692, 14);
            numericUpDownSatelliteRatio.Maximum = new decimal(new int[] { 80, 0, 0, 0 });
            numericUpDownSatelliteRatio.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDownSatelliteRatio.Name = "numericUpDownSatelliteRatio";
            numericUpDownSatelliteRatio.Size = new Size(120, 23);
            numericUpDownSatelliteRatio.TabIndex = 8;
            numericUpDownSatelliteRatio.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDownSatelliteRatio.ValueChanged += ParamsChanged;
            // 
            // buttonBuild
            // 
            buttonBuild.Location = new Point(822, 12);
            buttonBuild.Name = "buttonBuild";
            buttonBuild.Size = new Size(130, 25);
            buttonBuild.TabIndex = 7;
            buttonBuild.Text = "Построить";
            buttonBuild.Click += buttonBuild_Click;
            // 
            // textBoxTrace
            // 
            textBoxTrace.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxTrace.Location = new Point(825, 50);
            textBoxTrace.Multiline = true;
            textBoxTrace.Name = "textBoxTrace";
            textBoxTrace.ScrollBars = ScrollBars.Vertical;
            textBoxTrace.Size = new Size(263, 588);
            textBoxTrace.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 4;
            label1.Text = "Уровни N";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(209, 16);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 3;
            label2.Text = "Спутников";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(407, 17);
            label3.Name = "label3";
            label3.Size = new Size(59, 15);
            label3.TabIndex = 2;
            label3.Text = "К орбиты";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(598, 16);
            label4.Name = "label4";
            label4.Size = new Size(88, 15);
            label4.TabIndex = 1;
            label4.Text = "К спутника (%)";
            // 
            // buttonResetView
            // 
            buttonResetView.Location = new Point(958, 12);
            buttonResetView.Name = "buttonResetView";
            buttonResetView.Size = new Size(130, 25);
            buttonResetView.TabIndex = 0;
            buttonResetView.Text = "Сброс вида";
            buttonResetView.Click += buttonResetView_Click;
            // 
            // Lab3Part3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 650);
            Controls.Add(buttonResetView);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxTrace);
            Controls.Add(panelPlanetSystem);
            Controls.Add(buttonBuild);
            Controls.Add(numericUpDownSatelliteRatio);
            Controls.Add(numericUpDownOrbit);
            Controls.Add(numericUpDownSatellites);
            Controls.Add(numericUpDownDepth);
            Name = "Lab3Part3";
            Text = "ЛР 3 - Часть 3. Планетарная система";
            Load += Lab3Part3_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDownDepth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSatellites).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownOrbit).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSatelliteRatio).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
