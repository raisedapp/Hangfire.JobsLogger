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

        public static string GetKeyName(int seq, string jobId) 
        {
            return string
                .Format(Common.LoggerKeyStorageName, seq.ToString("0000"), jobId);
        }
    }
}