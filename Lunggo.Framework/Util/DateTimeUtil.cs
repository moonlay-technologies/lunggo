using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
