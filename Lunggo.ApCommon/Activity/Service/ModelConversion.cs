using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public ActivityReservationForDisplay ConvertToReservationForDisplay(ActivityReservation activityReservation)
        {
            if (activityReservation == null)

                return null;

            var convertedRsv = new ActivityReservationForDisplay
            {
                CancellationTime = activityReservation.CancellationTime,
                CancellationType = activityReservation.CancellationType,
                Contact = activityReservation.Contact,
                ActivityDetail = ConvertToActivityDetailForDisplay(activityReservation.ActivityDetails),
                DateTime = activityReservation.DateTime,
                Pax = ConvertToPaxForDisplay(activityReservation.Pax),
                RsvNo = activityReservation.RsvNo,
                Payment = PaymentService.GetInstance().ConvertToPaymentDetailsForDisplay(activityReservation.Payment),
                RsvTime = activityReservation.RsvTime,
                RsvDisplayStatus = MapReservationStatus(activityReservation)
            };

            return convertedRsv;
        }

        public ActivityDetailForDisplay ConvertToActivityDetailForDisplay(ActivityDetail activityDetail)
        {
            if (activityDetail == null)
                return null;
            var convertedHotel = new ActivityDetailForDisplay
            {
                ActivityId = activityDetail.ActivityId,
                Name = activityDetail.Name,
                Category = activityDetail.Category,
                ShortDesc = activityDetail.ShortDesc,
                City = activityDetail.City,
                Country = activityDetail.Country,
                Address = activityDetail.Address,
                Latitude = activityDetail.Latitude,
                Longitude = activityDetail.Longitude,
                Price = activityDetail.Price,
                PriceDetail = activityDetail.PriceDetail,
                Duration = activityDetail.Duration,
                OperationTime = activityDetail.OperationTime,
                MediaSrc = activityDetail.MediaSrc,
                Contents = activityDetail.Contents,
                AdditionalContent = activityDetail.AdditionalContent,
                Cancellation = activityDetail.Cancellation,
                Date = activityDetail.Date
            };

            return convertedHotel;
        }
        
        public List<ActivityDetailForDisplay> ConvertToActivityDetailForDisplay(List<ActivityDetail> activityDetails)
        {
            if (activityDetails == null)
                return null;

            var convertedactivities = new List<ActivityDetailForDisplay>();
            foreach (var activityDetail in activityDetails)
            {
                var activity = new ActivityDetailForDisplay
                {
                    ActivityId = activityDetail.ActivityId,
                    Name = activityDetail.Name,
                    Category = activityDetail.Category,
                    ShortDesc = activityDetail.ShortDesc,
                    City = activityDetail.City,
                    Country = activityDetail.Country,
                    Address = activityDetail.Address,
                    Latitude = activityDetail.Latitude,
                    Longitude = activityDetail.Longitude,
                    Price = activityDetail.Price,
                    PriceDetail = activityDetail.PriceDetail,
                    Duration = activityDetail.Duration,
                    OperationTime = activityDetail.OperationTime,
                    MediaSrc = activityDetail.MediaSrc,
                    Contents = activityDetail.Contents,
                    AdditionalContent = activityDetail.AdditionalContent,
                    Cancellation = activityDetail.Cancellation,
                    Date = activityDetail.Date
                };
                convertedactivities.Add(activity);
            };
            return convertedactivities.ToList();
        }

        

        public static RsvDisplayStatus MapReservationStatus(ActivityReservation reservation)
        {
            if(reservation.Payment == null) { return RsvDisplayStatus.Undefined; }

            var paymentStatus = reservation.Payment.Status;
            var paymentMethod = reservation.Payment.Method;
            var rsvStatus = reservation.RsvStatus;

            if (rsvStatus == RsvStatus.Cancelled || paymentStatus == PaymentStatus.Cancelled)
                return RsvDisplayStatus.Cancelled;
            if (rsvStatus == RsvStatus.Expired || paymentStatus == PaymentStatus.Expired)
                return RsvDisplayStatus.Expired;
            if (paymentStatus == PaymentStatus.Denied)
                return RsvDisplayStatus.PaymentDenied;
            if (paymentStatus == PaymentStatus.Failed)
                return RsvDisplayStatus.FailedUnpaid;
            if (rsvStatus == RsvStatus.Failed)
                return paymentStatus == PaymentStatus.Settled
                    ? RsvDisplayStatus.FailedPaid
                    : RsvDisplayStatus.FailedUnpaid;
            if (paymentMethod == PaymentMethod.Undefined)
                return RsvDisplayStatus.Reserved;
            if (paymentStatus == PaymentStatus.Settled)
                return reservation.RsvStatus == RsvStatus.Completed
                    ? RsvDisplayStatus.Issued
                    : RsvDisplayStatus.Paid;
            if (paymentStatus != PaymentStatus.Settled)
                return (paymentMethod == PaymentMethod.VirtualAccount || paymentMethod == PaymentMethod.BankTransfer)
                    ? RsvDisplayStatus.PendingPayment
                    : RsvDisplayStatus.VerifyingPayment;
            return RsvDisplayStatus.Undefined;
        }

    }
}
