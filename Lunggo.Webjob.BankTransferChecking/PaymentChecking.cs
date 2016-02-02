using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Webjob.BankTransferChecking
{
    public partial class Program
    {
        private static void PaymentCheck(Dictionary<string, string> dict) 
        {
            Debug.Print("Payment Checking Started");
            FlightService flight = FlightService.GetInstance();
            //Get all booking data from database
            List<FlightReservation> unpaidReservation = flight.GetUnpaidReservations();
            var format = "dd/MM/yyyy HH:mm:ss";
            CultureInfo provider = new CultureInfo("id-ID");
            foreach (var pair in dict) 
            {
                foreach (var getUnpaid in unpaidReservation) 
                {
                    var transacDate = DateTime.ParseExact(pair.Value, format, provider).AddHours(-7);
                    if (decimal.Parse(pair.Key) == getUnpaid.Payment.FinalPrice)
                    {
                        Debug.Print("The Price is Same ");
                        if (transacDate >= getUnpaid.RsvTime && transacDate <= getUnpaid.Payment.TimeLimit) 
                        {
                            Debug.Print("---->FIND : Same price and vaid date transaction");
                            //Change Payment Reservation
                            getUnpaid.Payment.PaidAmount = decimal.Parse(pair.Key);
                            getUnpaid.Payment.Status = PaymentStatus.Settled;
                            getUnpaid.Payment.Time = DateTime.ParseExact(pair.Value, format, provider);
                            Debug.Print("Reservation No : " + getUnpaid.RsvNo + ", Price : " + getUnpaid.Payment.FinalPrice + " Has Paid");

                            //Updating in database
                            FlightService.GetInstance().UpdateFlightPayment(getUnpaid.RsvNo, getUnpaid.Payment);
                            FlightService.GetInstance().SendPendingPaymentConfirmedNotifToCustomer(getUnpaid.RsvNo);
                        }
                    }
                }
            }
        }
    }
}
