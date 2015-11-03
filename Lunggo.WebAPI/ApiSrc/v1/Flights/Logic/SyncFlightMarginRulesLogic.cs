using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static void SyncFlightMarginRules()
        {
            var flight = FlightService.GetInstance();
            flight.PullRemotePriceMarginRules();
        }
    }
}