using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAppointmentListApiResponse : ApiResponseBase
    {
        [JsonProperty("appointments", NullValueHandling = NullValueHandling.Ignore)]
        public List<AppointmentListForDisplay> Appointments { get; set; }
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }
        [JsonProperty("perPage", NullValueHandling = NullValueHandling.Ignore)]
        public int? PerPage { get; set; }
    }
}