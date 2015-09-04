using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public RevalidateFlightOutput RevalidateFlight(RevalidateFlightInput input)
        {
            var output = new RevalidateFlightOutput();
            if (input.Token == null)
                input.Token = SelectFlight(input.SearchId, input.ItinIndex);
            var itins = input.Token.Substring(0, 4) == ItinBundleKeyPrefix 
                ? GetItinerarySetFromCache(input.Token) 
                : new List<FlightItinerary>{GetItineraryFromCache(input.Token)};
            foreach (var itin in itins)
            {
                var outputSet = new RevalidateFlightOutputSet();
                var request = new RevalidateConditions
                {
                    FareId = itin.FareId,
                    Trips = itin.FlightTrips
                };
                var response = RevalidateFareInternal(request);
                if (response.IsSuccess)
                {
                    outputSet.IsSuccess = true;
                    outputSet.IsValid = response.IsValid;
                    outputSet.Itinerary = response.Itinerary;
                }
                else
                {
                    outputSet.IsSuccess = false;
                    output.Errors = response.Errors;
                    output.ErrorMessages = response.ErrorMessages;
                }
                output.Sets.Add(outputSet);
            }
            var newItins = output.Sets.Select(set => set.Itinerary).ToList();
            if (input.Token.Substring(0, 4) == ItinBundleKeyPrefix)
                SaveItinerarySetAndBundleToCache(newItins, BundleItineraries(newItins), input.Token);
            else
                SaveItineraryToCache(newItins.Single(), input.Token);
            if (output.Sets.TrueForAll(set => set.IsSuccess))
            {
                output.IsSuccess = true;
                output.IsValid = output.Sets.TrueForAll(set => set.IsValid);
                if (output.Sets.Any(set => set.Itinerary == null))
                    output.NewFare = null;
                else
                    output.NewFare = output.Sets.Sum(set => set.Itinerary.LocalPrice);
                output.Token = input.Token;
            }
            else
            {
                if (output.Sets.Any(set => set.IsSuccess))
                    output.PartiallySucceed();
                output.IsSuccess = false;
                output.DistinguishErrors();
            }
            return output;
        }
    }
}
