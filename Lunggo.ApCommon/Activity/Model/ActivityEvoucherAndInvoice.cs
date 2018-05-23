﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model
{
    public class ActivityETicketAndInvoiceDataForEmailAndPdf
    {
        public string CartId { get; set; }
        public ActivityETicketAndInvoiceData ActivityETicketAndInvoice { get; set; }
    }

    public class ActivityETicketAndInvoiceData
    {
        public TrxList TrxList { get; set; }
        public List<ActivityReservationForDisplay> ActivityReservations { get; set; }
    }

    public class ActivityEVoucher
    {
        public BookingDetail BookingDetail { get; set; }
        public ActivityReservationForDisplay ActivityReservation { get; set; }
    }
}
