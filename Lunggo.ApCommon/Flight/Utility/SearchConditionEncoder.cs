﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Flight.Model;

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
            
            return conditionString.ToString().Base64Encode();
        }

        public SearchFlightConditions DecodeConditions(string searchId)
        {
            var conditionString = searchId.Base64Decode();

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
                    conditions.AdultCount = int.Parse(infoPart.Substring(0, 1));
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
    }
}
