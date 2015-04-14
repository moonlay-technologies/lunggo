using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    public class FlightReservationQueryRecord : QueryRecord
    {
        public string RsvNo { get; set; }
        public DateTime RsvTime { get; set; }
        public string RsvStatusCode { get; set; }
        public string LanguageCode { get; set; }
        public ContactData ContactData { get; set; }
        public PaymentData PaymentData { get; set; }
        public PriceData PriceData { get; set; }
    }

    public class ContactData
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class PriceData
    {
        public decimal TotalSourcePrice { get; set; }
        public decimal PaymentFeeForCustomer { get; set; }
        public decimal PaymentFeeForUs { get; set; }
        public decimal GrossProfit { get; set; }

    }
}
