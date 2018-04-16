using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Encoder;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {

        public string EncodeSearchConditions(SearchFlightConditions conditions)
        {
            var searchId = new StringBuilder();
            foreach (var trip in conditions.Trips)
            {
                searchId.Append(trip.OriginAirport);
                searchId.Append(trip.DestinationAirport);
                searchId.Append(trip.DepartureDate.ToString("ddMMyy"));
                if (trip != conditions.Trips.Last())
                    searchId.Append("~");
            }
            searchId.Append("-");
            searchId.Append(conditions.AdultCount.ToString(CultureInfo.InvariantCulture));
            searchId.Append(conditions.ChildCount.ToString(CultureInfo.InvariantCulture));
            searchId.Append(conditions.InfantCount.ToString(CultureInfo.InvariantCulture));
            searchId.Append(ParseCabinClass(conditions.CabinClass));

            return searchId.ToString();
        }

        public SearchFlightConditions DecodeSearchConditions(string searchId)
        {

                var parts = searchId.Split('-');
                var tripPart = parts.First();
                var infoPart = parts.Last();
                var isValid = ((tripPart.Length + 1) % 13 == 0) && (infoPart.Length == 4);

                if (isValid)
                {
                    var conditions = new SearchFlightConditions();
                    conditions.Trips = tripPart.Split('~').Select(info => new FlightTrip
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

        public bool IsSearchIdValid(string searchId)
        {
            var conditions = DecodeSearchConditions(searchId);
            if (conditions == null)
                return false;
            return
                conditions.AdultCount >= 1 &&
                conditions.ChildCount >= 0 &&
                conditions.InfantCount >= 0 &&
                conditions.AdultCount + conditions.ChildCount + conditions.InfantCount <= 9 &&
                conditions.InfantCount <= conditions.AdultCount &&
                conditions.Trips.TrueForAll(data => data.DepartureDate >= DateTime.UtcNow.Date);
        }
    }
}
