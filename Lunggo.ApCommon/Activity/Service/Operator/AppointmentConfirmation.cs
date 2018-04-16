using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public AppointmentConfirmationOutput ConfirmAppointment(AppointmentConfirmationInput input)
        {
            try
            {
                var rejectStatus = new List<string> { "CAOP", "CACU", "CAAD", "DENY", "TKTD" };
                var bookingDetail = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo });
                var status = bookingDetail.BookingDetail.BookingStatus;
                if(status == "CONF" || rejectStatus.Contains(status))
                {
                    return new AppointmentConfirmationOutput
                    {
                        IsSuccess = false
                    };
                }
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.Confirmed);
                InsertStatusHistoryToDb(input.RsvNo, BookingStatus.Confirmed);
                UpdateTicketNumberReservationDb(input.RsvNo);
                GeneratePayStepOperator(input.RsvNo);
                var activityDetail = GetActivityDetailFromDb(new GetDetailActivityInput { ActivityId = bookingDetail.BookingDetail.ActivityId });
                if(activityDetail.ActivityDetail.IsInstantConfirmation == true)
                {
                    var statusTicket = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo });
                    var activityQueue = QueueService.GetInstance().GetQueueByReference("ActivityEVoucherAndInvoice");
                    activityQueue.AddMessage(new CloudQueueMessage(input.RsvNo));
                    UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.Ticketed);
                    InsertStatusHistoryToDb(input.RsvNo, BookingStatus.Ticketed);
                }                                             
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
                var rejectStatus = new List<string> { "CAOP", "CACU", "CAAD", "DENY", "CONF" };
                var status = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo }).BookingDetail.BookingStatus;
                if (status == "CANC" || rejectStatus.Contains(status))
                {
                    return new AppointmentConfirmationOutput
                    {
                        IsSuccess = false
                    };
                }
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.Cancelled);
                InsertStatusHistoryToDb(input.RsvNo, BookingStatus.Cancelled);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            }
            catch
            {
                return new AppointmentConfirmationOutput { IsSuccess = false };
            }
        }

        public AppointmentConfirmationOutput DenyAppointment(AppointmentConfirmationInput input)
        {
            try
            {
                var rejectStatus = new List<string> { "CAOP", "CACU", "CAAD", "CONF" };
                var status = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo }).BookingDetail.BookingStatus;
                if (status == "DENY" || rejectStatus.Contains(status))
                {
                    return new AppointmentConfirmationOutput
                    {
                        IsSuccess = false
                    };
                }
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.Denied);
                InsertStatusHistoryToDb(input.RsvNo, BookingStatus.Denied);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            }
            catch
            {
                return new AppointmentConfirmationOutput { IsSuccess = false };
            }
        }



        public AppointmentConfirmationOutput CancelAppointmentByOperator(AppointmentConfirmationInput input)
        {
            try
            {
                var cancel = new List<string> { "CAOP", "CACU", "CAAD", "DENY" };
                var status = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo }).BookingDetail.BookingStatus;
                if (cancel.Contains(status))
                {
                    return new AppointmentConfirmationOutput
                    {
                        IsSuccess = false
                    };
                }
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.CancelledByOperator);
                InsertStatusHistoryToDb(input.RsvNo, BookingStatus.CancelledByOperator);
                InsertRefundAmountCancelByOperatorToDb(input.RsvNo);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            }
            catch
            {
                return new AppointmentConfirmationOutput { IsSuccess = false };
            }
        }

        public AppointmentConfirmationOutput CancelAppointmentByAdmin(AppointmentConfirmationInput input)
        {
            try
            {
                var cancel = new List<string> { "CAOP", "CACU", "CAAD", "DENY" };
                var status = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo }).BookingDetail.BookingStatus;
                if (cancel.Contains(status))
                {
                    return new AppointmentConfirmationOutput
                    {
                        IsSuccess = false
                    };
                }
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.CancelledByAdmin);
                InsertStatusHistoryToDb(input.RsvNo, BookingStatus.CancelledByAdmin);
                InsertRefundAmountOperator(input.RsvNo);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            }
            catch
            {
                return new AppointmentConfirmationOutput { IsSuccess = false };
            }
        }


        public AppointmentConfirmationOutput CancelAppointmentByCustomer(AppointmentConfirmationInput input)
        {
            try
            {
                var cancel = new List<string> { "CAOP", "CACU", "CAAD", "DENY" };
                var status = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo }).BookingDetail.BookingStatus;
                if (cancel.Contains(status))
                {
                    return new AppointmentConfirmationOutput
                    {
                        IsSuccess = false
                    };
                }
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.CancelledByCustomer);
                InsertStatusHistoryToDb(input.RsvNo, BookingStatus.CancelledByCustomer);
                InsertRefundAmountOperator(input.RsvNo);
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
                var rejectStatus = new List<string> { "CAOP", "CACU", "CAAD", "DENY", "CONF" };
                var status = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo }).BookingDetail.BookingStatus;
                if (status == "FORW" || rejectStatus.Contains(status))
                {
                    return new AppointmentConfirmationOutput
                    {
                        IsSuccess = false
                    };
                }
                UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.ForwardedToOperator);
                InsertStatusHistoryToDb(input.RsvNo, BookingStatus.ForwardedToOperator);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            }
            catch
            {
                return new AppointmentConfirmationOutput { IsSuccess = false };
            }
        }
        
    }
}