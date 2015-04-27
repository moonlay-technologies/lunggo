using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Logic;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;
using Lunggo.Framework.Util;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Logic
{
    public class FlightCacheUtil
    {
        public static RedisKey GetFlightDetailKeyInCache(String FlightId)
        {
            return "flightdetail:" + FlightId;
        }

        public static RedisKey GetFlightDetailKeyInCache(int FlightId)
        {
            return GetFlightDetailKeyInCache(FlightId.ToString(CultureInfo.InvariantCulture));
        }

        public static RedisKey[] GetFlightDetailKeyInCacheArray(int[] FlightIdList)
        {
            return FlightIdList.Select(GetFlightDetailKeyInCache).ToArray();
        }

        public static String SerializeFlightItin(FlightItineraryFare ItinList)
        {
            return JsonConvert.SerializeObject(ItinList);
        }

        public static FlightItineraryFare DeserializeFlightItin(String FlightItinListJsoned)
        {
            return JsonConvert.DeserializeObject<FlightItineraryFare>(FlightItinListJsoned);
        }
    }
}