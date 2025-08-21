using Nop.Core;
using Nop.Plugin.Widgets.AskVendor.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.AskVendor
{
    public class AskVendorPlugin : BasePlugin, IWidgetPlugin 
    {
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;

        /// <summary>
        /// ctor
        /// </summary>
        public AskVendorPlugin(IWebHelper webHelper, ILocalizationService localizationService, ISettingService settingService)
        {
            _webHelper = webHelper;
            _settingService = settingService;
            _localizationService = localizationService;

        }


        /// <summary>
        /// Widget listesinde widget'ın görünüp görünmeyeceğini belirler. false görünür true görünmez
        /// </summary>
        public bool HideInWidgetList => false;


        /// <summary>
        /// Hangi widget gösterilecek
        /// /// </summary>
        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(AskVendorViewComponent);
        }


        /// <summary>
        /// Widget nerede gösterilecek.
        /// </summary>
        public async Task<IList<string>> GetWidgetZonesAsync()
        {
            var widgetZones = new List<string> {PublicWidgetZones.ProductDetailsInsideOverviewButtonsAfter };

            return await Task.FromResult(widgetZones);
        }

        //Configure Page
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsAskVendor/Configure";
        }

        public override async Task InstallAsync()
        {
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.AskVendor.Header"] = "API KEY",
                ["Plugins.Widgets.AskVendor.Save"]="SAVE",
                ["Plugin.Widgets.AskVendor.Button"] = "Chatbot",
                ["Plugin.Widgets.AskVendor.Send"]="SEND",
                ["Plugin.Widgets.AskVendor.Cache.MessageCount"] = "Your right to send messages has expired.",
                ["Plugin.Widgets.AskVendor.Api.Error"] = "Please check the api servise!",
                ["Plugin.Widgets.AskVendor.Cache.Error"] = "Check the message limit!",
                ["Plugin.Widgets.AskVendor.Hi"]= "Hi! You can ask any questions you have about the product you are examining. Don't forget! You have the right to ask a limited number of questions.",
            });
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            
            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.AskVendor");

            await base.UninstallAsync();
        }
    }
}

