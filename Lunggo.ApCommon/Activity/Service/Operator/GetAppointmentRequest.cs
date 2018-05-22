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
            var expiredAppointmentRequest = appointmentRequest.Appointments.Where(appointment => appointment.TimeLimit < DateTime.UtcNow).ToList();
            if(expiredAppointmentRequest.Count > 0)
            {
                var denyAppointment = expiredAppointmentRequest.Select(a => DenyAppointmentByOperator(new AppointmentConfirmationInput
                {
                    RsvNo = a.RsvNo
                })).ToList();
            }
            var activeAppointmentRequest = appointmentRequest.Appointments.Where(appointment => appointment.TimeLimit > DateTime.UtcNow).ToList();
            appointmentRequest.Appointments = activeAppointmentRequest;
            return appointmentRequest;
        }
        
    }
}