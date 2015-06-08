using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Util;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Utility
{
    internal static class FlightCacheUtil
    {
        internal static RedisValue ToCacheObject<T>(this T input)
        {
            var serializedInput = input.Serialize();
            var compressedInput = CompressionUtil.Compress(serializedInput);
            return compressedInput;
        }

        internal static T DeconvertTo<T>(this RedisValue cacheObject)
        {
            if (cacheObject.IsNullOrEmpty)
            {
                return default(T);
            }
            else
            {
                byte[] compressedJson = cacheObject;
                var json = CompressionUtil.Decompress(compressedJson);
                return json.Deserialize<T>();
            }
        }

        internal static string Serialize<T>(this T input)
        {
            return JsonConvert.SerializeObject(input);
        }

        internal static T Deserialize<T>(this string jsonInput)
        {
            return JsonConvert.DeserializeObject<T>(jsonInput);
        }
    }
}
