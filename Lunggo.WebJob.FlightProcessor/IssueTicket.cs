using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Log;
using Microsoft.Azure.WebJobs;
using Lunggo.ApCommon.Log;
using Lunggo.Framework.Environment;

namespace Lunggo.WebJob.FlightProcessor
{
    public partial class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void FlightIssueTicket([QueueTrigger("flightissueticket")] string rsvNo)
        {
            var TableLog = new GlobalLog();
            
            TableLog.PartitionKey = "FLIGHT ISSUE LOG";
            
            var flight = FlightService.GetInstance();
            Console.WriteLine("Processing Flight Issue Ticket for RsvNo " + rsvNo + "...");
            var sw = Stopwatch.StartNew();
            var log = LogService.GetInstance();
            var env = EnvVariables.Get("general", "environment");
            TableLog.Log = "Logging Issue for *Rsv No : " + rsvNo + "*";
            log.Post(TableLog.Log, "#logging-issueflight");
            TableLog.Logging();
            var issueTimeout = int.Parse(EnvVariables.Get("flight", "issueTimeout"));
            var issueCancellationSource = new CancellationTokenSource();
            var issueCancellation = issueCancellationSource.Token;
            var issueTask = Task.Run(() =>
            {
                flight.CommenceIssueTicket(new IssueTicketInput { RsvNo = rsvNo });
            }, issueCancellation);
            if (Task.WhenAny(issueTask, Task.Delay(new TimeSpan(0, 0, issueTimeout), issueCancellation)).Result == issueTask)
            {
                issueTask.Wait(issueCancellation);
            }
            else
            {
                issueCancellationSource.Cancel();
                //Sending Email
                FlightService.GetInstance().SendIssueTimeoutNotifToDeveloper(rsvNo);
            }
            sw.Stop();
            Console.WriteLine("Done Processing Flight Issue Ticket for RsvNo " + rsvNo + "... (" + sw.Elapsed.TotalSeconds + "s)");
        }
    }
}
