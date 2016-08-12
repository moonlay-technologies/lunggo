using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Extension
{
    public static class DateTimeExtension
    {
        public static DateTime SpecifyUtc(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        public static DateTime? SpecifyUtc(this DateTime? dateTime)
        {
            return dateTime.HasValue
                ? dateTime.Value.SpecifyUtc()
                : (DateTime?) null;
        }

        public static DateTime Truncate(this DateTime dateTime, TimeSpan rounding)
        {
            return rounding == TimeSpan.Zero 
                ? dateTime 
                : dateTime.AddTicks(-(dateTime.Ticks % rounding.Ticks));
        }

        public static DateTime? Truncate(this DateTime? dateTime, TimeSpan rounding)
        {
            return dateTime.HasValue
                ? dateTime.Value.Truncate(rounding)
                : (DateTime?)null;
        }

        public static DateTime TruncateMilliseconds(this DateTime dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromSeconds(1));
        }

        public static DateTime? TruncateMilliseconds(this DateTime? dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromSeconds(1));
        }

        public static DateTime TruncateSeconds(this DateTime dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromMinutes(1));
        }

        public static DateTime? TruncateSeconds(this DateTime? dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromMinutes(1));
        }

        public static DateTime TruncateMinutes(this DateTime dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromHours(1));
        }

        public static DateTime? TruncateMinutes(this DateTime? dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromHours(1));
        }

        public static DateTime TruncateTime(this DateTime dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromHours(1));
        }

        public static DateTime? TruncateTime(this DateTime? dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromHours(1));
        }
    }
}
