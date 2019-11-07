using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger.Model
{
    internal class LogMessage
    {
        public LogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public DateTime DateCreation { get; set; }
    }
}