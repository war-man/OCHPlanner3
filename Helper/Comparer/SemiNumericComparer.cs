using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Helper.Comparer
{

    public class SemiNumericComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            int xVal, yVal;
            var xIsVal = int.TryParse(s1, out xVal);
            var yIsVal = int.TryParse(s2, out yVal);

            if (xIsVal && yIsVal)   // both are numbers...
                return xVal.CompareTo(yVal);
            if (!xIsVal && !yIsVal) // both are strings...
                return s1.CompareTo(s2);
            if (xIsVal)             // x is a number, sort first
                return -1;
            return 1;
        }

        public static bool IsNumeric(object value)
        {
            try
            {
                long i = Convert.ToInt64(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

}
