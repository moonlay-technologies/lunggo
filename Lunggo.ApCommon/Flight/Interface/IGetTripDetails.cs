using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.SharedModel;

namespace Lunggo.ApCommon.Flight.Interface
{
    public interface IGetTripDetails
    {
        GetTripDetailsResult GetTripDetails(FlightBooking booking);
    }
}
