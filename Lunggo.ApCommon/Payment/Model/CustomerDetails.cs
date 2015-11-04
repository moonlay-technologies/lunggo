using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class CustomerDetails
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("billing_address")]
        public AddressingDetails BillingAddress { get; set; }
        [JsonProperty("shipping_address")]
        public AddressingDetails ShippingAddress { get; set; }
    }
}
