using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MystiflyOnePointAPI.Handler;
using MystiflyOnePointAPI.OnePointService;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new ClientHandler())
            {
                var ses = client.SessionId;
                var origins = new OriginDestinationInformation[1];
                origins[0] = new OriginDestinationInformation()
                {
                    DepartureDateTime = new DateTime(2015, 4, 2),
                    DestinationLocationCode = "LHR",
                    OriginLocationCode = "CGK"
                };
                var pass = new PassengerTypeQuantity[1];
                pass[0] = new PassengerTypeQuantity()
                {
                    Code = PassengerType.ADT,
                    Quantity = 1
                };
                var trav = new TravelPreferences()
                {
                    AirTripType = AirTripType.OneWay,
                    CabinPreference = CabinType.Y,
                    MaxStopsQuantity = MaxStopsQuantity.Direct
                };
                var searchRq = new AirLowFareSearchRQ()
                {
                    SessionId = ses,
                    OriginDestinationInformations = origins,
                    PassengerTypeQuantities = pass,
                    TravelPreferences = trav,
                    RequestOptions = RequestOptions.Fifty,
                    PricingSourceType = PricingSourceType.Public,
                    Target = Target.Test,
                    IsRefundable = true

                };
                var searchRs = client.AirLowFareSearch(searchRq);
            }
        }
    }
}
