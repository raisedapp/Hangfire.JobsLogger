using Hangfire.Server;
using System;
using Hangfire.Console;

namespace Hangfire.JobsLogger.ExampleShared
{
    public class TaskExample
    {
        public void TaskMethod(PerformContext context)
        {
            context.SetTextColor(ConsoleTextColor.Red);
            context.WriteLine("Error!");
            context.ResetTextColor();

            context.LogTrace($"Trace Message.. {DateTime.UtcNow.Ticks}");
            context.LogDebug($"Debug Message.. {DateTime.UtcNow.Ticks}");
            context.LogInformation($"Information Message.. {DateTime.UtcNow.Ticks}");
            context.LogWarning($"Warning Message.. {DateTime.UtcNow.Ticks}");
            context.LogError($"Error Message.. {DateTime.UtcNow.Ticks}");
            context.LogCritical($"Critical Message.. {DateTime.UtcNow.Ticks}");
        }
    }
}