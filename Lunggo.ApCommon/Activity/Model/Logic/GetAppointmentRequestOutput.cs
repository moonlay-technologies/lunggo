using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetAppointmentRequestOutput
    {
        public List<AppointmentDetail> Appointments { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool MustUpdate { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
