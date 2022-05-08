using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstaller
{
    public enum LoggerLevel
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4,
    }

    public static class Log
    {
        private static List<string> _logs = new();

        public static void WriteLog(LoggerLevel level, string message)
        {
            if (level == LoggerLevel.FATAL)
                logger.writefatal(message);
            else if (level == LoggerLevel.ERROR)
                logger.writeerror(message);
            else if (level == LoggerLevel.WARN)
                logger.writewarn(message);
            else if (level == LoggerLevel.INFO)
                logger.writeinfo(message);
            else
                logger.writedebug(message);
        }
    }
}
