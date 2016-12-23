using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelCancelBookingResult
    {
        public string status { get; set; }
        public string Reference { get; set; }
        public string ClientReference { get; set; }
        public string CancellationReference { get; set; }
        public decimal CancellationAmount { get; set; }
        //public DateTime CancellationDate { get; set; }
    }

}