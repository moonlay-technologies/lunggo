﻿using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAppointmentRequestApiResponse : ApiResponseBase
    {
        [JsonProperty("appointmentRequests", NullValueHandling = NullValueHandling.Ignore)]
        public List<AppointmentDetailForDisplay> Appointments { get; set; }
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }
        [JsonProperty("perPage", NullValueHandling = NullValueHandling.Ignore)]
        public int? PerPage { get; set; }
    }
}