using System;
namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Model
{
    public class CheckVoucherApiRequest
    {
        public String Code { get; set; }
        public String Email { get; set; }
        public decimal Price { get; set; }
    }
}