using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Voucher.Model;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;

namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Logic
{
    public partial class VoucherLogic
    {
        public static CheckVoucherApiResponse CheckVoucher(CheckVoucherApiRequest request)
        {
            var service = CampaignService.GetInstance();
            var voucher = new VoucherRequest
            {
                Email = request.Email,
                Price = request.Price,
                VoucherCode = request.Code
            };
            var response = service.ValidateVoucherRequest(voucher);
            var apiResponse = AssembleApiResponse(response, request);
            return apiResponse;
        }

        private static CheckVoucherApiResponse AssembleApiResponse(VoucherResponse response, CheckVoucherApiRequest request)
        {
            if (response.UpdateStatus == "Success")
                return new CheckVoucherApiResponse
                {
                    Discount = response.TotalDiscount,
                    DisplayName = response.CampaignVoucher.DisplayName,
                    StatusCode = HttpStatusCode.OK,
                    StatusMessage = response.UpdateStatus + ".",
                    OriginalRequest = request
                };
            else
                return new CheckVoucherApiResponse
                {
                    Discount = response.TotalDiscount,
                    DisplayName = response.CampaignVoucher.DisplayName,
                    StatusCode = HttpStatusCode.Accepted,
                    StatusMessage = response.UpdateStatus + ".",
                    OriginalRequest = request
                };
        }
    }
}