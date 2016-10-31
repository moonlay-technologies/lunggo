using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using Supplier = Lunggo.ApCommon.Flight.Constant.Supplier;

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
                output.Errors = new List<FlightError> {FlightError.NotEligibleToIssue};
                return output;
            }
        }

        public IssueTicketOutput CommenceIssueTicket(IssueTicketInput input)
        {
            var supplier = new List<Supplier>();
            var balance = new List<decimal>();
            var localPrice = new List<decimal>();
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
                Parallel.ForEach(reservation.Itineraries, itin =>
                {
                    var response = IssueTicketInternal(new IssueTicketInfo
                    {
                        BookingId = itin.BookingId,
                        CanHold = itin.CanHold,
                        Supplier = itin.Supplier
                    });
                    balance.Add(response.CurrentBalance);
                    supplier.Add(response.SupplierName);
                    localPrice.Add(itin.Price.Supplier);
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
                            BookingId = itin.BookingId,
                            NewBookingId = orderResult.BookingId,
                        });
                    }
                    else
                    {
                        orderResult.IsSuccess = false;
                        orderResult.BookingId = itin.BookingId;
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
                    }
                    else
                        {
                            SendEticketSlightDelayNotifToCustomer(input.RsvNo);
                        }
                }
                else
                {
                    int casetype = GetCaseType();
                    var supplierInfoItin = ConcatenateMessage(supplier, balance, localPrice);
                    if (casetype != 0)
                    {
                        SendIssueSlightDelayNotifToCustomer(reservation.RsvNo + "+" + casetype);

                    }
                    else
                    {
                        SendSaySorryFailedIssueNotifToCustomer(reservation.RsvNo);
                    }

                    SendIssueFailedNotifToDeveloper(reservation.RsvNo + "+" + supplierInfoItin);
                    
                    if (output.OrderResults.Any(set => set.IsSuccess))
                    {
                        output.PartiallySucceed();
                    }//End
                    output.IsSuccess = false;

                }
                //UpdateIssueStatus(input.RsvNo, output);
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

        public IssueTicketResult IssueTicketInternal(IssueTicketInfo info)
        {
            var supplierName = info.Supplier;
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();
            IssueTicketResult result = supplier.OrderTicket(info.BookingId, info.CanHold);
            result.SupplierName = supplier.SupplierName;
            return result;
        }

        //private static void UpdateIssueStatus(string rsvNo, IssueTicketOutput output)
        //{
        //    if (output.Errors == null)
        //    {
        //        //UpdateIssueProgressToDb(rsvNo, "Generating Eticket File");
        //    }
        //    else
        //    {
        //        var errorMsgs = string.Join("; ", output.ErrorMessages);
        //        var errors = string.Join("; ", output.Errors);
        //        var progressMessages = "Issue Failed : " + errorMsgs + "; " + errors;
        //        //UpdateIssueProgressToDb(rsvNo, progressMessages);
        //    }
        //}

        private static string ConcatenateMessage(List<Supplier> supplierName, List<decimal> balance, List<decimal> localPrice)
        {
       
            var balanceAndItinPrice = "";
            for (var i = 0; i < supplierName.Count; i++)
            {
                if (i != supplierName.Count - 1)
                {
                    balanceAndItinPrice = balanceAndItinPrice + supplierName[i] + ";" + localPrice[i] + ";" + balance[i] + "+";
                }
                else 
                {
                    balanceAndItinPrice = balanceAndItinPrice + supplierName[i] + ";" + localPrice[i] + ";" + balance[i];
                }
                
            }
            return balanceAndItinPrice;
        }

        private static int GetCaseType() 
        {
            var datenow = DateTime.UtcNow.AddHours(+7);
            int casetype;
            DateTime myDate = DateTime.Parse(datenow.Month + "/" + datenow.Day + "/" + datenow.Year + " 04:00:00 PM");
            if ((datenow <= myDate) && (datenow.DayOfWeek != DayOfWeek.Sunday && datenow.DayOfWeek != DayOfWeek.Saturday))
            {
                //Issue ticket 3 jam
                casetype = (int)IssueManualType.WorkingHourCase;
            }
            else if ((datenow > myDate && datenow.DayOfWeek != DayOfWeek.Sunday && datenow.DayOfWeek != DayOfWeek.Saturday && datenow.DayOfWeek != DayOfWeek.Friday) || (datenow.DayOfWeek == DayOfWeek.Sunday))
            {
                //Issue ticket 1 hari
                casetype = (int)IssueManualType.SundayOrNotWorkingHourCase;
            }
            else if (datenow > myDate && datenow.DayOfWeek == DayOfWeek.Friday)
            {
                //Issue ticket 3 hari
                casetype = (int)IssueManualType.FridayNotWorkingHourCase;
            }
            else if (datenow.DayOfWeek == DayOfWeek.Saturday)
            {
                //Issue ticket 2 hari
                casetype = (int)IssueManualType.SaturdayCase;
            }
            else
            {
                //Error System
                casetype = (int)IssueManualType.ErrorSystem;
            }
            return casetype;
        }
    }
}
