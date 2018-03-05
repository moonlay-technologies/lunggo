using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetTransactionStatementApiResponse : ApiResponseBase
    {
        [JsonProperty("transactionStatements", NullValueHandling = NullValueHandling.Ignore)]
        public List<TransactionStatementForDisplay> TransactionStatements { get; set; }
    }
}