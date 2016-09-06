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
            var binDiscount = CampaignService.GetInstance().CheckBinDiscount(request.RsvNo, request.Bin, request.HashedPan, request.VoucherCode);
            var apiResponse = AssembleApiRspn(binDiscount);
            return apiResponse;
        }

        private static CheckBinDiscountResponse AssembleApiRspn(BinDiscount binDiscount)
        {
            if (binDiscount != null)
                return new CheckBinDiscountResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    DiscountAmount = binDiscount.Amount,
                    DiscountName = binDiscount.DisplayName,
                    ReplaceOriginalDiscount = true
                };
            else
            {
                return new CheckBinDiscountResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    DiscountAmount = 0M
                };
            }
        }
    }
}