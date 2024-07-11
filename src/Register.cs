using Microsoft.Win32;
using System.Security;
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
            RegistryKey shellKey = Registry.ClassesRoot.OpenSubKey(@"*\shell", true) ?? Registry.ClassesRoot.CreateSubKey(@"*\shell");

            // 在shell下创建一个新的键（即新的右键菜单项）
            RegistryKey newKey = shellKey.CreateSubKey(menuName);
            if (newKey != null)
            {
                // 在新的键下创建command键，并设置其默认值为程序路径
                RegistryKey commandKey = newKey.CreateSubKey("command");
                if (commandKey != null)
                {
                    commandKey.SetValue("", menuCommand); // 设置默认值
                    commandKey.Close();
                }
                newKey.Close();
            }
            shellKey.Close();

        }

        public static void Remove()
        {
            // 定义要移除的右键菜单的名称
            string menuName = "发送到服务器";

            // 打开HKEY_CLASSES_ROOT\*\shell
            RegistryKey? shellKey = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);

            if (shellKey != null)
            {
                // 移除指定的键（即右键菜单项）
                shellKey.DeleteSubKeyTree(menuName, false);

                shellKey.Close();
            }
        }
    }
}