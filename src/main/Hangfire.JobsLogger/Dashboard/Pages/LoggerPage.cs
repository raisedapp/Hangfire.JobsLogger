using Hangfire.Dashboard;
using Hangfire.Dashboard.Pages;
using Hangfire.JobsLogger.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger.Dashboard.Pages
{
    internal sealed class LoggerPage : RazorPage
    {
        public const string Title = "Logging";
        public const string PageRoute = "/Logger";

        private static readonly string PageHtml;

        static LoggerPage()
        {
            PageHtml = Util.ReadStringResource("Hangfire.JobsLogger.Dashboard.Page.Html.Logger.html");
        }

        public override void Execute()
        {
            WriteEmptyLine();
            Layout = new LayoutPage(Title);
            WriteLiteralLine(PageHtml);
            WriteEmptyLine();
        }

        private void WriteLiteralLine(string textToAppend)
        {
            WriteLiteral(textToAppend);
            WriteLiteral("\r\n");
        }

        private void WriteEmptyLine()
        {
            WriteLiteral("\r\n");
        }
    }
}
