using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nop.Core;
using Nop.Plugin.Widgets.AskVendor.Service;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.AskVendor.Controllers
{
    public class GptController : BasePluginController    
    {
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly ILocalizationService _localizationService;

        // ctor
        public GptController(ISettingService settingService, IStoreContext storeContext, ILogger logger, IMemoryCache cache, ILocalizationService localizationService)
        {
            _settingService = settingService;
            _storeContext = storeContext;
            _logger = logger;
            _memoryCache = cache;
            _localizationService = localizationService;
        }


        // Kullanıcıdan mesaj alınır, GptService'e bağlanılır ve mesaja cevap döndürülür.
        [HttpGet]
        public async Task<string> SendNewRequest(string request)
        {
            var storeScope = _storeContext.GetActiveStoreScopeConfigurationAsync().Result;
            var askVendorSettings = _settingService.LoadSettingAsync<AskVendorSettings>(storeScope).Result;
            string key = askVendorSettings.apiKey;
            string url = askVendorSettings.apiEndPoint;
            int count = askVendorSettings.message_count;
            GptService gptService = new GptService(_logger, _localizationService);
            string cacheKey = "MessageCount";
            
            if (_memoryCache.TryGetValue(cacheKey, out int messageCount))
            {
                var response = gptService.Answer(request, key, url);
                messageCount++; 

                _memoryCache.Set(cacheKey, messageCount, TimeSpan.FromMinutes(30));

                if(messageCount > count)
                {
                    var result = await _localizationService.GetResourceAsync("Plugin.Widgets.AskVendor.Cache.MessageCount");
                    return result;
                }
                return await response;
            }
            else
            {
                // burada eğer mesaj girme hakkı sıfırsa, kullanıcı soru sorduğunda hata mesajı almasını sağlıyoruz.
                
                if (count != 0)
                {
                    _memoryCache.Set(cacheKey, 1, TimeSpan.FromMinutes(30));
                    var response = await gptService.Answer(request, key, url);
                    return  response;
                }
                else
                {
                    var response = await _localizationService.GetResourceAsync("Plugin.Widgets.AskVendor.Cache.Error");
                    return response;
                }
                
            }            
        }

    }
}
