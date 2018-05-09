using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.Framework.TestHelpers
{
    public static partial class TestHelper
    {
        public static string RandomString()
        {
            return Guid.NewGuid().ToString();
        }

        public static long RandomLong()
        {
            var rand = new Random();
            long result = rand.Next(int.MinValue, int.MaxValue);
            result = result << 32;
            result = result | rand.Next(int.MinValue, int.MaxValue);
            return result;
        }

        public static int RandomInt()
        {
            var rand = new Random();
            return rand.Next(int.MinValue, int.MaxValue);
        }

        public static int RandomInt(int min, int max)
        {
            var rand = new Random();
            return rand.Next(min, max);
        }

        public static decimal RandomDecimal()
        {
            return RandomInt();
        }

        public static decimal RandomDecimal(decimal min, decimal max)
        {
            return RandomInt((int) min, max > int.MaxValue ? int.MaxValue : (int) max);
        }

        public static DateTime RandomDateTime()
        {
            return DateTime.FromBinary(RandomInt());
        }

        public static DateTime RandomDateTime(DateTime min, DateTime max)
        {
            var rand = new Random();
            var range = max - min;
            var random = rand.Next(range.TotalSeconds > int.MaxValue ? int.MaxValue : Convert.ToInt32(range.TotalSeconds));
            var randomDateTime = min.AddSeconds(random);
            return randomDateTime;
        }
    }
}
