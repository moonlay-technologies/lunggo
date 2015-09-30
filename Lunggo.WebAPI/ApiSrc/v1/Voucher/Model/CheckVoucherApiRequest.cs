namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Model
{
    public class CheckVoucherApiRequest
    {
        public string Token { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
    }
}