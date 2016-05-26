using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using System;

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
            List<string> supplier = new List<string>();
            List<decimal> balance = new List<decimal>();
            List<decimal> localPrice = new List<decimal>();
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservation = GetReservation(input.RsvNo);
                var output = new IssueTicketOutput();
                Parallel.ForEach(reservation.Itineraries, itin =>
                {
                    var bookingId = itin.BookingId;
                    var canHold = itin.CanHold;
                    var response = OrderTicketInternal(bookingId, canHold);
                    balance.Add(response.CurrentBalance);
                    supplier.Add(response.SupplierName);
                    localPrice.Add(itin.SupplierPrice);
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
                    UpdateDb.BookingStatus(new List<BookingStatusInfo>
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
                        //Send Eticket Slight Delay if Choosing Mystifly
                        if (supplier.Contains("Mystifly")) 
                        {
                            SendEticketSlightDelayNotifToCustomer(input.RsvNo);
                        }
                        if (reservation.Payment.Method != PaymentMethod.BankTransfer)
                            SendInstantPaymentConfirmedNotifToCustomer(input.RsvNo);
                        InsertDb.SavedPassengers(reservation.Contact.Email, reservation.Passengers);
                    }
                }
                else
                {
                    int casetype = GetCaseType();
                    var supplierInfoItin = ConcatenateMessage(supplier,balance,localPrice);
                    if (casetype != 0)
                    {
                        SendIssueSlightDelayNotifToCustomer(reservation.RsvNo + "+" + casetype.ToString());
                        //Testing
                        /*if (supplier.Contains("Mystifly"))
                        {
                            SendEticketSlightDelayNotifToCustomer(input.RsvNo);
                        }
                        SendFailedVerificationCreditCardNotifToCustomer(input.RsvNo);
                        SendEticketSlightDelayNotifToCustomer(input.RsvNo); 
                        SendSaySorryFailedIssueNotifToCustomer(reservation.RsvNo);*/
                    }
                    else 
                    {
                        SendSaySorryFailedIssueNotifToCustomer(reservation.RsvNo);
                    }

                    SendIssueFailedNotifToDeveloper(reservation.RsvNo + "+" + supplierInfoItin);
                    
                    //Jika berhasil cuma berhasil satu doang
                    if (output.OrderResults.Any(set => set.IsSuccess))
                    {
                        output.PartiallySucceed();
                    }//End
                    output.IsSuccess = false;
                    //output.Errors = output.Errors.Distinct().ToList();
                    //output.ErrorMessages = output.ErrorMessages.Distinct().ToList();
                }
                UpdateIssueStatus(input.RsvNo, output);
                return output;
            }
        }

        private static void UpdateIssueStatus(string rsvNo, IssueTicketOutput output)
        {
            if (output.Errors == null)
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

        private string ConcatenateMessage(List<string>supplierName, List<decimal>balance,List<decimal>localPrice)
        {
       
            string balanceAndItinPrice = "";
            for(int i=0;i<supplierName.Count;i++)
            {
                if (i != supplierName.Count - 1 && i>1)
                {
                    balanceAndItinPrice = supplierName[i] + ";" + localPrice[i] + ";" + balance[i] + "+";
                }
                else 
                {
                    balanceAndItinPrice = supplierName[i] + ";" + localPrice[i] + ";" + balance[i];
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
