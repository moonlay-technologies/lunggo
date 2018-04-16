using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetBookedActivitiesResponse
    {
        [JsonProperty("reservations", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityReservation> Reservations { get; set; }
    }
}