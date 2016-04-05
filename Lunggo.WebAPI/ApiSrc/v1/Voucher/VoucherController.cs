using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.v1.Voucher.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Voucher.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Voucher
{
    public class VoucherController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/voucher/check")]
        public CheckVoucherApiResponse CheckVoucher(CheckVoucherApiRequest request)
        {
            var apiResponse = VoucherLogic.CheckVoucher(request);
            return apiResponse;
        }
    }
}
