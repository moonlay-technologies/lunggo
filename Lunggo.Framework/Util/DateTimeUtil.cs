using System;

namespace Lunggo.Framework.Util
{
    public class DateTimeUtil
    {
        public static DateTime GetUtcPlusDateTime(int hourOffset)
        {
            return DateTime.UtcNow.AddHours(hourOffset);
        }

        public static DateTime GetJakartaDateTime()
        {
            const int jakartaOffset = 7;
            return GetUtcPlusDateTime(jakartaOffset);
        }
    }
}
