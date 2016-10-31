using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelReservationForDisplay
    {
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("rsvTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RsvTime { get; set; }
        [JsonProperty("rsvStatus", NullValueHandling = NullValueHandling.Ignore)]
        public RsvDisplayStatus RsvDisplayStatus { get; set; }
        [JsonProperty("cancelType", NullValueHandling = NullValueHandling.Ignore)]
        public CancellationType CancellationType { get; set; }
        [JsonProperty("cancelTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CancellationTime { get; set; }
        [JsonProperty("payment", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentDetailsForDisplay Payment { get; set; }
        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact { get; set; }
        [JsonProperty("hotelDetail", NullValueHandling = NullValueHandling.Ignore)]
        public HotelDetailForDisplay HotelDetail { get; set; }
        [JsonProperty("pax", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public string DeviceId { get; set; }
    }
    public class HotelReservation : ReservationBase<HotelReservation>
    {
        public override ProductType Type
        {
            get { return ProductType.Hotel; }
        }

        public HotelDetail HotelDetails { get; set; }
    }
}
