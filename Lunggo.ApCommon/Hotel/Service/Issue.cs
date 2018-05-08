using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Log;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;
using BookingStatusCd = Lunggo.ApCommon.Hotel.Constant.BookingStatusCd;
using Occupancy = Lunggo.ApCommon.Hotel.Model.Occupancy;
using Lunggo.ApCommon.Log;
using Lunggo.Framework.Environment;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        internal override void Issue(string rsvNo)
        {
            IssueHotel(new IssueHotelTicketInput {RsvNo = rsvNo});
        }

        public IssueHotelTicketOutput IssueHotel(IssueHotelTicketInput input)
        {
            var rsvData = GetReservationFromDb(input.RsvNo);
            if (rsvData == null)
            {
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                };
            }

            var output = new IssueHotelTicketOutput();
            //if (rsvData.Payment.Method == PaymentMethod.Credit ||
            //    (rsvData.Payment.Method != PaymentMethod.Credit &&
            //     rsvData.Payment.Status == PaymentStatus.Settled))
            if (rsvData.Payment.Status == PaymentStatus.Settled)
            {
                var queueService = QueueService.GetInstance();
                var queue = queueService.GetQueueByReference("HotelIssueVoucher");
                queue.AddMessage(new CloudQueueMessage(input.RsvNo));
                output.IsSuccess = true;
                return output;
            }
            output.IsSuccess = false;
            return output;
        }

        public IssueHotelTicketOutput CommenceIssueHotel(IssueHotelTicketInput input)
        {
            var rsvData = GetReservationFromDb(input.RsvNo);
            if (rsvData == null)
            {
                Console.WriteLine("Reservation not found");
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                };
            }

            if (!string.IsNullOrEmpty(rsvData.HotelDetails.BookingReference))
            {
                Console.WriteLine("Reservation has been issued");
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false
                };
            }
            
            var oldPrice = rsvData.HotelDetails.Rooms.Sum(room => room.Rates.Sum(rate => rate.Price.Supplier));
            var occupancies = new List<Occupancy>();

            foreach (var room in rsvData.HotelDetails.Rooms)
            {
                occupancies.AddRange(room.Rates.Select(rate => new Occupancy
                {
                    RoomCount = rate.RateCount,
                    AdultCount = rate.AdultCount,
                    ChildCount = rate.ChildCount,
                    ChildrenAges = rate.ChildrenAges
                }));
            }

            occupancies = occupancies.Distinct().ToList();
            var rateFound = Enumerable.Repeat(false, occupancies.Count).ToList();
            var allCurrency = Currency.GetAllCurrencies();
            Guid generatedSearchId = Guid.NewGuid();
            SaveAllCurrencyToCache(generatedSearchId.ToString(), allCurrency);

            var request = new SearchHotelCondition
            {
                Occupancies = occupancies,
                CheckIn = rsvData.HotelDetails.CheckInDate,
                Checkout = rsvData.HotelDetails.CheckOutDate,
                HotelCode = rsvData.HotelDetails.HotelCode,
                SearchId = generatedSearchId.ToString()
            };

            var hotelbeds = new HotelBedsSearchHotel();
            var searchResult = hotelbeds.SearchHotel(request);
            AddPriceMargin(searchResult.HotelDetails, rsvData.State.Currency);

            if (searchResult.HotelDetails == null || searchResult.HotelDetails.Count == 0)
            {
                Console.WriteLine("When refresh ratekeys, no hotel result");
                return new IssueHotelTicketOutput
                {
                    ErrorMessages = new List<string> {"When refresh ratekeys, no hotel result"},
                    IsSuccess = false
                };
            }

            if (searchResult.HotelDetails.Any(hotel => hotel.Rooms == null || hotel.Rooms.Count == 0))
            {
                Console.WriteLine("When refresh ratekeys, there is at least a hotel without Rooms");
                return new IssueHotelTicketOutput
                {
                    ErrorMessages = new List<string> {"When refresh ratekeys, there is at least a hotel without Rooms"},
                    IsSuccess = false
                };
            }

            if (searchResult.HotelDetails.Any(hotel => hotel.Rooms.Any(room => room.Rates == null || room.Rates.Count == 0)))
            {
                Console.WriteLine("When refresh ratekeys, there is at least a room without Rates");
                return new IssueHotelTicketOutput
                {
                    ErrorMessages = new List<string> {"When refresh ratekeys, there is at least a room without Rates"},
                    IsSuccess = false
                };
            }

            foreach (var room in rsvData.HotelDetails.Rooms)
            {
                foreach (var rate in room.Rates)
                {
                    if (rate.TimeLimit >= DateTime.UtcNow) continue;
                    var sampleRatekey = rate.RateKey.Split('|');
                    var roomCd = sampleRatekey[5];
                    var someData = sampleRatekey[6];
                    var board = sampleRatekey[7];
                    var roomCount = rate.RateCount;
                    var adultCount = rate.AdultCount;
                    var childCount = rate.ChildCount;
                    var childrenAges = sampleRatekey[10];
                    var index = room.Rates.IndexOf(rate);

                    foreach (var rooma in searchResult.HotelDetails[0].Rooms)
                    {
                        foreach (var ratea in rooma.Rates)
                        {
                            var rateKey = ratea.RateKey.Split('|');
                            if (rateKey[5] == roomCd && rateKey[6] == someData && rateKey[7] == board &&
                                Convert.ToInt32(rateKey[9].Split('~')[0]) == roomCount &&
                                Convert.ToInt32(rateKey[9].Split('~')[1]) == adultCount &&
                                Convert.ToInt32(rateKey[9].Split('~')[2]) == childCount &&
                                rateKey[10] == childrenAges)
                            {
                                if (BookingStatusCd.Mnemonic(rate.Type) == CheckRateStatus.Recheck)
                                {
                                    var revalidateResult = CheckRate(ratea.RegsId.Split(',')[2], ratea.Price.Supplier);
                                    if (revalidateResult.IsPriceChanged)
                                    {
                                        rate.Price.SetSupplier(revalidateResult.NewPrice.GetValueOrDefault(),
                                            rate.Price.SupplierCurrency);
                                    }
                                    rate.RateKey = revalidateResult.RateKey;
                                    rate.Price.SetSupplier(revalidateResult.NewPrice.GetValueOrDefault(),
                                        rate.Price.SupplierCurrency);
                                    rate.RateCommentsId = ratea.RateCommentsId;

                                }
                                else
                                {
                                    rate.RateKey = ratea.RateKey;
                                    rate.Price.SetSupplier(ratea.Price.OriginalIdr, rate.Price.SupplierCurrency);
                                    rate.RateCommentsId = ratea.RateCommentsId;
                                    rateFound[index] = true;
                                }
                            }
                        }
                    }
                }
            }

            if (rateFound.Any(r => r == false))
            {
                Console.WriteLine("At least one rate can not be found in refresh ratekey");
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> {"At least one rate can not be found in refresh ratekey"}
                };
            }

            rsvData.HotelDetails.Rooms.ForEach(ro => ro.Rates = BundleRates(ro.Rates));
            
            var newPrice = rsvData.HotelDetails.Rooms.Sum(room => room.Rates.Sum(rate => rate.Price.Supplier));
            if (newPrice != oldPrice)
            {
                UpdateRsvDetail(rsvData.RsvNo, "FAIL", rsvData.HotelDetails);
                _paymentService.UpdatePayment(input.RsvNo, new PaymentDetails {Status = PaymentStatus.Cancelled});
                SendFailedIssueNotifToCustomerAndInternal(rsvData.RsvNo);
                LogIssuanceFailure(rsvData.RsvNo, "Price Changed (" + oldPrice + " -> " + newPrice + ")");
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Price is changed" }
                };
            }
            var newRooms = rsvData.HotelDetails.Rooms;
            var issueInfo = new HotelIssueInfo
            {
                RsvNo = rsvData.RsvNo,
                Pax = rsvData.Pax,
                Contact = rsvData.Contact,
                Rooms = newRooms,
                SpecialRequest = rsvData.HotelDetails.SpecialRequest
            };

            var issue = new HotelBedsIssue();
            var issueResult = new HotelIssueTicketResult();
            try
            {
                issueResult = issue.IssueHotel(issueInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UpdateRsvDetail(rsvData.RsvNo, "FAIL", rsvData.HotelDetails);
                _paymentService.UpdatePayment(input.RsvNo, new PaymentDetails { Status = PaymentStatus.Cancelled });
                SendFailedIssueNotifToCustomerAndInternal(rsvData.RsvNo);
                while (e.InnerException != null)
                    e = e.InnerException;
                LogIssuanceFailure(rsvData.RsvNo, "\n*Exception :* "
                    + e.Message
                    + "\n*Stack Trace :* \n"
                    + e.StackTrace);
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { issueResult.Status }
                };
            }
           
            if (issueResult.IsSuccess == false)
            {
                UpdateRsvDetail(rsvData.RsvNo, "FAIL", rsvData.HotelDetails);
                _paymentService.UpdatePayment(input.RsvNo, new PaymentDetails { Status = PaymentStatus.Cancelled });
                SendFailedIssueNotifToCustomerAndInternal(rsvData.RsvNo);
                LogIssuanceFailure(rsvData.RsvNo, "Status : " + issueResult.Status);
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> {issueResult.Status}
                };
            }

            UpdateRsvStatusDb(rsvData.RsvNo, issueResult.IsSuccess ? RsvStatus.Completed : RsvStatus.Failed);
            UpdateRsvDetail(rsvData.RsvNo, "TKTD", rsvData.HotelDetails);
            UpdateReservationDetailsToDb(issueResult);
            SendEticketToCustomer(rsvData.RsvNo);
            var order = issueResult.BookingId.Select(id => new OrderResult
            {
                BookingId = id,
                BookingStatus = BookingStatus.Ticketed,
                IsSuccess = true
            }).ToList();

            Console.WriteLine("Success issuing");
            return new IssueHotelTicketOutput
            {
                IsSuccess = true,
                OrderResults = order
            };
        }


        private static void LogIssuanceFailure(string rsvNo, string message)
        {
            var TableLog = new GlobalLog();
            TableLog.PartitionKey = "HOTEL ISSUANCE FAILURE";
            var log = LogService.GetInstance();
            var env = EnvVariables.Get("general", "environment");
            TableLog.Log = "```Hotel Issuance Failure```"
                + "\n*Environment :* " + env.ToUpper()
                + "\n*Reservation :* \n"
                + rsvNo
                + "\n*Message :* \n"
                + message;
            log.Post(TableLog.Log,
                env == "production" ? "#logging-prod" : "#logging-dev");
            TableLog.Logging();
        }
    }
}
