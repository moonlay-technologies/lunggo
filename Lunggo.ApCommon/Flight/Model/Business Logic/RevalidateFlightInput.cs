using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class RevalidateFlightInput
    {
        public string FareId { get; set; }
        public string ReturnFareId { get; set; }
        public decimal TotalFare { get; set; }
        public decimal TotalAdultFare { get; set; }
        public decimal TotalChildFare { get; set; }
        public decimal TotalInfantFare { get; set; }
        public decimal PSCFare { get; set; }
        public string RBD { get; set; }
    }
}
