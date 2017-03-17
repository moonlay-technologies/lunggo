using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Model
{
    public class ReservationListModel
    {
        [JsonProperty("bookerId")]
        public string BookerId { get; set; }
        [JsonProperty("bookerName")]
        public string BookerName { get; set; }
        [JsonProperty("bookerMessageTitle")]
        public string BookerMessageTitle { get; set; }
        [JsonProperty("bookerMessageDescription")]
        public string BookerMessageDescription { get; set; }
        [JsonProperty("reservationList")]
        public ReservationList ReservationList { get; set; }
    }

    public class ReservationList
    {
        [JsonProperty("flights")]
        public List<FlightReservationForDisplay> Flights { get; set; }
        [JsonProperty("hotels")]
        public List<HotelReservationForDisplay> Hotels { get; set; }
    }
}
