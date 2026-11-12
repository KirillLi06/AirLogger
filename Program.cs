using System;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Run(new Form1());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AppLogger.LogException(e.Exception, "UI Thread");
            MessageBox.Show("Произошла обработанная ошибка. Подробности записаны в лог.",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                AppLogger.LogException(ex, "AppDomain");
        }
    }
}
