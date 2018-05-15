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
                VoucherStatus status;
                var response = !string.IsNullOrWhiteSpace(request.RsvNo)
                    ? new PaymentService().GetVoucherDiscount(request.RsvNo, request.DiscountCode, out status)
                    : new PaymentService().GetVoucherDiscountForCart(request.CartId, request.DiscountCode, out status);
                if (request.DiscountCode == "REFERRALCREDIT")
                {
                    var userId = HttpContext.Current.User.Identity.GetId();
                    var referral = AccountService.GetInstance().GetReferral(userId);
                    if (referral == null)
                    {
                        status = VoucherStatus.TermsConditionsNotEligible;
                    }
                    else if (referral.ReferralCredit <= 0M)
                    {
                        status = VoucherStatus.VoucherDepleted;
                    }
                    else if (DateTime.UtcNow > referral.ExpDate)
                    {
                        status = VoucherStatus.VoucherDepleted;
                    }
                    else if (referral.ReferralCredit < response.TotalDiscount)
                    {
                        response.TotalDiscount = referral.ReferralCredit;
                    }
                }
                var apiResponse = AssembleApiResponse(response, status);
                return apiResponse;
            }
            else
            {
                return new CheckVoucherApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
        }

        private static bool IsValid(CheckVoucherApiRequest request)
        {
            return
                request != null &&
                !string.IsNullOrWhiteSpace(request.DiscountCode) &&
                (!string.IsNullOrWhiteSpace(request.RsvNo) || !string.IsNullOrWhiteSpace(request.CartId));
        }

        private static ApiResponseBase AssembleApiResponse(VoucherDiscount discount, VoucherStatus status)
        {
            switch (status)
            {
                case VoucherStatus.Success:
                    return new CheckVoucherApiResponse
                    {
                        Discount = discount.TotalDiscount,
                        DisplayName = discount.Discount.DisplayName,
                        StatusCode = HttpStatusCode.OK
                    };
                case VoucherStatus.OutsidePeriod:
                case VoucherStatus.VoucherNotFound:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERR_INVALID_CODE"
                    };
                case VoucherStatus.VoucherDepleted:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERR_NO_LONGER_AVAILABLE"
                    };
                case VoucherStatus.BelowMinimumSpend:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERR_TRX_VALUE_NOT_ENOUGH"
                    };
                //case VoucherStatus.EmailNotEligible:
                //    return new CheckVoucherApiResponse
                //    {
                //        StatusCode = HttpStatusCode.Accepted,
                //        ErrorCode = "ERPVCH05"
                //    };
                //case VoucherStatus.VoucherDepleted:
                //    return new CheckVoucherApiResponse
                //    {
                //        StatusCode = HttpStatusCode.Accepted,
                //        ErrorCode = "ERPVCH06"
                //    };
                case VoucherStatus.TermsConditionsNotEligible:
                    return new CheckVoucherApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERR_TNC_NOT_FULFILLED"
                    };
                //case VoucherStatus.VoucherDepleted:
                //    return new CheckVoucherApiResponse
                //    {
                //        StatusCode = HttpStatusCode.Accepted,
                //        ErrorCode = "ERPVCH09"
                //    };
                //case VoucherStatus.TermsConditionsNotEligible:
                //    return new CheckVoucherApiResponse
                //    {
                //        StatusCode = HttpStatusCode.Accepted,
                //        ErrorCode = "ERPVCH10"
                //    };
                //case VoucherStatus.TermsConditionsNotEligible:
                //    return new CheckVoucherApiResponse
                //    {
                //        StatusCode = HttpStatusCode.Accepted,
                //        ErrorCode = "ERPVCH11"
                //    };
                case VoucherStatus.InternalError:
                default:
                    return ApiResponseBase.Error500();
            }
        }
    }
}