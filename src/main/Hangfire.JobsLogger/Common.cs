using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger
{
    internal static class Common
    {
        public static readonly string LoggerContextName = "LoggerContext";

        public static readonly string LoggerCounterStorageName = "Logger_Counter_JobId={0}";

        public static readonly string LoggerKeyStorageName = "Logger_Page_{0}_JobId={1}";
    }
}
