using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public AppointmentConfirmationOutput ConfirmAppointment(AppointmentConfirmationInput input)
        {
            try
            {
                var status = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo }).BookingDetail.BookingStatus;
                if(status == "CONF")
                {
                    return new AppointmentConfirmationOutput
                    {
                        IsSuccess = false
                    };
                }
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.Confirmed);
                UpdateTicketNumberReservationDb(input.RsvNo);
                var activityQueue = QueueService.GetInstance().GetQueueByReference("ActivityEVoucherAndInvoice");               
                activityQueue.AddMessage(new CloudQueueMessage(input.RsvNo));
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