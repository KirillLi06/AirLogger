using System;

namespace WinFormsApp1
{
    public sealed class ProcessStatusEventArgs : EventArgs
    {
        public ProcessStatusEventArgs(string processName, int progress, bool isActive)
        {
            ProcessName = processName;
            Progress = Math.Clamp(progress, 0, 100);
            IsActive = isActive;
        }

        public string ProcessName { get; }
        public int Progress { get; }
        public bool IsActive { get; }
    }

    public static class ProcessStatusService
    {
        public static event EventHandler<ProcessStatusEventArgs>? StatusChanged;

        public static void Start(string processName)
        {
            Report(processName, 0, true);
        }

        public static void Report(string processName, int progress)
        {
            Report(processName, progress, true);
        }

        public static void Complete(string processName)
        {
            Report(processName, 100, false);
        }

        public static void Reset()
        {
            Report("Готово", 0, false);
        }

        private static void Report(string processName, int progress, bool isActive)
        {
            StatusChanged?.Invoke(null, new ProcessStatusEventArgs(processName, progress, isActive));
        }
    }
}
