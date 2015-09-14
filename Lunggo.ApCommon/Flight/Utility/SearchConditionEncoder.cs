using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Encoder;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public string EncodeConditions(SearchFlightConditions conditions)
        {
            var conditionString = new StringBuilder();
            foreach (var trip in conditions.Trips)
            {
                conditionString.Append(trip.OriginAirport);
                conditionString.Append(trip.DestinationAirport);
                conditionString.Append(trip.DepartureDate.ToString("ddMMyy"));
                if (trip != conditions.Trips.Last())
                    conditionString.Append(".");
            }
            conditionString.Append("-");
            conditionString.Append(conditions.AdultCount.ToString(CultureInfo.InvariantCulture));
            conditionString.Append(conditions.ChildCount.ToString(CultureInfo.InvariantCulture));
            conditionString.Append(conditions.InfantCount.ToString(CultureInfo.InvariantCulture));
            conditionString.Append(ParseCabinClass(conditions.CabinClass));
            return conditionString.ToString();
        }

        public string HashEncodeConditions(SearchFlightConditions conditions)
        {
            var conditionString = EncodeConditions(conditions);
            return conditionString.Base64Encode();
        }

        public SearchFlightConditions DecodeConditions(string conditionString)
        {
            try
            {
                var parts = conditionString.Split('-');
                var tripPart = parts.First();
                var infoPart = parts.Last();
                var isValid = ((tripPart.Length + 1) % 13 == 0) && (infoPart.Length == 4);

                if (isValid)
                {
                    var conditions = new SearchFlightConditions();
                    conditions.Trips = tripPart.Split('.').Select(info => new FlightTrip
                    {
                        OriginAirport = info.Substring(0, 3),
                        DestinationAirport = info.Substring(3, 3),
                        DepartureDate = new DateTime(
                            2000 + int.Parse(info.Substring(10, 2)),
                            int.Parse(info.Substring(8, 2)),
                            int.Parse(info.Substring(6, 2)))
                    }).ToList();
                    conditions.AdultCount = int.Parse(infoPart.Substring(0,1));
                    conditions.ChildCount = int.Parse(infoPart.Substring(1, 1));
                    conditions.InfantCount = int.Parse(infoPart.Substring(2, 1));
                    conditions.CabinClass = ParseCabinClass(infoPart.Substring(3, 1));
                    return conditions;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public SearchFlightConditions UnhashDecodeConditions(string hashedConditionString)
        {
            var conditionString = hashedConditionString.Base64Decode();
            return DecodeConditions(conditionString);
        }
    }
}
