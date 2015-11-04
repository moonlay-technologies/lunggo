using System;
using System.ComponentModel.DataAnnotations;
using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetSearchHotelRecord : QueryRecord
    {
        public String RsvNo { get; set; }
        public String ContactName { get; set; }
        public String HotelNo { get; set; }
        public String HotelName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? RsvTime { get; set; }
        [DataType(DataType.Date)] 
        public DateTime? RsvDateStart { get; set; }
        [DataType(DataType.Date)] 
        public DateTime? RsvDateEnd { get; set; }
        public int? RsvMonth { get; set; }
        public int? RsvYear { get; set; }
        [DataType(DataType.Date)] 
        public DateTime? CheckInDate { get; set; }
        [DataType(DataType.Date)] 
        public DateTime? CheckOutDate { get; set; }
        public String GuestName { get; set; }
        public String RsvStatusCd { get; set; }
        public String FinalPrice { get; set; }
        public String PaymentMethodCd { get; set; }
        public String PaymentStatusCd { get; set; }
        public int rdSelection { get; set; }
    }
}
