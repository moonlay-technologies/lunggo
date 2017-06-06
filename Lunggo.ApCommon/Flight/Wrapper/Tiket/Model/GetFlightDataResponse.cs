using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket.Model
{
    public class GetFlightDataResponse : TiketBaseResponse
    {
        [JsonProperty("required", NullValueHandling = NullValueHandling.Ignore)]
        public RequireData Required { get; set; }
        [JsonProperty("departures", NullValueHandling = NullValueHandling.Ignore)]
        public Result Departure { get; set; }
    }

    public class RequireData
    {
        [JsonProperty("separator", NullValueHandling = NullValueHandling.Ignore)]
        public Details Separator { get; set; }
        [JsonProperty("conSalutation", NullValueHandling = NullValueHandling.Ignore)]
        public Details ConSalutation { get; set; }
        [JsonProperty("conFirstName", NullValueHandling = NullValueHandling.Ignore)]
        public Details ConFirstName { get; set; }
        [JsonProperty("conLastName", NullValueHandling = NullValueHandling.Ignore)]
        public Details ConLastName { get; set; }
        [JsonProperty("conPhone", NullValueHandling = NullValueHandling.Ignore)]
        public Details ConPhone { get; set; }
        [JsonProperty("separator_adult1", NullValueHandling = NullValueHandling.Ignore)]
        public Details Separator_adult1 { get; set; }
        [JsonProperty("firstnamea1", NullValueHandling = NullValueHandling.Ignore)]
        public Details Firstnamea1 { get; set; }
        [JsonProperty("lastnamea1", NullValueHandling = NullValueHandling.Ignore)]
        public Details Lastnamea1 { get; set; }
        [JsonProperty("ida1", NullValueHandling = NullValueHandling.Ignore)]
        public Details Ida1 { get; set; }
        [JsonProperty("titlea1", NullValueHandling = NullValueHandling.Ignore)]
        public Details Titlea1 { get; set; }
        [JsonProperty("birthdatea1", NullValueHandling = NullValueHandling.Ignore)]
        public Details Birthdatea1 { get; set; }
        [JsonProperty("passportnationalitya1", NullValueHandling = NullValueHandling.Ignore)]
        public Details Passportnationalitya1 { get; set; }
        [JsonProperty("dcheckinbaggagea11", NullValueHandling = NullValueHandling.Ignore)]
        public BaggageCapacity DeptCheckinBaggage { get; set; }
    }

    public class Details
    {
        [JsonProperty("mandatory", NullValueHandling = NullValueHandling.Ignore)]
        public int Mandatory { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("example", NullValueHandling = NullValueHandling.Ignore)]
        public string Example { get; set; }
        [JsonProperty("FieldText", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldText { get; set; }
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
        //[JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
        //public List<Resource> Resource { get; set; }
    }


    public class BaggageCapacity
    {
        [JsonProperty("mandatory", NullValueHandling = NullValueHandling.Ignore)]
        public int Mandatory { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("example", NullValueHandling = NullValueHandling.Ignore)]
        public string Example { get; set; }
        [JsonProperty("FieldText", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldText { get; set; }
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
        [JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
        public List<Resource> Resource { get; set; }
    }

    public class Resource
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class PasportRequired
    {
        [JsonProperty("mandatory", NullValueHandling = NullValueHandling.Ignore)]
        public string Mandatory { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("example", NullValueHandling = NullValueHandling.Ignore)]
        public string Example { get; set; }
        [JsonProperty("FieldText", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldText { get; set; }
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
        //[JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
        //public string Resource { get; set; }
    }

}
