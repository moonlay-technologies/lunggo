using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket.Model
{
    public class HistoryOrderResponse : TiketBaseResponse
    {
        
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public HistoryOrder OrderData { get; set; }
    }

    public class HistoryOrder
    {
        [JsonProperty("order_id", NullValueHandling = NullValueHandling.Ignore)]
        public string order_id { get; set; }
        [JsonProperty("payment_status", NullValueHandling = NullValueHandling.Ignore)]
        public string payment_status { get; set; }
        [JsonProperty("total_customer_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal total_customer_price { get; set; }
        [JsonProperty("mobile_phone", NullValueHandling = NullValueHandling.Ignore)]
        public string mobile_phone { get; set; }
        [JsonProperty("all_order_type", NullValueHandling = NullValueHandling.Ignore)]
        public string all_order_type { get; set; }
        [JsonProperty("order__cart_detail", NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderCartDetail> OrderCartDetail { get; set; }
        [JsonProperty("order__payment", NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderPayment> OrderPayment { get; set; }
    }



    public class OrderCartDetail
    {
        [JsonProperty("order_detail_id", NullValueHandling = NullValueHandling.Ignore)]
        public string order_detail_id { get; set; }
        [JsonProperty("order_type", NullValueHandling = NullValueHandling.Ignore)]
        public string order_type { get; set; }
        [JsonProperty("order_name", NullValueHandling = NullValueHandling.Ignore)]
        public string order_name { get; set; }
        [JsonProperty("order_name_detail", NullValueHandling = NullValueHandling.Ignore)]
        public string order_name_detail { get; set; }
        [JsonProperty("customer_currency", NullValueHandling = NullValueHandling.Ignore)]
        public string customer_currency { get; set; }
        [JsonProperty("customer_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal customer_price { get; set; }
        [JsonProperty("total_ticket", NullValueHandling = NullValueHandling.Ignore)]
        public string total_ticket { get; set; }
        [JsonProperty("detail", NullValueHandling = NullValueHandling.Ignore)]
        public CartDetail CartDetail { get; set; }
        [JsonProperty("passanger", NullValueHandling = NullValueHandling.Ignore)]
        public List<HistoryPassanger> HistoryPassanger { get; set; }
    }

    public class CartDetail
    {
        [JsonProperty("flight_number", NullValueHandling = NullValueHandling.Ignore)]
        public string flight_number { get; set; }
        [JsonProperty("trip", NullValueHandling = NullValueHandling.Ignore)]
        public string trip { get; set; }
        [JsonProperty("airlines_name", NullValueHandling = NullValueHandling.Ignore)]
        public string airlines_name { get; set; }
        [JsonProperty("departure_city", NullValueHandling = NullValueHandling.Ignore)]
        public string departure_city { get; set; }
        [JsonProperty("departure_time", NullValueHandling = NullValueHandling.Ignore)]
        public string departure_time { get; set; }

        [JsonProperty("arrival_city", NullValueHandling = NullValueHandling.Ignore)]
        public string arrival_city { get; set; }
        [JsonProperty("arrival_time", NullValueHandling = NullValueHandling.Ignore)]
        public string arrival_time { get; set; }
        [JsonProperty("ticket_class", NullValueHandling = NullValueHandling.Ignore)]
        public string ticket_class { get; set; }
        [JsonProperty("booking_code", NullValueHandling = NullValueHandling.Ignore)]
        public string booking_code { get; set; }
        [JsonProperty("count_adult", NullValueHandling = NullValueHandling.Ignore)]
        public int count_adult { get; set; }
        [JsonProperty("count_child", NullValueHandling = NullValueHandling.Ignore)]
        public int count_child { get; set; }
        [JsonProperty("count_infant", NullValueHandling = NullValueHandling.Ignore)]
        public int count_infant { get; set; }
        [JsonProperty("contact_title", NullValueHandling = NullValueHandling.Ignore)]
        public string contact_title { get; set; }
        [JsonProperty("contact_first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string contact_first_name { get; set; }
        [JsonProperty("contact_phone", NullValueHandling = NullValueHandling.Ignore)]
        public string contact_phone { get; set; }
        [JsonProperty("ticket_status", NullValueHandling = NullValueHandling.Ignore)]
        public string ticket_status { get; set; }
        [JsonProperty("depart_airport", NullValueHandling = NullValueHandling.Ignore)]
        public string depart_airport { get; set; }
        [JsonProperty("arrival_airport", NullValueHandling = NullValueHandling.Ignore)]
        public string arrival_airport { get; set; }
    }

    public class HistoryPassanger
    {
        [JsonProperty("passanger_baggage", NullValueHandling = NullValueHandling.Ignore)]
        public int passanger_baggage { get; set; }
        [JsonProperty("passenger_name", NullValueHandling = NullValueHandling.Ignore)]
        public string passenger_name { get; set; }
        [JsonProperty("passenger_age_group", NullValueHandling = NullValueHandling.Ignore)]
        public string passenger_age_group { get; set; }
        [JsonProperty("passenger_id_number", NullValueHandling = NullValueHandling.Ignore)]
        public string passenger_id_number { get; set; }
    }

    public class OrderPayment
    {
        [JsonProperty("payment_currency", NullValueHandling = NullValueHandling.Ignore)]
        public string payment_currency { get; set; }
        [JsonProperty("payment_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal payment_amount { get; set; }
        [JsonProperty("transfer_date", NullValueHandling = NullValueHandling.Ignore)]
        public string transfer_date { get; set; }
        [JsonProperty("payment_source", NullValueHandling = NullValueHandling.Ignore)]
        public string payment_source { get; set; }
    }
}
