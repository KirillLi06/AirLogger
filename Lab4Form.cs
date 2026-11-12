using LabParsingDll;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace WinFormsApp1
{
    public class Lab4Form : Form
    {
        private readonly Form1? mainForm;
        private readonly RichTextBox richLog = new();
        private readonly TextBox textResult = new();
        private readonly ComboBox comboSignature = new();
        private readonly DateTimePicker dateFrom = new();
        private readonly DateTimePicker dateTo = new();
        private readonly Dictionary<string, Color> signatureColors = new(StringComparer.OrdinalIgnoreCase);

        public Lab4Form(Form1? mainForm = null)
        {
            this.mainForm = mainForm;
            Text = "ЛР 4 — парсинг лог-журнала, вариант 2";
            Width = 1200;
            Height = 760;
            StartPosition = FormStartPosition.CenterScreen;
            BuildInterface();
            DetailedActionLogger.Attach(this);
            LoadMainLog();
            AppLogger.Log("ЛР4: форма открыта.");
        }

        private void BuildInterface()
        {
            FlowLayoutPanel top = new()
            {
                Dock = DockStyle.Top,
                Height = 95,
                Padding = new Padding(8),
                AutoScroll = true,
                WrapContents = false
            };

            Button btnOpen = new() { Text = "Открыть лог из файла", Width = 145, Height = 30 };
            btnOpen.Click += (_, _) => OpenLogFile();

            Button btnMain = new() { Text = "Взять лог 1-й формы", Width = 150, Height = 30 };
            btnMain.Click += (_, _) => LoadMainLog();

            Button btnSample = new() { Text = "Тестовый лог", Width = 105, Height = 30 };
            btnSample.Click += (_, _) => SetLogText(MakeSampleLog(), "Загружен тестовый лог.");

            comboSignature.Width = 210;
            comboSignature.DropDownStyle = ComboBoxStyle.DropDown;
            comboSignature.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboSignature.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboSignature.Items.AddRange(new object[] { "StrintToNumber", "ИСКЛЮЧЕНИЕ", "Дата журнала", "Имя формы" });
            comboSignature.SelectedIndex = 0;
            comboSignature.SelectedIndexChanged += (_, _) => HighlightSelectedSignature();
            comboSignature.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    HighlightSelectedSignature();
                }
            };

            Button btnHighlight = new() { Text = "Выделить включения", Width = 150, Height = 30 };
            btnHighlight.Click += (_, _) => HighlightSelectedSignature();

            dateFrom.Format = DateTimePickerFormat.Custom;
            dateFrom.CustomFormat = "HH:mm:ss";
            dateFrom.ShowUpDown = true;
            dateFrom.Width = 90;
            dateFrom.Value = DateTime.Today.AddHours(0);

            dateTo.Format = DateTimePickerFormat.Custom;
            dateTo.CustomFormat = "HH:mm:ss";
            dateTo.ShowUpDown = true;
            dateTo.Width = 90;
            dateTo.Value = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);

            Button btnVariant = new() { Text = "Вариант 2: StrintToNumber по времени", Width = 235, Height = 30 };
            btnVariant.Click += (_, _) => AnalyzeVariant2();

            Button btnForms = new() { Text = "Исключения по формам", Width = 175, Height = 30 };
            btnForms.Click += (_, _) => AnalyzeFormsInLog();

            Button btnClear = new() { Text = "Очистить выделение", Width = 145, Height = 30 };
            btnClear.Click += (_, _) => ClearFormatting();

            top.Controls.AddRange(new Control[]
            {
                btnOpen, btnMain, btnSample,
                new Label { Text = "Сигнатура:", AutoSize = true, Padding = new Padding(8, 7, 0, 0) }, comboSignature, btnHighlight,
                new Label { Text = "Время от:", AutoSize = true, Padding = new Padding(8, 7, 0, 0) }, dateFrom,
                new Label { Text = "до:", AutoSize = true, Padding = new Padding(8, 7, 0, 0) }, dateTo,
                btnVariant, btnForms, btnClear
            });

            SplitContainer split = new()
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 500
            };

            richLog.Dock = DockStyle.Fill;
            richLog.Font = new Font("Consolas", 10);
            richLog.WordWrap = false;
            richLog.HideSelection = false;

            textResult.Dock = DockStyle.Fill;
            textResult.Multiline = true;
            textResult.ScrollBars = ScrollBars.Both;
            textResult.ReadOnly = true;
            textResult.WordWrap = false;
            textResult.Font = new Font("Consolas", 10);

            split.Panel1.Controls.Add(richLog);
            split.Panel2.Controls.Add(textResult);
            Controls.Add(split);
            Controls.Add(top);
        }

        private void OpenLogFile()
        {
            try
            {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Лог-журналы и текстовые файлы|*.txt;*.log|Все файлы|*.*";
                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                string text = File.ReadAllText(ofd.FileName, Encoding.UTF8);
                SetLogText(text, "Открыт лог-файл: " + ofd.FileName);
                AppLogger.Log("ЛР4: открыт лог-файл " + ofd.FileName);
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Lab4Form.OpenLogFile");
                MessageBox.Show("Не удалось открыть лог: " + ex.Message, "ЛР4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMainLog()
        {
            try
            {
                string text = string.Empty;
                if (mainForm != null)
                    text = mainForm.GetMainLogText();
                if (string.IsNullOrWhiteSpace(text) && File.Exists(AppLogger.LogFilePath))
                    text = File.ReadAllText(AppLogger.LogFilePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(text))
                    text = MakeSampleLog();

                SetLogText(text, "Загружен лог из первой формы/файла app_log.txt.");
                AppLogger.Log("ЛР4: загружен основной лог.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Lab4Form.LoadMainLog");
                MessageBox.Show("Не удалось загрузить основной лог: " + ex.Message, "ЛР4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetLogText(string text, string status)
        {
            richLog.Text = text ?? string.Empty;
            textResult.Text = status + Environment.NewLine + "Теперь можно выбрать сигнатуру или выполнить вариант 2.";
            ClearFormatting();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(richLog.Text))
            {
                MessageBox.Show("Лог-журнал пустой.", "ЛР4", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void HighlightSelectedSignature()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(richLog.Text))
                    return;

                string signature = comboSignature.Text.Trim();

                if (string.IsNullOrWhiteSpace(signature))
                {
                    MessageBox.Show("Введите сигнатуру.", "ЛР4", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                AddUserSignatureToComboBox(signature);

                Color color = GetColorForSignature(signature);

                ParserResult result;
                string pattern;

                if (IsStandardSignature(signature))
                {
                    ILogRegexParser parser = ParserFactory.CreateByUiName(signature);
                    result = parser.Parse(richLog.Text);
                    pattern = parser.Pattern;
                }
                else
                {
                    result = ParseByUserRegex(signature);
                    pattern = signature;
                }

                HighlightMatches(result.Matches, color, clearBefore: false);

                textResult.Text =
                    $"Сигнатура: {signature}{Environment.NewLine}" +
                    $"Regex: {pattern}{Environment.NewLine}" +
                    $"Цвет RGB: R={color.R}, G={color.G}, B={color.B}{Environment.NewLine}" +
                    $"Найдено включений: {result.Count}{Environment.NewLine}" +
                    "Выделения не очищаются: каждая новая сигнатура подсвечивается своим цветом.";

                AppLogger.Log($"ЛР4: выделена сигнатура {signature}, RGB({color.R},{color.G},{color.B}), найдено {result.Count}.");
            }
            catch (ArgumentException ex)
            {
                AppLogger.LogException(ex, "Lab4Form.HighlightSelectedSignature");
                MessageBox.Show("Ошибка регулярного выражения: " + ex.Message, "ЛР4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Lab4Form.HighlightSelectedSignature");
                MessageBox.Show("Ошибка выделения сигнатур: " + ex.Message, "ЛР4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddUserSignatureToComboBox(string signature)
        {
            if (string.IsNullOrWhiteSpace(signature))
                return;

            foreach (object item in comboSignature.Items)
            {
                if (string.Equals(item.ToString(), signature, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            comboSignature.Items.Add(signature);
        }
        private static bool IsStandardSignature(string signature)
        {
            return signature.Equals("StrintToNumber", StringComparison.OrdinalIgnoreCase)
                || signature.Equals("ИСКЛЮЧЕНИЕ", StringComparison.OrdinalIgnoreCase)
                || signature.Equals("Дата журнала", StringComparison.OrdinalIgnoreCase)
                || signature.Equals("Имя формы", StringComparison.OrdinalIgnoreCase);
        }

        private Color GetColorForSignature(string signature)
        {
            string key = signature.Trim();

            if (!signatureColors.TryGetValue(key, out Color color))
            {
                color = GetColorFromSignature(key);
                signatureColors[key] = color;
            }

            return color;
        }

        private static Color GetColorFromSignature(string signature)
        {
            if (string.IsNullOrWhiteSpace(signature))
                return Color.Yellow;

            using SHA256 sha = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(signature.Trim().ToLowerInvariant());
            byte[] hash = sha.ComputeHash(bytes);

            int r = 80 + hash[0] % 140;
            int g = 80 + hash[1] % 140;
            int b = 80 + hash[2] % 140;

            return Color.FromArgb(r, g, b);
        }

        private ParserResult ParseByUserRegex(string pattern)
        {
            DateTime start = DateTime.Now;

            Regex regex = new Regex(
                pattern,
                RegexOptions.IgnoreCase | RegexOptions.Multiline,
                TimeSpan.FromSeconds(2));

            List<ParserMatch> matches = new();

            Match match = regex.Match(richLog.Text);

            while (match.Success)
            {
                matches.Add(new ParserMatch(
                    match.Index,
                    match.Length,
                    match.Value,
                    null,
                    "Пользовательская сигнатура"));

                match = match.NextMatch();
            }

            long elapsed = (long)(DateTime.Now - start).TotalMilliseconds;

            return new ParserResult(
                pattern,
                matches.Count,
                elapsed,
                matches);
        }
        private void AnalyzeVariant2()
        {
            try
            {
                if (!ValidateInput())
                    return;

                StrintToNumberRegexParser parser = new();
                ParserResult result = parser.Parse(richLog.Text, dateFrom.Value, dateTo.Value);
                HighlightMatches(result.Matches, Color.Gold, clearBefore: true);

                StringBuilder sb = new();
                sb.AppendLine("Вариант 2.");
                sb.AppendLine("Задача: выделить все включения StrintToNumber без учета регистра, если исключение попало в интервал времени.");
                sb.AppendLine($"Интервал времени: {dateFrom.Value:HH:mm:ss} — {dateTo.Value:HH:mm:ss}");
                sb.AppendLine($"Найдено включений StrintToNumber: {result.Count}");
                sb.AppendLine($"Время парсинга: {result.ElapsedMilliseconds} мс");
                sb.AppendLine();
                foreach (ParserMatch m in result.Matches.Take(100))
                    sb.AppendLine($"{m.LogDate:dd.MM.yyyy HH:mm:ss} | позиция {m.Index} | {m.Value}");
                if (result.Matches.Count > 100)
                    sb.AppendLine("... показаны первые 100 совпадений.");

                textResult.Text = sb.ToString();
                AppLogger.Log($"ЛР4: выполнен вариант 2, найдено {result.Count} StrintToNumber.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Lab4Form.AnalyzeVariant2");
                MessageBox.Show("Ошибка анализа варианта 2: " + ex.Message, "ЛР4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AnalyzeFormsInLog()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(richLog.Text))
                    return;

                List<FormExceptionLine> exceptionLines = FormExceptionAnalyzer.FindExceptionFormLines(richLog.Text);
                List<FormExceptionCount> counts = FormExceptionAnalyzer.CountForms(richLog.Text);

                // Подсвечиваем именно строки, где одновременно есть ИСКЛЮЧЕНИЕ и любое слово вида \w*Form\w*.
                List<ParserMatch> lineMatches = exceptionLines
                    .Select(x => new ParserMatch(x.LineStartIndex, x.LineLength, x.LineText, null, "Строка: ИСКЛЮЧЕНИЕ + Form"))
                    .ToList();
                HighlightMatches(lineMatches, Color.LightGreen, clearBefore: true);

                StringBuilder sb = new();
                sb.AppendLine("Анализ исключений по формам.");
                sb.AppendLine("Условие поиска: строка должна одновременно содержать слово «ИСКЛЮЧЕНИЕ» и любое слово содержащее Form.");
                sb.AppendLine();

                if (counts.Count == 0)
                {
                    sb.AppendLine("Исключения по формам не найдены.");
                }
                else
                {
                    sb.AppendLine("Количество исключений по формам:");
                    foreach (FormExceptionCount item in counts)
                        sb.AppendLine($"{item.FormName}: {item.Count}");

                    sb.AppendLine();
                    sb.AppendLine("Строки, подходящие под условие:");
                    int number = 1;
                    foreach (FormExceptionLine line in exceptionLines)
                        sb.AppendLine($"{number++}. [{line.FormName}] {line.LineText}");
                }

                textResult.Text = sb.ToString();
                AppLogger.Log($"ЛР4: выполнен анализ строк с ИСКЛЮЧЕНИЕ + Form, найдено строк: {exceptionLines.Count}, форм: {counts.Count}.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Lab4Form.AnalyzeFormsInLog");
                MessageBox.Show("Ошибка анализа форм: " + ex.Message, "ЛР4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HighlightMatches(IEnumerable<ParserMatch> matches, Color color, bool clearBefore)
        {
            if (clearBefore)
                ClearFormatting();

            int oldStart = richLog.SelectionStart;
            int oldLength = richLog.SelectionLength;
            try
            {
                richLog.SuspendLayout();
                foreach (ParserMatch match in matches)
                {
                    if (match.Index < 0 || match.Index + match.Length > richLog.TextLength)
                        continue;
                    richLog.Select(match.Index, match.Length);
                    richLog.SelectionBackColor = color;
                    richLog.SelectionColor = Color.Black;
                }
            }
            finally
            {
                richLog.Select(Math.Min(oldStart, richLog.TextLength), Math.Min(oldLength, Math.Max(0, richLog.TextLength - oldStart)));
                richLog.ResumeLayout();
            }
        }

        private void ClearFormatting()
        {
            int oldStart = richLog.SelectionStart;
            int oldLength = richLog.SelectionLength;
            richLog.SelectAll();
            richLog.SelectionBackColor = richLog.BackColor;
            richLog.SelectionColor = richLog.ForeColor;
            richLog.Select(Math.Min(oldStart, richLog.TextLength), Math.Min(oldLength, Math.Max(0, richLog.TextLength - oldStart)));
        }

        private static string MakeSampleLog()
        {
            return string.Join(Environment.NewLine, new[]
            {
                "[14.03.2026 00:44:59] Приложение запущено.",
                "[14.03.2026 00:45:12] ИСКЛЮЧЕНИЕ в FormStrintToNumberTest.button1_Click",
                "Сообщение: Входная строка имела неверный формат.",
                "Метод: StrintToNumber(String value)",
                new string('-', 70),
                "[20.04.2026 12:10:15] ИСКЛЮЧЕНИЕ в Lab2.buttonCompare_Click",
                "Сообщение: Ошибка преобразования.",
                "Метод: system.helpers.strinttonumber(String value)",
                new string('-', 70),
                "[25.04.2026 09:30:00] ИСКЛЮЧЕНИЕ в Form1.открытьToolStripMenuItem_Click",
                "Сообщение: Время вне выбранного интервала.",
                "Метод: StrintToNumber"
            });
        }
    }
}
