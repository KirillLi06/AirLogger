using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Lab2 : Form
    {
        private int[] currentArray = Array.Empty<int>();
        private int[] lastArrayA = Array.Empty<int>();
        private int[] lastModifiedArray = Array.Empty<int>();
        private readonly Random random = new Random();

        public Lab2()
        {
            InitializeComponent();
            DetailedActionLogger.Attach(this);

            numericUpDownSize.Minimum = 5;
            numericUpDownSize.Maximum = 1000;
            numericUpDownSize.Value = 15;

            numericUpDownDisplayCount.Minimum = 1;
            numericUpDownDisplayCount.Maximum = numericUpDownSize.Value;
            numericUpDownDisplayCount.Value = numericUpDownSize.Value;

            textBoxMin.Text = "-20";
            textBoxMax.Text = "50";
            textBoxFreqValue.Text = "5";
            textBoxFreqPercent.Text = "20";

            radioButtonRandom.Checked = true;
            numericUpDownSize.ValueChanged += numericUpDownSize_ValueChanged;
            numericUpDownDisplayCount.ValueChanged += numericUpDownDisplayCount_ValueChanged;

            InitGrid(dataGridViewSource);
            InitGrid(dataGridViewA);
            InitGrid(dataGridViewModified);
        }

        private static void InitGrid(DataGridView dgv)
        {
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.ColumnCount = 0;
            dgv.Rows.Clear();
        }

        private void numericUpDownSize_ValueChanged(object? sender, EventArgs e)
        {
            decimal newMaximum = numericUpDownSize.Value;
            if (numericUpDownDisplayCount.Value > newMaximum)
                numericUpDownDisplayCount.Value = newMaximum;

            numericUpDownDisplayCount.Maximum = newMaximum;
        }

        private void numericUpDownDisplayCount_ValueChanged(object? sender, EventArgs e)
        {
            if (currentArray.Length > 0)
                ShowArray(dataGridViewSource, currentArray, radioButtonManual.Checked);

            if (lastArrayA.Length > 0)
                ShowArray(dataGridViewA, lastArrayA);

            if (lastModifiedArray.Length > 0)
                ShowArray(dataGridViewModified, lastModifiedArray);
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            const string processName = "Формирование массива ЛР2";

            try
            {
                ProcessStatusService.Start(processName);
                int n = (int)numericUpDownSize.Value;
                int minValue = ParseOrDefault(textBoxMin.Text, -20);
                int maxValue = ParseOrDefault(textBoxMax.Text, 50);

                ValidateRange(minValue, maxValue);
                ProcessStatusService.Report(processName, 20);

                currentArray = new int[n];

                if (radioButtonRandom.Checked)
                {
                    FillRandom(currentArray, minValue, maxValue);
                }
                else if (radioButtonFrequency.Checked)
                {
                    if (!int.TryParse(textBoxFreqValue.Text, out int freqValue))
                        throw new ArgumentException("Введите целое значение для заданной частоты.");

                    if (freqValue < ArrayProcessor.MinSafeValue || freqValue > ArrayProcessor.MaxSafeValue)
                        throw new ArgumentOutOfRangeException(nameof(freqValue), $"Значение должно быть от {ArrayProcessor.MinSafeValue} до {ArrayProcessor.MaxSafeValue}.");

                    if (!int.TryParse(textBoxFreqPercent.Text, out int freqPercent) || freqPercent < 0 || freqPercent > 100)
                        throw new ArgumentException("Процент частоты должен быть от 0 до 100.");

                    FillRandomWithExactFrequency(currentArray, minValue, maxValue, freqValue, freqPercent);
                }
                else if (radioButtonManual.Checked)
                {
                    Array.Clear(currentArray, 0, currentArray.Length);
                }
                else
                {
                    throw new InvalidOperationException("Выберите способ формирования массива.");
                }

                ProcessStatusService.Report(processName, 80);

                lastArrayA = Array.Empty<int>();
                lastModifiedArray = Array.Empty<int>();
                dataGridViewSource.ReadOnly = !radioButtonManual.Checked;
                ShowArray(dataGridViewSource, currentArray, radioButtonManual.Checked);
                ClearGrid(dataGridViewA);
                ClearGrid(dataGridViewModified);
                labelTimeSequential.Text = "Время без потоков:";
                labelTimeParallel.Text = "Время с потоками:";

                ProcessStatusService.Complete(processName);
                AppLogger.Log($"ЛР2: массив сформирован. Размер: {n}; показано элементов: {GetDisplayCount(n)}.");
            }
            catch (Exception ex)
            {
                ProcessStatusService.Reset();
                AppLogger.LogException(ex, "Lab2.buttonGenerate_Click");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonSequential_Click(object sender, EventArgs e)
        {
            const string processName = "Последовательная обработка массива ЛР2";

            try
            {
                SetProcessingButtonsEnabled(false);
                ReadArrayFromGridIfManual();

                ProcessStatusService.Start(processName);
                ProcessStatusService.Report(processName, 10);

                ArrayProcessResult result = await Task.Run(() => ArrayProcessor.ProcessSequential(currentArray));

                ProcessStatusService.Report(processName, 90);
                ShowResult(result);
                labelTimeSequential.Text = $"Время без потоков: {result.Ticks} тиков";

                ProcessStatusService.Complete(processName);
                AppLogger.Log("ЛР2: последовательная обработка выполнена.");
            }
            catch (Exception ex)
            {
                ProcessStatusService.Reset();
                AppLogger.LogException(ex, "Lab2.buttonSequential_Click");
                MessageBox.Show(GetExceptionMessage(ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetProcessingButtonsEnabled(true);
            }
        }

        private async void buttonParallel_Click(object sender, EventArgs e)
        {
            const string processName = "Параллельная обработка массива ЛР2";

            try
            {
                SetProcessingButtonsEnabled(false);
                ReadArrayFromGridIfManual();

                ProcessStatusService.Start(processName);
                ProcessStatusService.Report(processName, 10);

                ArrayProcessResult result = await Task.Run(() => ArrayProcessor.ProcessParallel(currentArray));

                ProcessStatusService.Report(processName, 90);
                ShowResult(result);
                labelTimeParallel.Text = $"Время с потоками: {result.Ticks} тиков";

                ProcessStatusService.Complete(processName);
                AppLogger.Log("ЛР2: многопоточная обработка выполнена.");
            }
            catch (Exception ex)
            {
                ProcessStatusService.Reset();
                AppLogger.LogException(ex, "Lab2.buttonParallel_Click");
                MessageBox.Show(GetExceptionMessage(ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetProcessingButtonsEnabled(true);
            }
        }

        private async void buttonCompare_Click(object sender, EventArgs e)
        {
            const string processName = "Сравнение последовательной и параллельной обработки ЛР2";
            const int repeats = 500;

            try
            {
                SetProcessingButtonsEnabled(false);
                ReadArrayFromGridIfManual();
                ProcessStatusService.Start(processName);

                (long sequentialTicks, long parallelTicks, ArrayProcessResult lastResult) = await Task.Run(() =>
                {
                    // Один прогревочный запуск исключает значительную часть влияния JIT-компиляции.
                    ArrayProcessor.ProcessSequential(currentArray);
                    ArrayProcessor.ProcessParallel(currentArray);

                    long sequentialTicks = 0;
                    long parallelTicks = 0;
                    ArrayProcessResult? lastSeq = null;
                    ArrayProcessResult? lastPar = null;

                    for (int i = 0; i < repeats; i++)
                    {
                        lastSeq = ArrayProcessor.ProcessSequential(currentArray);
                        sequentialTicks += lastSeq.Ticks;

                        if (i % 10 == 0)
                            ProcessStatusService.Report(processName, (i + 1) * 50 / repeats);
                    }

                    for (int i = 0; i < repeats; i++)
                    {
                        lastPar = ArrayProcessor.ProcessParallel(currentArray);
                        parallelTicks += lastPar.Ticks;

                        if (i % 10 == 0)
                            ProcessStatusService.Report(processName, 50 + (i + 1) * 50 / repeats);
                    }

                    if (lastSeq == null || lastPar == null)
                        throw new InvalidOperationException("Не удалось получить результат сравнения.");

                    if (!lastSeq.A.SequenceEqual(lastPar.A) || !lastSeq.Modified.SequenceEqual(lastPar.Modified))
                        throw new InvalidOperationException("Результаты последовательной и параллельной обработки не совпадают.");

                    return (sequentialTicks, parallelTicks, lastPar!);
                });

                ShowResult(lastResult);
                labelTimeSequential.Text = $"Время без потоков: {sequentialTicks / repeats} тиков (среднее)";
                labelTimeParallel.Text = $"Время с потоками: {parallelTicks / repeats} тиков (среднее)";

                ProcessStatusService.Complete(processName);
                AppLogger.Log($"ЛР2: выполнено сравнение времени по среднему значению ({repeats} запусков). Результаты совпадают.");
            }
            catch (Exception ex)
            {
                ProcessStatusService.Reset();
                AppLogger.LogException(ex, "Lab2.buttonCompare_Click");
                MessageBox.Show(GetExceptionMessage(ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetProcessingButtonsEnabled(true);
            }
        }

        private void FillRandom(int[] target, int minValue, int maxValue)
        {
            for (int i = 0; i < target.Length; i++)
                target[i] = NextInclusive(minValue, maxValue);
        }

        private void FillRandomWithExactFrequency(int[] target, int minValue, int maxValue, int freqValue, int freqPercent)
        {
            int frequencyCount = (int)Math.Round(target.Length * freqPercent / 100.0, MidpointRounding.AwayFromZero);

            for (int i = 0; i < target.Length; i++)
                target[i] = NextInclusiveExcluding(minValue, maxValue, freqValue);

            int[] positions = Enumerable.Range(0, target.Length).ToArray();
            for (int i = positions.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (positions[i], positions[j]) = (positions[j], positions[i]);
            }

            for (int i = 0; i < frequencyCount; i++)
                target[positions[i]] = freqValue;
        }

        private int NextInclusive(int minValue, int maxValue)
        {
            return (int)random.NextInt64(minValue, (long)maxValue + 1);
        }

        private int NextInclusiveExcluding(int minValue, int maxValue, int excludedValue)
        {
            if (excludedValue < minValue || excludedValue > maxValue)
                return NextInclusive(minValue, maxValue);

            int value;
            do
            {
                value = NextInclusive(minValue, maxValue);
            }
            while (value == excludedValue);

            return value;
        }

        private static int ParseOrDefault(string text, int defaultValue)
        {
            return int.TryParse(text, out int value) ? value : defaultValue;
        }

        private static void ValidateRange(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("Минимальное значение должно быть меньше максимального.");

            if (minValue < ArrayProcessor.MinSafeValue || maxValue > ArrayProcessor.MaxSafeValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    $"Для корректного возведения в квадрат используйте значения от {ArrayProcessor.MinSafeValue} до {ArrayProcessor.MaxSafeValue}.");
            }
        }

        private void ReadArrayFromGridIfManual()
        {
            if (!radioButtonManual.Checked)
            {
                if (currentArray.Length == 0)
                    throw new InvalidOperationException("Сначала сформируйте массив.");
                return;
            }

            if (dataGridViewSource.ColumnCount == 0 || currentArray.Length == 0)
                throw new InvalidOperationException("Сначала нажмите 'Сформировать массив'.");

            if (dataGridViewSource.ColumnCount != currentArray.Length)
                throw new InvalidOperationException("Для ручного ввода должны отображаться все элементы массива. Сформируйте массив заново.");

            for (int i = 0; i < currentArray.Length; i++)
            {
                object? value = dataGridViewSource.Rows[0].Cells[i].Value;
                if (value == null || !int.TryParse(value.ToString(), out int parsed))
                    throw new FormatException($"Некорректное значение в ячейке {i + 1}.");

                if (parsed < ArrayProcessor.MinSafeValue || parsed > ArrayProcessor.MaxSafeValue)
                    throw new ArgumentOutOfRangeException($"Элемент {i + 1}", $"Допустимый диапазон: {ArrayProcessor.MinSafeValue}..{ArrayProcessor.MaxSafeValue}.");

                currentArray[i] = parsed;
            }
        }

        private void ShowResult(ArrayProcessResult result)
        {
            lastArrayA = result.A;
            lastModifiedArray = result.Modified;
            ShowArray(dataGridViewA, lastArrayA);
            ShowArray(dataGridViewModified, lastModifiedArray);
        }

        private void ShowArray(DataGridView dgv, int[] array, bool showAll = false)
        {
            ClearGrid(dgv);

            int count = showAll ? array.Length : GetDisplayCount(array.Length);
            for (int i = 0; i < count; i++)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
                {
                    Name = $"col{i}",
                    HeaderText = (i + 1).ToString(),
                    Width = 60,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                dgv.Columns.Add(column);
            }

            if (count == 0)
                return;

            dgv.Rows.Add();
            for (int i = 0; i < count; i++)
                dgv.Rows[0].Cells[i].Value = array[i];
        }

        private int GetDisplayCount(int arrayLength)
        {
            return Math.Min((int)numericUpDownDisplayCount.Value, arrayLength);
        }

        private static void ClearGrid(DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Rows.Clear();
        }

        private void SetProcessingButtonsEnabled(bool enabled)
        {
            buttonGenerate.Enabled = enabled;
            buttonSequential.Enabled = enabled;
            buttonParallel.Enabled = enabled;
            buttonCompare.Enabled = enabled;
        }

        private static string GetExceptionMessage(Exception ex)
        {
            if (ex is AggregateException aggregate)
                return aggregate.Flatten().InnerException?.Message ?? aggregate.Message;

            return ex.Message;
        }
    }
}
