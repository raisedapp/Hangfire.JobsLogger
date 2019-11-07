using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Hangfire.JobsLogger
{
    public class JobsLoggerOptions
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public Color LogTraceColor { get; set; } = Color.LightGreen; 

        public Color LogDebugColor { get; set; } = Color.DarkGreen;

        public Color LogInformationColor { get; set; } = Color.Blue;

        public Color LogWarningColor { get; set; } = Color.DarkOrange;

        public Color LogErrorColor { get; set; } = Color.Red;

        public Color LogCriticalColor { get; set; } = Color.DarkRed;
    }
}