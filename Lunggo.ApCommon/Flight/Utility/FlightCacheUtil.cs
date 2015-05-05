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
    internal class FlightCacheUtil
    {
        internal static RedisValue ConvertToCacheObject<T>(T input)
        {
            var serializedInput = Serialize(input);
            var compressedInput = CompressionUtil.Compress(serializedInput);
            return compressedInput;
        }

        internal static T DeconvertFromCacheObject<T>(RedisValue cacheObject)
        {
            if (cacheObject.IsNullOrEmpty)
            {
                return default(T);
            }
            else
            {
                byte[] compressedJson = cacheObject;
                var json = CompressionUtil.Decompress(compressedJson);
                return Deserialize<T>(json);
            }
        }

        internal static string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }

        internal static T Deserialize<T>(string jsonInput)
        {
            return JsonConvert.DeserializeObject<T>(jsonInput);
        }
    }
}
