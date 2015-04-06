using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public RevalidateFlightOutput RevalidateFlight(RevalidateFlightInput input)
        {
            var response = RevalidateFareInternal(input.FareId);
            var output = new RevalidateFlightOutput();
            output.IsValid = response.IsValid;
            output.Itinerary = response.Itinerary;
            if (input.ReturnFareId != null)
            {
                response = RevalidateFareInternal(input.ReturnFareId);
                output.ReturnIsValid = response.IsValid;
                output.ReturnItinerary = response.Itinerary;
            }
            return output;
        }
    }
}
