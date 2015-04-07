using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.SessionState;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public SearchFlightOutput SearchFlight(SearchFlightInput input)
        {
            var inputTuple = input.Conditions.OriDestInfos;
            var conditions = new SearchFlightConditions();
            var returnConditions = new SearchFlightConditions();
            SearchFlightResult result;
            var output = new SearchFlightOutput();
            if (input.TripType == TripType.Return && input.IsReturnSeparated)
            {
                conditions.AdultCount = input.AdultCount;
                conditions.ChildCount = input.ChildCount;
                conditions.InfantCount = input.InfantCount;
                conditions.CabinClass = input.Conditions.CabinClass;
                conditions.OriDestInfos = new List<OriginDestinationInfo>
                {
                    new OriginDestinationInfo
                    {
                        OriginAirport = inputTuple[0].OriginAirport,
                        DestinationAirport = inputTuple[0].DestinationAirport,
                        DepartureDate = inputTuple[0].DepartureDate
                    }
                };

                returnConditions.AdultCount = input.AdultCount;
                returnConditions.ChildCount = input.ChildCount;
                returnConditions.InfantCount = input.InfantCount;
                returnConditions.CabinClass = input.Conditions.CabinClass;
                returnConditions.OriDestInfos = new List<OriginDestinationInfo>
                {
                    new OriginDestinationInfo
                    {
                        OriginAirport = inputTuple[1].OriginAirport,
                        DestinationAirport = inputTuple[1].DestinationAirport,
                        DepartureDate = inputTuple[1].DepartureDate
                    }
                };

                result = SearchFlightInternal(conditions);
                var returnResult = SearchFlightInternal(returnConditions);

                output.IsReturnType = true;
                output.Itineraries = result.FlightItineraries;
                output.ReturnItineraries = returnResult.FlightItineraries;
                if (result.FlightItineraries != null)
                    output.Any = true;
                if (returnResult.FlightItineraries != null)
                    output.ReturnAny = true;

                if (!result.IsSuccess || !returnResult.IsSuccess)
                {
                    output.Errors.AddRange(result.Errors);
                    output.Errors.AddRange(returnResult.Errors);
                    output.Errors = output.Errors.Distinct().ToList();
                    output.ErrorMessages.AddRange(result.ErrorMessages);
                    output.ErrorMessages.AddRange(returnResult.ErrorMessages);
                    output.ErrorMessages = output.ErrorMessages.Distinct().ToList();
                }
            }
            else
            {
                conditions.AdultCount = input.AdultCount;
                conditions.ChildCount = input.ChildCount;
                conditions.InfantCount = input.InfantCount;
                conditions.CabinClass = input.Conditions.CabinClass;
                switch (input.TripType)
                {
                    case TripType.OneWay :
                        conditions.OriDestInfos = MapOriginDestinationDates(input.Conditions.OriDestInfos, 1);
                        break;
                    case TripType.Return :
                        conditions.OriDestInfos = MapOriginDestinationDates(input.Conditions.OriDestInfos, 2);
                        break;
                    default :
                        conditions.OriDestInfos = MapOriginDestinationDates(input.Conditions.OriDestInfos);
                        break;
                }
                

                result = SearchFlightInternal(conditions);

                output.IsReturnType = false;
                output.Itineraries = result.FlightItineraries;
                output.ReturnItineraries = null;
                if (result.FlightItineraries != null)
                    output.Any = true;

                if (!result.IsSuccess)
                {
                    output.Errors.AddRange(result.Errors);
                    output.ErrorMessages.AddRange(result.ErrorMessages);
                }
            }
            return output;
        }

        private static List<OriginDestinationInfo> MapOriginDestinationDates(IEnumerable<OriginDestinationInfo> source, int count = 0)
        {
            var result = source.Select(item => new OriginDestinationInfo
            {
                OriginAirport = item.OriginAirport,
                DestinationAirport = item.DestinationAirport,
                DepartureDate = item.DepartureDate
            });
            return count == 0 ? result.ToList() : result.Take(count).ToList();
        }
    }
}
