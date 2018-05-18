﻿using System;
using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Collections.Generic;
using Lunggo.ApCommon.Notifications;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public AppointmentConfirmationOutput ConfirmAppointment(AppointmentConfirmationInput input)
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
                if(activityDetail.ActivityDetail.HasOperator)
                {
                    var statusTicket = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = input.RsvNo });
                    var activityQueue = QueueService.GetInstance().GetQueueByReference("ActivityEVoucherAndInvoice");
                    activityQueue.AddMessage(new CloudQueueMessage(input.RsvNo));
                    UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.Ticketed);
                    InsertStatusHistoryToDb(input.RsvNo, BookingStatus.Ticketed);
                }
                else
                {
                    UpdateActivityBookingStatusInDb(input.RsvNo, BookingStatus.Ticketing);
                    InsertStatusHistoryToDb(input.RsvNo, BookingStatus.Ticketing);
                }      
                var acceptanceQueue = QueueService.GetInstance().GetQueueByReference("activityacceptanceemail");
                acceptanceQueue.AddMessage(new CloudQueueMessage(input.RsvNo));
                var pushNotifConfirm = PushNotificationConfirmAppointment(input.RsvNo);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            
        }

        internal bool PushNotificationConfirmAppointment(string rsvNo)
        {
            var userId = GetUserIdByRsvNo(rsvNo);
            var reservation = GetReservation(rsvNo);
            var notifTitle = "Aktivitas telah di terima oleh operator";
            var notifData = new NotificationData();
            notifData.Function = "refreshMyBooking";
            notifData.Status = "OK";
            var notifBody = "Aktivitas dengan nama \"" + reservation.ActivityDetails.Name + "\" pada tanggal " +
                            reservation.DateTime.Date.Value.Date.ToShortDateString() + " telah di terima oleh operator";
            var notifResult = NotificationService.GetInstance().SendNotificationsCustomer(notifTitle, notifBody, userId, notifData);
            return notifResult;
        }

        public AppointmentConfirmationOutput DeclineAppointment(AppointmentConfirmationInput input)
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

        public AppointmentConfirmationOutput DenyAppointment(AppointmentConfirmationInput input)
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
                var rejectionQueue = QueueService.GetInstance().GetQueueByReference("activityrejectionemail");
                rejectionQueue.AddMessage(new CloudQueueMessage(input.RsvNo));
                var pushNotifDeny = PushNotificationDenyAppointment(input.RsvNo);
                return new AppointmentConfirmationOutput { IsSuccess = true };
            
            
        }

        internal bool PushNotificationDenyAppointment(string rsvNo)
        {
            var userId = GetUserIdByRsvNo(rsvNo);
            var reservation = GetReservation(rsvNo);
            var notifTitle = "Aktivitas telah ditolak";       
            var notifData = new NotificationData();
            notifData.Function = "refreshMyBooking";
            notifData.Status = "OK";
            var notifBody = "Aktivitas dengan nama \"" + reservation.ActivityDetails.Name + "\" pada tanggal " +
                            reservation.DateTime.Date.Value.Date.ToShortDateString() + " telah di tolak";
            var notifResult = NotificationService.GetInstance().SendNotificationsCustomer(notifTitle, notifBody, userId, notifData);
            return notifResult;
        }



        public AppointmentConfirmationOutput CancelAppointmentByOperator(AppointmentConfirmationInput input)
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

        public AppointmentConfirmationOutput CancelAppointmentByAdmin(AppointmentConfirmationInput input)
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


        public AppointmentConfirmationOutput CancelAppointmentByCustomer(AppointmentConfirmationInput input)
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

        public AppointmentConfirmationOutput ForwardAppointment(AppointmentConfirmationInput input)
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
                var pushNotifForward = PushNotificationForwardAppointment(input.RsvNo);
                return new AppointmentConfirmationOutput { IsSuccess = true };

        }

        internal bool PushNotificationForwardAppointment(string rsvNo)
        {
            var customerId = GetUserIdByRsvNo(rsvNo);
            var reservation = GetReservation(rsvNo);
            var customerResult = PushNotifForwardAppointmentForCustomer(customerId, reservation.ActivityDetails.Name ,reservation.DateTime.Date.Value.Date);
            var operatorId = GetOperatorIdByActivityId(reservation.ActivityDetails.ActivityId);
            var operatorResult = PushNotifForwardAppointmentForOperator(operatorId, reservation.ActivityDetails.Name,
                reservation.DateTime.Date.Value.Date);
            return operatorResult && customerResult;
        }

        internal bool PushNotifForwardAppointmentForCustomer(string customerId, string activityName, DateTime activityDate)
        {
            var notifTitle = "Aktivitas telah di teruskan ke operator";
            var notifBody = "Aktivitas dengan nama \"" + activityName + "\" pada tanggal " +
                            activityDate.ToShortDateString() + " telah di teruskan ke operator";
            var notifData = new NotificationData();
            notifData.Function = "refreshMyBooking";
            notifData.Status = "OK";
            var notifResult = NotificationService.GetInstance().SendNotificationsCustomer(notifTitle, notifBody, customerId, notifData);
            return notifResult;
        }
        
        internal bool PushNotifForwardAppointmentForOperator(string operatorId, string activityName, DateTime activityDate)
        {
            var notifTitle = "Anda mendapatkan pesanan baru";
            var notifBody = "Anda mendapatkan pesanan baru dengan nama aktivitas \"" + activityName + "\" pada tanggal " +
                            activityDate.ToShortDateString();
            var notifData = new NotificationData();
            notifData.Function = "refreshMyBooking";
            notifData.Status = "OK";
            var notifResult = NotificationService.GetInstance().SendNotificationsOperator(notifTitle, notifBody, operatorId, notifData);
            return notifResult;
        }
    }
}