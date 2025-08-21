using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.AskVendor.Models;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.AskVendor.Components
{
    public class AskVendorViewComponent : NopViewComponent
    {
       
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var model = (ProductDetailsModel)additionalData;
          
            return View("~/Plugins/Widgets.AskVendor/Views/AskVendor.cshtml", model);
        }
    }
}

    
