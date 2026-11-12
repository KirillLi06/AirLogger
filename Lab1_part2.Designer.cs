namespace WinFormsApp1
{
    partial class Lab1_part2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBoxN = new TextBox();
            textBoxA = new TextBox();
            textBoxB = new TextBox();
            button1 = new Button();
            button2 = new Button();
            richTextBoxOut = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // textBoxN
            // 
            textBoxN.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxN.Location = new Point(172, 79);
            textBoxN.Name = "textBoxN";
            textBoxN.Size = new Size(172, 29);
            textBoxN.TabIndex = 0;
            // 
            // textBoxA
            // 
            textBoxA.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxA.Location = new Point(172, 149);
            textBoxA.Name = "textBoxA";
            textBoxA.Size = new Size(172, 29);
            textBoxA.TabIndex = 1;
            // 
            // textBoxB
            // 
            textBoxB.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxB.Location = new Point(172, 223);
            textBoxB.Name = "textBoxB";
            textBoxB.Size = new Size(172, 29);
            textBoxB.TabIndex = 2;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button1.Location = new Point(172, 289);
            button1.Name = "button1";
            button1.Size = new Size(172, 72);
            button1.TabIndex = 3;
            button1.Text = "Проверить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += buttonSolve1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button2.Location = new Point(172, 367);
            button2.Name = "button2";
            button2.Size = new Size(172, 73);
            button2.TabIndex = 4;
            button2.Text = "Найти все симметричные 4-знычные числа";
            button2.UseVisualStyleBackColor = true;
            button2.Click += buttonShowSym_Click;
            // 
            // richTextBoxOut
            // 
            richTextBoxOut.Location = new Point(408, 22);
            richTextBoxOut.Name = "richTextBoxOut";
            richTextBoxOut.Size = new Size(464, 445);
            richTextBoxOut.TabIndex = 5;
            richTextBoxOut.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(172, 55);
            label1.Name = "label1";
            label1.Size = new Size(54, 21);
            label1.TabIndex = 6;
            label1.Text = "Число";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(172, 125);
            label2.Name = "label2";
            label2.Size = new Size(73, 21);
            label2.TabIndex = 7;
            label2.Text = "Цифра А";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(172, 199);
            label3.Name = "label3";
            label3.Size = new Size(72, 21);
            label3.TabIndex = 8;
            label3.Text = "Цифра В";
            // 
            // Lab1_part2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(893, 489);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(richTextBoxOut);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBoxB);
            Controls.Add(textBoxA);
            Controls.Add(textBoxN);
            Margin = new Padding(2, 1, 2, 1);
            Name = "Lab1_part2";
            Text = "Lab1_part2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxN;
        private TextBox textBoxA;
        private TextBox textBoxB;
        private Button button1;
        private Button button2;
        private RichTextBox richTextBoxOut;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}