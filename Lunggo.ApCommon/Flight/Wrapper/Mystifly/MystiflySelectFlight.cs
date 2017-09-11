using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using FareType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.FareType;
using FlightSegment = Lunggo.ApCommon.Flight.Model.FlightSegment;
using PassengerType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.PassengerType;

namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override bool SelectFlight(FlightItinerary itin)
        {
            return true;
        }
    }
}