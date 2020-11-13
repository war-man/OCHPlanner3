using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OCHPlanner3.Helper
{
    public static class UserHelper
    {
        public static string GetStartUpUrl(string language, string returnUrl)
        {
            string defaultScreen = "Sticker/Simple";

            return  $"~/{language.ToLower()}/{defaultScreen}";
        }
    }
}
