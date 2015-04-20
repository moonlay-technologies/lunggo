using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private static readonly FlightService Instance = new FlightService();
        private static readonly MystiflyWrapper APIServiceWrapper = MystiflyWrapper.GetInstance();
        private bool _isInitialized;

        private FlightService()
        {
            
        }

        public static FlightService GetInstance()
        {
            return Instance;
        }

        public void Init(string accountNumber, string userName, string password, TargetServer target)
        {
            if (!_isInitialized)
            {
                APIServiceWrapper.Init(accountNumber, userName, password, target);
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("FlightService is already initialized");
            }
        }

        private SearchFlightResult SearchFlightInternal(SearchFlightConditions conditions)
        {
            return APIServiceWrapper.SearchFlight(conditions);
        }

        private SearchFlightResult SpecificSearchFlightInternal(SpecificSearchConditions conditions)
        {
            return APIServiceWrapper.SpecificSearchFlight(conditions);
        }

        private RevalidateFareResult RevalidateFareInternal(RevalidateConditions conditions)
        {
            return APIServiceWrapper.RevalidateFare(conditions);
        }

        private BookFlightResult BookFlightInternal(FlightBookingInfo bookInfo)
        {
            return APIServiceWrapper.BookFlight(bookInfo);
        }

        private OrderTicketResult OrderTicketInternal(string bookingId)
        {
            return APIServiceWrapper.OrderTicket(bookingId);
        }

        private GetTripDetailsResult GetTripDetailsInternal(TripDetailsConditions conditions)
        {
            return APIServiceWrapper.GetTripDetails(conditions);
        }

        private GetBookingStatusResult GetBookingStatusInternal()
        {
            return APIServiceWrapper.GetBookingStatus();
        }

        private CancelBookingResult CancelBookingInternal(string bookingId)
        {
            return APIServiceWrapper.CancelBooking(bookingId);
        }

        private GetRulesResult GetRulesInternal(string fareId)
        {
            return APIServiceWrapper.GetRules(fareId);
        }
    }
}
