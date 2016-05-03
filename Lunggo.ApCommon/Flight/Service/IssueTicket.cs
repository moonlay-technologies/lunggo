using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        internal override void Issue(string rsvNo)
        {
            IssueTicket(new IssueTicketInput {RsvNo = rsvNo});
        }

        public IssueTicketOutput IssueTicket(IssueTicketInput input)
        {
            var reservation = GetReservation(input.RsvNo);
            var output = new IssueTicketOutput();

            if (reservation == null)
            {
                output.IsSuccess = false;
                output.Errors = new List<FlightError> { FlightError.InvalidInputData };
                return output;
            }

            if (reservation.Payment.Method == PaymentMethod.Credit ||
                (reservation.Payment.Method != PaymentMethod.Credit &&
                 reservation.Payment.Status == PaymentStatus.Settled))
            {
                var queueService = QueueService.GetInstance();
                var queue = queueService.GetQueueByReference("FlightIssueTicket");
                queue.AddMessage(new CloudQueueMessage(input.RsvNo));
                output.IsSuccess = true;
                return output;
            }
            else
            {
                output.IsSuccess = false;
                output.Errors = new List<FlightError> { FlightError.NotEligibleToIssue };
                return output;
            }
        }

        public IssueTicketOutput CommenceIssueTicket(IssueTicketInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservation = GetReservation(input.RsvNo);
                var output = new IssueTicketOutput();

                if (reservation == null)
                {
                    output.IsSuccess = false;
                    output.Errors = new List<FlightError> { FlightError.InvalidInputData };
                    return output;
                }

                if (reservation.Payment.Method == PaymentMethod.Credit ||
                    (reservation.Payment.Method != PaymentMethod.Credit &&
                     reservation.Payment.Status == PaymentStatus.Settled))
                {
                    Parallel.ForEach(reservation.Orders, itin =>
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
                    UpdateBookingStatusToDb(new List<BookingStatusInfo>
                    {
                        new BookingStatusInfo
                    {
                        BookingId = orderResult.BookingId,
                        BookingStatus = orderResult.BookingStatus
                        }
                    });
                    output.OrderResults.Add(orderResult);
                });
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
                            InsertSavedPassengersToDb(reservation.Contact.Email, reservation.Passengers);
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
                else
                {
                    output.IsSuccess = false;
                    output.Errors = new List<FlightError> { FlightError.NotEligibleToIssue };
                    return output;
                }
            }
        }

        public OrderTicketResult OrderTicketInternal(string bookingId, bool canHold)
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

        private static void UpdateIssueStatus(string rsvNo, IssueTicketOutput output)
        {
            if (output.Errors == null)
            {
                //UpdateIssueProgressToDb(rsvNo, "Generating Eticket File");
            }
            else
            {
                var errorMsgs = string.Join("; ", output.ErrorMessages);
                var errors = string.Join("; ", output.Errors);
                var progressMessages = "Issue Failed : " + errorMsgs + "; " + errors;
                //UpdateIssueProgressToDb(rsvNo, progressMessages);
            }
        }
    }
}
