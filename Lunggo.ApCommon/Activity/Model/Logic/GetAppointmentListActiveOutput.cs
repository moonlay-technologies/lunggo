using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetAppointmentListActiveOutput
    {
        public List<AppointmentList> Appointments { get; set; }
        public bool MustUpdate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
