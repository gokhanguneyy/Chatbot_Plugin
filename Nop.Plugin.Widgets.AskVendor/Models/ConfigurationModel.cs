using System.ComponentModel.DataAnnotations;
using Nop.Services.Localization;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.AskVendor.Models
{
    public record class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; } 

        public string apiKeys { get; set; } 
        public bool apiKeys_OverrideForStore { get; set; } 

        public string apiEndPoints { get; set; }
        public bool apiEndPoints_OverrideForStroe { get; set; }

        

        [Range(0, int.MaxValue, ErrorMessage = "The counter value cannot be less than zero.")]
        public int message_counts { get; set; }  
        public bool counts_OverrideForStore { get; set; }
    }
}

