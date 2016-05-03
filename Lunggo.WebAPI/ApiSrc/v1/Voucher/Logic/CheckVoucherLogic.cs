using System.Net;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.WebAPI.ApiSrc.v1.Voucher.Model;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;

namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Logic
{
    public partial class VoucherLogic
    {
        public static CheckVoucherApiResponse CheckVoucher(CheckVoucherApiRequest request)
        {
            try
            {
                if (IsValid(request))
                {
                    var service = CampaignService.GetInstance();
                    var voucher = new ValidateVoucherRequest
                    {
                        Email = request.Email,
                        Token = request.Token,
                        VoucherCode = request.Code
                    };
                    var response = service.ValidateVoucherRequest(voucher);
                    var apiResponse = AssembleApiResponse(response);
                    return apiResponse;
                }
                else
                {
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERVCHE01"
                    };
                }
            }
            catch
            {
                return new CheckVoucherApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERVCHE99"
                };
            }
        }

        private static bool IsValid(CheckVoucherApiRequest request)
        {
            return
                request != null &&
                request.Code != null &&
                request.Email != null &&
                request.Token != null;
        }

        private static CheckVoucherApiResponse AssembleApiResponse(VoucherResponse response)
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
                        ErrorCode = "ERVCHE02"
                    };
                case VoucherStatus.NoVoucherRemaining:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERVCHE03"
                    };
                case VoucherStatus.BelowMinimumSpend:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERVCHE04"
                    };
                case VoucherStatus.EmailNotEligible:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERVCHE05"
                    };
                case VoucherStatus.VoucherAlreadyUsed:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERVCHE06"
                    };
                default:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERVCHE99"
                    };
            }
        }
    }
}