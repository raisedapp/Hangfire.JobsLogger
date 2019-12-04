using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Hangfire.JobsLogger
{
    /// <summary>github
    /// Options for configuring the Hangfire.JobsLogger plugin
    /// </summary>
    public class JobsLoggerOptions
    {
        /// <summary>
        /// Set the log level to be stored in hangfire
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Trace;

        /// <summary>
        /// Color that will be used to display log messages of this type
        /// </summary>
        public Color LogTraceColor { get; set; } = Color.LightGreen;

        /// <summary>
        /// Color that will be used to display log messages of this type
        /// </summary>
        public Color LogDebugColor { get; set; } = Color.DarkGreen;

        /// <summary>
        /// Color that will be used to display log messages of this type
        /// </summary>
        public Color LogInformationColor { get; set; } = Color.Blue;

        /// <summary>
        /// Color that will be used to display log messages of this type
        /// </summary>
        public Color LogWarningColor { get; set; } = Color.DarkOrange;

        /// <summary>
        /// Color that will be used to display log messages of this type
        /// </summary>
        public Color LogErrorColor { get; set; } = Color.Red;

        /// <summary>
        /// Color that will be used to display log messages of this type
        /// </summary>
        public Color LogCriticalColor { get; set; } = Color.DarkRed;
    }
}