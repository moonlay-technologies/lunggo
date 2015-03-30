using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Flight.Service
{
    public class FlightService
    {
        private static readonly FlightService Instance = new FlightService();
        private static readonly MystiflyWrapper MystiflyWrapper = MystiflyWrapper.GetInstance();
        private bool _isInitialized;

        private FlightService()
        {
            
        }

        public static FlightService GetInstance()
        {
            return Instance;
        }

        public void Init(string accountNumber, string userName, string password, Target target)
        {
            if (!_isInitialized)
            {
                MystiflyClientHandler.Init(accountNumber, userName, password, target);
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("FlightService is already initialized");
            }
        }

        public List<FlightFareItinerary> SearchFlight(SearchFlightConditions conditions)
        {
            return MystiflyWrapper.SearchFlight(conditions).FlightItineraries;
        }
        /*
        public SelectFlightRs SelectFlight(string itineraryId)
        {
            return null;
        }

        
        public BookFlightRs BookFlight(BookFlightRq request)
        {
            return null;
        }

        public OrderTicketRs OrderTicket(OrderTicketRq request)
        {
            return null;
        }

        public GetTripDetailsRs GetTripDetails(GetTripDetailsRq request)
        {
            return null;
        }*/
    }
}
