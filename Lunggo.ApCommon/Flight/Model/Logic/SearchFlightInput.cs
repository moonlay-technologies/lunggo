using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class SearchFlightInput
    {
        public SearchFlightConditions Conditions { get; set; }
        public List<int> RequestedSupplierIds { get; set; }
        public bool IsDateFlexible { get; set; }
    }
}
