using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket.Model
{
    public class OrderResponse : TiketBaseResponse
    {
        [JsonProperty("myorder", NullValueHandling = NullValueHandling.Ignore)]
        public MyOrder Myorder { get; set; }
        [JsonProperty("checkout", NullValueHandling = NullValueHandling.Ignore)]
        public string Checkout { get; set; }
    }

    public class MyOrder
    {
        [JsonProperty("order_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Order_id { get; set; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public List<Data> Data { get; set; }
        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public int Total { get; set; }
        [JsonProperty("total_tax", NullValueHandling = NullValueHandling.Ignore)]
        public int Total_tax { get; set; }
        [JsonProperty("total_without_tax", NullValueHandling = NullValueHandling.Ignore)]
        public int Total_without_tax { get; set; }
        [JsonProperty("count_installment", NullValueHandling = NullValueHandling.Ignore)]
        public int Count_installment { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public string Discount { get; set; }
        [JsonProperty("discount_amount", NullValueHandling = NullValueHandling.Ignore)]
        public string Discount_amount { get; set; }
    }

    public class Data
    {
        [JsonProperty("expire", NullValueHandling = NullValueHandling.Ignore)]
        public int expire { get; set; }
        [JsonProperty("order_expire_datetime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime order_expire_datetime { get; set; }
        [JsonProperty("uri", NullValueHandling = NullValueHandling.Ignore)]
        public string uri { get; set; }
        [JsonProperty("order_detail_id", NullValueHandling = NullValueHandling.Ignore)]
        public string order_detail_id { get; set; }
        [JsonProperty("order_type", NullValueHandling = NullValueHandling.Ignore)]
        public string order_type { get; set; }
        [JsonProperty("customer_price", NullValueHandling = NullValueHandling.Ignore)]
        public string customer_price { get; set; }
        [JsonProperty("order_name", NullValueHandling = NullValueHandling.Ignore)]
        public string order_name { get; set; }
        [JsonProperty("order_name_detail", NullValueHandling = NullValueHandling.Ignore)]
        public string order_name_detail { get; set; }
        [JsonProperty("order_detail_status", NullValueHandling = NullValueHandling.Ignore)]
        public string order_detail_status { get; set; }
        [JsonProperty("detail", NullValueHandling = NullValueHandling.Ignore)]
        public DetailOrder Detail { get; set; }
        [JsonProperty("order_photo", NullValueHandling = NullValueHandling.Ignore)]
        public string order_photo { get; set; }
        [JsonProperty("order_icon", NullValueHandling = NullValueHandling.Ignore)]
        public string order_icon { get; set; }
        [JsonProperty("tax_and_charge", NullValueHandling = NullValueHandling.Ignore)]
        public string tax_and_charge { get; set; }
        [JsonProperty("subtotal_and_charge", NullValueHandling = NullValueHandling.Ignore)]
        public string subtotal_and_charge { get; set; }
        [JsonProperty("delete_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string delete_uri { get; set; }
    }

    public class DetailOrder
    {
        [JsonProperty("order_detail_id", NullValueHandling = NullValueHandling.Ignore)]
        public string order_detail_id { get; set; }
        [JsonProperty("airlines_name", NullValueHandling = NullValueHandling.Ignore)]
        public string airlines_name { get; set; }
        [JsonProperty("flight_number", NullValueHandling = NullValueHandling.Ignore)]
        public string flight_number { get; set; }
        [JsonProperty("price_adult", NullValueHandling = NullValueHandling.Ignore)]
        public string price_adult { get; set; }
        [JsonProperty("price_child", NullValueHandling = NullValueHandling.Ignore)]
        public string price_child { get; set; }
        [JsonProperty("price_infant", NullValueHandling = NullValueHandling.Ignore)]
        public string price_infant { get; set; }
        [JsonProperty("flight_date", NullValueHandling = NullValueHandling.Ignore)]
        public string flight_date { get; set; }
        [JsonProperty("departure_time", NullValueHandling = NullValueHandling.Ignore)]
        public string departure_time { get; set; }
        [JsonProperty("arrival_time", NullValueHandling = NullValueHandling.Ignore)]
        public string arrival_time { get; set; }
        [JsonProperty("baggage_fee", NullValueHandling = NullValueHandling.Ignore)]
        public string baggage_fee { get; set; }
        [JsonProperty("departure_airport_name", NullValueHandling = NullValueHandling.Ignore)]
        public string departure_airport_name { get; set; }
        [JsonProperty("arrival_airport_name", NullValueHandling = NullValueHandling.Ignore)]
        public string arrival_airport_name { get; set; }
        [JsonProperty("passengers", NullValueHandling = NullValueHandling.Ignore)]
        public Passenger Passenger { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public int price { get; set; }
        [JsonProperty("breakdown_price", NullValueHandling = NullValueHandling.Ignore)]
        public List<BreakdownPrice> breakdown_price { get; set; }
    }

    public class Passenger
    {
        [JsonProperty("adult", NullValueHandling = NullValueHandling.Ignore)]
        public List<PassengerDetail> Adult { get; set; }
    }

    public class PassengerDetail
    {
        [JsonProperty("order_passenger_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Order_passenger_id { get; set; }
        [JsonProperty("order_detail_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Order_detail_id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string First_name { get; set; }
        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Last_name { get; set; }
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("id_number", NullValueHandling = NullValueHandling.Ignore)]
        public string Id_number { get; set; }
        [JsonProperty("birth_date", NullValueHandling = NullValueHandling.Ignore)]
        public string Birth_date { get; set; }
        [JsonProperty("adult_index", NullValueHandling = NullValueHandling.Ignore)]
        public string Adult_index { get; set; }
        [JsonProperty("passport_no", NullValueHandling = NullValueHandling.Ignore)]
        public string Passport_no { get; set; }
        [JsonProperty("passport_expiry", NullValueHandling = NullValueHandling.Ignore)]
        public string Passport_expiry { get; set; }
        [JsonProperty("passport_issuing_country", NullValueHandling = NullValueHandling.Ignore)]
        public string Passport_issuing_country { get; set; }
        [JsonProperty("passport_nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Passport_nationality { get; set; }
        [JsonProperty("check_in_baggage", NullValueHandling = NullValueHandling.Ignore)]
        public string Check_in_baggage { get; set; }
        [JsonProperty("check_in_baggage_size", NullValueHandling = NullValueHandling.Ignore)]
        public string Check_in_baggage_size { get; set; }
        [JsonProperty("passport_issued_date", NullValueHandling = NullValueHandling.Ignore)]
        public string Passport_issued_date { get; set; }
        [JsonProperty("birth_country", NullValueHandling = NullValueHandling.Ignore)]
        public string Birth_country { get; set; }
    }

    public class BreakdownPrice
    {
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public int Value { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }
}
