using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Currency.Service;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper;
using Lunggo.ApCommon.Flight.Wrapper.AirAsia;
using Lunggo.ApCommon.Flight.Wrapper.Citilink;
using Lunggo.ApCommon.Flight.Wrapper.Mystifly;
using Lunggo.ApCommon.Flight.Wrapper.Sriwijaya;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private static readonly FlightService Instance = new FlightService();
        private static readonly MystiflyWrapper MystiflyWrapper = MystiflyWrapper.GetInstance();
        private static readonly AirAsiaWrapper AirAsiaWrapper = AirAsiaWrapper.GetInstance();
        private static readonly CitilinkWrapper CitilinkWrapper = CitilinkWrapper.GetInstance();
        private static readonly SriwijayaWrapper SriwijayaWrapper = SriwijayaWrapper.GetInstance();
        private static readonly Dictionary<String,FlightSupplierWrapperBase> Suppliers = new Dictionary<string, FlightSupplierWrapperBase>()
        {
            { "1", MystiflyWrapper},
            { "2", AirAsiaWrapper},
            { "3", CitilinkWrapper},
            { "4", SriwijayaWrapper}
        };

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
                foreach (var supplier in Suppliers.Select(entry => entry.Value))
                {
                    supplier.Init();
                }

                //CurrencyService.GetInstance().Init();
                VoucherService.GetInstance().Init();
                InitPriceMarginRules();
                InitPriceDiscountRules();
                _isInitialized = true;
            }
        }

        private void SearchFlightInternal(SearchFlightConditions conditions, int supplierIndex)
        {
            var supplier = Suppliers[supplierIndex.ToString(CultureInfo.InvariantCulture)];
            var searchId = EncodeConditions(conditions);
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));

            var result = supplier.SearchFlight(conditions);
            result.Itineraries = result.Itineraries ?? new List<FlightItinerary>();
            if (result.IsSuccess)
                foreach (var itin in result.Itineraries)
                {
                    itin.FareId = IdUtil.ConstructIntegratedId(itin.FareId, supplier.SupplierName, itin.FareType);
                }
            SaveSearchedItinerariesToCache(result.Itineraries, EncodeConditions(conditions), timeout, supplierIndex);
            InvalidateSearchingStatusInCache(searchId, supplierIndex);
        }

        private SearchFlightResult SpecificSearchFlightInternal(SearchFlightConditions conditions)
        {
            var results = MystiflyWrapper.SpecificSearchFlight(conditions);
            results.Itineraries.ForEach(itin => itin.FareId = IdUtil.ConstructIntegratedId(itin.FareId, Supplier.Mystifly, itin.FareType));
            return results;
        }

        private RevalidateFareResult RevalidateFareInternal(RevalidateConditions conditions)
        {
            var supplierName = IdUtil.GetSupplier(conditions.FareId);
            conditions.FareId = IdUtil.GetCoreId(conditions.FareId);
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();

            var result = supplier.RevalidateFare(conditions);
            if (result.Itinerary != null)
                result.Itinerary.FareId = IdUtil.ConstructIntegratedId(result.Itinerary.FareId, supplierName, result.Itinerary.FareType);
            return result;
        }

        private BookFlightResult BookFlightInternal(FlightBookingInfo bookInfo)
        {
            var fareType = IdUtil.GetFareType(bookInfo.FareId);
            var supplierName = IdUtil.GetSupplier(bookInfo.FareId);
            bookInfo.FareId = IdUtil.GetCoreId(bookInfo.FareId);
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();
            BookFlightResult result = supplier.BookFlight(bookInfo);
            if (result.Status.BookingId != null)
                result.Status.BookingId = IdUtil.ConstructIntegratedId(result.Status.BookingId,
                    supplierName, fareType);
            var defaultTimeout = DateTime.UtcNow.AddMinutes(double.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout")));
            result.Status.TimeLimit = defaultTimeout < result.Status.TimeLimit
                ? defaultTimeout
                : result.Status.TimeLimit;
            return result;
        }

        private OrderTicketResult OrderTicketInternal(string bookingId, bool canHold)
        {
            var fareType = IdUtil.GetFareType(bookingId);
            var supplierName = IdUtil.GetSupplier(bookingId);
            bookingId = IdUtil.GetCoreId(bookingId);
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();
            OrderTicketResult result = supplier.OrderTicket(bookingId, canHold);
            if (result.BookingId != null)
                result.BookingId = IdUtil.ConstructIntegratedId(result.BookingId, supplierName, fareType);
            return result;
        }

        private GetTripDetailsResult GetTripDetailsInternal(TripDetailsConditions conditions)
        {
            var fareType = IdUtil.GetFareType(conditions.BookingId);
            var supplierName = IdUtil.GetSupplier(conditions.BookingId);
            conditions.BookingId = IdUtil.GetCoreId(conditions.BookingId);
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();
            GetTripDetailsResult result = supplier.GetTripDetails(conditions);
            if (result.BookingId != null)
                result.BookingId = IdUtil.ConstructIntegratedId(result.BookingId, supplierName, fareType);
            return result;
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
