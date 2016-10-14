using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Microsoft.SqlServer.Server;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class HotelContent
    {
        public int code;
        public Name name;
        public string accommodationTypeCode;
        public Address address;
        public List<string> boardCodes;
        public string categoryCode;
        public string categoryGroupCode;
        public string chainCode;
        public City city;
        public Coordinates coordinates;
        public Description description;
        public string destinationCode;
        public string email;
        public List<Facility> facilities;
        public List<interestPoints> InterestPoints;
        public string license;
        public List<Phone> phones;
        public string postalCode;
        public List<Room> rooms;
        public string S2C;
        public List<int> segmentCodes;
        public List<Terminal> terminals;
        public string web;
        public int zoneCode;
        public string countryCode;
        public List<Image> images;
    }

    public class Image
    {
        public int order;
        public string imageTypeCode;
        public string path;
    }

    public class Types
    {
        public string code { get; set; }
        public Description description { get; set; }
    }

    public class Coordinates
    {
        public decimal longitude;
        public decimal latitude;
    }

    public class Terminal
    {
        public int distance;
        public string terminalCode;
        public Description name;
        public Description description;
    }

    public class Room
    {
        public string characteristicCode;
        public string roomCode;
        public List<RoomStay> roomStays;
        public string roomType;
        public List<RoomFacility> roomFacilities;
    }

    public class RoomFacility
    {
        public int facilityCode;
        public int facilityGroupCode;
    }

    public class RoomStay
    {
        public string description;
        public int order;
        public string stayType;
    }

    public class Phone
    {
        public string phoneNumber;
        public string phoneType;
    }

    public class interestPoints
    {
        public int distance;
        public int facilityCode;
        public int facilityGroupCode;
        public int order;
        public string poiName;
    }

    public class Facility
    {
        public int facilityCode;
        public int facilityGroupCode;
        public bool indFee;
        public bool indYesOrNo;
        public int order;
        //additional from API
        //public int distance;
        //public int number;
    }

    public class Description
    {
        public string languageCode;
        public string content;
    }

    public class City
    {
        public string content;
    }

    public class Address
    {
        public string content;
    }

    public class Name
    {
        public string content;
    }

    public class BoardCodes
    {
        public string code; // not correct yet
    }
}
