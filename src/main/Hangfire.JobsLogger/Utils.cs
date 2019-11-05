using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger
{
    internal static class Utils
    {
        public static string GetKeyName(string page, string jobId) 
        {
            return string.Format(Common.LoggerKeyStorageName, page, jobId);
        }
    }
}
