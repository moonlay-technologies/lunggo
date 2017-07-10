using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model
{
    public class TiketCheckoutCustomerResponse : TiketBaseResponse
    {
        [JsonProperty("currProfileArr", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrProfileArr { get; set; }
        [JsonProperty("currProfileId", NullValueHandling = NullValueHandling.Ignore)]
        public CurProfileArr CurrProfileId { get; set; }
        [JsonProperty("travellerProfileArr", NullValueHandling = NullValueHandling.Ignore)]
        public TravellerProfile TravellerProfileArr { get; set; }
        [JsonProperty("contactProfileArr", NullValueHandling = NullValueHandling.Ignore)]
        public ContactProfile ContactProfileArr { get; set; }
        [JsonProperty("statusClass", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusClass { get; set; }
        [JsonProperty("next", NullValueHandling = NullValueHandling.Ignore)]
        public string Next { get; set; }
        [JsonProperty("SideArr", NullValueHandling = NullValueHandling.Ignore)]
        public SideArr SideArr { get; set; }
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

    public class CurProfileArr
    {
        [JsonProperty("accountid", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountId { get; set; }
        [JsonProperty("accountfirstname", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountFirstName { get; set; }
        [JsonProperty("accountlastname", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountLastName { get; set; }
        [JsonProperty("accountmobile", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountmobile { get; set; }
        [JsonProperty("accountsalutationname", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountsalutationname { get; set; }
        [JsonProperty("accountphone", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountphone { get; set; }
        [JsonProperty("accountusername", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountusername { get; set; }
        [JsonProperty("profileid", NullValueHandling = NullValueHandling.Ignore)]
        public string Profileid { get; set; }
        [JsonProperty("Name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("addresscountry", NullValueHandling = NullValueHandling.Ignore)]
        public string Addresscountry { get; set; }
        [JsonProperty("addressaddress1", NullValueHandling = NullValueHandling.Ignore)]
        public string Address1 { get; set; }
        [JsonProperty("addressaddress2", NullValueHandling = NullValueHandling.Ignore)]
        public string Address2 { get; set; }
        [JsonProperty("addresskabupaten", NullValueHandling = NullValueHandling.Ignore)]
        public string Addresskabupaten { get; set; }
        [JsonProperty("addressprovince", NullValueHandling = NullValueHandling.Ignore)]
        public string Addressprovince { get; set; }
        [JsonProperty("addresszipcode", NullValueHandling = NullValueHandling.Ignore)]
        public string Addresszipcode { get; set; }
        [JsonProperty("accountcreated", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountcreated { get; set; }
        [JsonProperty("accountpassword", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountpassword { get; set; }
        [JsonProperty("accountsource", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountsource { get; set; }
        [JsonProperty("photo", NullValueHandling = NullValueHandling.Ignore)]
        public string Photo { get; set; }
        [JsonProperty("accountprofilemodified", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountprofilemodified { get; set; }
        [JsonProperty("accountbirthdate", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountbirthdate { get; set; }
        [JsonProperty("accountgender", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountgender { get; set; }
        [JsonProperty("accountidcard", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountidcard { get; set; }
    }

    public class TravellerProfile
    {
        [JsonProperty("accountsalutationname", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountsalutationname { get; set; }
        [JsonProperty("accountfirstname", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountfirstname { get; set; }
        [JsonProperty("accountlastname", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountlastname { get; set; }
        [JsonProperty("accountphone", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountphone { get; set; }
    }

    public class ContactProfile
    {
        [JsonProperty("accountsalutationname", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountsalutationname { get; set; }
        [JsonProperty("accountfirstname", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountfirstname { get; set; }
        [JsonProperty("accountlastname", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountlastname { get; set; }
        [JsonProperty("accountusername", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountusername { get; set; }
        [JsonProperty("accountphone", NullValueHandling = NullValueHandling.Ignore)]
        public string Accountphone { get; set; }
    }

    public class SideArr
    {
        [JsonProperty("currBookingArr", NullValueHandling = NullValueHandling.Ignore)]
        public CurBookingArr CurrBookingArr { get; set; }
    }


    public class CurBookingArr
    {
        [JsonProperty("detailid", NullValueHandling = NullValueHandling.Ignore)]
        public string Detailid { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("mastername", NullValueHandling = NullValueHandling.Ignore)]
        public string Mastername { get; set; }
        [JsonProperty("detailname", NullValueHandling = NullValueHandling.Ignore)]
        public string Detailname { get; set; }
        [JsonProperty("contactperson", NullValueHandling = NullValueHandling.Ignore)]
        public string Contactperson { get; set; }
        [JsonProperty("lastupdatecontactperson", NullValueHandling = NullValueHandling.Ignore)]
        public string Lastupdatecontactperson { get; set; }
        [JsonProperty("ordertype", NullValueHandling = NullValueHandling.Ignore)]
        public string Ordertype { get; set; }
        [JsonProperty("hotelname", NullValueHandling = NullValueHandling.Ignore)]
        public string Hotelname { get; set; }
        [JsonProperty("nights", NullValueHandling = NullValueHandling.Ignore)]
        public string Nights { get; set; }
        [JsonProperty("checkinfirst", NullValueHandling = NullValueHandling.Ignore)]
        public string Checkinfirst { get; set; }
        [JsonProperty("checkinlast", NullValueHandling = NullValueHandling.Ignore)]
        public string Checkinlast { get; set; }
        [JsonProperty("adult", NullValueHandling = NullValueHandling.Ignore)]
        public string Adult { get; set; }
        [JsonProperty("child", NullValueHandling = NullValueHandling.Ignore)]
        public string Child { get; set; }
        [JsonProperty("rooms", NullValueHandling = NullValueHandling.Ignore)]
        public string Rooms { get; set; }
        [JsonProperty("filename", NullValueHandling = NullValueHandling.Ignore)]
        public string Filename { get; set; }
        [JsonProperty("lastupdatecp", NullValueHandling = NullValueHandling.Ignore)]
        public string Lastupdatecp { get; set; }

        [JsonProperty("roomid", NullValueHandling = NullValueHandling.Ignore)]
        public string Roomid { get; set; }

        [JsonProperty("needprocess", NullValueHandling = NullValueHandling.Ignore)]
        public int Needprocess { get; set; }
    }

}
