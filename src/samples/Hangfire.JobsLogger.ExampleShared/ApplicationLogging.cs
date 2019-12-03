using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger.ExampleShared
{
    public static class ApplicationLogging
    {
        private static NullLoggerFactory NullLoggerFactory { get; } = new NullLoggerFactory();

        public static ILoggerFactory LoggerFactory { get; set; }

        public static ILogger CreateLogger<T>() => LoggerFactory?.CreateLogger<T>() ?? NullLoggerFactory.CreateLogger<T>();

        public static ILogger CreateLogger(string categoryName) => LoggerFactory?.CreateLogger(categoryName) ?? NullLoggerFactory.CreateLogger(categoryName);
    }
}
