# Hangfire.JobsLogger
[![NuGet](https://buildstats.info/nuget/Hangfire.JobsLogger)](https://www.nuget.org/packages/Hangfire.JobsLogger)
[![Actions Status Master](https://github.com/raisedapp/Hangfire.JobsLogger/workflows/CI-HF-JOBSLOGGER/badge.svg?branch=master)](https://github.com/raisedapp/Hangfire.JobsLogger/actions)
[![Actions Status Develop](https://github.com/raisedapp/Hangfire.JobsLogger/workflows/CI-HF-JOBSLOGGER/badge.svg?branch=develop)](https://github.com/raisedapp/Hangfire.JobsLogger/actions)
[![Official Site](https://img.shields.io/badge/site-hangfire.io-blue.svg)](http://hangfire.io)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](http://opensource.org/licenses/MIT)

## Overview
A Hangfire extension to store a log during job execution

![joblogshistory](content/job_logs_history.png)

## Installation
Install a package from Nuget. 
```
Install-Package Hangfire.JobsLogger
```

## Usage

### DotNetCore

For service side:
```csharp
services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"))
                                                 .UseJobsLogger();
```

### NetFramework

For startup side:
```csharp
GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection").UseJobsLogger();
```

### Example

```csharp
using Hangfire.JobsLogger;

RecurringJob.AddOrUpdate(() => taskExample.TaskMethod(null), Cron.Minutely);

//...

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
    _log.LogTrace(jobId: jobId, $"{i} - Trace Message.. {DateTime.UtcNow.Ticks}");
    _log.LogDebug(jobId: jobId, $"{i} - Debug Message.. {DateTime.UtcNow.Ticks}");
    _log.LogInformation(jobId: jobId, $"{i} - Information Message.. {DateTime.UtcNow.Ticks}");
    _log.LogWarning(jobId: jobId, $"{i} - Warning Message.. {DateTime.UtcNow.Ticks}");
    _log.LogError(jobId: jobId, $"{i} - Error Message.. {DateTime.UtcNow.Ticks}");
    _log.LogCritical(jobId: jobId, $"{i} - Critical Message.. {DateTime.UtcNow.Ticks}");
  }
}
```

**Note**
Hangfire is responsible for injecting an instance of the PerformContext class.

The logs can be consulted in the detail of the logs on the enqueued state card:
![jobdetail](content/job_detail.png)

#### Options

In the UseJobsLogger method you can use an instance of the Hangfire.JobsLogger.JobsLoggerOptions class to specify some options of this plugin.

Below is a description of them:

`Option` | `Description` | `Default Value`
--- | --- | ---
**LogLevel** | Set the log level to be stored in hangfire | **Microsoft.Extensions.Logging.LogLevel.Trace**
**LogTraceColor** |  Color that will be used to display log messages of this type | **System.Drawing.Color.LightGreen**
**LogDebugColor** |  Color that will be used to display log messages of this type | **System.Drawing.Color.DarkGreen**
**LogInformationColor** |  Color that will be used to display log messages of this type | **System.Drawing.Color.Blue**
**LogWarningColor** |  Color that will be used to display log messages of this type | **System.Drawing.Color.DarkOrange**
**LogErrorColor** |  Color that will be used to display log messages of this type | **System.Drawing.Color.Red**
**LogCriticalColor** |  Color that will be used to display log messages of this type | **System.Drawing.Color.DarkRed**

## Credits
 * Brayan Mota
 * Lucas Ferreras
 
## Thanks

This project would not have been possible, without the collaboration of the following projects:

 * [Hangfire.Console](https://github.com/pieceofsummer/Hangfire.Console)
 * [Hangfire.Hearbeat](https://github.com/ahydrax/Hangfire.Heartbeat)
 * [Hangfire.RecurringJobAdmin](https://github.com/bamotav/Hangfire.RecurringJobAdmin)
 * [Hangfire.LiteDb](https://github.com/codeyu/Hangfire.LiteDB)
 * [Hangfire.Tags](https://github.com/face-it/Hangfire.Tags)
 
## Donation
If this project help you reduce time to develop, you can give me a cup of coffee :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=RMLQM296TCM38&item_name=For+the+development+of+Hangfire.JobsLogger&currency_code=USD&source=url)

## License
This project is under MIT license. You can obtain the license copy [here](https://github.com/raisedapp/Hangfire.JobsLogger/blob/master/LICENSE).
