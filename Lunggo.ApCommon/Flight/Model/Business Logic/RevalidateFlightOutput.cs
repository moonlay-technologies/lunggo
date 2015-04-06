﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class RevalidateFlightOutput
    {
        public bool IsValid { get; set; }
        public FlightFareItinerary Itinerary { get; set; }
        public bool ReturnIsValid { get; set; }
        public FlightFareItinerary ReturnItinerary { get; set; }
    }
}
