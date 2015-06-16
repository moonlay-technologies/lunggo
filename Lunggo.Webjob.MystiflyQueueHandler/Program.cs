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
            flightService.GetAndUpdateBookingStatus(out ticketedRsvNos, out scheduleChangedRsvNos);
            ProcessTicketed(ticketedRsvNos);
        }

        private static void ProcessTicketed(IEnumerable<string> ticketedRsvNos)
        {
            var flightService = FlightService.GetInstance();
            foreach (var ticketedRsvNo in ticketedRsvNos)
            {
                var detailsInput = new GetDetailsInput {BookingId = ticketedRsvNo};
                flightService.GetAndUpdateNewDetails(detailsInput);
                var queueService = QueueService.GetInstance();
                var queue = queueService.GetQueueByReference(QueueService.Queue.Eticket);
                queue.AddMessage(new CloudQueueMessage(ticketedRsvNo));
            }
        }

        private static void Init()
        {
            InitConfigurationManager();
            InitFlightService();
            InitQueueService();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init(@"Config\");
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
