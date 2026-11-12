using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class lab1_part1 : Form
    {
        public lab1_part1()
        {
            InitializeComponent();
            DetailedActionLogger.Attach(this);
        }

        private static byte AddByteBitwise(byte a, byte b, ref int carry, StringBuilder log, int byteIndex)
        {
            int result = 0;
            int carryIn = carry;

            log.AppendLine($"--- Byte #{byteIndex} ---");
            log.AppendLine($"A = {a} (0x{a:X2})  {Convert.ToString(a, 2).PadLeft(8, '0')}");
            log.AppendLine($"B = {b} (0x{b:X2})  {Convert.ToString(b, 2).PadLeft(8, '0')}");
            log.AppendLine($"CarryIn = {carryIn}");

            for (int bit = 0; bit < 8; bit++)
            {
                int abit = (a >> bit) & 1;
                int bbit = (b >> bit) & 1;

                int sumBit = abit ^ bbit ^ carry;
                int newCarry = (abit & bbit) | (carry & (abit ^ bbit));

                result |= (sumBit << bit);

                log.AppendLine($" bit {bit}: a={abit} b={bbit} sum={sumBit} carry_out={newCarry}");
                carry = newCarry;
            }

            log.AppendLine($"ResultByte = {result} (0x{result:X2})  {Convert.ToString(result, 2).PadLeft(8, '0')}");
            log.AppendLine($"CarryOut = {carry}");
            log.AppendLine();

            return (byte)result;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string s1 = textBoxS1.Text ?? "";
                string s2 = textBoxS2.Text ?? "";

                if (string.IsNullOrWhiteSpace(s1) || string.IsNullOrWhiteSpace(s2))
                    throw new ArgumentException("Обе строки должны быть непустыми.");

                byte[] aBytes = Encoding.UTF8.GetBytes(s1);
                byte[] bBytes = Encoding.UTF8.GetBytes(s2);

                int maxLen = Math.Max(aBytes.Length, bBytes.Length);
                Array.Resize(ref aBytes, maxLen);
                Array.Resize(ref bBytes, maxLen);

                var log = new StringBuilder();
                log.AppendLine("=== Побитовое сложение двух строк (UTF-8) ===");
                log.AppendLine($"S1: \"{s1}\"");
                log.AppendLine($"S2: \"{s2}\"");
                log.AppendLine();

                byte[] sum = new byte[maxLen];
                int carry = 0;

                for (int i = 0; i < maxLen; i++)
                    sum[i] = AddByteBitwise(aBytes[i], bBytes[i], ref carry, log, i);

                log.AppendLine("=== Final ===");
                log.AppendLine($"Carry after last byte: {carry}");
                log.AppendLine("SUM (hex): " + string.Join(" ", sum.Select(x => x.ToString("X2"))));

                richTextBoxLog.Text = log.ToString();
                AppLogger.Log("ЛР1 Часть 1 выполнена успешно.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "lab1_part1.buttonAdd_Click");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
