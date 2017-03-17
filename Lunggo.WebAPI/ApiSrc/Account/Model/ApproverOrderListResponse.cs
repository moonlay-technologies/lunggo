using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ReservationOrderListResponse : ApiResponseBase
    {
        [JsonProperty("reservations")]
        public List<ReservationListModel> Reservations { get; set; }
    } 
}