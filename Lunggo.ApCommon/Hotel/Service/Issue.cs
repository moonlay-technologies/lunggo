using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Log;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;
using BookingStatusCd = Lunggo.ApCommon.Hotel.Constant.BookingStatusCd;
using Occupancy = Lunggo.ApCommon.Hotel.Model.Occupancy;

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
            if (rsvData.Payment.Method == PaymentMethod.Credit ||
                (rsvData.Payment.Method != PaymentMethod.Credit &&
                 rsvData.Payment.Status == PaymentStatus.Settled))
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

            var allCurrency = Currency.GetAllCurrencies();
            Guid generatedSearchId = Guid.NewGuid();
            SaveAllCurrencyToCache(generatedSearchId.ToString(), allCurrency);
            
            var issueInfo = new HotelIssueInfo
            {
                SearchId = generatedSearchId.ToString(),
                RsvNo = rsvData.RsvNo,
                Pax = rsvData.Pax,
                Contact = rsvData.Contact,
                Hotel = rsvData.HotelDetails,
                State = rsvData.State,
                BookingId = rsvData.HotelDetails.ClientReference
            };
            var issueResult = new HotelIssueTicketResult();
            try
            {
               
                /*Hotel Beds Client*/
                //var issue = new HotelBedsIssue();
                

                /*Tiket Hotel Client*/
                var issue = new TiketIssue();
                issueResult = issue.IssueHotel(issueInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UpdateRsvDetail(rsvData.RsvNo, "FAIL", rsvData.HotelDetails);
                PaymentService.GetInstance()
                    .UpdatePayment(input.RsvNo, new PaymentDetails { Status = PaymentStatus.Cancelled });
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

            if (issueResult.IsSuccess == false && issueResult.IsPriceChanged)
            {
                UpdateRsvDetail(rsvData.RsvNo, "FAIL", rsvData.HotelDetails);
                PaymentService.GetInstance()
                    .UpdatePayment(input.RsvNo, new PaymentDetails { Status = PaymentStatus.Cancelled });
                SendFailedIssueNotifToCustomerAndInternal(rsvData.RsvNo);
                LogIssuanceFailure(rsvData.RsvNo, issueResult.ErrorMessages.FirstOrDefault());
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Price is changed" }
                };
            }

            if (issueResult.IsSuccess == false)
            {
                UpdateRsvDetail(rsvData.RsvNo, "FAIL", rsvData.HotelDetails);
                PaymentService.GetInstance()
                    .UpdatePayment(input.RsvNo, new PaymentDetails { Status = PaymentStatus.Cancelled });
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
            var log = LogService.GetInstance();
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            log.Post(
                "```Hotel Issuance Failure```"
                + "\n*Environment :* " + env.ToUpper()
                + "\n*Reservation :* \n"
                + rsvNo
                + "\n*Message :* \n"
                + message,
                env == "production" ? "#logging-prod" : "#logging-dev");
        }
    }
}
