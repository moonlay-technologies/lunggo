using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class IssueTicketOutput : OutputBase
    {
        public string BookingId { get; set; }
        public BookingStatus BookingStatus { get; set; }
    }
}
