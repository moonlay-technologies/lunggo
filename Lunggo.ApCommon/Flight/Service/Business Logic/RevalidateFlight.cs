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
            var output = new RevalidateFlightOutput();
            var response = RevalidateFareInternal(input.FareId);
            if (response.IsSuccess)
            {
                output.IsSuccess = true;
                output.IsValid = response.IsValid;
                output.Itinerary = response.Itinerary;
                if (input.ReturnFareId != null)
                {
                    response = RevalidateFareInternal(input.ReturnFareId);
                    if (response.IsSuccess)
                    {
                        output.IsSuccess = true;
                        output.ReturnIsValid = response.IsValid;
                        output.ReturnItinerary = response.Itinerary;
                    }
                    else
                    {
                        output.IsSuccess = false;
                        output.Errors = response.Errors;
                        output.ErrorMessages = response.ErrorMessages;
                    }
                }
            }
            else
            {
                output.IsSuccess = false;
                output.Errors = response.Errors;
                output.ErrorMessages = response.ErrorMessages;
            }
            return output;
        }
    }
}
