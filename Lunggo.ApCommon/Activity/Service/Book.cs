using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Lunggo.ApCommon.Activity.Constant;
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
using Lunggo.Framework.Context;
using System.Collections.Generic;
using Lunggo.Framework.Environment;
using Lunggo.Repository.TableRepository;

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

            var packageIds = ActivityService.GetInstance().GetPackageIdFromDb(Convert.ToInt64(input.ActivityId)).ToList();
            if (!packageIds.Contains(input.PackageId))
            {
                return new BookActivityOutput
                {
                    IsValid = false,
                    errStatus = "ERR_INVALID_PACKAGE_ID"
                };
            }

            var dateAndSession = ActivityService.GetInstance().GetAvailableDatesFromDb(new GetAvailableDatesInput { ActivityId = Convert.ToInt32(input.ActivityId) }).AvailableDateTimes;
            var dates = new List<DateTime?>();
            foreach(var date in dateAndSession)
            {
                dates.Add(date.Date);
            }

            if (!dates.Contains(input.DateTime.Date))
            {
                return new BookActivityOutput
                {
                    IsValid = false,
                    errStatus ="ERR_INVALID_DATE"
                };
            }

            var sessions = dateAndSession.Where(session => session.Date == input.DateTime.Date).First().AvailableHours;
            if (!sessions.Contains(input.DateTime.Session) && sessions.Count() != 0)
            {
                return new BookActivityOutput
                {
                    IsValid = false,
                    errStatus = "ERR_INVALID_SESSION"
                };
            }

            getDetail.ActivityDetail.BookingStatus = BookingStatus.Booked;

            var rsvDetail = CreateActivityReservation(input, getDetail.ActivityDetail, dateAndSession, out var originalPrice);
            if (rsvDetail.RsvNo == "ERR_INVALID_TICKET_TYPE" || rsvDetail.RsvNo == "ERR_INVALID_TICKET_TYPE_COUNT" || rsvDetail.RsvNo == "ERR_INVALID_ALL_TICKET_COUNT")
            {
                return new BookActivityOutput
                {
                    IsValid = false,
                    errStatus = rsvDetail.RsvNo
                };
            }

            InsertActivityRsvToDb(rsvDetail, originalPrice);
            //ExpireReservationWhenTimeout(rsvDetail.RsvNo, rsvDetail.Payment.TimeLimit);
            return new BookActivityOutput
            {
                IsPriceChanged = false,
                IsValid = true,
                RsvNo = rsvDetail.RsvNo,
                //TimeLimit = rsvDetail.Payment.TimeLimit
            };
        }

        public ActivityReservation CreateActivityReservation(BookActivityInput input, ActivityDetail activityInfo, List<DateAndAvailableHour> dateAndSession, out decimal originalPrice)
        {
            originalPrice = 0;
            var rsvNo = RsvNoSequence.GetInstance().GetNext(ProductType.Activity);

            var env = EnvVariables.Get("general", "environment");
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

            decimal originalPriceIdr = 0;
            int allTicketCount = 0;
            var pricePackages = ActivityService.GetInstance().GetPackagePriceFromDb(input.PackageId);
            var typePricePackages = new List<string>();
            var packageAttribute = ActivityService.GetInstance().GetPackageAttributeFromDb(input.PackageId);

            foreach (var pricePackage in pricePackages)
            {
                typePricePackages.Add(pricePackage.Type);
            }
            foreach (var ticketCount in input.TicketCount)
            {
                allTicketCount += ticketCount.Count;
                if (!typePricePackages.Contains(ticketCount.Type))
                {
                    return new ActivityReservation
                    {
                        RsvNo = "ERR_INVALID_TICKET_TYPE"
                    };
                }
                var price = pricePackages.Where(package => package.Type == ticketCount.Type).First();
                if (ticketCount.Count < price.MinCount)
                {
                    return new ActivityReservation
                    {
                        RsvNo = "ERR_INVALID_TICKET_TYPE_COUNT"
                    };
                }
                originalPriceIdr += (price.Amount * ticketCount.Count);
            }
            
            var dateResults = dateAndSession.Where(a => a.Date == input.DateTime.Date).First();
            int paxSlot = 0;
            if (input.DateTime.Session != null)
            {
                paxSlot = dateResults.AvailableSessionAndPaxSlots.Where(a => a.AvailableHour == input.DateTime.Session).First().PaxSlot;
            }

            else
            {
                paxSlot = dateResults.AvailableSessionAndPaxSlots.First().PaxSlot;
            }

            if (allTicketCount > packageAttribute[0].MaxCount || allTicketCount < packageAttribute[0].MinCount || allTicketCount > paxSlot)
            {
                return new ActivityReservation
                {
                    RsvNo = "ERR_INVALID_ALL_TICKET_COUNT"
                }; 
            }

            originalPrice = originalPriceIdr;

            var rsvDetail = new ActivityReservation
            {
                RsvNo = rsvNo,
                Contact = input.Contact,
                ActivityDetails = activityInfo,
                PackageId = input.PackageId,
                TicketCount = input.TicketCount,
                DateTime = input.DateTime,
                Pax = input.Passengers,
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

            return rsvDetail;
        }

        public bool CheckActivityHasOperator(string activityId)
        {
            var activity = GetActivityDetailFromDb(new GetDetailActivityInput
            {
                ActivityId = Convert.ToInt64(activityId)
            });
            return activity.ActivityDetail.HasOperator;
        }
    }
}