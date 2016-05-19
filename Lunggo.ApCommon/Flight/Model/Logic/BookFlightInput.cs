using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Payment.Model;
using System;
using Lunggo.ApCommon.ProductBase.Model;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class BookFlightInput
    {
        public string Token { get; set; }
        public List<FlightPassenger> Passengers { get; set; }
        public Contact Contact { get; set; }
    }
}
