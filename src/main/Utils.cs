﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger
{
    internal static class Utils
    {
        public static string GetKeyName(string jobId) 
        {
            return string.Format(Common.LoggerKeyStorageName, jobId);
        }
    }
}