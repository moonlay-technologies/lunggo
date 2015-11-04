using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Util;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Lunggo.ApCommon.Flight.Utility
{
    public class FlightCacheUtil
    {
        public static TopDestinations ConvertTopDestinationRawObjectToTopDestination(RedisValue topDestinationCacheObject)
        {
            if (topDestinationCacheObject.IsNullOrEmpty)
            {
                return null;
            }
            else
            {
                byte[] topDestinationJsonCompressed = topDestinationCacheObject;
                var topDestinationjson = CompressionUtil.Decompress(topDestinationJsonCompressed);
                return DeserializeTopDestination(topDestinationjson);
            }

        }
        private static TopDestinations DeserializeTopDestination(String topDestinationJsoned)
        {
            return JsonConvert.DeserializeObject<TopDestinations>(topDestinationJsoned);
        }
    }
}
