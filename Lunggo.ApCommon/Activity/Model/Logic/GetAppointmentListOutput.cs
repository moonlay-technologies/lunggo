using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetAppointmentListOutput
    {
        public List<AppointmentDetail> Appointments { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
