using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightSelectData
    {
        public string token { get; set; }
        public FlightError? error { get; set; }
    }
}