using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Constant;

namespace Lunggo.ApCommon.Activity.Model
{
    public class ActivityModel
    {
        public long SupplierId { get; set; }
        public int AreaCd { get; set; }
        public string ActivityName { get; set; }
        public ActivityTypeEnum ActivityType { get; set; }
        public string ActivityShortDesc { get; set; }
        public string HotelMeetLocation { get; set; }
        public string ToKnow { get; set; }
        public int MaxGuest { get; set; }
        public int MinGuest { get; set; }
        public float LocationMeetUpLat { get; set; }
        public float LocationMeetUpLang { get; set; }
        public string OtherRules { get; set; }
        public string WeatherPolicy { get; set; }
        public string VenueAddress { get; set; }
        public bool Publish { get; set; }
    }
}
