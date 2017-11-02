using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model.Logic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class IssueActivityTicketOutput : OutputBase
    {
        public List<OrderResult> OrderResults { get; set; }

        public IssueActivityTicketOutput()
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
        public string Supplier { get; set; }
    }
}
