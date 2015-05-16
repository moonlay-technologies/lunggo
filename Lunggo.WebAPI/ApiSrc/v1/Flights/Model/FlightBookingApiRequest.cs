using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookingApiRequest
    {
        public string FareId { get; set; }
        public bool CanHold { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public bool PassportReq { get; set; }
        public string PassengerDataJson { get; set; }
        public List<PassengerData> PassengerData { get; set; }
        //TODO FLIGHT : Booking Payment
    }

    public class PassengerData
    {
        public PassengerType Type { get; set; }
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string IdNumber { get; set; }
        public DateTime PassportExpiryDate { get; set; }
        public string Country { get; set; }
    }
}