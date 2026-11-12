using LabParsingDll;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class Lab5Form : Form
    {
        private sealed record FileParseResult(string FileName, int SizeKb, int Count, long ReadMilliseconds, long ParseMilliseconds, long TotalMilliseconds, string? Error);

        private readonly ListBox listFiles = new();
        private readonly TextBox textResult = new();
        private readonly DateTimePicker dateFrom = new();
        private readonly DateTimePicker dateTo = new();
        private readonly List<string> files = new();
        private readonly List<BenchmarkPoint> lastPoints = new();
        private CancellationTokenSource? cancellationTokenSource;

        // Число контрольных проходов нужно, чтобы замер был устойчивым даже на маленьких логах.
        // Count в таблице остается реальным количеством StrintToNumber в одном журнале, а время — временем серии одинаковых проходов.
        private const int BenchmarkPasses = 250;

        public Lab5Form()
        {
            Text = "ЛР 5 — DLL и асинхронная обработка, вариант 2";
            Width = 1180;
            Height = 740;
            StartPosition = FormStartPosition.CenterScreen;
            BuildInterface();
            DetailedActionLogger.Attach(this);
            AppLogger.Log("ЛР5: форма открыта.");
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

            Button btnAdd = new() { Text = "Добавить журналы", Width = 140, Height = 30 };
            btnAdd.Click += (_, _) => AddFiles();

            Button btnMainLog = new() { Text = "Добавить app_log", Width = 125, Height = 30 };
            btnMainLog.Click += (_, _) => AddMainLog();

            Button btnRemove = new() { Text = "Убрать выбранный", Width = 130, Height = 30 };
            btnRemove.Click += (_, _) => RemoveSelectedFile();

            Button btnRun = new() { Text = "Обычный парсинг", Width = 145, Height = 30 };
            btnRun.Click += async (_, _) => await RunParsingAsync(asyncMode: false);

            Button btnRunAsync = new() { Text = "Асинхронный парсинг", Width = 165, Height = 30 };
            btnRunAsync.Click += async (_, _) => await RunParsingAsync(asyncMode: true);

            Button btnCompare = new() { Text = "Сравнить режимы", Width = 145, Height = 30 };
            btnCompare.Click += async (_, _) => await CompareModesAsync();

            Button btnCancel = new() { Text = "Отмена", Width = 85, Height = 30 };
            btnCancel.Click += (_, _) => cancellationTokenSource?.Cancel();

            Button btnChart = new() { Text = "Показать график", Width = 130, Height = 30 };
            btnChart.Click += (_, _) => new Lab5ChartForm(lastPoints.ToList()).Show();

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

            top.Controls.AddRange(new Control[]
            {
                btnAdd, btnMainLog, btnRemove,
                new Label { Text = "Время от:", AutoSize = true, Padding = new Padding(8, 7, 0, 0) }, dateFrom,
                new Label { Text = "до:", AutoSize = true, Padding = new Padding(8, 7, 0, 0) }, dateTo,
                btnRun, btnRunAsync, btnCompare, btnCancel, btnChart
            });

            SplitContainer split = new()
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 330,
                Orientation = Orientation.Vertical
            };

            listFiles.Dock = DockStyle.Fill;
            listFiles.Font = new System.Drawing.Font("Consolas", 9);
            listFiles.HorizontalScrollbar = true;

            textResult.Dock = DockStyle.Fill;
            textResult.Multiline = true;
            textResult.ScrollBars = ScrollBars.Both;
            textResult.ReadOnly = true;
            textResult.WordWrap = false;
            textResult.Font = new System.Drawing.Font("Consolas", 10);
            textResult.Text = "Добавь один или несколько сохраненных лог-журналов. ЛР5 использует DLL: каждый Regex-парсер вынесен в отдельный класс.";

            split.Panel1.Controls.Add(listFiles);
            split.Panel2.Controls.Add(textResult);
            Controls.Add(split);
            Controls.Add(top);
        }

        private void AddFiles()
        {
            try
            {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Лог-журналы и текстовые файлы|*.txt;*.log|Все файлы|*.*";
                ofd.Multiselect = true;
                ofd.CheckFileExists = true;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                foreach (string file in ofd.FileNames)
                    AddFile(file);

                textResult.Text = $"Добавлено файлов: {ofd.FileNames.Length}. Всего выбрано журналов: {files.Count}.";
                AppLogger.Log($"ЛР5: добавлено журналов {ofd.FileNames.Length}.");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, "Lab5Form.AddFiles");
                MessageBox.Show("Ошибка добавления файлов: " + ex.Message, "ЛР5", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddMainLog()
        {
            if (!File.Exists(AppLogger.LogFilePath))
            {
                MessageBox.Show("Файл app_log.txt пока не создан.", "ЛР5", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AddFile(AppLogger.LogFilePath);
            textResult.Text = "Добавлен основной журнал приложения:\r\n" + AppLogger.LogFilePath;
            AppLogger.Log("ЛР5: добавлен основной журнал app_log.txt.");
        }

        private void AddFile(string file)
        {
            if (!File.Exists(file))
            {
                MessageBox.Show("Файл не найден: " + file, "ЛР5", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!files.Contains(file, StringComparer.OrdinalIgnoreCase))
            {
                files.Add(file);
                listFiles.Items.Add(file);
            }
        }

        private void RemoveSelectedFile()
        {
            if (listFiles.SelectedIndex < 0)
                return;

            int index = listFiles.SelectedIndex;
            files.RemoveAt(index);
            listFiles.Items.RemoveAt(index);
            textResult.Text = $"Выбранный журнал убран. Осталось файлов: {files.Count}.";
            AppLogger.Log("ЛР5: журнал убран из списка.");
        }

        private bool ValidateBeforeParsing()
        {
            if (files.Count == 0)
            {
                MessageBox.Show("Сначала выбери хотя бы один лог-журнал.", "ЛР5", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private FileParseResult ParseOneFileSequential(string file, DateTime from, DateTime to)
        {
            try
            {
                Stopwatch total = Stopwatch.StartNew();
                string text;
                long readMs;
                using (LogFileReader reader = new())
                {
                    Stopwatch read = Stopwatch.StartNew();
                    text = reader.ReadAllText(file);
                    read.Stop();
                    readMs = Math.Max(0, read.ElapsedMilliseconds);
                }

                ILogRegexParser parser = new StrintToNumberRegexParser();
                ParserResult result = new(parser.Name, 0, 0, new List<ParserMatch>());
                Stopwatch parse = Stopwatch.StartNew();
                for (int i = 0; i < BenchmarkPasses; i++)
                    result = LogBenchmarkRunner.RunSequential(text, parser, from, to);
                parse.Stop();

                total.Stop();
                int sizeKb = Math.Max(1, (int)Math.Ceiling(new FileInfo(file).Length / 1024.0));
                return new FileParseResult(Path.GetFileName(file), sizeKb, result.Count, readMs, parse.ElapsedMilliseconds, total.ElapsedMilliseconds, null);
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, $"Lab5Form.ParseOneFileSequential: {file}");
                return new FileParseResult(Path.GetFileName(file), 0, 0, 0, 0, 0, ex.Message);
            }
        }

        private async Task<FileParseResult> ParseOneFileAsync(string file, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            try
            {
                Stopwatch total = Stopwatch.StartNew();
                string text;
                long readMs;
                using (LogFileReader reader = new())
                {
                    Stopwatch read = Stopwatch.StartNew();
                    // Для честного сравнения алгоритмов чтение выполняется тем же способом, что и в обычном режиме.
                    // Асинхронность применяется именно к этапу парсинга: форма не блокируется, а крупные журналы уходят в Task.Run/Parallel.For.
                    text = reader.ReadAllText(file);
                    read.Stop();
                    readMs = Math.Max(0, read.ElapsedMilliseconds);
                }

                Stopwatch parse = Stopwatch.StartNew();
                ParserResult result = await LogBenchmarkRunner.RunAsyncBenchmark(text, from, to, BenchmarkPasses, cancellationToken);
                parse.Stop();

                total.Stop();
                int sizeKb = Math.Max(1, (int)Math.Ceiling(new FileInfo(file).Length / 1024.0));
                return new FileParseResult(Path.GetFileName(file), sizeKb, result.Count, readMs, parse.ElapsedMilliseconds, total.ElapsedMilliseconds, null);
            }
            catch (OperationCanceledException)
            {
                return new FileParseResult(Path.GetFileName(file), 0, 0, 0, 0, 0, "Операция отменена");
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex, $"Lab5Form.ParseOneFileAsync: {file}");
                return new FileParseResult(Path.GetFileName(file), 0, 0, 0, 0, 0, ex.Message);
            }
        }

        private async Task RunParsingAsync(bool asyncMode)
        {
            if (!ValidateBeforeParsing())
                return;

            string processName = asyncMode
                ? "Асинхронный парсинг журналов ЛР5"
                : "Последовательный парсинг журналов ЛР5";

            try
            {
                ProcessStatusService.Start(processName);

                cancellationTokenSource?.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
                CancellationToken token = cancellationTokenSource.Token;
                DateTime from = dateFrom.Value;
                DateTime to = dateTo.Value;

                StringBuilder sb = new();
                Stopwatch all = Stopwatch.StartNew();
                sb.AppendLine(asyncMode
                    ? $"Асинхронный режим: быстрый DLL-парсер StrintToNumber + Task/Parallel, контрольных проходов: {BenchmarkPasses}."
                    : $"Обычный режим: лекционный Regex/Match/NextMatch, контрольных проходов: {BenchmarkPasses}.");
                sb.AppendLine($"Интервал времени: {from:HH:mm:ss} — {to:HH:mm:ss}");
                sb.AppendLine(new string('-', 118));
                sb.AppendLine($"{"Файл",-42} {"Размер, КБ",10} {"StrintToNumber",8} {"Чтение",8} {"Парсинг",9} {"Итого",8} {"Статус",20}");
                sb.AppendLine(new string('-', 118));

                List<FileParseResult> results;
                if (asyncMode)
                {
                    int completed = 0;
                    Task<FileParseResult>[] tasks = files.Select(async f =>
                    {
                        FileParseResult result = await ParseOneFileAsync(f, from, to, token);
                        int done = Interlocked.Increment(ref completed);
                        ProcessStatusService.Report(processName, done * 100 / files.Count);
                        return result;
                    }).ToArray();

                    results = (await Task.WhenAll(tasks)).ToList();
                }
                else
                {
                    results = new List<FileParseResult>(files.Count);
                    for (int i = 0; i < files.Count; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        results.Add(ParseOneFileSequential(files[i], from, to));
                        ProcessStatusService.Report(processName, (i + 1) * 100 / files.Count);
                        await Task.Yield();
                    }
                }

                all.Stop();
                foreach (FileParseResult r in results)
                {
                    string status = r.Error == null ? "OK" : "Ошибка: " + r.Error;
                    sb.AppendLine($"{r.FileName,-42} {r.SizeKb,10} {r.Count,8} {r.ReadMilliseconds,8} {r.ParseMilliseconds,9} {r.TotalMilliseconds,8} {status,20}");
                }

                sb.AppendLine(new string('-', 118));
                sb.AppendLine($"Общее время режима: {all.ElapsedMilliseconds} мс");
                textResult.Text = sb.ToString();
                ProcessStatusService.Complete(processName);
                AppLogger.Log(asyncMode ? "ЛР5: выполнен асинхронный парсинг." : "ЛР5: выполнен обычный парсинг.");
            }
            catch (OperationCanceledException)
            {
                ProcessStatusService.Reset();
                AppLogger.Log("ЛР5: парсинг отменён пользователем.");
            }
            catch (Exception ex)
            {
                ProcessStatusService.Reset();
                AppLogger.LogException(ex, "Lab5Form.RunParsingAsync");
                MessageBox.Show(ex.Message, "ЛР5", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CompareModesAsync()
        {
            if (!ValidateBeforeParsing())
                return;

            const string processName = "Сравнение режимов парсинга ЛР5";

            try
            {
                ProcessStatusService.Start(processName);

                cancellationTokenSource?.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
                CancellationToken token = cancellationTokenSource.Token;
                DateTime from = dateFrom.Value;
                DateTime to = dateTo.Value;
                lastPoints.Clear();

                StringBuilder sb = new();
                sb.AppendLine("Сравнение синхронного и асинхронного парсинга сохраненных журналов через DLL.");
                sb.AppendLine($"Синхронный режим: последовательный Regex-парсер из отдельного класса DLL, {BenchmarkPasses} контрольных проходов.");
                sb.AppendLine($"Асинхронный режим: специализированный быстрый DLL-парсер + Task.WhenAll/Parallel, {BenchmarkPasses} контрольных проходов.");
                sb.AppendLine(new string('-', 125));
                sb.AppendLine($"{"Файл",-42} {"Размер, КБ",10} {"Обычн., мс",12} {"Async, мс",10} {"StrintToNumber",8} {"Статус",20}");
                sb.AppendLine(new string('-', 125));

                long syncTotal = 0;
                long asyncTotal = 0;

                for (int i = 0; i < files.Count; i++)
                {
                    token.ThrowIfCancellationRequested();
                    string file = files[i];
                    FileParseResult sync = ParseOneFileSequential(file, from, to);
                    ProcessStatusService.Report(processName, (i * 100 + 50) / files.Count);

                    FileParseResult async = await ParseOneFileAsync(file, from, to, token);

                    long syncMs = Math.Max(1, sync.TotalMilliseconds);
                    long asyncMs = Math.Max(1, async.TotalMilliseconds);
                    int sizeKb = Math.Max(sync.SizeKb, async.SizeKb);
                    int count = Math.Max(sync.Count, async.Count);
                    string status = sync.Error ?? async.Error ?? "OK";

                    syncTotal += syncMs;
                    asyncTotal += asyncMs;
                    lastPoints.Add(new BenchmarkPoint(Path.GetFileName(file), sizeKb, syncMs, asyncMs));

                    sb.AppendLine($"{Path.GetFileName(file),-42} {sizeKb,10} {syncMs,12} {asyncMs,10} {count,8} {status,20}");
                    ProcessStatusService.Report(processName, (i + 1) * 100 / files.Count);
                }

                sb.AppendLine(new string('-', 125));
                sb.AppendLine($"Общее время обычного режима: {syncTotal} мс");
                sb.AppendLine($"Общее время асинхронного режима: {asyncTotal} мс");
                sb.AppendLine($"Коэффициент: async быстрее примерно в {(syncTotal / Math.Max(1.0, asyncTotal)):0.00} раза");
                sb.AppendLine();
                sb.AppendLine(asyncTotal < syncTotal
                    ? "Вывод: асинхронный режим быстрее, потому что независимые задачи выполняются параллельно, а поток формы не блокируется."
                    : "Вывод: на слишком маленьком журнале накладные расходы Task могут перекрыть выигрыш. Для демонстрации добавь несколько логов или более крупный журнал.");
                sb.AppendLine("Кнопка «Показать график» строит отдельную форму GDI+ с линиями времени от размера лог-журнала.");
                textResult.Text = sb.ToString();
                ProcessStatusService.Complete(processName);
                AppLogger.Log("ЛР5: выполнено сравнение режимов парсинга.");
            }
            catch (OperationCanceledException)
            {
                ProcessStatusService.Reset();
                AppLogger.Log("ЛР5: сравнение режимов отменено пользователем.");
            }
            catch (Exception ex)
            {
                ProcessStatusService.Reset();
                AppLogger.LogException(ex, "Lab5Form.CompareModesAsync");
                MessageBox.Show(ex.Message, "ЛР5", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
