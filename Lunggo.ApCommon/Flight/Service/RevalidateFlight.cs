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
            var itins = input.ItinCacheId.Substring(0, 4) == ItinBundleKeyPrefix 
                ? GetItinerarySetFromCache(input.ItinCacheId) 
                : new List<FlightItinerary>{GetItineraryFromCache(input.ItinCacheId)};
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
                    foreach (var error in response.Errors)
                    {
                        output.AddError(error);
                    }
                    foreach (var errorMessage in response.ErrorMessages)
                    {
                        output.AddError(errorMessage);
                    }
                }
                output.Sets.Add(outputSet);
            }
            var newItins = output.Sets.Select(set => set.Itinerary).ToList();
            if (input.ItinCacheId.Substring(0, 4) == ItinBundleKeyPrefix)
                SaveItinerarySetAndBundleToCache(newItins, BundleItineraries(newItins), input.ItinCacheId);
            else
                SaveItineraryToCache(newItins.Single(), input.ItinCacheId);
            if (output.Sets.TrueForAll(set => set.IsSuccess))
            {
                output.IsSuccess = true;
                output.IsValid = output.Sets.TrueForAll(set => set.IsValid);
                if (output.Sets.Any(set => set.Itinerary == null))
                    output.NewFare = null;
                else
                    output.NewFare = output.Sets.Sum(set => set.Itinerary.LocalPrice);
            }
            else
            {
                if (output.Sets.Any(set => set.IsSuccess))
                    output.PartiallySucceed();
                output.IsSuccess = false;
                output.Errors = output.Errors.Distinct().ToList();
                output.ErrorMessages = output.ErrorMessages.Distinct().ToList();
            }
            return output;
        }
    }
}
