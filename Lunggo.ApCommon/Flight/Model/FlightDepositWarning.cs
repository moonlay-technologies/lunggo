using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightDepositWarning
    {
        public string Suppplier { get; set; }
        public decimal CurrentDeposit { get; set; }
        public decimal BookingPrice { get; set; }
    }
}
