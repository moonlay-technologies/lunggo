using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class GetDetailsInput
    {
        public string BookingId { get; set; }
        public List<TripInfo> TripInfos { get; set; }
    }
}
