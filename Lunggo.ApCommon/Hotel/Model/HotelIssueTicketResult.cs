using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelIssueTicketResult : ResultBase
    {
        public List<string> BookingId { get; set; }
        public string RsvNo { get; set; }
        public string Status { get; set; }
        public bool IsInstantIssuance { get; set; }
        public bool IsSuccess { get; set; }
        public string SupplierName { get; set; }
        public string SupplierVat { get; set; }
        public string HotelPhone { get; set; }
        public string HotelRating { get; set; }
        public string HotelAddress { get; set; }
        public string BookingReference { get; set; }
        public string ClientReference { get; set; }
    }

    public class Supplier
    {
        public string Name { get; set; }
        public string VatNumber { get; set; }
    }
}
