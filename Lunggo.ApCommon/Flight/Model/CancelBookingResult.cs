﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class CancelBookingResult : ResultBase
    {
        public string BookingId { get; set; }
    }
}
