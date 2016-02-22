using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class SearchFlightInput
    {
        public string SearchId { get; set; }
        public List<int> RequestedSupplierIds { get; set; }
    }
}
