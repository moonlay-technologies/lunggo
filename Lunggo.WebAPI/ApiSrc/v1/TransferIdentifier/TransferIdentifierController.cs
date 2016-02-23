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

            Guid generatorToken;
            // Create and display the value of two GUIDs.
            generatorToken = Guid.NewGuid();
            var response = TransferIdentifierService.GetInstance().GetTransferIdentifier(price, generatorToken.ToString());
            return new TransferIdentifierApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Generated Code Success",
                TransferCode = response,
                Token = generatorToken.ToString()
            };
        }

       /* [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/saveprice")]
        public SavePriceApiResponse SaveUniquePrice([FromUri] decimal finalPrice)
        {

            bool isSaved = TransferIdentifierService.GetInstance().SavePrice(finalPrice);
            if (isSaved)
            {
                return new SavePriceApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    StatusMessage = "The Final Price is successfully saved"
                };
            }
            else 
            {
                return new SavePriceApiResponse 
                {
                    StatusCode = HttpStatusCode.OK,
                    StatusMessage = "Failed to Save"
                };
            }
            
        }*/
    }
}
