using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class IssueTicketOutput : OutputBase
    {
        public List<OrderResult> OrderResults { get; set; }

        public IssueTicketOutput()
        {
            OrderResults = new List<OrderResult>();
        }
    }

    public class OrderResult
    {
        public bool IsSuccess { get; set; }
        public string BookingId { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public bool IsInstantIssuance { get; set; }
    }
}
