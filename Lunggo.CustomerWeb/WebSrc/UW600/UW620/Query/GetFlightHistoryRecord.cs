using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620.Query
{
    public class GetFlightHistoryRecord : QueryRecord
    {
        public String RsvNo { get; set; }
        public String PaymentStatusCd { get; set; }     
        public String MemberCd { get; set; }
        public int ItineraryId { get; set; }
        public String AirlineCd { get; set; }
        public int TripId { get; set; }
        public String OverallTripTypeCd { get; set; }
        public String OriginAirportCd { get; set; }
        public String DestinationAirportCd { get; set; }
        public DateTime? DepartureDate { get; set; }
        

    }
}
