using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Text;

namespace Hangfire.JobsLogger.Helper
{
    internal static class Util
    {
        public static string GetLoggerContextName(string jobId)
        {
            return string.Format(Common.LoggerContextName, jobId);
        }

        public static string GetCounterName(string jobId) 
        {
            return string.Format(Common.LoggerCounterStorageName, jobId);
        }

        public static string GetKeyName(int seq, string jobId) 
        {
            return string
                .Format(Common.LoggerKeyStorageName, seq.ToString("0000"), jobId);
        }

        public static string ReadStringResource(string resourceName)
        {
            var assembly = typeof(Util).Assembly;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new MissingManifestResourceException($"Cannot find resource {resourceName}");

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}