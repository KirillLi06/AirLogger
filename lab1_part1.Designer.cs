namespace WinFormsApp1
{
    partial class lab1_part1
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
            buttonAdd = new Button();
            textBoxS1 = new TextBox();
            textBoxS2 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            richTextBoxLog = new RichTextBox();
            SuspendLayout();
            // 
            // buttonAdd
            // 
            buttonAdd.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            buttonAdd.Location = new Point(49, 260);
            buttonAdd.Margin = new Padding(2, 1, 2, 1);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(110, 55);
            buttonAdd.TabIndex = 0;
            buttonAdd.Text = "Сложить поразрядно";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // textBoxS1
            // 
            textBoxS1.Location = new Point(49, 113);
            textBoxS1.Margin = new Padding(2, 1, 2, 1);
            textBoxS1.Name = "textBoxS1";
            textBoxS1.Size = new Size(110, 23);
            textBoxS1.TabIndex = 1;
            // 
            // textBoxS2
            // 
            textBoxS2.Location = new Point(49, 185);
            textBoxS2.Margin = new Padding(2, 1, 2, 1);
            textBoxS2.Name = "textBoxS2";
            textBoxS2.Size = new Size(110, 23);
            textBoxS2.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(49, 97);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(34, 15);
            label1.TabIndex = 4;
            label1.Text = "Имя:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(49, 169);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(61, 15);
            label2.TabIndex = 5;
            label2.Text = "Фамилия:";
            // 
            // richTextBoxLog
            // 
            richTextBoxLog.Location = new Point(214, 20);
            richTextBoxLog.Name = "richTextBoxLog";
            richTextBoxLog.Size = new Size(484, 404);
            richTextBoxLog.TabIndex = 6;
            richTextBoxLog.Text = "";
            // 
            // lab1_part1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(715, 442);
            Controls.Add(richTextBoxLog);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxS2);
            Controls.Add(textBoxS1);
            Controls.Add(buttonAdd);
            Margin = new Padding(2, 1, 2, 1);
            Name = "lab1_part1";
            Text = "lab1_part1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonAdd;
        private TextBox textBoxS1;
        private TextBox textBoxS2;
        private Label label1;
        private Label label2;
        private RichTextBox richTextBoxLog;
    }
}