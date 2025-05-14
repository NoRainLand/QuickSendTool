using System.Security;
using Microsoft.Win32;

namespace NoRain
{
    public static class Register
    {
        public static void Add()
        {
            // 定义右键菜单的名称和要执行的程序路径
            string menuName = "发送到服务器";
            string menuCommand = $"\"{Application.ExecutablePath}\" \"%1\""; // %1 表示选中的文件路径

            // 打开HKEY_CLASSES_ROOT\*\shell
            RegistryKey shellKey =
                Registry.ClassesRoot.OpenSubKey(@"*\shell", true)
                ?? Registry.ClassesRoot.CreateSubKey(@"*\shell");

            RegistryKey quickSendToolKey = shellKey.CreateSubKey(Config.AppName);

            quickSendToolKey.SetValue(null, menuName);

            quickSendToolKey.SetValue(
                "Icon",
                Application.ExecutablePath,
                RegistryValueKind.ExpandString
            );

            RegistryKey commandKey = quickSendToolKey.CreateSubKey("command");
            commandKey.SetValue(null, menuCommand);
        }

        public static void Remove()
        {
            // 打开HKEY_CLASSES_ROOT\*\shell
            RegistryKey? shellKey = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);

            if (shellKey != null)
            {
                // 移除指定的键（即右键菜单项）
                shellKey.DeleteSubKeyTree(Config.AppName, false);

                shellKey.Close();
            }
        }
    }
}
