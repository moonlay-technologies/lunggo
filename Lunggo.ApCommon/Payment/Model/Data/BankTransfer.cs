using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class BankTransfer
    {
        [JsonProperty("fee")]
        public decimal TransferFee { get; set; }
    }
}