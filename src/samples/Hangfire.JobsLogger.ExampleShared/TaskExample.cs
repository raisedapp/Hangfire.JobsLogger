using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Hangfire.JobsLogger.ExampleShared
{
    public class TaskExample
    {
        public static readonly string ConnectiongStringLiteDb = @"Filename=Hangfire.db;Mode=Exclusive";

        private readonly ILogger _log = ApplicationLogging.CreateLogger<TaskExample>();

        public void TaskMethod(PerformContext context)
        {
            var jobId = context.BackgroundJob.Id;

            foreach (int i in Enumerable.Range(1, 10)) 
            {
                context.LogTrace($"{i} - Trace Message.. {DateTime.UtcNow.Ticks}");
                context.LogDebug($"{i} - Debug Message.. {DateTime.UtcNow.Ticks}");
                context.LogInformation($"{i} - Information Message.. {DateTime.UtcNow.Ticks}");
                context.LogWarning($"{i} - Warning Message.. {DateTime.UtcNow.Ticks}");
                context.LogError($"{i} - Error Message.. {DateTime.UtcNow.Ticks}");
                context.LogCritical($"{i} - Critical Message.. {DateTime.UtcNow.Ticks}");

                //Traditional ILogger Usage
                _log.LogTrace($"{i} - Trace Message.. {DateTime.UtcNow.Ticks}" + "{0}", jobId);
                _log.LogDebug($"{i} - Debug Message.. {DateTime.UtcNow.Ticks}", jobId);
                _log.LogInformation($"{i} - Information Message.. {DateTime.UtcNow.Ticks}", jobId);
                _log.LogWarning($"{i} - Warning Message.. {DateTime.UtcNow.Ticks}", jobId);
                _log.LogError($"{i} - Error Message.. {DateTime.UtcNow.Ticks}", jobId);
                _log.LogCritical($"{i} - Critical Message.. {DateTime.UtcNow.Ticks}", jobId);
            }
        }
    }
}