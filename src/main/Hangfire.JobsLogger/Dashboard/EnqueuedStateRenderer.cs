using Hangfire.Dashboard;
using Hangfire.JobsLogger.Dashboard.Extensions;
using Hangfire.JobsLogger.Helper;
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
            var page = helper.GetPage();
            var jobId = Util.GetFileNameFromURL(page.RequestPath);

            var stateCard = new StringBuilder();
            stateCard.AppendLine("<dl class=\"dl-horizontal\">");
            stateCard.AppendLine($"<dt>Queue:</dt><dd>{helper.QueueLabel(stateData["Queue"])}</dd>");
            stateCard.AppendLine($"<dt>Logs:</dt><dd><a class=\"text-uppercase\" href=\"../../jobs/logs/{jobId}\">View here</a></dd>");
            stateCard.AppendLine("</dl>");

            return new NonEscapedString(stateCard.ToString());
        }
    }
}
