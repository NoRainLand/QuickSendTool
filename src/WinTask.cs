using Microsoft.Win32;

namespace NoRain
{
    public static class WinTask
    {

        public static string TaskName = "QuickSendToolTask";
        private static readonly string registryKeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";


        public static void Add()
        {
            AddStartupTask(TaskName, Application.ExecutablePath);
        }

        public static void Remove()
        {
            RemoveStartupTask(TaskName);
        }



        public static void AddStartupTask(string taskName, string applicationPath)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyPath, true) ?? Registry.CurrentUser.CreateSubKey(registryKeyPath))
            {
                key.SetValue(taskName, applicationPath);
            }
        }

        public static void RemoveStartupTask(string taskName)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(registryKeyPath, true);
            if (key != null)
            {
                using (key)
                {
                    if (key.GetValue(taskName) != null)
                    {
                        key.DeleteValue(taskName);
                    }
                }
            }
        }
    }
}