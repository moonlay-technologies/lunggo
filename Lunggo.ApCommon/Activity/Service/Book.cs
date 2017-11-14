using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public BookActivityOutput BookActivity(BookActivityInput input)
        {
            var getDetail = GetActivityDetailFromDb(new GetDetailActivityInput()
            {
                ActivityId = Convert.ToInt32(input.ActivityId)
            });
            
            var rsvDetail = CreateActivityReservation(input, getDetail.ActivityDetail);
            InsertActivityRsvToDb(rsvDetail);
            ExpireReservationWhenTimeout(rsvDetail.RsvNo, rsvDetail.Payment.TimeLimit);
            return new BookActivityOutput
            {
                IsPriceChanged = false,
                IsValid = true,
                RsvNo = rsvDetail.RsvNo,
                TimeLimit = rsvDetail.Payment.TimeLimit
            };
        }

        public ActivityReservation CreateActivityReservation(BookActivityInput input, ActivityDetail activityInfo)
        {
            var rsvNo = RsvNoSequence.GetInstance().GetNext(ProductType.Activity);

            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            PlatformType platform;
            string deviceId;
            if (env == "production")
            {
                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
                platform = Client.GetPlatformType(clientId);
                deviceId = identity.Claims.Single(claim => claim.Type == "Device ID").Value;
            }
            else
            {
                platform = PlatformType.Undefined;
                deviceId = null;
            }

            var rsvDetail = new ActivityReservation
            {
                RsvNo = rsvNo,
                Contact = input.Contact,
                ActivityDetails = activityInfo,
                DateTime = input.DateTime,
                TicketCount = input.TicketCount,
                Pax = input.Passengers,
                Payment = new PaymentDetails
                {
                    Status = PaymentStatus.Pending,
                    LocalCurrency = new Currency(OnlineContext.GetActiveCurrencyCode()),
                    //OriginalPriceIdr = bookInfo.Rooms.SelectMany(r => r.Rates).Sum(r => r.Price.Local),
                    TimeLimit = DateTime.UtcNow.AddHours(1)
                    //TimeLimit = bookInfo.Rooms.SelectMany(r => r.Rates).Min(order => order.TimeLimit).AddMinutes(-10),
                },
                RsvStatus = RsvStatus.InProcess,
                RsvTime = DateTime.UtcNow,
                State = new ReservationState
                {
                    Platform = platform,
                    DeviceId = deviceId,
                    Language = "id", //OnlineContext.GetActiveLanguageCode();
                    Currency = new Currency("IDR"), //OnlineContext.GetActiveCurrencyCode());
                },
                User = HttpContext.Current.User.Identity.IsUserAuthorized()
                    ? HttpContext.Current.User.Identity.GetUser()
                    : null
            };

            PaymentService.GetInstance().GetUniqueCode(rsvDetail.RsvNo, null, null);

            return rsvDetail;
        }

    }
}