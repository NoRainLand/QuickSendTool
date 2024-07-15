using System;
using System.CodeDom;
using System.Diagnostics;

namespace NoRain
{
    public static class StartupTaskManager
    {

        private static string TaskName = "QuickToolTask";
        public static void Add()
        {
            AddStartupTask(TaskName, Application.ExecutablePath);
        }

        public static void Remove()
        {
            RemoveStartupTask(TaskName);
        }

        public static bool HasTask()
        {
            return CheckTaskExists(TaskName);
        }


        public static void AddStartupTask(string taskName, string applicationPath)
        {
            string command = $"/C schtasks /create /tn \"{taskName}\" /tr \"{applicationPath}\" /sc onlogon /rl highest /f";
            ExecuteCommand(command);
        }

        public static void RemoveStartupTask(string taskName)
        {
            string command = $"/C schtasks /delete /tn \"{taskName}\" /f";
            ExecuteCommand(command);
        }

        public static bool CheckTaskExists(string taskName)
        {
            string command = $"/C schtasks /query /tn \"{taskName}\"";
            string output = ExecuteCommand(command, true);
            return !output.Contains("找不到指定的任务");
        }

        private static string ExecuteCommand(string command, bool captureOutput = false)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = captureOutput,
                RedirectStandardError = captureOutput
            };

            using (Process? process = Process.Start(startInfo))
            {
                if (captureOutput && process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return output;
                }
            }

            return "";
        }
    }
}