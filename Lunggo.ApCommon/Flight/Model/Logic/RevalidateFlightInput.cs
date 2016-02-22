using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class RevalidateFlightInput
    {
        public string SearchId { get; set; }
        public List<int> ItinIndices { get; set; }
        public string Token { get; set; }
    }
}
