using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class ActivityAddSessionInput
    {
        public long ActivityId { get; set; }
        public List<AvailableDayAndHours> RegularAvailableDates { get; set; }
        public List<DateAndAvailableHour> WhitelistCustomDates { get; set; }
    }
}