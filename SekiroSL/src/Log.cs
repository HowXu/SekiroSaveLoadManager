using System;

namespace SekiroSL
{
    class Log
    {
        public static void log(string info)
        {
            if (MainWindow.LogLevel == 1)
            {
                Console.WriteLine(info);
            }
        }
    }
}
