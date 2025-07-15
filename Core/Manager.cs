using Microsoft.Win32;

namespace RecycleBinTray.Core
{
    public static class Manager
    {
        const string appName = "Recycle Bin Tray";

        /// <summary>
        /// Check if the application is already running, and if so, exit the current instance.
        /// </summary>
        public static void AlreadyRunning()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var runningProcesses = System.Diagnostics.Process.GetProcessesByName(currentProcess.ProcessName);
            if (runningProcesses.Length > 1)
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Add the application to Windows startup if it is not already registered.
        /// </summary>
        public static void AddToStartup()
        {
            // Registry key name
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                // Check If Already Registered
                var currentValue = key?.GetValue(appName)?.ToString();
                if (currentValue == null || !string.Equals(currentValue, $"\"{appPath}\"", StringComparison.OrdinalIgnoreCase))
                {
                    key?.SetValue(appName, $"\"{appPath}\"");
                }
            }
        }

        /// <summary>
        /// Remove the application from Windows startup.
        /// </summary>
        public static void RemoveFromStartup()
        {

            using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                key?.DeleteValue(appName, false);
            }
        }

    }
}
