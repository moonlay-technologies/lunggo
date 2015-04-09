using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MystiflyOnePointAPI.Handler;
using MystiflyOnePointAPI.OnePointService;

namespace MystiflyOnePointAPI.Wrapper
{
    public static class AirLowFareSearchWrapper
    {
        public static void RequestAirLowFareSearch(this ClientHandler client, List<OriginDestinationInformation> originDestinationInformations, List<PassengerTypeQuantity> passengerTypeQuantities, AirLowFareSearchParams otherParams, Target target)
        {
            var travelPreferences = new TravelPreferences()
            {
                AirTripType = otherParams.AirTripType,
                CabinPreference = otherParams.CabinType,
                MaxStopsQuantity = otherParams.MaxStopsQuantity,
                VendorPreferenceCodes = null,
                VendorExcludeCodes = null,
                ExtensionData = null
            };
            var request = new AirLowFareSearchRQ()
            {
                SessionId = client.SessionId,
                OriginDestinationInformations = originDestinationInformations.ToArray(),
                PassengerTypeQuantities = passengerTypeQuantities.ToArray(),
                IsRefundable = otherParams.IsRefundable,
                IsResidentFare = otherParams.IsResidentFare,
                NearByAirports = otherParams.NearByAirports,
                PricingSourceType = otherParams.PricingSourceType,
                RequestOptions = otherParams.RequestOptions,
                Target = target,
                TravelPreferences = travelPreferences,
                ExtensionData = null
            };
            var response = client.AirLowFareSearch(request);
        }
    }

    public class AirLowFareSearchParams
    {
        public bool IsRefundable { get; set; }
        public bool IsResidentFare { get; set; }
        public bool NearByAirports { get; set; }
        public PricingSourceType PricingSourceType { get; set; }
        public RequestOptions RequestOptions { get; set; }
        public MaxStopsQuantity MaxStopsQuantity { get; set; }
        public CabinType CabinType { get; set; }
        public AirTripType AirTripType { get; set; }
        public List<string> VendorPreferences { get; set; }
        public List<string> VendorExclude { get; set; }
    }
}
