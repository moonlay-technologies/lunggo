using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public AppointmentConfirmationOutput ConfirmAppointment(AppointmentConfirmationInput input)
        {
            try
            {
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.Confirmed);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            }
            catch
            {
                return new AppointmentConfirmationOutput { IsSuccess = false };
            }
        }

        public AppointmentConfirmationOutput DeclineAppointment(AppointmentConfirmationInput input)
        {
            try
            {
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.Cancelled);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            }
            catch
            {
                return new AppointmentConfirmationOutput { IsSuccess = false };
            }
        }

        public AppointmentConfirmationOutput ForwardAppointment(AppointmentConfirmationInput input)
        {
            try
            {
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.ForwardedToOperator);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            }
            catch
            {
                return new AppointmentConfirmationOutput { IsSuccess = false };
            }
        }
        
    }
}