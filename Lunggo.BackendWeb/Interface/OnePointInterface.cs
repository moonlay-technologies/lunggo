using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using Lunggo.BackendWeb.Model;
using MystiflyOnePointAPI.Handler;
using MystiflyOnePointAPI.OnePointService;
using MystiflyOnePointAPI.Wrapper;

namespace Lunggo.BackendWeb.Interface
{
    public static class OnePointInterface
    {
        public static void AirLowFareSearch(FlightReservationSearch search)
        {
            
            var origin = new List<OriginDestinationInformation>();
            origin.Add(new OriginDestinationInformation()
            {
                OriginLocationCode = "CGK",
                DestinationLocationCode = "KUL",
                DepartureDateTime = search.DepartureDate.GetValueOrDefault()
            });
            var passenger = new List<PassengerTypeQuantity>();
            passenger.Add(new PassengerTypeQuantity()
            {
                Code = PassengerType.ADT,
                Quantity = 1
            });
            var other = new AirLowFareSearchParams()
            {
                AirTripType = AirTripType.OneWay,
                CabinType = CabinType.Default,
                IsRefundable = true,
                IsResidentFare = false,
                MaxStopsQuantity = MaxStopsQuantity.Direct,
                NearByAirports = false,
                PricingSourceType = PricingSourceType.Public,
                RequestOptions = RequestOptions.Fifty
            };
            ClientHandler.Init("MCN004085", "GOAXML", "GA2014_xml", Target.Test);
            var client = new ClientHandler();
            client.RequestAirLowFareSearch(origin,passenger,other,Target.Test);
        }
    }
}