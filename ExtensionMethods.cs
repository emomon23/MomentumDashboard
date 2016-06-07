using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com
{
    public static class ExtensionMethods
    {
        public static bool IsGuid(this string str)
        {
            Guid g;
            return Guid.TryParse(str, out g);
        }

        public static Guid ToGuid(this string str)
        {
            return Guid.Parse(str);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }


        public static bool IsNotNull(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsNumeric(this string str)
        {
            long l;
            bool result = long.TryParse(str, out l);

            if (!result)
            {
                double d;
                result = double.TryParse(str, out d);
            }

            return result;
        }

        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }
    }
}