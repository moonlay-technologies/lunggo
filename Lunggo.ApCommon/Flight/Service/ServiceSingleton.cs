using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Currency.Service;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper;
using Lunggo.ApCommon.Flight.Wrapper.AirAsia;
using Lunggo.ApCommon.Flight.Wrapper.Mystifly;
using Lunggo.ApCommon.Mystifly;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;
using Microsoft.Data.OData.Query;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private static readonly FlightService Instance = new FlightService();
        private static readonly MystiflyWrapper MystiflyWrapper = MystiflyWrapper.GetInstance();
        private static readonly AirAsiaWrapper AirAsiaWrapper = AirAsiaWrapper.GetInstance();
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
                AirAsiaWrapper.Init();
                CurrencyService.GetInstance().Init();
                VoucherService.GetInstance().Init();
                InitPriceMarginRules();
                InitPriceDiscountRules();
                _isInitialized = true;
            }
        }

        private SearchFlightResult SearchFlightInternal(SearchFlightConditions conditions)
        {
            var suppliers = new WrapperBase[] {MystiflyWrapper, AirAsiaWrapper};
            var results = new SearchFlightResult();
            results.Itineraries = new List<FlightItinerary>();
            for (var i = 0; i< 2; i++)
            {
                var result = suppliers[i].SearchFlight(conditions);
                if (result.IsSuccess)
                {
                    foreach (var itin in result.Itineraries)
                    {
                        var currency = CurrencyService.GetInstance();
                        itin.SupplierRate = currency.GetSupplierExchangeRate(suppliers[i].SupplierName);
                        itin.OriginalIdrPrice = itin.SupplierPrice*itin.SupplierRate;
                        AddPriceMargin(itin);
                        itin.LocalCurrency = "IDR";
                        itin.LocalRate = 1;
                        itin.LocalPrice = itin.FinalIdrPrice*itin.LocalRate;
                        itin.FareId = IdUtil.ConstructIntegratedId(itin.FareId, suppliers[i].SupplierName, itin.FareType);
                    }
                    results.IsSuccess = true;
                    results.Itineraries.AddRange(result.Itineraries);
                }
                else
                {
                    result.Errors.ForEach(results.AddError);
                    if (result.ErrorMessages != null) 
                        result.ErrorMessages.ForEach(results.AddError);
                }
            }
            return results;
        }

        private SearchFlightResult SpecificSearchFlightInternal(SearchFlightConditions conditions)
        {
            var results = MystiflyWrapper.SpecificSearchFlight(conditions);
            results.Itineraries.ForEach(itin => itin.FareId = IdUtil.ConstructIntegratedId(itin.FareId, Supplier.Mystifly, itin.FareType));
            return results;
        }

        private RevalidateFareResult RevalidateFareInternal(RevalidateConditions conditions)
        {
            var supplier = IdUtil.GetSupplier(conditions.FareId);
            conditions.FareId = IdUtil.GetCoreId(conditions.FareId);
            RevalidateFareResult result;
            var currency = CurrencyService.GetInstance();
            switch (supplier)
            {
                case Supplier.Mystifly:
                    result = MystiflyWrapper.RevalidateFare(conditions);
                    if (result.Itinerary != null)
                    {
                        result.Itinerary.SupplierRate = currency.GetSupplierExchangeRate(Supplier.Mystifly);
                        result.Itinerary.OriginalIdrPrice = result.Itinerary.SupplierPrice * result.Itinerary.SupplierRate;
                        AddPriceMargin(result.Itinerary);
                        result.Itinerary.LocalCurrency = "IDR";
                        result.Itinerary.LocalRate = 1;
                        result.Itinerary.LocalPrice = result.Itinerary.FinalIdrPrice * result.Itinerary.LocalRate;
                        result.Itinerary.FareId = IdUtil.ConstructIntegratedId(result.Itinerary.FareId, Supplier.Mystifly, result.Itinerary.FareType);
                    }
                    return result;
                case Supplier.AirAsia:
                    result = AirAsiaWrapper.RevalidateFare(conditions);
                    if (result.Itinerary != null)
                    {
                        result.Itinerary.SupplierRate = currency.GetSupplierExchangeRate(Supplier.AirAsia);
                        result.Itinerary.OriginalIdrPrice = result.Itinerary.SupplierPrice * result.Itinerary.SupplierRate;
                        AddPriceMargin(result.Itinerary);
                        result.Itinerary.LocalCurrency = "IDR";
                        result.Itinerary.LocalRate = 1;
                        result.Itinerary.LocalPrice = result.Itinerary.FinalIdrPrice * result.Itinerary.LocalRate;
                        result.Itinerary.FareId = IdUtil.ConstructIntegratedId(result.Itinerary.FareId, Supplier.AirAsia, result.Itinerary.FareType);
                    }
                    return result;
                default:
                    return null;
            }
        }

        private BookFlightResult BookFlightInternal(FlightBookingInfo bookInfo)
        {
            var fareType = IdUtil.GetFareType(bookInfo.FareId);
            var supplier = IdUtil.GetSupplier(bookInfo.FareId);
            bookInfo.FareId = IdUtil.GetCoreId(bookInfo.FareId);
            BookFlightResult result;
            switch (supplier)
            {
                case Supplier.Mystifly:
                    result = MystiflyWrapper.BookFlight(bookInfo, fareType);
                    if (result.Status.BookingId != null)
                        result.Status.BookingId = IdUtil.ConstructIntegratedId(result.Status.BookingId, Supplier.Mystifly, fareType);
                    return result;
                case Supplier.AirAsia:
                    result = AirAsiaWrapper.BookFlight(bookInfo, fareType);
                    if (result.Status != null)
                        result.Status.BookingId = IdUtil.ConstructIntegratedId(result.Status.BookingId, Supplier.AirAsia, fareType);
                    return result;
                default:
                    return null;
            }

        }

        private OrderTicketResult OrderTicketInternal(string bookingId)
        {
            var fareType = IdUtil.GetFareType(bookingId);
            var supplier = IdUtil.GetSupplier(bookingId);
            bookingId = IdUtil.GetCoreId(bookingId);
            OrderTicketResult result;
            switch (supplier)
            {
                case Supplier.Mystifly:
                    result = MystiflyWrapper.OrderTicket(bookingId, fareType);
                    if (result.BookingId != null)
                        result.BookingId = IdUtil.ConstructIntegratedId(result.BookingId, Supplier.Mystifly, fareType);
                    return result;
                case Supplier.AirAsia:
                    result = AirAsiaWrapper.OrderTicket(bookingId, fareType);
                    if (result.BookingId != null)
                        result.BookingId = IdUtil.ConstructIntegratedId(result.BookingId, Supplier.AirAsia, fareType);
                    return result;
                default:
                    return null;
            }
        }

        private GetTripDetailsResult GetTripDetailsInternal(TripDetailsConditions conditions)
        {
            var fareType = IdUtil.GetFareType(conditions.BookingId);
            var supplier = IdUtil.GetSupplier(conditions.BookingId);
            conditions.BookingId = IdUtil.GetCoreId(conditions.BookingId);
            GetTripDetailsResult result;
            switch (supplier)
            {
                case Supplier.Mystifly:
                    result = MystiflyWrapper.GetTripDetails(conditions);
                    if (result.BookingId != null)
                        result.BookingId = IdUtil.ConstructIntegratedId(result.BookingId, Supplier.Mystifly, fareType);
                    return result;
                case Supplier.AirAsia:
                    result = AirAsiaWrapper.GetTripDetails(conditions);
                    if (result.BookingId != null)
                        result.BookingId = IdUtil.ConstructIntegratedId(result.BookingId, Supplier.AirAsia, fareType);
                    return result;
                default:
                    return null;
            }
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
