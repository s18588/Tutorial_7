using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial_5and6.Middleware
{
    public class Logger
    {   

        public static void WriteLog(string logMessage)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Log(logMessage, w);
          
            }
        }
        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("-------------------------------");
        }

    }

    

}