using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace Frontend
{
    internal static class Program
    {
        private static MainForm mainForm;

        [DllImport("Shcore.dll")]
        private static extern int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);
        private enum DpiAwareness
        {
            None = 0,
            SystemAware = 1,
            PerMonitorAware = 2
        }

        /// <summary>
        /// The entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SetProcessDpiAwareness((int)DpiAwareness.PerMonitorAware);
            mainForm = new MainForm();
            SingleInstanceApplication.Run(mainForm, StartupNextInstanceEventHandler);
        }

        public static void StartupNextInstanceEventHandler(object sender, StartupNextInstanceEventArgs e)
        {
            mainForm.Activate();
        }
    }
    public class SingleInstanceApplication : WindowsFormsApplicationBase
    {
        private SingleInstanceApplication()
        {
            IsSingleInstance = true;
        }

        public static void Run(Form f, StartupNextInstanceEventHandler startupHandler)
        {
            SingleInstanceApplication app = new SingleInstanceApplication { MainForm = f };
            app.StartupNextInstance += startupHandler;
            app.Run(Environment.GetCommandLineArgs());
        }
    }
}