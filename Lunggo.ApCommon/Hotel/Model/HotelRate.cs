using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;
using Microsoft.Azure.Documents;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelRate : OrderBase
    {
        public string RateKey { get; set; }
        public int RegsId { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string PaymentType { get; set; }
        public string Boards { get; set; }
        public Cancellation Cancellation { get; set; }
        public int RoomCount { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public List<Offer> Offers { get; set; }
        
    }

    public class Offer
    {
        public string Code { get; set; }
        public decimal Amount { get; set; }
    }

    public class Cancellation
    {
        public decimal Fee { get; set; }
        public DateTime StartTime { get; set; }
    }
}
