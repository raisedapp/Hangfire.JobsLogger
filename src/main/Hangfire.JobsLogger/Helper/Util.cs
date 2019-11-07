using Hangfire.JobsLogger.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static string GetFileNameFromURL(string url) 
        {
            int index = url.IndexOf("?");

            if (index != -1)
                url = url.Substring(0, index);

            return Path.GetFileNameWithoutExtension(url);
        }

        public static Color GetColorByLogLevel(LogLevel logLevel) 
        {
            var color = Color.Transparent;

            switch (logLevel)
            {
                case LogLevel.Trace: color = JobsLoggerFilter.Options.LogTraceColor; break;
                case LogLevel.Debug: color = JobsLoggerFilter.Options.LogDebugColor; break;
                case LogLevel.Information: color = JobsLoggerFilter.Options.LogInformationColor; break;
                case LogLevel.Warning: color = JobsLoggerFilter.Options.LogWarningColor; break;
                case LogLevel.Error: color = JobsLoggerFilter.Options.LogErrorColor; break;
                case LogLevel.Critical: color = JobsLoggerFilter.Options.LogCriticalColor; break;
            }

            return color;
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