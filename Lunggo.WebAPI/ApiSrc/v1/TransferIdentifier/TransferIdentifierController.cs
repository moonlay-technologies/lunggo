using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Cors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunggo.ApCommon.TransferIdentifier;
using Lunggo.WebAPI.ApiSrc.v1.TransferIdentifier.Model;


namespace Lunggo.WebAPI.ApiSrc.v1.PriceIdentifier
{
    public class TransferIdentifierController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/transferidentifier")]
        public TransferIdentifierApiResponse GetIdentifier([FromUri] decimal price)
        {
           
            var response = TransferIdentifierService.GetInstance().GetTransferIdentifier(price);
            return new TransferIdentifierApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Generated Code Success",
                TransferCode = response
            };
        }
    }
}
