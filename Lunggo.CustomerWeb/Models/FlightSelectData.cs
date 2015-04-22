using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightSelectData
    {
        public string Message { get; set; }
        public string FareId { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public bool IsPassportRequired { get; set; }
        public bool IsBirthDateRequired { get; set; }
    }
}