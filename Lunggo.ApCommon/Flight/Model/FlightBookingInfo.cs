using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class FlightBookingInfo
    {
        internal string FareId { get; set; }
        internal List<PassengerInfoFare> PassengerInfoFares { get; set; }
        internal ContactData ContactData { get; set; }
    }
}
