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
        public static ApiResponseBase CheckMethodDiscount(CheckMethodDiscountApiRequest request)
        {
            if (request.RsvNo == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPMP01"
                };
            }
            var binDiscount = CampaignService.GetInstance().CheckMethodDiscount(request.RsvNo, request.VoucherCode);
            var apiResponse = AssembleApiResponse(binDiscount);
            return apiResponse;
        }
    }
}