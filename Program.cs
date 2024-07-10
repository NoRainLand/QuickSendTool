namespace NoRain
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            if (QSConfig.ReadConfig())
            {

            }
            else
            {

                QSView.ShowView();
            }
        }
    }
}