﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OCHPlanner3.Helper
{
    public static class UserHelper
    {
        public static string GetStartUpUrl()
        {
            string defaultScreen = "Sticker/Simple";

            return $"~/{defaultScreen}"; //default
        }
    }
}