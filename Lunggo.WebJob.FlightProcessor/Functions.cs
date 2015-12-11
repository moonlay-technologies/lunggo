using System.IO;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.FlightProcessor
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void FlightIssueTicket([QueueTrigger("flightissueticket")] string rsvNo)
        {
            var flight = FlightService.GetInstance();
            flight.CommenceIssueTicket(new IssueTicketInput {RsvNo = rsvNo});
        }
    }
}
