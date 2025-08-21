using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.AskVendor.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;

namespace Nop.Plugin.Widgets.AskVendor.Controllers
{
    public class WidgetsAskVendorController : BaseAdminController
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        public WidgetsAskVendorController(ILocalizationService localizationService,
             INotificationService notificationService,
             IPermissionService permissionService,
             ISettingService settingService,
             IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();
         

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var askVendorSettings = await _settingService.LoadSettingAsync<AskVendorSettings>(storeScope);
            
            
            var model = new ConfigurationModel()
            {
                apiKeys = askVendorSettings.apiKey, 
                apiEndPoints = askVendorSettings.apiEndPoint,
                message_counts = askVendorSettings.message_count,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.apiKeys_OverrideForStore = await _settingService.SettingExistsAsync(askVendorSettings, x => x.apiKey, storeScope);
                model.apiEndPoints_OverrideForStroe = await _settingService.SettingExistsAsync(askVendorSettings, x => x.apiEndPoint, storeScope); 
                model.counts_OverrideForStore=await _settingService.SettingExistsAsync(askVendorSettings, x=>x.message_count, storeScope);
            }

            return View("~/Plugins/Widgets.AskVendor/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {

        
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var askVendorSettings = await _settingService.LoadSettingAsync<AskVendorSettings>(storeScope);


            askVendorSettings.apiKey = model.apiKeys;
            askVendorSettings.apiEndPoint = model.apiEndPoints;
            // burada mesaj girme hakkının 0'dan küçük olmadığını kontrol ediyorum
            if (model.message_counts < 0)
            {
                askVendorSettings.message_count = 0;
            }
            else
            {
                askVendorSettings.message_count = model.message_counts;
            }


            await _settingService.SaveSettingOverridablePerStoreAsync(askVendorSettings, x=> x.apiKey, model.apiKeys_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(askVendorSettings, x => x.apiEndPoint, model.apiEndPoints_OverrideForStroe, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(askVendorSettings, x => x.message_count, model.counts_OverrideForStore, storeScope, false);

            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        
    }
}
