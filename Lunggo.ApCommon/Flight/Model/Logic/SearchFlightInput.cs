namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class SearchFlightInput
    {
        public string SearchId { get; set; }
        public SearchFlightConditions Conditions { get; set; }
        public bool IsDateFlexible { get; set; }
    }
}
