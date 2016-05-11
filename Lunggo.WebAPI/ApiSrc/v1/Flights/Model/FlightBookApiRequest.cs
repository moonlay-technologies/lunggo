﻿using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.ProductBase.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookApiRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
        [JsonProperty("pax")]
        public List<Passenger> Passengers { get; set; }
        [JsonProperty("voucher")]
        public string DiscountCode { get; set; }
        [JsonProperty("lang")]
        public string Language { get; set; }
    }

    public class Passenger
    {
        [JsonProperty("type")]
        public PassengerType Type { get; set; }
        [JsonProperty("title")]
        public Title Title { get; set; }
        [JsonProperty("first")]
        public string FirstName { get; set; }
        [JsonProperty("last")]
        public string LastName { get; set; }
        [JsonProperty("dob")]
        public DateTime? BirthDate { get; set; }
        [JsonProperty("passportNo")]
        public string PassportNumber { get; set; }
        [JsonProperty("passportExp")]
        public DateTime? PassportExpiryDate { get; set; }
        [JsonProperty("passportCountry")]
        public string PassportCountry { get; set; }
        [JsonProperty("nationality")]
        public string Nationality { get; set; }
    }
}