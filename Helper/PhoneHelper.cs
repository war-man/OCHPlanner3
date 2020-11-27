using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Helper
{
    public static class PhoneHelper
    {
        public static string ToPhone(this string phone)
        {
            return String.Format("{0:(###) ###-####}", double.Parse(phone));
        }

        public static string ToPhoneDatabase(this string phone)
        {
            return phone.Replace("(", "")
                        .Replace(")", "")
                        .Replace(" ", "")
                        .Replace("-", "");
        }
    }
}
