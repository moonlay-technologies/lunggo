using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Constant
{
    public static class CacheIdentifier
    {
        public const string Flight = "4";
        public const string Hotel = "2";
        public const string Activity = "1";

        public static bool IsFlightCache(this string cacheId)
        {
            return cacheId.Substring(0, 1) == Flight;
        }

        public static bool IsHotelCache(this string cacheId)
        {
            return cacheId.Substring(0, 1) == Hotel;
        }

        public static bool IsActivityCache(this string cacheId)
        {
            return cacheId.Substring(0, 1) == Activity;
        }
    }
}
