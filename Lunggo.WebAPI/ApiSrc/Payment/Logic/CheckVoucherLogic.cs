using System.Net;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Account.Service;
using System;
using System.Web;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public partial class VoucherLogic
    {
        public static ApiResponseBase CheckVoucher(CheckVoucherApiRequest request)
        {
            if (IsValid(request))
            {
                var response = new PaymentService().ValidateVoucherRequest(request.RsvNo, request.DiscountCode);                
                if(request.DiscountCode == "REFERRALCREDIT")
                {
                    var userId = HttpContext.Current.User.Identity.GetId();
                    var referral = AccountService.GetInstance().GetReferral(userId);
                    if(referral == null)
                    {
                        response.VoucherStatus = VoucherStatus.ProductNotEligible;
                    }
                    else if (referral.ReferralCredit <= 0M)
                    {
                        response.VoucherStatus = VoucherStatus.NoBudgetRemaining;
                    }
                    else if (DateTime.UtcNow > referral.ExpDate)
                    {
                        response.VoucherStatus = VoucherStatus.NoVoucherRemaining;
                    }
                    else if (referral.ReferralCredit < response.TotalDiscount)
                    {
                        response.TotalDiscount = referral.ReferralCredit;
                    }
                }
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
                case VoucherStatus.ProductNotEligible:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPVCH08"
                    };
                case VoucherStatus.NoBudgetRemaining:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPVCH09"
                    };
                case VoucherStatus.ReservationNotEligible:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPVCH10"
                    };
                case VoucherStatus.PlatformNotEligible:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPVCH11"
                    };
                default:
                    return ApiResponseBase.Error500();
            }
        }
    }
}