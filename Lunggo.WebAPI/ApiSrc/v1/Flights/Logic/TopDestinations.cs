using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static TopDestinations TopDestinations()
        {
            var flight = FlightService.GetInstance();
            return flight.GetTopDestination();
        }
    }
}
