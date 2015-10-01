using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Currency.Constant;
using Lunggo.ApCommon.Currency.Service;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.Mystifly;
using Lunggo.ApCommon.Mystifly;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.ApCommon.Voucher;
using Lunggo.Flight.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;
using Lunggo.Flight.Crawler;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
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

        public void Init()
        {
            if (!_isInitialized)
            {
                MystiflyWrapper.Init();
                CurrencyService.GetInstance().Init();
                VoucherService.GetInstance().Init();
                InitPriceMarginRules();
                InitPriceDiscountRules();
                _isInitialized = true;
            }
        }

        private SearchFlightResult SearchFlightInternal(SearchFlightConditions conditions)
        {
            //_sriwijayaWrapper.SearchFlight(conditions);
            var jimbet = MystiflyWrapper.SearchFlight(conditions);
            var citilink = new CitilinkCrawler();
            var hasil = citilink.Try();
            return jimbet;
        }


        private SearchFlightResult SpecificSearchFlightInternal(SpecificSearchConditions conditions)
        {
            return MystiflyWrapper.SpecificSearchFlight(conditions);
        }

        private RevalidateFareResult RevalidateFareInternal(RevalidateConditions conditions)
        {
            return MystiflyWrapper.RevalidateFare(conditions);
        }

        private BookFlightResult BookFlightInternal(FlightBookingInfo bookInfo)
        {
            var supplier = IdUtil.GetSupplier(bookInfo.FareId);
            switch (supplier)
            {
                case FlightSupplier.Mystifly:
                    return MystiflyWrapper.BookFlight(bookInfo);
                default:
                    return null;
            }
            
        }

        private OrderTicketResult OrderTicketInternal(string bookingId)
        {
            var supplier = IdUtil.GetSupplier(bookingId);
            switch (supplier)
            {
                case FlightSupplier.Mystifly:
                    return MystiflyWrapper.OrderTicket(bookingId);
                default:
                    return null;
            }
        }

        private GetTripDetailsResult GetTripDetailsInternal(TripDetailsConditions conditions)
        {
            return MystiflyWrapper.GetTripDetails(conditions);
        }

        private List<BookingStatusInfo> GetBookingStatusInternal()
        {
            return MystiflyWrapper.GetBookingStatus();
        }

        private CancelBookingResult CancelBookingInternal(string bookingId)
        {
            return MystiflyWrapper.CancelBooking(bookingId);
        }

        private GetRulesResult GetRulesInternal(string fareId)
        {
            return MystiflyWrapper.GetRules(fareId);
        }
    }
}
