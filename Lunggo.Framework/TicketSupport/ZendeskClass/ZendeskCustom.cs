using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskCustom
    {

        [JsonProperty("time_spent")]
        public string TimeSpent { get; set; }
    }
}
