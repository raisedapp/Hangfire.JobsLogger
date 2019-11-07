using Hangfire.Dashboard;
using Hangfire.JobsLogger.Dashboard.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger.Dashboard
{
    /// <summary>
    /// Renderer for the tags in Enqueued state
    /// </summary>
    internal class EnqueuedStateRenderer
    {
        public NonEscapedString Render(HtmlHelper helper, IDictionary<string, string> stateData)
        {
            var bld = new StringBuilder();

            bld.Append("JEJEJEJEJEJE");

            var page = helper.GetPage();
            if (page.RequestPath.StartsWith("/jobs/details"))
            {
                // Find the jobid
                var jobId = page.RequestPath.Substring(" /jobs/details".Length);

            }

            return new NonEscapedString(bld.ToString());
        }
    }
}
