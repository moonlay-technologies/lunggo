using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightBookingInfo
    {
        public string FareId { get; set; }
        public List<PassengerInfoFare> PassengerInfoFares { get; set; }
        public ContactData ContactData { get; set; }
    }
}
