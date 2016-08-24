using System.Net;
using System.Security.Principal;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase CheckBinDiscount(CheckBinDiscountApiRequest request)
        {
            if (request.RsvNo == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPBIN01"
                };
            }
            var binDiscount = CampaignService.GetInstance().CheckBinDiscount(request.RsvNo, request.CardNumber, request.DiscountCode);
            var apiResponse = AssembleApiRspn(binDiscount);
            return apiResponse;
        }

        private static CheckBinDiscountResponse AssembleApiRspn(BinDiscount binDiscount)
        {
            return new CheckBinDiscountResponse
            {
               DiscountAmount = binDiscount.Amount,
               DiscountName = binDiscount.DisplayName
            };
        }
    }
}