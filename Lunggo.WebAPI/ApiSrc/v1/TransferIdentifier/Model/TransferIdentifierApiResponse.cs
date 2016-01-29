using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.TransferIdentifier.Model
{
    public class TransferIdentifierApiResponse
    {
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode{get;set;}
        [JsonProperty("status_message")]
        public string StatusMessage{get;set;}
        [JsonProperty("transfer_code")]
        //public IEnumerable<ModelName> price{get;set;}  
        public int TransferCode { get; set; }
    }
}