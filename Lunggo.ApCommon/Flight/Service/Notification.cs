using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Notifications;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void PushRefreshTrxHistoryNotif(string email)
        {
            var service = NotificationService.GetInstance();
            var tags = new Dictionary<string, string>
            {
                {TagType.Email, email}
            };
            service.PushSilentNotification(new {apagitu = "yes"}, tags);
        }

        public void PushFlightReminderNotif(string rsvNo)
        {
            var rsv = GetReservation(rsvNo);
            var userEmail = rsv.User.Email;
            var userPhone = rsv.User.PhoneNumber;
            var service = NotificationService.GetInstance();
            var tags = new Dictionary<string, string>
            {
                {TagType.Email, userEmail}
            };
            var notif = new Notification
            {
                Title = "Jadwal Penerbangan Esok Hari",
                Message = string.Join("\n", rsv.Itineraries.SelectMany(itin => itin.Trips).Select(trip => trip.OriginAirport + "-" + trip.DestinationAirport + " " + trip.DepartureDate.ToString("dd-MM-yyyy")))
            };
            service.PushNotification(notif, tags);
        }

        public void PushEticketIssuedNotif(string rsvNo)
        {
            var rsv = GetReservation(rsvNo);
            var service = NotificationService.GetInstance();
            var tags = new Dictionary<string, string>
            {
                {TagType.Email, rsv.User.Email}
            };
            var notif = new Notification
            {
                Title = "E-tiket Anda telah terbit",
                Message = string.Join("\n", rsv.Itineraries.SelectMany(itin => itin.Trips).Select(trip => trip.OriginAirport + "-" + trip.DestinationAirport + " " + trip.DepartureDate.ToString("dd-MM-yyyy")))
            };
            service.PushNotification(notif, tags);
        }
    }
}
