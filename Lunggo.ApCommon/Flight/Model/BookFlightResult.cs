﻿using System;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Model
{
    public class BookFlightResult : ResultBase
    {
        public BookingStatusInfo Status { get; set; }
    }
}
