using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void IssueTicket(IssueTicketInput input)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightIssueTicket");
            queue.AddMessage(new CloudQueueMessage(input.RsvNo));
        }

        public IssueTicketOutput CommenceIssueTicket(IssueTicketInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservation = GetReservation(input.RsvNo);
                var output = new IssueTicketOutput();
                foreach (var itin in reservation.Itineraries)
                {
                    var bookingId = itin.BookingId;
                    var canHold = itin.CanHold;
                    var response = OrderTicketInternal(bookingId, canHold);
                    var orderResult = new OrderResult();
                    if (response.IsSuccess)
                    {
                        orderResult.IsSuccess = true;
                        orderResult.BookingId = response.BookingId;
                        orderResult.BookingStatus = response.IsInstantIssuance
                            ? BookingStatus.Ticketed
                            : BookingStatus.Ticketing;
                        orderResult.IsInstantIssuance = response.IsInstantIssuance;
                        UpdateBookingIdQuery.GetInstance().Execute(conn, new
                        {
                            BookingId = bookingId,
                            NewBookingId = orderResult.BookingId,
                        });
                    }
                    else
                    {
                        orderResult.IsSuccess = false;
                        orderResult.BookingId = bookingId;
                        orderResult.BookingStatus = BookingStatus.Failed;
                        output.Errors = response.Errors;
                        output.ErrorMessages = response.ErrorMessages;
                    }
                    UpdateDb.BookingStatus(new List<BookingStatusInfo> {new BookingStatusInfo
                    {
                        BookingId = orderResult.BookingId,
                        BookingStatus = orderResult.BookingStatus
                    }});
                    output.OrderResults.Add(orderResult);
                }
                if (output.OrderResults.TrueForAll(result => result.IsSuccess))
                {
                    output.IsSuccess = true;
                    if (output.OrderResults.TrueForAll(result => result.IsInstantIssuance))
                    {
                        var detailsInput = new GetDetailsInput { RsvNo = input.RsvNo };
                        GetAndUpdateNewDetails(detailsInput);
                        SendEticketToCustomer(input.RsvNo);
                        if (reservation.Payment.Method != PaymentMethod.BankTransfer)
                            SendInstantPaymentConfirmedNotifToCustomer(input.RsvNo);
                        InsertDb.SavedPassengers(reservation.Contact.Email, reservation.Passengers);
                    }
                }
                else
                {
                    if (output.OrderResults.Any(set => set.IsSuccess))
                    {
                        output.PartiallySucceed();
                    }
                    output.IsSuccess = false;
                    output.Errors = output.Errors.Distinct().ToList();
                    output.ErrorMessages = output.ErrorMessages.Distinct().ToList();
                }
                UpdateIssueStatus(input.RsvNo, output);
                return output;
            }
        }

        private static void UpdateIssueStatus(string rsvNo, IssueTicketOutput output)
        {
            if (!output.Errors.Any())
            {
                UpdateDb.IssueProgress(rsvNo, "Generating Eticket File");
            }
            else
            {
                var errorMsgs = string.Join("; ", output.ErrorMessages);
                var errors = string.Join("; ", output.Errors);
                var progressMessages = "Issue Failed : " + errorMsgs + "; " + errors;
                UpdateDb.IssueProgress(rsvNo, progressMessages);
            }
        }
    }
}
