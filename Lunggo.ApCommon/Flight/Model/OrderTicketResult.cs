namespace Lunggo.ApCommon.Flight.Model
{
    internal class OrderTicketResult : ResultBase
    {
        internal string BookingId { get; set; }
        internal bool IsInstantIssuance { get; set; }
    }
}
