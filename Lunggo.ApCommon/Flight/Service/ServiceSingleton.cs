using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Currency.Constant;
using Lunggo.ApCommon.Currency.Service;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.Citilink;
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
                CitilinkWrapper.GetInstance().Init();
                CurrencyService.GetInstance().Init();
                VoucherService.GetInstance().Init();
                InitPriceMarginRules();
                InitPriceDiscountRules();
                _isInitialized = true;
            }
        }

        public SearchFlightResult SearchFlightInternal(SearchFlightConditions conditions)
        {
            //var jimbet = MystiflyWrapper.SearchFlight(conditions);
            //var citilinks = new CitilinkCrawler.Citilink();
            //var hasil = citilinks.Login();
            //var hasil2 = citilinks.search();
            //var hasil3 = citilinks.Select();
            //var hasil4 = citilinks.Passenger();
            //var hasil5 = citilinks.Kursi();
            //var hasil6 = citilinks.Payment();
            var aa = CitilinkWrapper.GetInstance();
            var hasil = aa.SearchFlight(conditions);
            //aa.BookFlight(null);
            return hasil;
        }


        //private SearchFlightResult SpecificSearchFlightInternal(SpecificSearchConditions conditions)
        //{
        //    return MystiflyWrapper.SpecificSearchFlight(conditions);
        //}

        public RevalidateFareResult RevalidateFareInternal(RevalidateConditions conditions)
        {
            var a = CitilinkWrapper.GetInstance().RevalidateFare(conditions);
            return MystiflyWrapper.RevalidateFare(conditions);
        }

        public BookFlightResult BookFlightInternal(FlightBookingInfo bookInfo)
        {
            //var supplier = IdUtil.GetSupplier(bookInfo.FareId);
            //switch (supplier)
            //{
            //    case FlightSupplier.Mystifly:
            //        return MystiflyWrapper.BookFlight(bookInfo);
            //    default:
            //        return null;
            //}
            var a = CitilinkWrapper.GetInstance().BookFlight(bookInfo);
            return null;
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
