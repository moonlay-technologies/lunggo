using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSelectRoomApiRequest
    {
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
        [JsonProperty("regs")]
        public List<RegsId> RegsIds { get; set; }    
    } 
}
