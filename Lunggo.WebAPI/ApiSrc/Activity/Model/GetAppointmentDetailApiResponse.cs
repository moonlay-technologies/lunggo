using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAppointmentDetailApiResponse : ApiResponseBase
    {
        [JsonProperty("appointmentDetail", NullValueHandling = NullValueHandling.Ignore)]
        public AppointmentDetailForDisplay AppointmentDetail { get; set; }
    }
}