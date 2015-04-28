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
        public static RedisValue ConvertItineraryListToFlightCacheObject(List<FlightItineraryFare> itineraryList)
        {
            var itineraryListJson = SerializeItineraryList(itineraryList);
            var itineraryListJsonCompressed = CompressionUtil.Compress(itineraryListJson);
            return itineraryListJsonCompressed;
        }

        public static List<FlightItineraryFare> ConvertFlightCacheObjectToItineraryList(RedisValue flightCacheObject)
        {
            if (flightCacheObject.IsNullOrEmpty)
            {
                return null;
            }
            else
            {
                byte[] itineraryListJsonCompressed = flightCacheObject;
                var itineraryListJson = CompressionUtil.Decompress(itineraryListJsonCompressed);
                return DeserializeItineraryList(itineraryListJson);
            }
        }

        private static string SerializeItineraryList(List<FlightItineraryFare> itineraryList)
        {
            return JsonConvert.SerializeObject(itineraryList);
        }

        private static List<FlightItineraryFare> DeserializeItineraryList(string itineraryListJsoned)
        {
            return JsonConvert.DeserializeObject<List<FlightItineraryFare>>(itineraryListJsoned);
        }
    }
}
