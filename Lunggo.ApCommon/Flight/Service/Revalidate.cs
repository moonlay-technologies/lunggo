using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
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
            {
                output.IsSuccess = true;
                return output;
            }

                var revalidateSets = new List<RevalidateFlightOutputSet>();
            var itins = GetItinerariesFromCache(input.Token);
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
                var searchId = itins[0].SearchId;
                var tripType = ParseTripType(searchId);
                newItins.ForEach(itin => itin.RequestedTripType = tripType);
                    AddPriceMargin(newItins);

                SaveItinerariesToCache(newItins, input.Token);

                    output.IsSuccess = true;
                    output.IsValid = revalidateSets.TrueForAll(set => set.IsValid);
                    if (output.IsValid)
                    {
                        output.IsItineraryChanged = revalidateSets.Exists(set => set.IsItineraryChanged);
                        if (output.IsItineraryChanged)
                        output.NewItinerary = ConvertToItineraryForDisplay(newItins);
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

        private RevalidateFareResult RevalidateFareInternal(RevalidateConditions conditions)
        {
            var supplierName = IdUtil.GetSupplier(conditions.Itinerary.FareId);
            conditions.Itinerary.FareId = IdUtil.GetCoreId(conditions.Itinerary.FareId);
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();

            var result = supplier.RevalidateFare(conditions);
            if (result.NewItinerary != null)
                result.NewItinerary.FareId = IdUtil.ConstructIntegratedId(result.NewItinerary.FareId, supplierName, result.NewItinerary.FareType);
            return result;
        }
    }
}
