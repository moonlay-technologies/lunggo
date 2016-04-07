﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public RevalidateFlightOutput RevalidateFlight(RevalidateFlightInput input)
        {
            var output = new RevalidateFlightOutput();
            if (input.Token == null)
                input.Token = SelectFlight(input.SearchId, input.ItinIndex, input.RequestId);
            if (input.Token == null)
            {
                output.IsSuccess = true;
                return output;
            }
            else
            {
                var revalidateSets = new List<RevalidateFlightOutputSet>();
                var itins = IsItinBundleCacheId(input.Token)
                    ? GetItinerarySetFromCache(input.Token)
                    : new List<FlightItinerary> {GetItineraryFromCache(input.Token)};
                Parallel.ForEach(itins, itin =>
                {
                    var outputSet = new RevalidateFlightOutputSet();
                    var request = new RevalidateConditions
                    {
                        Itinerary = itin,
                        Trips = itin.Trips
                    };
                    var response = RevalidateFareInternal(request);
                    if (response.IsSuccess)
                    {
                        outputSet.IsSuccess = true;
                        outputSet.IsValid = response.IsValid;
                        outputSet.IsItineraryChanged = response.IsItineraryChanged;
                        outputSet.IsPriceChanged = response.IsPriceChanged;
                        outputSet.NewItinerary = response.NewItinerary;
                        if (outputSet.NewItinerary != null)
                            outputSet.NewItinerary.RegisterNumber = itin.RegisterNumber;
                    }
                    else
                    {
                        outputSet.IsSuccess = false;
                        response.Errors.ForEach(output.AddError);
                        if (response.ErrorMessages != null)
                            response.ErrorMessages.ForEach(output.AddError);
                    }
                    revalidateSets.Add(outputSet);
                });

                if (revalidateSets.TrueForAll(set => set.IsSuccess))
                {
                    var newItins = revalidateSets.Select(set => set.NewItinerary).ToList();
                    var asReturn = GetFlightRequestTripType(input.RequestId);
                    if (asReturn == null)
                        newItins = new List<FlightItinerary>();
                    else
                        newItins.ForEach(itin => itin.AsReturn = (bool)asReturn);
                    AddPriceMargin(newItins);
                    var searchedPrices = GetFlightRequestPrices(input.RequestId, input.SearchId);
                    if (IsItinBundleCacheId(input.Token))
                        SaveItinerarySetAndBundleToCache(newItins, BundleItineraries(newItins), input.Token);
                    else
                        SaveItineraryToCache(newItins.Single(), input.Token);

                    output.IsSuccess = true;
                    output.IsValid = revalidateSets.TrueForAll(set => set.IsValid);
                    if (output.IsValid)
                    {
                        output.IsItineraryChanged = revalidateSets.Exists(set => set.IsItineraryChanged);
                        if (output.IsItineraryChanged)
                            output.NewItinerary = ConvertToItineraryForDisplay(BundleItineraries(newItins));
                        output.IsPriceChanged = revalidateSets.Exists(set => set.IsPriceChanged);
                        if (output.IsPriceChanged)
                            output.NewPrice = revalidateSets.Sum(set => set.NewPrice);
                        SaveItineraryToCache(newItins[0], input.Token);
                    }
                    output.Token = input.Token;
                }
                else
                {
                    if (revalidateSets.Any(set => set.IsSuccess))
                        output.PartiallySucceed();
                    output.IsSuccess = false;
                    output.DistinguishErrors();
                }
                return output;
            }
        }
    }
}
