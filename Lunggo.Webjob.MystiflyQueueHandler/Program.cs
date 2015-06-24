using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.Webjob.MystiflyQueueHandler
{
    class Program
    {
        static void Main()
        {
            Init();

            var flightService = FlightService.GetInstance();
            List<string> ticketedRsvNos;
            List<string> scheduleChangedRsvNos;
            Console.WriteLine("Retrieving Queue from Mystifly...");
            flightService.GetAndUpdateBookingStatus(out ticketedRsvNos, out scheduleChangedRsvNos);
            Console.WriteLine("Done Retrieving Queue from Mystifly...");
            if (ticketedRsvNos.Any())
                ProcessTicketed(ticketedRsvNos);
            else
                Console.WriteLine("No Ticketed Queue");
            if (scheduleChangedRsvNos.Any())
                ProcessScheduleChanged(scheduleChangedRsvNos);
            else
                Console.WriteLine("No Schedule Changed Queue");
        }

        private static void ProcessTicketed(IEnumerable<string> ticketedRsvNos)
        {
            var flightService = FlightService.GetInstance();
            foreach (var ticketedRsvNo in ticketedRsvNos)
            {
                Console.WriteLine("Processing Ticketed " + ticketedRsvNo + "...");
                var detailsInput = new GetDetailsInput {RsvNo = ticketedRsvNo};
                flightService.GetAndUpdateNewDetails(detailsInput);
                var queueService = QueueService.GetInstance();
                var queue = queueService.GetQueueByReference(Queue.Eticket);
                queue.AddMessage(new CloudQueueMessage(ticketedRsvNo));
            }
        }

        private static void ProcessScheduleChanged(IEnumerable<string> scheduleChangedRsvNos)
        {
            var flightService = FlightService.GetInstance();
            foreach (var scheduleChangedRsvNo in scheduleChangedRsvNos)
            {
                Console.WriteLine("Processing Schedule Changed " + scheduleChangedRsvNo + "...");
                var detailsInput = new GetDetailsInput { RsvNo = scheduleChangedRsvNo };
                flightService.GetAndUpdateNewDetails(detailsInput);
                var queueService = QueueService.GetInstance();
                var queue = queueService.GetQueueByReference(Queue.ChangedEticket);
                queue.AddMessage(new CloudQueueMessage(scheduleChangedRsvNo));
            }
        }

        private static void Init()
        {
            InitConfigurationManager();
            InitDatabaseService();
            InitFlightService();
            InitQueueService();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init(@"Config\");
        }

        private static void InitDatabaseService()
        {
            var db = DbService.GetInstance();
            db.Init();
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init();
        }

        private static void InitQueueService()
        {
            var queue = QueueService.GetInstance();
            queue.Init();
        }
    }
}
