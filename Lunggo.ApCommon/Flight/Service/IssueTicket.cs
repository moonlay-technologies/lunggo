﻿using System.Collections.Generic;
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
            decimal  currentBalance=0;
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservation = GetReservation(input.RsvNo);
                var output = new IssueTicketOutput();
                Parallel.ForEach(reservation.Itineraries, itin =>
                {
                    var bookingId = itin.BookingId;
                    var canHold = itin.CanHold;
                    var response = OrderTicketInternal(bookingId, canHold);
                    currentBalance = response.CurrentBalance;
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
                        if (reservation.Payment.Method != PaymentMethod.BankTransfer)
                            SendInstantPaymentConfirmedNotifToCustomer(input.RsvNo);
                        InsertDb.SavedPassengers(reservation.Contact.Email, reservation.Passengers);
                    }
                }
                else
                {
                    int casetype = GetCaseType();
                    var depositPriceItin = IsInsufficientDeposit(input.RsvNo ,currentBalance);
                    if (casetype != 0)
                    {
                        SendIssueSlightDelayNotifToCustomer(reservation.RsvNo + "+" + casetype.ToString());
                    }
                    else 
                    {
                        SendSaySorryFailedIssueNotifToCustomer(reservation.RsvNo);
                    }

                    SendIssueFailedNotifToDeveloper(reservation.RsvNo + "+" + depositPriceItin);
                    
                    //Jika berhasil cuma berhasil satu doang
                    if (output.OrderResults.Any(set => set.IsSuccess))
                    {
                        output.PartiallySucceed();
                    }//End
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

        private string IsInsufficientDeposit(string rsvNo, decimal balance)
        {
            string balanceAndItinPrice = "";
            var reservation = FlightService.GetInstance().GetReservation(rsvNo);
            var supplierPrice = reservation.Itineraries.Sum(itin => itin.LocalPrice);
            if (balance != 0)
            {
               if(balance<supplierPrice)
               {
                   balanceAndItinPrice = balance.ToString() + ";" + supplierPrice.ToString();
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
