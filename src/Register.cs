using Microsoft.Win32;
using System.Security;
namespace NoRain
{
    public static class Register
    {

        public static void Add()
        {
            try
            {
                // 在 HKEY_CLASSES_ROOT\*\shell 下创建新键
                // RegistryKey shellKey = Registry.ClassesRoot.OpenSubKey(@"*\shell", true) ?? Registry.ClassesRoot.CreateSubKey(@"*\shell");
                // if (shellKey != null)
                // {
                //     RegistryKey newKey = shellKey.CreateSubKey("发送到目标服务器");
                //     RegistryKey commandKey = newKey.CreateSubKey("command");

                //     // 设置命令行
                //     commandKey.SetValue("", $"cmd.exe /C start \"\" /B \"{Application.ExecutablePath}\" \"%1\"");

                //     // 关闭注册表键
                //     commandKey.Close();
                //     newKey.Close();
                //     shellKey.Close();

                // }


                // 注册表项路径
                string keyPath = @"Software\Classes\*\shell\SendToServer";

                // 创建新的子项
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath))
                {
                    key.SetValue("", "发送到服务器");
                    using (RegistryKey subKey = key.CreateSubKey("command"))
                    {
                        // 替换为你的程序路径
                        string command = $"\"{Application.ExecutablePath}\" \"%1\"";
                        subKey.SetValue("", command);
                    }
                }

            }
            catch (SecurityException secEx)
            {
                // 权限问题
                Console.WriteLine("权限不足: " + secEx.Message);
            }
            catch (Exception ex)
            {
                // 其他错误
                Console.WriteLine("发生错误: " + ex.Message);
            }
        }


        public static void Remove()
        {
             string keyPath = @"Software\Classes\*\shell\SendToServer";
            try
            {
                // 删除注册表键
                Registry.ClassesRoot.DeleteSubKeyTree(keyPath);
            }
            catch (SecurityException secEx)
            {
                // 权限问题
                Console.WriteLine("权限不足: " + secEx.Message);
            }
            catch (Exception ex)
            {
                // 其他错误
                Console.WriteLine("发生错误: " + ex.Message);
            }
        }
    }
}