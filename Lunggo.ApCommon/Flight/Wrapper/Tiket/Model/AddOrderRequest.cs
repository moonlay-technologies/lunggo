using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket.Model
{
    public class AddOrderRequest
    {
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
        [JsonProperty("flight_id", NullValueHandling = NullValueHandling.Ignore)]
        public string FlightId { get; set; }
        [JsonProperty("ret_flight_id", NullValueHandling = NullValueHandling.Ignore)]
        public string RetFlightId { get; set; }
        [JsonProperty("lioncaptcha", NullValueHandling = NullValueHandling.Ignore)]
        public string Lioncaptcha { get; set; }
        [JsonProperty("lionsessionid", NullValueHandling = NullValueHandling.Ignore)]
        public string Lionsessionid { get; set; }
        [JsonProperty("child", NullValueHandling = NullValueHandling.Ignore)]
        public int Child { get; set; }
        [JsonProperty("adult", NullValueHandling = NullValueHandling.Ignore)]
        public int Adult { get; set; }
        [JsonProperty("conSalutation", NullValueHandling = NullValueHandling.Ignore)]
        public string ConSalutation { get; set; }
        [JsonProperty("conFirstName", NullValueHandling = NullValueHandling.Ignore)]
        public string ConFirstName { get; set; }
        [JsonProperty("conLastName", NullValueHandling = NullValueHandling.Ignore)]
        public string ConLastName { get; set; }
        [JsonProperty("conPhone", NullValueHandling = NullValueHandling.Ignore)]
        public string ConPhone { get; set; }
        [JsonProperty("conOtherPhone", NullValueHandling = NullValueHandling.Ignore)]
        public string ConOtherPhone { get; set; }
        [JsonProperty("conEmailAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string ConEmailAddress { get; set; }
        [JsonProperty("firstnamea1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> FirstNameAdult { get; set; }
        [JsonProperty("lastnamea1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> LastNameAdult { get; set; }
        [JsonProperty("birthdatea1", NullValueHandling = NullValueHandling.Ignore)]
        public List<DateTime> Birthdatea1 { get; set; }
        [JsonProperty("ida1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Ida1 { get; set; }
        [JsonProperty("titlea1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Titlea1 { get; set; }

         [JsonProperty("firstnamec1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> FirstNameChild{ get; set; }
        [JsonProperty("lastnamec1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> LastNameChild { get; set; }
        [JsonProperty("birthdatec1", NullValueHandling = NullValueHandling.Ignore)]
        public List<DateTime> BirthdateChild { get; set; }
        [JsonProperty("idc1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Idc1 { get; set; }
        [JsonProperty("titlec1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Titlec1 { get; set; }

        [JsonProperty("parenti1", NullValueHandling = NullValueHandling.Ignore)]
        public int Parenti1 { get; set; }
        [JsonProperty("firstnamei1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Firstnamei1 { get; set; }
        [JsonProperty("lastnamei1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Lastnamei1 { get; set; }
        [JsonProperty("birthdatec1", NullValueHandling = NullValueHandling.Ignore)]
        public List<DateTime> Birthdatei1 { get; set; }
        [JsonProperty("idi1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Idi1 { get; set; }

        //Additional Field
        [JsonProperty("passportnoa1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Passportnoa1 { get; set; }
        [JsonProperty("passportExpiryDatea1", NullValueHandling = NullValueHandling.Ignore)]
        public List<DateTime> PassportExpiryDatea1 { get; set; }
        [JsonProperty("passportissueddatea1", NullValueHandling = NullValueHandling.Ignore)]
        public List<DateTime> Passportissueddatea1 { get; set; }
        [JsonProperty("passportissuinga1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Passportissuinga1Idi1 { get; set; }
        [JsonProperty("passportnationalitya1", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Passportnationalitya1 { get; set; }

        //Additional For Tiger, Airasia and Mandala
        [JsonProperty("dcheckinbaggagea11", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Deptcheckinbaggagea11 { get; set; }
        [JsonProperty("dcheckinbaggagea11", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Deptcheckinbaggagec11 { get; set; }
        [JsonProperty("rcheckinbaggagea11", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Retcheckinbaggagea11 { get; set; }
        [JsonProperty("rcheckinbaggagec11", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Retcheckinbaggagec11 { get; set; }
    }
}
