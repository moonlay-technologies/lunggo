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
            var output = new SearchFlightOutput();
            conditions.AdultCount = input.Conditions.AdultCount;
            conditions.ChildCount = input.Conditions.ChildCount;
            conditions.InfantCount = input.Conditions.InfantCount;
            conditions.CabinClass = input.Conditions.CabinClass;
            conditions.OriDestInfos = input.Conditions.OriDestInfos.Select(data => new OriginDestinationInfo
            {
                OriginAirport = data.OriginAirport,
                DestinationAirport = data.DestinationAirport,
                DepartureDate = data.DepartureDate
            }).ToList();

            var result = SearchFlightInternal(conditions);

            output.Itineraries = result.FlightItineraries;

            if (!result.IsSuccess)
            {
                output.Errors = result.Errors;
                output.ErrorMessages = result.ErrorMessages;
            }
            return output;
        }
    }
}
