using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Queue;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.FlightProcessor
{
    public partial class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void FlightSearchTimeout1([QueueTrigger("flightcrawltimeout1")] string searchIdWithTimeout)
        {
            var flight = FlightService.GetInstance();
            flight.SetSearchAsEmptyIfNotSet(searchIdWithTimeout, 1);
        }

        public static void FlightSearchTimeout2([QueueTrigger("flightcrawltimeout2")] string searchIdWithTimeout)
        {
            var flight = FlightService.GetInstance();
            flight.SetSearchAsEmptyIfNotSet(searchIdWithTimeout, 2);
        }

        public static void FlightSearchTimeout3([QueueTrigger("flightcrawltimeout3")] string searchIdWithTimeout)
        {
            var flight = FlightService.GetInstance();
            flight.SetSearchAsEmptyIfNotSet(searchIdWithTimeout, 3);
        }

        public static void FlightSearchTimeout4([QueueTrigger("flightcrawltimeout4")] string searchIdWithTimeout)
        {
            var flight = FlightService.GetInstance();
            flight.SetSearchAsEmptyIfNotSet(searchIdWithTimeout, 4);
        }

        public static void FlightSearchTimeout5([QueueTrigger("flightcrawltimeout5")] string searchIdWithTimeout)
        {
            var flight = FlightService.GetInstance();
            flight.SetSearchAsEmptyIfNotSet(searchIdWithTimeout, 5);
        }

        public static void FlightSearchTimeout6([QueueTrigger("flightcrawltimeout6")] string searchIdWithTimeout)
        {
            var flight = FlightService.GetInstance();
            flight.SetSearchAsEmptyIfNotSet(searchIdWithTimeout, 6);
        }
    }
}
