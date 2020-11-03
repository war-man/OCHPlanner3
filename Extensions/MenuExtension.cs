using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class MenuExtension
    {
        public static bool IsMenuActive(this IHtmlHelper htmlHelper, string menuItemUrl)
        {
            var viewContext = htmlHelper.ViewContext;
            var currentPageUrl = viewContext.ViewData["ActiveMenu"] as string ?? viewContext.HttpContext.Request.Path;
            return currentPageUrl.StartsWith("/en" + menuItemUrl, StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.StartsWith("/fr" + menuItemUrl, StringComparison.OrdinalIgnoreCase);
        }
    }
}
