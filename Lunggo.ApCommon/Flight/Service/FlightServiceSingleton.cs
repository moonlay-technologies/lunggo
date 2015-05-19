using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Utility;
using Lunggo.ApCommon.Mystifly;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.ApCommon.Sriwijaya;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private static readonly FlightService Instance = new FlightService();
        private static MystiflyWrapper _mystiflyWrapper;
        private static SriwijayaWrapper _sriwijayaWrapper;
        private bool _isInitialized;

        private FlightService()
        {
            
        }

        public static FlightService GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                _mystiflyWrapper = MystiflyWrapper.GetInstance();
                _mystiflyWrapper.Init();
                _sriwijayaWrapper = SriwijayaWrapper.GetInstance();
                _sriwijayaWrapper.Init();
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("FlightService is already initialized");
            }
        }

        private SearchFlightResult SearchFlightInternal(SearchFlightConditions conditions)
        {
            //_sriwijayaWrapper.SearchFlight(conditions);
            return _mystiflyWrapper.SearchFlight(conditions);
        }

        private SearchFlightResult SpecificSearchFlightInternal(SpecificSearchConditions conditions)
        {
            return _mystiflyWrapper.SpecificSearchFlight(conditions);
        }

        private RevalidateFareResult RevalidateFareInternal(RevalidateConditions conditions)
        {
            return _mystiflyWrapper.RevalidateFare(conditions);
        }

        private BookFlightResult BookFlightInternal(FlightBookingInfo bookInfo)
        {
            var supplier = FlightIdUtil.GetSupplier(bookInfo.FareId);
            switch (supplier)
            {
                case FlightSupplier.Mystifly:
                    return _mystiflyWrapper.BookFlight(bookInfo);
                default:
                    return null;
            }
            
        }

        private OrderTicketResult OrderTicketInternal(string bookingId)
        {
            var supplier = FlightIdUtil.GetSupplier(bookingId);
            switch (supplier)
            {
                case FlightSupplier.Mystifly:
                    return _mystiflyWrapper.OrderTicket(bookingId);
                default:
                    return null;
            }
        }

        private GetTripDetailsResult GetTripDetailsInternal(TripDetailsConditions conditions)
        {
            return _mystiflyWrapper.GetTripDetails(conditions);
        }

        private List<BookingStatusInfo> GetBookingStatusInternal()
        {
            return _mystiflyWrapper.GetBookingStatus();
        }

        private CancelBookingResult CancelBookingInternal(string bookingId)
        {
            return _mystiflyWrapper.CancelBooking(bookingId);
        }

        private GetRulesResult GetRulesInternal(string fareId)
        {
            return _mystiflyWrapper.GetRules(fareId);
        }
    }
}
