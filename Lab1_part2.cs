using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Lab1_part2 : Form
    {
        public Lab1_part2()
        {
            InitializeComponent();
            DetailedActionLogger.Attach(this);
        }

        private static int CountEvenDigits(ulong n)
        {
            int count = 0;
            while (n > 0)
            {
                ulong d = n % 10;
                if ((d % 2) == 0) count++;
                n /= 10;
            }
            return count;
        }

        private static bool ContainsDigit(ulong n, int digit)
        {
            if (digit < 0 || digit > 9) return false;
            if (n == 0) return digit == 0;

            while (n > 0)
            {
                int d = (int)(n % 10);
                if (d == digit) return true;
                n /= 10;
            }
            return false;
        }

        private static List<int> Symmetric4DigitNumbers()
        {
            var list = new List<int>();
            for (int a = 1; a <= 9; a++)
                for (int b = 0; b <= 9; b++)
                    list.Add(a * 1000 + b * 100 + b * 10 + a);
            return list;
        }

        private void buttonSolve1_Click(object sender, EventArgs e)
        {
            if (!ulong.TryParse(textBoxN.Text, out ulong n) || n == 0)
            {
                MessageBox.Show("Ошибка: N должно быть натуральным (целым > 0).");
                return;
            }

            if (!int.TryParse(textBoxA.Text, out int A) || A < 0 || A > 9)
            {
                MessageBox.Show("Ошибка: A должна быть цифрой 0..9.");
                return;
            }

            if (!int.TryParse(textBoxB.Text, out int B) || B < 0 || B > 9)
            {
                MessageBox.Show("Ошибка: B должна быть цифрой 0..9.");
                return;
            }

            int evenCount = CountEvenDigits(n);
            bool hasA = ContainsDigit(n, A);
            bool hasB = ContainsDigit(n, B);

            var sb = new StringBuilder();
            sb.AppendLine("=== Вариант 2: Пункт 1 ===");
            sb.AppendLine($"N = {n}");
            sb.AppendLine($"Чётных цифр: {evenCount}");
            sb.AppendLine($"A={A}: {(hasA ? "встречается" : "НЕ встречается")}");
            sb.AppendLine($"B={B}: {(hasB ? "встречается" : "НЕ встречается")}");
            sb.AppendLine($"Обе цифры A и B: {(hasA && hasB ? "ДА" : "НЕТ")}");

            richTextBoxOut.Text = sb.ToString();
        }

        private void buttonShowSym_Click(object sender, EventArgs e)
        {
            try
            {
                var sym = Symmetric4DigitNumbers();

                var sb = new StringBuilder();
                sb.AppendLine("=== Вариант 2: Пункт 2 ===");
                sb.AppendLine("Симметричные четырёхзначные числа (abba):");
                sb.AppendLine();

                for (int i = 0; i < sym.Count; i++)
                {
                    sb.Append(sym[i]);
                    if (i != sym.Count - 1) sb.Append(", ");
                    if ((i + 1) % 15 == 0) sb.AppendLine();
                }

                sb.AppendLine();
                sb.AppendLine($"Всего: {sym.Count}");

                richTextBoxOut.Text = sb.ToString();
                AppLogger.Log("ЛР1 Часть 2: выполнен расчёт варианта 2.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Lab1_part2.buttonShowSym_Click");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}