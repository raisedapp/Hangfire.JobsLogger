using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration UseJobsLogger(this IGlobalConfiguration configuration,
            JobsLoggerOptions options = null)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            options = options ?? new JobsLoggerOptions();

            return configuration;
        }
    }
}
