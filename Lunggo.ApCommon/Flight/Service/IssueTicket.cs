using System;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public IssueTicketOutput IssueTicket(IssueTicketInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservation = GetReservation(input.RsvNo);
                var bookingIds = reservation.Itineraries.Select(itin => itin.BookingId);
                var output = new IssueTicketOutput();
                foreach (var bookingId in bookingIds)
                {
                    var response = OrderTicketInternal(bookingId);
                    var orderResult = new OrderResult();
                    if (response.IsSuccess)
                    {
                        orderResult.IsSuccess = true;
                        orderResult.BookingId = response.BookingId;
                        orderResult.BookingStatus = response.IsInstantIssuance
                            ? BookingStatus.Ticketed
                            : BookingStatus.Ticketing;
                        orderResult.IsInstantIssuance = response.IsInstantIssuance;
                    }
                    else
                    {
                        orderResult.IsSuccess = false;
                        orderResult.BookingId = response.BookingId;
                        orderResult.BookingStatus = BookingStatus.Failed;
                        output.Errors = response.Errors;
                        output.ErrorMessages = response.ErrorMessages;
                    }
                    UpdateBookingStatusQuery.GetInstance().Execute(conn, new
                    {
                        BookingId = bookingId,
                        NewBookingId = orderResult.BookingId,
                        BookingStatusCd = BookingStatusCd.Mnemonic(orderResult.BookingStatus)
                    });
                    output.OrderResults.Add(orderResult);
                }
                if (output.OrderResults.TrueForAll(result => result.IsSuccess))
                {
                    output.IsSuccess = true;
                    //TODO voucher code di sini? no.
                    //TODO rapiin juga ini biar ga ada akses query lgsg
                    var usedVoucherCode =
                        GetVoucherCodeQuery.GetInstance().Execute(conn, new { input.RsvNo }).Single();
                    VoucherService.GetInstance().InvalidateVoucher(usedVoucherCode);
                    if (output.OrderResults.TrueForAll(result => result.IsInstantIssuance))
                    {
                        var detailsInput = new GetDetailsInput { RsvNo = input.RsvNo };
                        GetAndUpdateNewDetails(detailsInput);
                        SendEticketToCustomer(input.RsvNo);
                        if (reservation.Payment.Method != PaymentMethod.BankTransfer)
                            SendInstantPaymentNotifToCustomer(input.RsvNo);
                        else
                            SendPendingPaymentConfirmedNotifToCustomer(input.RsvNo);
                        InsertDb.SavedPassengers(reservation.Contact.Email, reservation.Passengers);
                    }
                }
                else
                {
                    if (output.OrderResults.Any(set => set.IsSuccess))
                    {
                        output.PartiallySucceed();
                        //TODO voucher code di sini? no.
                        //TODO rapiin juga ini biar ga ada akses query lgsg
                        var usedVoucherCode =
                            GetVoucherCodeQuery.GetInstance().Execute(conn, new { input.RsvNo }).Single();
                        VoucherService.GetInstance().InvalidateVoucher(usedVoucherCode);
                    }
                    output.IsSuccess = false;
                    output.Errors = output.Errors.Distinct().ToList();
                    output.ErrorMessages = output.ErrorMessages.Distinct().ToList();
                }
                return output;
            }
        }
    }
}
