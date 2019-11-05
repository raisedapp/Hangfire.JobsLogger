using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger
{
    internal static class Utils
    {
        public static string GetCounterName(string jobId) 
        {
            return string.Format(Common.LoggerCounterStorageName, jobId);
        }

        public static string GetKeyName(int page, string jobId) 
        {
            return string
                .Format(Common.LoggerKeyStorageName, Convert.ToString(page), jobId);
        }
    }
}
