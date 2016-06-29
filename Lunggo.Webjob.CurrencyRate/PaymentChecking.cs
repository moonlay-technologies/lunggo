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
using Lunggo.ApCommon.Payment.Service;

namespace Lunggo.Webjob.CurrencyRate
{
    public partial class Program
    {
        private static void PaymentCheck(List<KeyValuePair<string, string>> list)
        {
            Console.WriteLine("Payment Checking Started");
            var paymentService = PaymentService.GetInstance();
            var unpaids = paymentService.GetUnpaids();
            var format = "dd/MM/yyyy HH:mm:ss";
            CultureInfo provider = new CultureInfo("id-ID");
            foreach (var pair in list)
            {
                foreach (var unpaid in unpaids)
                {
                    var rsvNo = unpaid.Key;
                    var payment = unpaid.Value;
                    var transacDate = DateTime.ParseExact(pair.Value, format, provider).AddHours(-7);
                    var timelimit = (DateTime)payment.TimeLimit;
                    var transacUTCDate = DateTime.SpecifyKind(transacDate, DateTimeKind.Utc);
                    if (decimal.Parse(pair.Key) == payment.FinalPriceIdr)
                    {
                        Console.WriteLine("The Price is Same ");
                        if (transacUTCDate >= timelimit.AddMinutes(-120) && transacUTCDate <= timelimit)
                        {
                            Console.WriteLine("---->FIND : Same price and valid date transaction");
                            //Change Payment Reservation
                            payment.PaidAmountIdr = decimal.Parse(pair.Key);
                            payment.Status = PaymentStatus.Settled;
                            payment.Time = transacUTCDate;
                            Console.WriteLine("Reservation No : " + rsvNo + ", Price : " + payment.FinalPriceIdr + " Has Paid");

                            //Update in database
                            PaymentService.GetInstance().UpdatePayment(rsvNo, payment);
                            FlightService.GetInstance().SendPendingPaymentConfirmedNotifToCustomer(rsvNo);
                        }
                    }
                }
            }
        }
    }
}
