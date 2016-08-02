using System.Net;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public partial class VoucherLogic
    {
        public static ApiResponseBase CheckVoucher(CheckVoucherApiRequest request)
        {
            if (IsValid(request))
            {
                var service = CampaignService.GetInstance();
                var response = service.ValidateVoucherRequest(request.RsvNo, request.DiscountCode);
                var apiResponse = AssembleApiResponse(response);
                return apiResponse;
            }
            else
            {
                return new CheckVoucherApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPVCH01"
                };
            }
        }

        private static bool IsValid(CheckVoucherApiRequest request)
        {
            return
                request != null &&
                request.DiscountCode != null &&
                request.RsvNo != null;
        }

        private static ApiResponseBase AssembleApiResponse(VoucherResponse response)
        {
            switch (response.VoucherStatus)
            {
                case VoucherStatus.Success:
                    return new CheckVoucherApiResponse
                    {
                        Discount = response.TotalDiscount,
                        DisplayName = response.Discount.DisplayName,
                        StatusCode = HttpStatusCode.OK
                    };
                case VoucherStatus.CampaignHasEnded:
                case VoucherStatus.CampaignInactive:
                case VoucherStatus.CampaignNotStartedYet:
                case VoucherStatus.VoucherNotFound:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPVCH02"
                    };
                case VoucherStatus.NoVoucherRemaining:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPVCH03"
                    };
                case VoucherStatus.BelowMinimumSpend:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPVCH04"
                    };
                case VoucherStatus.EmailNotEligible:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPVCH05"
                    };
                case VoucherStatus.VoucherAlreadyUsed:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPVCH06"
                    };
                case VoucherStatus.ReservationNotFound:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERPVCH07"
                    };
                default:
                    return ApiResponseBase.Error500();
            }
        }
    }
}