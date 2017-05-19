using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;
using Supplier = Lunggo.ApCommon.Flight.Constant.Supplier;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public BookFlightOutput BookFlight(BookFlightInput input)
        {
            
            var output = new BookFlightOutput();
            var itins = GetItinerariesFromCache(input.Token);

            if (itins == null || itins.Contains(null))
                return new BookFlightOutput
                {
                    IsSuccess = false,
                    IsValid = false,
                    Errors = new List<FlightError> {FlightError.FareIdNoLongerValid},
                    ErrorMessages = new List<string> {"Null itin."}
                };

            var bookResults = BookItineraries(itins, input, output);
            UpdateItineraries(itins, bookResults);
            SaveItinerariesToCache(itins, input.Token);

            if (input.Test)
            {
                var isValid = bookResults.TrueForAll(result => result.IsSuccess || result.RevalidateSet.IsValid);
                if (isValid)
                {
                    return new BookFlightOutput
                    {
                        IsSuccess = true,
                        IsValid = true,
                    };
                }
                return new BookFlightOutput
                {
                    IsSuccess = false,
                    IsValid = false,
                };
            }

            output.IsValid = bookResults.TrueForAll(result => result.IsSuccess || result.RevalidateSet.IsValid);
            if (output.IsValid)
            {
                output.IsItineraryChanged = bookResults.Exists(result => result.RevalidateSet.IsItineraryChanged);
                if (output.IsItineraryChanged)
                    output.NewItinerary = ConvertToItineraryForDisplay(itins);
                output.IsPriceChanged = bookResults.Exists(result => result.RevalidateSet.IsPriceChanged);
                if (output.IsPriceChanged)
                    output.NewPrice = bookResults.Sum(result => result.RevalidateSet.NewPrice);
            }
            if (AllAreBooked(bookResults))
            {
                output.IsSuccess = true;
                var reservation = CreateReservation(itins, input, bookResults);
                InsertReservationToDb(reservation);
                output.RsvNo = reservation.RsvNo;
                output.TimeLimit = reservation.Itineraries.Min(itin => itin.TimeLimit);
                ExpireReservationWhenTimeout(reservation.RsvNo, reservation.Payment.TimeLimit);

                //DeleteItinerariesFromCache(input.Token);

                // DEVELOPMENT PURPOSE
                output.NewPrice = itins.Sum(itin => itin.Price.Local);
            }
            else
            {
                output.IsSuccess = false;
                if (!bookResults.Any())
                    output.AddError(FlightError.InvalidInputData);
                //if (AnyIsBooked(bookResults))
                //    output.PartiallySucceed();
                output.DistinguishErrors();
            }


            return output;
        }

        public string GetBookingRedirectionUrl(string rsvNo)
        {
            return GetPaymentRedirectionUrlInCache(rsvNo);
        }

        #region Helpers

        private static bool AnyIsBooked(IEnumerable<BookResult> bookResults)
        {
            return bookResults.Any(set => set.IsSuccess);
        }

        private static bool AllAreBooked(List<BookResult> bookResults)
        {
            return bookResults.Any() && bookResults.All(set => set.IsSuccess);
        }

        private FlightReservation CreateReservation(List<FlightItinerary> itins, BookFlightInput input, List<BookResult> bookResults)
        {
            var trips =
                itins.SelectMany(itin => itin.Trips).OrderBy(trip => trip.Segments.First().DepartureTime).ToList();
            var reservation = new FlightReservation();
            reservation.RsvNo = RsvNoSequence.GetInstance().GetNext(reservation.Type);
            reservation.RsvTime = DateTime.UtcNow;
            reservation.RsvStatus = RsvStatus.InProcess;
            reservation.Itineraries = itins;
            reservation.Contact = input.Contact;
            reservation.Pax = input.Passengers;
            reservation.User = HttpContext.Current.User.Identity.IsUserAuthorized()
                ? HttpContext.Current.User.Identity.GetUser()
                : null;
            reservation.Payment = new PaymentDetails
            {
                Status = PaymentStatus.Pending,
                LocalCurrency = new Currency(OnlineContext.GetActiveCurrencyCode()),
                OriginalPriceIdr = reservation.Itineraries.Sum(order => order.Price.FinalIdr),
                TimeLimit = reservation.Itineraries.Min(order => order.TimeLimit).AddMinutes(-10),
            };
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
            var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
            var platform = Client.GetPlatformType(clientId);
            var deviceId = identity.Claims.Single(claim => claim.Type == "Device ID").Value;
            reservation.State = new ReservationState
            {
                Platform = platform,
                DeviceId = deviceId,
                Language = "id", //OnlineContext.GetActiveLanguageCode();
                Currency = new Currency("IDR"), //OnlineContext.GetActiveCurrencyCode());
            };
            PaymentService.GetInstance().GetUniqueCode(reservation.RsvNo, null, null);
            return reservation;
        }

        private List<BookResult> BookItineraries(IEnumerable<FlightItinerary> itins, BookFlightInput input, BookFlightOutput output)
        {
            var bookResults = new List<BookResult>();
            if (itins != null)
                Parallel.ForEach(itins, itin =>
                {
                    var bookResult = BookItinerary(itin, input, output);
                    bookResults.Add(bookResult);
                });
            return bookResults;
        }

        private BookResult BookItinerary(FlightItinerary itin, BookFlightInput input, BookFlightOutput output)
        {
            var bookInfo = new FlightBookingInfo
            {
                Itinerary = itin,
                Contact = input.Contact,
                Passengers = input.Passengers,
                Test = input.Test
            };
            var response = BookFlightInternal(bookInfo);
            var bookResult = new BookResult();
            if (response.NewItinerary != null)
            {
                var newItin = response.NewItinerary;
                newItin.Price.SetMargin(itin.Price.Margin);
                newItin.Price.CalculateFinalAndLocal(itin.Price.LocalCurrency);
                RoundFinalAndLocalPrice(newItin);
                itin = newItin;
            }
            if (bookInfo.Test)
            {
                return new BookResult
                {
                    IsSuccess = response.IsSuccess
                };
            }
            if (response.IsSuccess)
            {
                bookResult.IsSuccess = true;
                itin.BookingId = response.Status.BookingId;
                itin.BookingStatus = response.Status.BookingStatus;
                if (response.Status.BookingStatus == BookingStatus.Booked)
                    itin.TimeLimit = bookResult.TimeLimit = response.Status.TimeLimit;
            }
            else
            {
                bookResult.IsSuccess = false;
                if (response.Errors != null)
                    response.Errors.ForEach(output.AddError);
                if (response.ErrorMessages != null)
                    response.ErrorMessages.ForEach(output.AddError);
            }
            bookResult.RevalidateSet = new RevalidateFlightOutputSet
            {
                IsValid = response.IsValid,
                IsItineraryChanged = response.IsItineraryChanged,
                NewItinerary = response.NewItinerary,
                IsPriceChanged = response.IsPriceChanged,
                NewPrice = response.NewPrice
            };
            bookResult.Deposit = response.Deposit.GetValueOrDefault();
            //TODO Check Deposit Here
            //Send Email with data
            
            if (bookResult.Deposit > 0 && bookResult.Deposit < 3000000)
            {
                var message = bookInfo.Itinerary.Supplier + "^" + bookResult.Deposit + "^" + itin.Price.Supplier;
                SendDepositWarningNotif(message);
            }
            return bookResult;
        }

        public BookFlightResult BookFlightInternal(FlightBookingInfo bookInfo)
        {
            var supplierName = bookInfo.Itinerary.Supplier;
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();
            //FOR TESTING ONLY
            //var result = new BookFlightResult();
            //var Airasia = Suppliers.Where(entry => entry.Value.SupplierName == Supplier.AirAsia).Select(entry => entry.Value).Single();
            //var Citilink = Suppliers.Where(entry => entry.Value.SupplierName == Supplier.Citilink).Select(entry => entry.Value).Single();
            //var LionAir = Suppliers.Where(entry => entry.Value.SupplierName == Supplier.LionAir).Select(entry => entry.Value).Single();
            //var Sriwijaya = Suppliers.Where(entry => entry.Value.SupplierName == Supplier.Sriwijaya).Select(entry => entry.Value).Single();
            
            //var CitilinkDeposit = Citilink.GetDeposit();
            //var LionAirDeposit = LionAir.GetDeposit();
            //var SriwijayaDeposit = Sriwijaya.GetDeposit();

            
            //var temp2 = CitilinkDeposit;
            //var temp3 = LionAirDeposit;
            //var temp4 = SriwijayaDeposit;

            //result.Deposit = 0;
            //var AirasiaDeposit = Airasia.GetDeposit();
            //var temp1 = AirasiaDeposit;
            //return result;
            var result = supplier.BookFlight(bookInfo);
            if (bookInfo.Test)
                return result;
            //Auto Retry Book
            var trial = 0;
            while (trial < 3 && result.Errors != null)
            {
                result = supplier.BookFlight(bookInfo);
                trial++;
            }

            var defaultTimeout = DateTime.UtcNow.AddMinutes(double.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout")));
            if (result.Status != null && result.IsSuccess)
            {
                result.Status.TimeLimit = defaultTimeout < result.Status.TimeLimit
                    ? defaultTimeout
                    : result.Status.TimeLimit;
                if (result.Status.TimeLimit < DateTime.UtcNow.AddMinutes(20))
                {
                    result.IsSuccess = false;
                    result.Status.BookingStatus = BookingStatus.Failed;
                    result.AddError(FlightError.FareIdNoLongerValid, "Time limit too short." + result.Status.TimeLimit);
                }
                else
                {
                    result.Status.TimeLimit = result.Status.TimeLimit.AddMinutes(-10);
                }
                result.Deposit = supplier.GetDeposit();
            }
            return result;
        }

        private static void UpdateItineraries(List<FlightItinerary> itins, List<BookResult> bookResults)
        {
            for (var i = 0; i < itins.Count; i++)
            {
                if (!bookResults[i].IsSuccess)
                    itins[i] = bookResults[i].RevalidateSet.NewItinerary;
            }
        }
        #endregion
    }
}
