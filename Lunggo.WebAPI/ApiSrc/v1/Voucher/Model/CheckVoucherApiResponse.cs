namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Model
{
    public class CheckVoucherApiResponse
    {
        public decimal Discount { get; set; }
        public string ValidationStatus { get; set; }
        public CheckVoucherApiRequest OriginalRequest { get; set; }
    }
}