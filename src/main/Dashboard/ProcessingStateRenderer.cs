using Hangfire.Dashboard;
using Hangfire.JobsLogger.Dashboard.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger.Dashboard
{
    internal class ProcessingStateRenderer
    {
        // ReSharper disable once NotAccessedField.Local

        //private readonly ConsoleOptions _options;

        //public ProcessingStateRenderer(ConsoleOptions options)
        //{
        //    _options = options ?? throw new ArgumentNullException(nameof(options));
        //}

        public NonEscapedString Render(HtmlHelper helper, IDictionary<string, string> stateData)
        {
            var builder = new StringBuilder();

            builder.Append("<dl class=\"dl-horizontal\">");


            builder.Append("<dt>Felix la leche:</dt>");
            builder.Append($"<dd>EST025665</dd>");


            builder.Append("<dt>Worker:</dt>");
            builder.Append($"<dd>#45</dd>");


            builder.Append("</dl>");


            var page = helper.GetPage();

            if (page.RequestPath.StartsWith("/jobs/details/"))
            {

            }
            //var page = helper.GetPage();
            //if (page.RequestPath.StartsWith("/jobs/details/"))
            //{
            //    // We cannot cast page to an internal type JobDetailsPage to get jobId :(
            //    var jobId = page.RequestPath.Substring("/jobs/details/".Length);

            //    var startedAt = JobHelper.DeserializeDateTime(stateData["StartedAt"]);
            //    var consoleId = new ConsoleId(jobId, startedAt);

            //    builder.Append("<div class=\"console-area\">");
            //    builder.AppendFormat("<div class=\"console\" data-id=\"{0}\">", consoleId);

            //    using (var storage = new ConsoleStorage(page.Storage.GetConnection()))
            //    {
            //        ConsoleRenderer.RenderLineBuffer(builder, storage, consoleId, 0);
            //    }

            //    builder.Append("</div>");
            //    builder.Append("</div>");
            //}

            return new NonEscapedString(builder.ToString());
        }
    }
}
