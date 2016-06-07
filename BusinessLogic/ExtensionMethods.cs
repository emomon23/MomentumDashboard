using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.BusinessLogic
{
    public static class ExtensionMethods
    {
        public static Guid ToGuid(this string str)
        {
            return Guid.Parse(str);
        }

        public static Guid ToGuid(this object obj)
        {
            return Guid.Parse(obj.ToString());
        }
    }
}