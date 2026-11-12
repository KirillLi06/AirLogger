using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class ArrayProcessResult
    {
        public int[] Source { get; set; } = Array.Empty<int>();
        public int[] A { get; set; } = Array.Empty<int>();
        public int[] Modified { get; set; } = Array.Empty<int>();
        public long Ticks { get; set; }
    }

    public static class ArrayProcessor
    {
        // Ограничение необходимо, поскольку по условию варианта часть элементов возводится в квадрат,
        // а результат хранится в Int32.
        public const int MinSafeValue = -46340;
        public const int MaxSafeValue = 46340;

        public static ArrayProcessResult ProcessSequential(int[] source)
        {
            ValidateSource(source);
            int[] c = (int[])source.Clone();
            Stopwatch sw = Stopwatch.StartNew();

            int[] a = BuildArrayASequential(c);
            int[] modified = ModifyArraySequential(c);

            sw.Stop();

            return new ArrayProcessResult
            {
                Source = (int[])source.Clone(),
                A = a,
                Modified = modified,
                Ticks = sw.ElapsedTicks
            };
        }

        public static ArrayProcessResult ProcessParallel(int[] source)
        {
            ValidateSource(source);
            int[] c = (int[])source.Clone();
            Stopwatch sw = Stopwatch.StartNew();

            int[] a = BuildArrayAParallel(c);
            int[] modified = ModifyArrayParallel(c);

            sw.Stop();

            return new ArrayProcessResult
            {
                Source = (int[])source.Clone(),
                A = a,
                Modified = modified,
                Ticks = sw.ElapsedTicks
            };
        }

        private static void ValidateSource(int[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source.Length == 0)
                throw new ArgumentException("Массив не должен быть пустым.", nameof(source));

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < MinSafeValue || source[i] > MaxSafeValue)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(source),
                        $"Элемент {i + 1} выходит за допустимый диапазон {MinSafeValue}..{MaxSafeValue}.");
                }
            }
        }

        private static int[] BuildArrayASequential(int[] c)
        {
            int[] a = new int[c.Length];

            for (int i = 0; i < c.Length; i++)
            {
                if (i >= 2 && i <= 11)
                    a[i] = -(c[i] * c[i]);
                else
                    a[i] = c[i] - 1;
            }

            return a;
        }

        private static int[] BuildArrayAParallel(int[] c)
        {
            int[] a = new int[c.Length];

            Parallel.For(0, c.Length, i =>
            {
                if (i >= 2 && i <= 11)
                    a[i] = -(c[i] * c[i]);
                else
                    a[i] = c[i] - 1;
            });

            return a;
        }

        private static int[] ModifyArraySequential(int[] c)
        {
            int[] arr = (int[])c.Clone();

            int maxIndex = 0;
            int minIndex = 0;

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] > arr[maxIndex])
                    maxIndex = i;

                if (arr[i] < arr[minIndex])
                    minIndex = i;
            }

            arr[maxIndex] = -arr[maxIndex];

            int left = Math.Min(minIndex, maxIndex) + 1;
            int right = Math.Max(minIndex, maxIndex);

            for (int i = left; i < right; i++)
                arr[i] = 0;

            return arr;
        }

        private static int[] ModifyArrayParallel(int[] c)
        {
            int[] arr = (int[])c.Clone();

            int minIndex = -1;
            int maxIndex = -1;
            object sync = new object();

            Parallel.For(
                0,
                arr.Length,
                () => (localMinIndex: -1, localMaxIndex: -1),
                (i, state, local) =>
                {
                    if (local.localMinIndex == -1 ||
                        arr[i] < arr[local.localMinIndex] ||
                        (arr[i] == arr[local.localMinIndex] && i < local.localMinIndex))
                    {
                        local.localMinIndex = i;
                    }

                    if (local.localMaxIndex == -1 ||
                        arr[i] > arr[local.localMaxIndex] ||
                        (arr[i] == arr[local.localMaxIndex] && i < local.localMaxIndex))
                    {
                        local.localMaxIndex = i;
                    }

                    return local;
                },
                local =>
                {
                    lock (sync)
                    {
                        if (local.localMinIndex >= 0 &&
                            (minIndex == -1 ||
                             arr[local.localMinIndex] < arr[minIndex] ||
                             (arr[local.localMinIndex] == arr[minIndex] && local.localMinIndex < minIndex)))
                        {
                            minIndex = local.localMinIndex;
                        }

                        if (local.localMaxIndex >= 0 &&
                            (maxIndex == -1 ||
                             arr[local.localMaxIndex] > arr[maxIndex] ||
                             (arr[local.localMaxIndex] == arr[maxIndex] && local.localMaxIndex < maxIndex)))
                        {
                            maxIndex = local.localMaxIndex;
                        }
                    }
                });

            if (minIndex < 0 || maxIndex < 0)
                throw new InvalidOperationException("Не удалось определить минимальный или максимальный элемент массива.");

            arr[maxIndex] = -arr[maxIndex];

            int left = Math.Min(minIndex, maxIndex) + 1;
            int right = Math.Max(minIndex, maxIndex);

            if (right > left)
            {
                Parallel.For(left, right, i =>
                {
                    arr[i] = 0;
                });
            }

            return arr;
        }
    }
}
