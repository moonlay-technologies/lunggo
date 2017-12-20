using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public List<ActivityReservation> GetBookedActivities()
        {
            var reservations = GetBookedActivitiesFromDb();
            return reservations;
        }

        public void ForwardActivityToOperator(string rsvNo)
        {
            UpdateActivityBookingStatusInDb(rsvNo, BookingStatus.ForwardedToOperator);
        }
    }
}
