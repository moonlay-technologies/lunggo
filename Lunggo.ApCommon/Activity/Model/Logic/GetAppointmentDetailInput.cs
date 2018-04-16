using System;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetAppointmentDetailInput
    {
        public long? ActivityId { get; set; }
        public DateTime Date { get; set; }
        public string Session { get; set; }
    }
}
