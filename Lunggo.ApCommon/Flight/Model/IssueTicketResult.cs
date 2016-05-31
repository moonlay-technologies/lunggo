namespace Lunggo.ApCommon.Flight.Model
{
    public class IssueTicketResult : ResultBase
    {
        internal string BookingId { get; set; }
        internal bool IsInstantIssuance { get; set; }
        internal decimal CurrentBalance { get; set; }
        internal string SupplierName { get; set; }
    }
}
