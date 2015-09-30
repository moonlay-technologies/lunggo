using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Voucher;
using Lunggo.WebAPI.ApiSrc.v1.Voucher.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Logic
{
    public partial class VoucherLogic
    {
        public static CheckVoucherApiResponse CheckVoucher(CheckVoucherApiRequest request)
        {
            var service = VoucherService.GetInstance();
            var discount = service.CheckVoucherDiscount(request.Token, request.Code, request.Email);
            var apiResponse = AssembleApiResponse(discount, request);
            return apiResponse;
        }

        private static decimal PreprocessServiceRequest(CheckVoucherApiRequest request)
        {
            var voucher = VoucherService.GetInstance();
            return voucher.CheckVoucherDiscount(request.Token, request.Code, request.Email);
        }

        private static CheckVoucherApiResponse AssembleApiResponse(decimal discount, CheckVoucherApiRequest request)
        {
            return new CheckVoucherApiResponse
            {
                Discount = discount,
                OriginalRequest = request
            };
        }
    }
}