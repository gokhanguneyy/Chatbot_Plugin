using FluentMigrator.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.AskVendor
{
    public class AskVendorSettings : ISettings
    {
        public string apiKey { get; set; }
        public string apiEndPoint { get; set; }

        public int message_count { get; set; }
    }
}
