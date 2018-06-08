using Lunggo.ApCommon.Activity.Model.Logic;
using System;
using System.Linq;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetAppointmentRequestOutput GetAppointmentRequest(GetAppointmentRequestInput input)
        {
            var appointmentRequest = GetAppointmentRequestFromDb(input);
            return appointmentRequest;
        }
        
    }
}