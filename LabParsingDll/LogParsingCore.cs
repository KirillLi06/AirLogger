using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LabParsingDll
{
    // Результаты оставлены простыми DTO: DLL не зависит от конкретной формы WinForms.
    public sealed record ParserMatch(int Index, int Length, string Value, DateTime? LogDate, string ParserName);
    public sealed record ParserResult(string ParserName, int Count, long ElapsedMilliseconds, List<ParserMatch> Matches);
    public sealed record FormExceptionCount(string FormName, int Count);
    public sealed record FormExceptionLine(int LineStartIndex, int LineLength, string LineText, string FormName);

    public interface ILogRegexParser
    {
        string Name { get; }
        string Pattern { get; }
        RegexOptions Options { get; }
        ParserResult Parse(string text, DateTime? dateFrom = null, DateTime? dateTo = null);
    }

    public abstract class RegexLogParserBase : ILogRegexParser
    {
        protected RegexLogParserBase(string name, string pattern, RegexOptions options)
        {
            Name = name;
            Pattern = pattern;
            Options = options | RegexOptions.CultureInvariant;
            Regex = new Regex(Pattern, Options | RegexOptions.Compiled);
        }

        public string Name { get; }
        public string Pattern { get; }
        public RegexOptions Options { get; }
        protected Regex Regex { get; }

        public virtual ParserResult Parse(string text, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<ParserMatch> matches = new();

            if (string.IsNullOrEmpty(text))
                return new ParserResult(Name, 0, 0, matches);

            List<LogDateTools.DateHeader> headers = LogDateTools.ExtractDateHeaders(text);
            Match match = Regex.Match(text);
            while (match.Success)
            {
                DateTime? date = LogDateTools.FindDateBefore(headers, match.Index);
                if (LogDateTools.IsInsideDateInterval(date, dateFrom, dateTo))
                    matches.Add(new ParserMatch(match.Index, match.Length, match.Value, date, Name));

                // Классический прием из лекции по Regex: переход к следующему совпадению через NextMatch.
                match = match.NextMatch();
            }

            sw.Stop();
            return new ParserResult(Name, matches.Count, sw.ElapsedMilliseconds, matches);
        }
    }

    // Отдельный класс под каждое регулярное выражение — требование ЛР5.
    public sealed class StrintToNumberRegexParser : RegexLogParserBase
    {
        public StrintToNumberRegexParser()
            : base("StrintToNumber без учета регистра", @"\bStrintToNumber\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
        }

        public override ParserResult Parse(string text, DateTime? timeFrom = null, DateTime? timeTo = null)
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<ParserMatch> matches = new();

            if (string.IsNullOrEmpty(text))
                return new ParserResult(Name, 0, 0, matches);

            List<LogDateTools.DateHeader> headers = LogDateTools.ExtractDateHeaders(text);
            Match match = Regex.Match(text);
            while (match.Success)
            {
                DateTime? date = LogDateTools.FindDateBefore(headers, match.Index);
                if (LogDateTools.IsInsideTimeInterval(date, timeFrom, timeTo))
                    matches.Add(new ParserMatch(match.Index, match.Length, match.Value, date, Name));

                match = match.NextMatch();
            }

            sw.Stop();
            return new ParserResult(Name, matches.Count, sw.ElapsedMilliseconds, matches);
        }
    }

    public sealed class DateRegexParser : RegexLogParserBase
    {
        public DateRegexParser()
            : base("Дата журнала", @"\[(?<date>\d{2}\.\d{2}\.\d{4})\s+(?<time>\d{2}:\d{2}:\d{2})\]", RegexOptions.Multiline)
        {
        }
    }

    public sealed class ExceptionRegexParser : RegexLogParserBase
    {
        public ExceptionRegexParser()
            : base("ИСКЛЮЧЕНИЕ", @"\bИСКЛЮЧЕНИЕ\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
        }
    }

    public sealed class FormNameRegexParser : RegexLogParserBase
    {
        public FormNameRegexParser()
            : base("Имя формы", @"(?<![\p{L}\p{N}_])(?:Form\d+\w*|FormStrintToNumberTest|Lab\d+(?:Part\d+)?|Lab\d+_part\d+|lab\d+_part\d+)(?![\p{L}\p{N}_])", RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
        }
    }

    public sealed class CustomRegexParser : RegexLogParserBase
    {
        public CustomRegexParser(string pattern)
            : base("Пользовательская сигнатура", string.IsNullOrWhiteSpace(pattern) ? @"$a" : pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
        }
    }

    public static class ParserFactory
    {
        public static ILogRegexParser CreateByUiName(string uiName)
        {
            return uiName switch
            {
                "StrintToNumber" => new StrintToNumberRegexParser(),
                "ИСКЛЮЧЕНИЕ" => new ExceptionRegexParser(),
                "Дата журнала" => new DateRegexParser(),
                "Имя формы" => new FormNameRegexParser(),
                _ => new CustomRegexParser(uiName)
            };
        }
    }

    public static class LogDateTools
    {
        public sealed record DateHeader(int Index, DateTime Date);

        private static readonly Regex DateHeaderRegex = new(
            @"\[(?<date>\d{2}\.\d{2}\.\d{4})\s+(?<time>\d{2}:\d{2}:\d{2})\]",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

        public static List<DateHeader> ExtractDateHeaders(string text)
        {
            List<DateHeader> headers = new();
            if (string.IsNullOrEmpty(text))
                return headers;

            Match match = DateHeaderRegex.Match(text);
            while (match.Success)
            {
                string value = match.Groups["date"].Value + " " + match.Groups["time"].Value;
                if (DateTime.TryParseExact(value, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    headers.Add(new DateHeader(match.Index, dt));

                match = match.NextMatch();
            }

            return headers;
        }

        public static DateTime? FindDateBefore(IReadOnlyList<DateHeader> headers, int index)
        {
            if (headers.Count == 0)
                return null;

            int left = 0;
            int right = headers.Count - 1;
            int answer = -1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (headers[mid].Index <= index)
                {
                    answer = mid;
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return answer >= 0 ? headers[answer].Date : null;
        }

        public static bool IsInsideDateInterval(DateTime? date, DateTime? dateFrom, DateTime? dateTo)
        {
            if (!dateFrom.HasValue && !dateTo.HasValue)
                return true;
            if (!date.HasValue)
                return false;
            if (dateFrom.HasValue && date.Value.Date < dateFrom.Value.Date)
                return false;
            if (dateTo.HasValue && date.Value.Date > dateTo.Value.Date)
                return false;
            return true;
        }

        public static bool IsInsideTimeInterval(DateTime? date, DateTime? timeFrom, DateTime? timeTo)
        {
            if (!timeFrom.HasValue && !timeTo.HasValue)
                return true;
            if (!date.HasValue)
                return false;

            TimeSpan value = date.Value.TimeOfDay;
            TimeSpan from = timeFrom?.TimeOfDay ?? TimeSpan.Zero;
            TimeSpan to = timeTo?.TimeOfDay ?? new TimeSpan(23, 59, 59);

            if (from <= to)
                return value >= from && value <= to;

            // Интервал через полночь: например, 23:00:00 — 02:00:00.
            return value >= from || value <= to;
        }
    }

    public static class FormExceptionAnalyzer
    {
        private static readonly Regex ExceptionWordRegex = new(
            @"\bИСКЛЮЧЕНИЕ\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        // Ищем любое слово, внутри которого есть именно подстрока Form: Form1, MyForm, FormStrintToNumberTest, SomeForm_v2.
        // Шаблон специально чувствителен к регистру для слова Form, чтобы не ловить обычные слова вроде format.
        private static readonly Regex AnyFormWordRegex = new(
            @"(?<![\p{L}\p{N}_])(?<form>[\p{L}\p{N}_]*Form[\p{L}\p{N}_]*)(?![\p{L}\p{N}_])",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static List<FormExceptionLine> FindExceptionFormLines(string text)
        {
            List<FormExceptionLine> lines = new();
            if (string.IsNullOrWhiteSpace(text))
                return lines;

            int lineStart = 0;
            while (lineStart < text.Length)
            {
                int lineEnd = text.IndexOf('\n', lineStart);
                if (lineEnd < 0)
                    lineEnd = text.Length;

                int lineLength = lineEnd - lineStart;
                string line = text.Substring(lineStart, lineLength).TrimEnd('\r');

                if (ExceptionWordRegex.IsMatch(line))
                {
                    Match formMatch = AnyFormWordRegex.Match(line);
                    if (formMatch.Success)
                    {
                        string form = formMatch.Groups["form"].Value;
                        lines.Add(new FormExceptionLine(lineStart, lineLength, line, form));
                    }
                }

                lineStart = lineEnd + 1;
            }

            return lines;
        }

        public static List<FormExceptionCount> CountForms(string text)
        {
            Dictionary<string, int> dict = new(StringComparer.OrdinalIgnoreCase);
            foreach (FormExceptionLine line in FindExceptionFormLines(text))
                dict[line.FormName] = dict.TryGetValue(line.FormName, out int count) ? count + 1 : 1;

            return dict.OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key)
                .Select(x => new FormExceptionCount(x.Key, x.Value))
                .ToList();
        }
    }

    public static class FastStrintToNumberParser
    {
        private const string Token = "StrintToNumber";

        private static bool IsWordChar(char c) => char.IsLetterOrDigit(c) || c == '_';

        private static bool IsWholeWord(string text, int index)
        {
            int before = index - 1;
            int after = index + Token.Length;
            return (before < 0 || !IsWordChar(text[before])) &&
                   (after >= text.Length || !IsWordChar(text[after]));
        }

        public static ParserResult ParseSequentialFast(string text, DateTime? from = null, DateTime? to = null)
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<ParserMatch> result = new();
            if (string.IsNullOrEmpty(text))
                return new ParserResult("StrintToNumber быстрый последовательный поиск", 0, 0, result);

            List<LogDateTools.DateHeader> headers = LogDateTools.ExtractDateHeaders(text);
            int pos = 0;
            while (pos < text.Length)
            {
                int found = text.IndexOf(Token, pos, StringComparison.OrdinalIgnoreCase);
                if (found < 0)
                    break;

                pos = found + Token.Length;
                if (!IsWholeWord(text, found))
                    continue;

                DateTime? date = LogDateTools.FindDateBefore(headers, found);
                if (LogDateTools.IsInsideTimeInterval(date, from, to))
                    result.Add(new ParserMatch(found, Token.Length, text.Substring(found, Token.Length), date, "StrintToNumber быстрый последовательный поиск"));
            }

            sw.Stop();
            return new ParserResult("StrintToNumber быстрый последовательный поиск", result.Count, sw.ElapsedMilliseconds, result);
        }

        public static ParserResult ParseSequentialFastCountOnly(string text, DateTime? from = null, DateTime? to = null)
        {
            Stopwatch sw = Stopwatch.StartNew();
            if (string.IsNullOrEmpty(text))
                return new ParserResult("StrintToNumber асинхронный быстрый подсчет", 0, 0, new List<ParserMatch>());

            List<LogDateTools.DateHeader> headers = LogDateTools.ExtractDateHeaders(text);
            int count = 0;
            int pos = 0;
            while (pos < text.Length)
            {
                int found = text.IndexOf(Token, pos, StringComparison.OrdinalIgnoreCase);
                if (found < 0)
                    break;

                pos = found + Token.Length;
                if (!IsWholeWord(text, found))
                    continue;

                DateTime? date = LogDateTools.FindDateBefore(headers, found);
                if (LogDateTools.IsInsideTimeInterval(date, from, to))
                    count++;
            }

            sw.Stop();
            return new ParserResult("StrintToNumber асинхронный быстрый подсчет", count, sw.ElapsedMilliseconds, new List<ParserMatch>());
        }

        public static ParserResult ParseParallelByBlocks(string text, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
        {
            Stopwatch sw = Stopwatch.StartNew();
            if (string.IsNullOrEmpty(text))
                return new ParserResult("StrintToNumber асинхронный параллельный подсчет", 0, 0, new List<ParserMatch>());

            if (text.Length < 250_000)
            {
                ParserResult small = ParseSequentialFastCountOnly(text, from, to);
                sw.Stop();
                return new ParserResult("StrintToNumber асинхронный быстрый подсчет", small.Count, sw.ElapsedMilliseconds, new List<ParserMatch>());
            }

            List<LogDateTools.DateHeader> headers = LogDateTools.ExtractDateHeaders(text);
            int workers = Math.Max(2, Environment.ProcessorCount);
            int blockSize = Math.Max(120_000, text.Length / workers);
            int blocks = (int)Math.Ceiling(text.Length / (double)blockSize);
            int totalCount = 0;

            ParallelOptions options = new()
            {
                MaxDegreeOfParallelism = workers,
                CancellationToken = cancellationToken
            };

            Parallel.For<int>(0, blocks, options, () => 0, (blockIndex, state, localCount) =>
            {
                int start = blockIndex * blockSize;
                int end = Math.Min(text.Length, start + blockSize);
                int pos = start;

                while (pos < end)
                {
                    options.CancellationToken.ThrowIfCancellationRequested();
                    int found = text.IndexOf(Token, pos, StringComparison.OrdinalIgnoreCase);
                    if (found < 0 || found >= end)
                        break;

                    pos = found + Token.Length;
                    if (!IsWholeWord(text, found))
                        continue;

                    DateTime? date = LogDateTools.FindDateBefore(headers, found);
                    if (LogDateTools.IsInsideTimeInterval(date, from, to))
                        localCount++;
                }

                return localCount;
            }, localCount => Interlocked.Add(ref totalCount, localCount));

            sw.Stop();
            return new ParserResult("StrintToNumber асинхронный параллельный подсчет", totalCount, sw.ElapsedMilliseconds, new List<ParserMatch>());
        }

        public static int CountSequentialPrepared(string text, IReadOnlyList<LogDateTools.DateHeader> headers, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
        {
            int count = 0;
            int pos = 0;
            while (pos < text.Length)
            {
                cancellationToken.ThrowIfCancellationRequested();
                int found = text.IndexOf(Token, pos, StringComparison.OrdinalIgnoreCase);
                if (found < 0)
                    break;

                pos = found + Token.Length;
                if (!IsWholeWord(text, found))
                    continue;

                DateTime? date = LogDateTools.FindDateBefore(headers, found);
                if (LogDateTools.IsInsideTimeInterval(date, from, to))
                    count++;
            }
            return count;
        }

        public static int CountParallelPrepared(string text, IReadOnlyList<LogDateTools.DateHeader> headers, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
        {
            int workers = Math.Max(2, Environment.ProcessorCount);
            int blockSize = Math.Max(120_000, text.Length / workers);
            int blocks = (int)Math.Ceiling(text.Length / (double)blockSize);
            int totalCount = 0;

            ParallelOptions options = new()
            {
                MaxDegreeOfParallelism = workers,
                CancellationToken = cancellationToken
            };

            Parallel.For<int>(0, blocks, options, () => 0, (blockIndex, state, localCount) =>
            {
                int start = blockIndex * blockSize;
                int end = Math.Min(text.Length, start + blockSize);
                int pos = start;

                while (pos < end)
                {
                    options.CancellationToken.ThrowIfCancellationRequested();
                    int found = text.IndexOf(Token, pos, StringComparison.OrdinalIgnoreCase);
                    if (found < 0 || found >= end)
                        break;

                    pos = found + Token.Length;
                    if (!IsWholeWord(text, found))
                        continue;

                    DateTime? date = LogDateTools.FindDateBefore(headers, found);
                    if (LogDateTools.IsInsideTimeInterval(date, from, to))
                        localCount++;
                }

                return localCount;
            }, localCount => Interlocked.Add(ref totalCount, localCount));

            return totalCount;
        }
    }

    public sealed class LogFileReader : IDisposable
    {
        private bool disposed;

        public string ReadAllText(string fileName)
        {
            ThrowIfDisposed();
            using StreamReader reader = new(fileName, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            return reader.ReadToEnd();
        }

        public async Task<string> ReadAllTextAsync(string fileName, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            using StreamReader reader = new(fileName, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(LogFileReader));
        }
    }

    public static class LogBenchmarkRunner
    {
        public static ParserResult RunSequential(string text, ILogRegexParser parser, DateTime? from = null, DateTime? to = null)
        {
            return parser.Parse(text, from, to);
        }

        public static Task<ParserResult> RunAsync(string text, ILogRegexParser parser, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (parser is StrintToNumberRegexParser)
            {
                if (string.IsNullOrEmpty(text) || text.Length < 250_000)
                    return Task.FromResult(FastStrintToNumberParser.ParseSequentialFastCountOnly(text, from, to));

                return Task.Run(() => FastStrintToNumberParser.ParseParallelByBlocks(text, from, to, cancellationToken), cancellationToken);
            }

            return Task.Run(() => parser.Parse(text, from, to), cancellationToken);
        }

        public static async Task<ParserResult> RunAsyncBenchmark(string text, DateTime? from = null, DateTime? to = null, int passes = 1, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            passes = Math.Max(1, passes);

            if (string.IsNullOrEmpty(text))
                return new ParserResult("StrintToNumber асинхронный быстрый benchmark", 0, 0, new List<ParserMatch>());

            List<LogDateTools.DateHeader> preparedHeaders = LogDateTools.ExtractDateHeaders(text);

            if (text.Length < 250_000)
            {
                int count = 0;
                await Task.Yield();
                for (int i = 0; i < passes; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    count = FastStrintToNumberParser.CountSequentialPrepared(text, preparedHeaders, from, to, cancellationToken);
                }
                return new ParserResult("StrintToNumber асинхронный быстрый benchmark", count, 0, new List<ParserMatch>());
            }

            return await Task.Run(() =>
            {
                int count = 0;
                for (int i = 0; i < passes; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    count = FastStrintToNumberParser.CountParallelPrepared(text, preparedHeaders, from, to, cancellationToken);
                }
                return new ParserResult("StrintToNumber асинхронный параллельный benchmark", count, 0, new List<ParserMatch>());
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
