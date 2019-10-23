using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger
{
    public static class Common
    {
        public static readonly string LoggerContextName = "LoggerContext";

        public static readonly string LoggerKeyStorageName = "Logger_JobId={0}";
    }
}
