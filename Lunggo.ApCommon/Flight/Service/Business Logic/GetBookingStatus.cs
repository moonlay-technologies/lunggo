using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void GetBookingStatus()
        {
            var result = GetBookingStatusInternal();
            //TODO save to database
        }
    }
}
