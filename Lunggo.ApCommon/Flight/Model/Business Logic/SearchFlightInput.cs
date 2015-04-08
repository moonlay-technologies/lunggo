using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class SearchFlightInput
    {
        public SearchFlightConditions Conditions { get; set; }
        public TripType TripType { get; set; }
        public bool IsDateFlexible { get; set; }
        public bool IsReturnSeparated { get; set; }
    }
}
