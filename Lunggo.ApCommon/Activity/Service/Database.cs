using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.Framework.Database;
using System.Linq;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Payment.Model;
using System.Web;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.ApCommon.Identity.Users;
using BookingStatus = Lunggo.ApCommon.Activity.Constant.BookingStatus;
using BookingStatusCd = Lunggo.ApCommon.Activity.Constant.BookingStatusCd;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.BlobStorage;
using System.Security.Cryptography;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Context;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Environment;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        #region Get
        public SearchActivityOutput GetActivitiesFromDb(SearchActivityInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                string endDate = input.ActivityFilter.EndDate.ToString("yyyy/MM/dd");
                string startDate = input.ActivityFilter.StartDate.ToString("yyyy/MM/dd");
                input.Page = 1;
                input.PerPage = 10;
                var param = new
                {
                    Name = input.ActivityFilter.Name,
                    StartDate = startDate,
                    EndDate = endDate,
                    Page = input.Page <= 0 ? 1 : input.Page,
                    PerPage = input.PerPage <= 0 ? 10 : input.PerPage,
                    Id = input.ActivityFilter.Id,
                    userId = HttpContext.Current == null ? null : HttpContext.Current.User.Identity.GetId()
                };
                var savedActivities = GetSearchResultQuery.GetInstance()
                    .ExecuteMultiMap(conn, param, param,
                    (activities, duration) =>
                        {
                            activities.Duration = duration;
                            return activities;
                        }, "Amount").ToList();

                var output = new SearchActivityOutput
                {
                    ActivityList = savedActivities,
                    Page = input.Page,
                    PerPage = input.PerPage
                };
                return output;
            }
        }

        public GetDetailActivityOutput GetActivityDetailFromDb(GetDetailActivityInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {

                var details = GetActivityDetailQuery.GetInstance().ExecuteMultiMap(conn, new { ActivityId = input.ActivityId }, null,
                        (detail, duration) =>
                        {
                            detail.Duration = duration;
                            return detail;
                        }, "Amount");

                var mediaSrc = GetMediaActivityDetailQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId }).ToList();

                var additionalContentsDetail = GetAdditionalContentActivityDetailQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId });

                if (details.Count() == 0)
                {
                    return null;
                }
                var activityDetail = details.First();

                var cancellations = RefundRulesCustomerTableRepo.GetInstance().Find(conn,
                    new RefundRulesCustomerTableRecord
                    {
                        ActivityId = input.ActivityId
                    }).ToList();

                activityDetail.Cancellation = new List<Cancellation>();

                if (cancellations.Count > 0)
                {
                    foreach (var cancellation in cancellations)
                    {
                        var cancel = new Cancellation
                        {
                            RefundName = cancellation.RuleName,
                            RefundDescription = cancellation.Description,
                            ValuePercentage = cancellation.ValuePercentage ?? 0,
                            ValueConstant = cancellation.ValueConstant ?? 0,
                            MinValue = cancellation.MinValue ?? 0,
                            DateLimit = cancellation.PayDateLimit ?? 0,
                            State = cancellation.PayState
                        };
                        activityDetail.Cancellation.Add(cancel);
                    }
                    
                }
                
                activityDetail.BookingStatus = BookingStatus.Booked;
                activityDetail.MediaSrc = mediaSrc;
                activityDetail.AdditionalContents = new AdditionalContent
                {
                    Title = "Keterangan Tambahan",
                    Contents = additionalContentsDetail.ToList()
                };

                var inputWishlist = new SearchActivityInput();
                var activityIdWishlist = new ActivityFilter();
                var idList = new List<long>();
                idList.Add(input.ActivityId.GetValueOrDefault());
                activityIdWishlist.Id = idList;
                inputWishlist.ActivityFilter = activityIdWishlist;
                var wishlistedList = GetActivitiesFromDb(inputWishlist).ActivityList;
                activityDetail.Wishlisted = wishlistedList[0].Wishlisted;
                activityDetail.ViewCount = activityDetail.ViewCount + 1;

                UpdateViewCountDbQuery.GetInstance().Execute(conn, new { ActivityId = input.ActivityId, ViewCount = activityDetail.ViewCount });
                activityDetail.Package = GetActivityTicketDetailFromDb(input).Package;
                if (activityDetail.Package.Count > 0)
                {
                    var minPaxes = activityDetail.Package.Select(package => package.MinCount).OrderBy(paxes => paxes).ToList();
                    activityDetail.MinPax = minPaxes.First();

                    var maxPaxes = activityDetail.Package.Select(package => package.MaxCount).OrderByDescending(paxes => paxes).ToList();
                    activityDetail.MaxPax = maxPaxes.First();
                }

                var review = GetReviewFromDb(input.ActivityId);
                if (review.Count() != 0)
                {
                    activityDetail.Review = review[0];
                }
                var output = new GetDetailActivityOutput
                {
                    ActivityDetail = activityDetail
                };
                return output;
            }
        }

        public GetAvailableDatesOutput GetAvailableDatesFromDb(GetAvailableDatesInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var whitelistedDates = GetWhitelistedDateDbQuery.GetInstance().Execute(conn, new { ActivityId = input.ActivityId }).ToList();
                //var blacklistedDates = GetBlacklistedDateDbQuery.GetInstance().Execute(conn, new { ActivityId = input.ActivityId }).ToList();
                //var allDates = new List<DateTime>();
                var result = new List<DateAndAvailableHour>();
                //var allAvailableDates = new List<DateTime>();
                //var startDate = DateTime.Today;
                //var endDate = startDate.AddMonths(3);
                //for (var date = startDate; date <= endDate; date = date.AddDays(1))
                //{
                //    allDates.Add(date);
                //}
                //var savedDay = GetAvailableDatesQuery.GetInstance()
                //    .Execute(conn, new { ActivityId = input.ActivityId });
                //var availableDays = savedDay.ToList();
                //var dayEnum = new List<DayOfWeek>();
                //foreach (var day in availableDays)
                //{
                //    DayOfWeek myDays = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day);
                //    dayEnum.Add(myDays);
                //}

                //foreach (var date in allDates)
                //{
                //    if (dayEnum.Contains(date.DayOfWeek))
                //    {
                //        var savedHours = GetAvailableSessionQuery.GetInstance()
                //         .Execute(conn, new { ActivityId = input.ActivityId, AvailableDay = date.DayOfWeek.ToString() }).ToList();
                //        var customSavedHoursWL = GetCustomAvailableHoursWhitelistDbQuery.GetInstance().Execute(conn, new { CustomDate = date, ActivityId = input.ActivityId }).ToList();
                //        var customSavedHoursBL = GetCustomAvailableHoursBlacklistDbQuery.GetInstance().Execute(conn, new { CustomDate = date, ActivityId = input.ActivityId }).ToList();
                //        savedHours.Remove("");
                //        customSavedHoursBL.Remove("");
                //        customSavedHoursWL.Remove("");
                //        if (whitelistedDates.Contains(date) && customSavedHoursWL.Count() != 0)
                //        {
                //            foreach (var customSavedHour in customSavedHoursWL)
                //            {
                //                savedHours.Add(customSavedHour);
                //                whitelistedDates.Remove(date);
                //            }
                //        }
                //
                //        if (blacklistedDates.Contains(date) && customSavedHoursBL.Count() != 0)
                //        {
                //            foreach (var customSavedHour in customSavedHoursBL)
                //            {
                //                savedHours.Remove(customSavedHour);
                //            }
                //        }
                //
                //        if (!blacklistedDates.Contains(date) || savedHours.Count > 0)
                //        {
                //            var savedDateAndHour = new DateAndAvailableHour
                //            {
                //                AvailableHours = savedHours,
                //                Date = date
                //            };
                //            result.Add(savedDateAndHour);
                //        }
                //    }
                //}

                var validWhiteListedDates = whitelistedDates.Where(a => a.Date > DateTime.UtcNow);

                foreach (var whitelistedDate in validWhiteListedDates)
                {
                    var customSessionAndPaxSlots = GetCustomAvailableHoursWhitelistDbQuery.GetInstance().Execute(conn, new { CustomDate = whitelistedDate, ActivityId = input.ActivityId }).ToList();
                    var availableHours = customSessionAndPaxSlots.Select(a => a.AvailableHour).ToList();
                    availableHours.Remove("");
                    foreach (var customSessionAndPaxSlot in customSessionAndPaxSlots)
                    {
                        customSessionAndPaxSlot.AvailableHour = customSessionAndPaxSlot.AvailableHour == ""
                            ? null
                            : customSessionAndPaxSlot.AvailableHour;
                    }
                    var customSavedDateAndHour = new DateAndAvailableHour
                    {
                        Date = whitelistedDate,
                        AvailableHours = availableHours,
                        AvailableSessionAndPaxSlots = customSessionAndPaxSlots
                    };
                    result.Add(customSavedDateAndHour);
                }

                var output = new GetAvailableDatesOutput
                {
                    AvailableDateTimes = result
                };

                return output;
            }
        }

        public ActivityReservation GetReservationFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationRecord = ReservationTableRepo.GetInstance()
                    .Find1(conn, new ReservationTableRecord { RsvNo = rsvNo });

                if (reservationRecord == null)
                    return null;

                var activityReservation = new ActivityReservation
                {
                    RsvNo = rsvNo,
                    Contact = Contact.GetFromDb(rsvNo),
                    Payment = _paymentService.GetPaymentDetails(rsvNo),
                    State = ReservationState.GetFromDb(rsvNo),
                    ActivityDetails = new ActivityDetail(),
                    RsvTime = reservationRecord.RsvTime.GetValueOrDefault(),
                    RsvStatus = RsvStatusCd.Mnemonic(reservationRecord.RsvStatusCd),
                };


                if (activityReservation.Contact == null || activityReservation.Payment == null)
                    return null;

                var activityDetailRecord = ActivityReservationTableRepo.GetInstance()
                    .Find1(conn, new ActivityReservationTableRecord { RsvNo = rsvNo });

                var actDetail = GetActivityDetailFromDb(new GetDetailActivityInput() { ActivityId = activityDetailRecord.ActivityId });

                activityReservation.ActivityDetails = actDetail.ActivityDetail;
                activityReservation.DateTime = new DateAndSession()
                {
                    Date = activityDetailRecord.Date,
                    Session = activityDetailRecord.SelectedSession
                };

                var paxRecords = PaxTableRepo.GetInstance()
                        .Find(conn, new PaxTableRecord { RsvNo = rsvNo }).ToList();
                var pax = new List<Pax>();
                if (paxRecords.Count != 0)
                    foreach (var passengerRecord in paxRecords)
                    {
                        var passenger = new Pax
                        {
                            Title = TitleCd.Mnemonic(passengerRecord.TitleCd),
                            FirstName = passengerRecord.FirstName,
                            LastName = passengerRecord.LastName,
                            Type = PaxTypeCd.Mnemonic(passengerRecord.TypeCd)
                        };
                        pax.Add(passenger);
                    }
                activityReservation.Pax = pax;
                var reservationData = GetMyBookingDetailFromDb(new GetMyBookingDetailInput { RsvNo = rsvNo }).BookingDetail;
                activityReservation.PackageId = reservationData.PackageId;
                activityReservation.TicketCount = reservationData.PaxCount;
                return activityReservation;
            }
        }

        public string GetReservationUserIdFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationRecord = ReservationTableRepo.GetInstance()
                    .Find1(conn, new ReservationTableRecord { RsvNo = rsvNo });

                if (reservationRecord == null)
                    return null;
                return reservationRecord.UserId;
            }
        }

        public GetMyBookingsOutput GetMyBookingsFromDb(GetMyBookingsInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userName = HttpContext.Current.User.Identity.GetUser();
                var cartIdList = GetCartIdListDbQuery.GetInstance()
                    .Execute(conn, new { UserId = userName.Id, Page = input.Page, PerPage = input.PerPage }).ToList();
                var savedBookings = new List<CartList>();
                foreach (var cartId in cartIdList)
                {
                    var rsvNoList = GetCartRsvNoListDbQuery.GetInstance().Execute(conn, new { TrxId = cartId }).ToList();
                    var bookingDetails = rsvNoList.Select(rsvNo => GetMyBookingDetailFromDb(new GetMyBookingDetailInput { RsvNo = rsvNo }).BookingDetail).ToList();
                    var payments = rsvNoList.Select(rsvNo => _paymentService.GetPaymentDetails(rsvNo)).ToList();
                    decimal totalOriginalPrice = payments.Sum(payment => payment.OriginalPriceIdr);
                    decimal totalFinalPrice = payments.Sum(payment => payment.FinalPriceIdr);
                    decimal totalDiscount = payments.Sum(payment => payment.DiscountNominal);
                    decimal totalUniqueCode = payments.Sum(payment => payment.UniqueCode);
                    PaymentStatus paymentStatusEnum = payments.First().Status;
                    var paymentLastUpdate = payments.First().UpdateDate;
                    var paymentStatus = PaymentStatusConversion(paymentStatusEnum);
                    var cartList = new CartList
                    {
                        CartId = cartId,
                        Activities = bookingDetails,
                        TotalOriginalPrice = totalOriginalPrice,
                        TotalDiscount = totalDiscount,
                        TotalUniqueCode = totalUniqueCode,
                        TotalFinalPrice = totalFinalPrice,
                        PaymentStatus = paymentStatus,
                        PaymentLastUpdate = paymentLastUpdate
                    };
                    savedBookings.Add(cartList);
                }

                var output = new GetMyBookingsOutput
                {                
                    MyBookings = savedBookings.OrderBy(a => a.PaymentStatus).ToList(),
                    Page = input.Page,
                    PerPage = input.PerPage
                };
                return output;
            }
        }

        public GetMyBookingsCartActiveOutput GetMyBookingsCartActiveFromDb(GetMyBookingsCartActiveInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userName = HttpContext.Current.User.Identity.GetUser();
                var cartIdList = GetCartIdListDbQuery.GetInstance()
                    .Execute(conn, new { UserId = userName.Id, Page = 1, PerPage = 1000 }).ToList();
                var savedBookings = new List<CartList>();
                foreach (var cartId in cartIdList)
                {
                    var rsvNoList = GetCartRsvNoListDbQuery.GetInstance().Execute(conn, new { TrxId = cartId }).ToList();
                    var bookingDetails = rsvNoList.Select(rsvNo => GetMyBookingDetailFromDb(new GetMyBookingDetailInput { RsvNo = rsvNo }).BookingDetail).ToList();
                    var payments = rsvNoList.Select(rsvNo => _paymentService.GetPaymentDetails(rsvNo)).ToList();
                    decimal totalOriginalPrice = payments.Sum(payment => payment.OriginalPriceIdr);
                    decimal totalFinalPrice = payments.Sum(payment => payment.FinalPriceIdr);
                    decimal totalDiscount = payments.Sum(payment => payment.DiscountNominal);
                    decimal totalUniqueCode = payments.Sum(payment => payment.UniqueCode);
                    PaymentStatus paymentStatusEnum = payments.First().Status;
                    var paymentLastUpdate = payments.First().UpdateDate;
                    var paymentStatus = PaymentStatusConversion(paymentStatusEnum);
                    var cartList = new CartList
                    {
                        CartId = cartId,
                        Activities = bookingDetails,
                        TotalOriginalPrice = totalOriginalPrice,
                        TotalDiscount = totalDiscount,
                        TotalUniqueCode = totalUniqueCode,
                        TotalFinalPrice = totalFinalPrice,
                        PaymentStatus = paymentStatus,
                        PaymentLastUpdate = paymentLastUpdate
                    };
                    savedBookings.Add(cartList);
                }

                var lastUpdate = savedBookings.Select(a => a.PaymentLastUpdate > input.LastUpdate ? a.PaymentLastUpdate : null)
                    .Where(b => b != null).ToList();

                if (lastUpdate.Count < 1)
                {
                    return new GetMyBookingsCartActiveOutput
                    {
                        MyBookings = new List<CartList>(),
                        LastUpdate = input.LastUpdate,
                        MustUpdate = false
                    };
                }

                var lastUpdateOutput = lastUpdate.OrderByDescending(a => a.Value).First();

                var output = new GetMyBookingsCartActiveOutput
                {                
                    MyBookings = savedBookings.OrderBy(a => a.PaymentStatus).ToList(),
                    LastUpdate = lastUpdateOutput.Value,
                    MustUpdate = true
                };
                return output;
            }
        }

        public GetMyBookingsReservationActiveOutput GetMyBookingsReservationActiveFromDb(GetMyBookingsReservationActiveInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userName = HttpContext.Current.User.Identity.GetUser();
                var cartIdList = GetCartIdListDbQuery.GetInstance()
                    .Execute(conn, new { UserId = userName.Id, Page = 1, PerPage = 1000 }).ToList();
                var savedReservations = new List<BookingDetail>();
                foreach (var cartId in cartIdList)
                {
                    var rsvNoList = GetCartRsvNoListDbQuery.GetInstance().Execute(conn, new { TrxId = cartId }).ToList();
                    var bookingDetails = rsvNoList.Select(rsvNo => GetMyBookingDetailFromDb(new GetMyBookingDetailInput { RsvNo = rsvNo }).BookingDetail).ToList();
                    foreach (var bookingDetail in bookingDetails)
                    {
                        savedReservations.Add(bookingDetail);
                    }
                }

                var lastUpdate = savedReservations.Select(a => a.UpdateDate > input.LastUpdate ? a.UpdateDate : null)
                    .Where(b => b != null).ToList();

                if (lastUpdate.Count < 1)
                {
                    return new GetMyBookingsReservationActiveOutput
                    {
                        MyReservations = new List<BookingDetail>(),
                        LastUpdate = input.LastUpdate,
                        MustUpdate = false
                    };
                }

                var lastUpdateOutput = lastUpdate.OrderByDescending(a => a.Value).First();

                var output = new GetMyBookingsReservationActiveOutput
                {                
                    MyReservations = savedReservations.OrderBy(a => a.Date).ToList(),
                    LastUpdate = lastUpdateOutput.Value,
                    MustUpdate = true
                };
                return output;
            }
        }

        public GetMyBookingDetailOutput GetMyBookingDetailFromDb(GetMyBookingDetailInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedBookingCheck = GetMyBookingDetailQuery.GetInstance()
                    .Execute(conn, new { input.RsvNo });
                if (savedBookingCheck.Count() == 0)
                {
                    return null;
                }
                var savedBooking = savedBookingCheck.First();
                var savedPassengers = GetPassengersQuery.GetInstance().ExecuteMultiMap(conn, new { input.RsvNo }, null,
                        (passengers, typeCd, titleCd, genderCd) =>
                        {
                            passengers.Type = PaxTypeCd.Mnemonic(typeCd);
                            passengers.Title = TitleCd.Mnemonic(titleCd);
                            passengers.Gender = GenderCd.Mnemonic(genderCd);
                            return passengers;
                        }, "TypeCd, TitleCd, GenderCd").ToList();

                var savedPaxCountAndPackageId = GetPaxCountAndPackageIdDbQuery.GetInstance().Execute(conn, new { RsvNo = input.RsvNo }).ToList();
                if (savedPaxCountAndPackageId.Count() != 0)
                {
                    savedBooking.PackageId = savedPaxCountAndPackageId.First().PackageId;
                    savedBooking.PackageName = savedPaxCountAndPackageId.First().PackageName;
                }
                var savedPaxCounts = new List<ActivityPricePackageReservation>();
                var pricePackages = GetPackagePriceFromDb(savedBooking.PackageId);
                foreach (var savedPaxCount in savedPaxCountAndPackageId)
                {
                    var saved = new ActivityPricePackageReservation();
                    saved.Type = savedPaxCount.Type;
                    saved.Count = savedPaxCount.Count;
                    var amountType = pricePackages.Where(package => package.Type == savedPaxCount.Type);
                    if (amountType.Count() != 0)
                    {
                        saved.TotalPrice = savedPaxCount.Count * amountType.First().Amount;
                    }
                    savedPaxCounts.Add(saved);
                }
                savedBooking.PaxCount = savedPaxCounts;
                decimal priceBook = 0;
                foreach (var ticketCount in savedPaxCountAndPackageId)
                {
                    foreach (var pricePackage in pricePackages)
                    {
                        if (pricePackage.Type == ticketCount.Type)
                        {
                            var price = pricePackage.Amount * ticketCount.Count;
                            priceBook += price;
                        }
                    }
                }
                savedBooking.RsvNo = input.RsvNo;
                savedBooking.Price = priceBook;
                savedBooking.Passengers = ConvertToPaxForDisplay(savedPassengers);
                savedBooking.RequestReview = CheckReview(input.RsvNo, savedBooking.Date, savedBooking.SelectedSession);
                savedBooking.RequestRating = CheckRating(input.RsvNo, savedBooking.Date, savedBooking.SelectedSession);
                if (savedBooking.HasPdfVoucher && savedBooking.IsPdfUploaded)
                {
                    savedBooking.PdfUrl = EnvVariables.Get("azureStorage", "rootUrl") + "/eticket/" + input.RsvNo + ".pdf";
                }

                var output = new GetMyBookingDetailOutput
                {
                    BookingDetail = savedBooking
                };
                return output;
            }
        }

        public GetAppointmentRequestOutput GetAppointmentRequestFromDb(GetAppointmentRequestInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userName = HttpContext.Current.User.Identity.GetUser();

                var savedBookings = GetAppointmentRequestQuery.GetInstance()
                    .Execute(conn, new {UserId = userName.Id, Page = 1, PerPage = 1000}).ToList();

                var lastUpdate = savedBookings.Select(a => a.UpdateDate.HasValue ? a.UpdateDate.Value > input.LastUpdate ? a.UpdateDate : null : null)
                    .Where(b => b != null).ToList();

                if (lastUpdate.Count < 1)
                {
                    return new GetAppointmentRequestOutput
                    {
                        Appointments = new List<AppointmentDetail>(),
                        LastUpdate = input.LastUpdate,
                        MustUpdate = false,
                    };
                }

                foreach (var savedBooking in savedBookings)
                {
                    var savedPaxCountAndPackageId = GetPaxCountAndPackageIdDbQuery.GetInstance().Execute(conn, new { RsvNo = savedBooking.RsvNo }).ToList();
                    if (savedPaxCountAndPackageId.Count() != 0)
                    {
                        savedBooking.PackageId = savedPaxCountAndPackageId.First().PackageId;
                        savedBooking.PackageName = savedPaxCountAndPackageId.First().PackageName;
                    }
                    var savedPaxCounts = new List<ActivityPricePackageReservation>();
                    var pricePackages = ActivityService.GetInstance().GetPackagePriceFromDb(savedBooking.PackageId);
                    foreach (var savedPaxCount in savedPaxCountAndPackageId)
                    {
                        var saveds = new ActivityPricePackageReservation();
                        saveds.Type = savedPaxCount.Type;
                        saveds.Count = savedPaxCount.Count;
                        var amountType = pricePackages.Where(package => package.Type == savedPaxCount.Type).ToList();
                        if (amountType.Count != 0)
                        {
                            saveds.TotalPrice = savedPaxCount.Count * amountType.First().Amount;
                        }
                        savedPaxCounts.Add(saveds);
                    }
                    savedBooking.PaxCount = savedPaxCounts;
                }

                var lastUpdateOutput = lastUpdate.OrderByDescending(a => a.Value).ToList();               

                var output = new GetAppointmentRequestOutput
                {
                    Appointments = savedBookings.Select(a => new AppointmentDetail()
                    {
                        ActivityId = a.ActivityId,
                        RsvNo = a.RsvNo,
                        Name = a.Name,
                        Date = a.Date,
                        Session = a.Session,
                        RequestTime = a.RequestTime,
                        TimeLimit = a.RequestTime.AddDays(3),
                        PaxCount = a.PaxCount,
                        MediaSrc = a.MediaSrc,
                        ContactName = Contact.GetFromDb(a.RsvNo).Name
                    }).ToList(),
                    LastUpdate = lastUpdateOutput.First().Value,
                    MustUpdate = true

                };
                return output;
            }
        }

        public GetAppointmentListOutput GetAppointmentListFromDb(GetAppointmentListInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userName = HttpContext.Current.User.Identity.GetUser();

                var savedBookings = GetAppointmentListQuery.GetInstance()
                    .Execute(conn, new { UserId = userName.Id, Page = input.Page, PerPage = input.PerPage, StartDate = input.StartDate, EndDate = input.EndDate, BookingStatusCdList = input.BookingStatusCdList }).ToList();
                var saved2 = savedBookings.Select(savedBooking => new AppointmentList { ActivityId = savedBooking.ActivityId, Date = savedBooking.Date, Name = savedBooking.Name, Session = savedBooking.Session, MediaSrc = savedBooking.MediaSrc });
                var saved1 = saved2.GroupBy(p => new { p.ActivityId, p.Date, p.Session, p.MediaSrc, p.AppointmentReservations })
                            .Select(g => g.First());
                var saved = saved1.OrderBy(a => a.Date).ToList();


                foreach (var savedBooking in savedBookings)
                {
                    var savedPassengers = GetPassengersQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = savedBooking.RsvNo, }, null,
                        (passengers, typeCd, titleCd, genderCd) =>
                        {
                            passengers.Type = PaxTypeCd.Mnemonic(typeCd);
                            passengers.Title = TitleCd.Mnemonic(titleCd);
                            passengers.Gender = GenderCd.Mnemonic(genderCd);
                            return passengers;
                        }, "TypeCd, TitleCd, GenderCd").ToList();
                    var contact = Contact.GetFromDb(savedBooking.RsvNo);
                    var savedPaxCountAndPackageId = GetPaxCountAndPackageIdDbQuery.GetInstance().Execute(conn, new { RsvNo = savedBooking.RsvNo }).ToList();
                    if (savedPaxCountAndPackageId.Count() != 0)
                    {
                        savedBooking.PackageId = savedPaxCountAndPackageId.First().PackageId;
                        savedBooking.PackageName = savedPaxCountAndPackageId.First().PackageName;
                    }
                    var savedPaxCounts = new List<ActivityPricePackageReservation>();
                    var pricePackages = ActivityService.GetInstance().GetPackagePriceFromDb(savedBooking.PackageId);
                    foreach (var savedPaxCount in savedPaxCountAndPackageId)
                    {
                        var saveds = new ActivityPricePackageReservation();
                        saveds.Type = savedPaxCount.Type;
                        saveds.Count = savedPaxCount.Count;
                        var amountType = pricePackages.Where(package => package.Type == savedPaxCount.Type);
                        if (amountType.Count() != 0)
                        {
                            saveds.TotalPrice = savedPaxCount.Count * amountType.First().Amount;
                        }
                        savedPaxCounts.Add(saveds);
                    }
                    var paxGroup = new PaxGroup();
                    paxGroup.Passengers = ActivityService.GetInstance().ConvertToPaxForDisplay(savedPassengers);
                    paxGroup.Contact = contact;
                    paxGroup.PaxCounts = savedPaxCounts;
                    savedBooking.PaxGroup = paxGroup;
                    var reservationRecord = ReservationTableRepo.GetInstance()
                    .Find1(conn, new ReservationTableRecord { RsvNo = savedBooking.RsvNo });
                    savedBooking.RequestTime = (DateTime)reservationRecord.RsvTime;
                    if (input.OrderParam)
                    {
                        var paymentStepsDb = ActivityReservationStepOperatorTableRepo.GetInstance().Find(conn, new ActivityReservationStepOperatorTableRecord() { RsvNo = savedBooking.RsvNo }).ToList();
                        var paymentSteps = paymentStepsDb.Select(a => new PaymentStep { StepDescription = a.StepDescription, StepAmount = a.StepAmount ?? 0, StepDate = a.StepDate, StepStatus = (a.StepStatus.Value) ? "PAID" : "PENDING" }).ToList();
                        var cancelStatus = savedBooking.RsvStatus.Substring(0, 6);
                        if (cancelStatus == "Cancel")
                        {
                            var newPaymentSteps = paymentSteps.Where(a => a.StepStatus == "PAID").ToList();
                            var halfStatusHistory = ReservationStatusHistoryTableRepo.GetInstance().Find(conn, new ReservationStatusHistoryTableRecord
                            {
                                RsvNo = savedBooking.RsvNo
                            }).ToList();
                            var statusHistory = halfStatusHistory.Where(a => a.BookingStatusCd == savedBooking.RsvStatus).ToList();
                            var amount = RefundHistoryTableRepo.GetInstance().Find1(conn, new RefundHistoryTableRecord
                            {
                                RsvNo = savedBooking.RsvNo
                            }).RefundAmount;



                            newPaymentSteps.Add(new PaymentStep
                            {
                                StepDescription = "Cancel",
                                StepDate = statusHistory[0].TimeStamp,
                                StepAmount = amount.HasValue ? amount.Value : 0,
                                StepStatus = "CANCEL",
                            });
                            paymentSteps = newPaymentSteps;
                        }
                        savedBooking.PaymentSteps = paymentSteps;
                    }
                }

                foreach (var save in saved)
                {
                    var appointmentDetail = savedBookings.Where(saving => save.ActivityId == saving.ActivityId && save.Date == saving.Date && save.Session == saving.Session).ToList();
                    var appointmentReservations = appointmentDetail.Select(appointment => new AppointmentReservation
                    {
                        RsvNo = appointment.RsvNo,
                        Contact = appointment.PaxGroup.Contact,
                        Passengers = appointment.PaxGroup.Passengers,
                        PaxCounts = appointment.PaxGroup.PaxCounts,
                        RsvTime = appointment.RequestTime,
                        RsvStatus = appointment.RsvStatus,
                        PaymentSteps = appointment.PaymentSteps,
                        IsVerified = appointment.IsVerified ?? false
                    }).ToList();
                    save.AppointmentReservations = appointmentReservations;
                }

                


                var output = new GetAppointmentListOutput
                {
                    Appointments = saved.Where(a => a.AppointmentReservations.Count > 0).ToList(),
                    Page = input.Page,
                    PerPage = input.PerPage
                };


                return output;
            }
        }

        public GetListActivityOutput GetListActivityFromDb(GetListActivityInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userName = HttpContext.Current.User.Identity.GetUser();

                var savedBookings = GetListActivityQuery.GetInstance()
                    .Execute(conn, new { UserId = userName.Id, Page = input.Page, PerPage = input.PerPage });

                var output = new GetListActivityOutput
                {
                    ActivityList = savedBookings.Select(a => new SearchResult()
                    {
                        Id = a.Id,
                        Name = a.Name,
                        MediaSrc = a.MediaSrc,
                        Price = a.Price
                    }).ToList(),
                    Page = input.Page,
                    PerPage = input.PerPage
                };
                return output;
            }
        }

        public GetAppointmentDetailOutput GetAppointmentDetailFromDb(GetAppointmentDetailInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userName = HttpContext.Current.User.Identity.GetUser();

                string date = input.Date.ToString("yyyy/MM/dd");

                var savedAppointments = GetAppointmentDetailQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId, Date = date, Session = input.Session }, new { Session = input.Session });
                var savedAppointment = savedAppointments.First();

                var appointmentDetail = new AppointmentDetail()
                {
                    ActivityId = savedAppointment.ActivityId,
                    Name = savedAppointment.Name,
                    Date = savedAppointment.Date,
                    Session = savedAppointment.Session,
                    MediaSrc = savedAppointment.MediaSrc,
                };

                foreach (var appointment in savedAppointments.ToList())
                {
                    var savedPassengers = GetPassengersQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = appointment.RsvNo }, null,
                        (passengers, typeCd, titleCd, genderCd) =>
                        {
                            passengers.Type = PaxTypeCd.Mnemonic(typeCd);
                            passengers.Title = TitleCd.Mnemonic(titleCd);
                            passengers.Gender = GenderCd.Mnemonic(genderCd);
                            return passengers;
                        }, "TypeCd, TitleCd, GenderCd").ToList();
                    var contact = Contact.GetFromDb(appointment.RsvNo);
                    var paxgroup = new PaxGroup()
                    {
                        Contact = contact,
                        Passengers = ConvertToPaxForDisplay(savedPassengers)
                    };

                    //appointmentDetail.PaxGroup = paxgroup;
                }

                var output = new GetAppointmentDetailOutput
                {
                    AppointmentDetail = appointmentDetail
                };
                return output;
            }
        }

        internal List<ActivityReservation> GetBookedActivitiesFromDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rsvNos = GetBookedActivitiesRsvNoQuery.GetInstance().Execute(conn, null);
                var reservations = rsvNos.Select(GetReservationFromDb).ToList();
                return reservations;
            }
        }

        #endregion

        #region Insert

        private void InsertActivityRsvToDb(ActivityReservation reservation, decimal originalPrice)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationRecord = new ReservationTableRecord
                {
                    RsvNo = reservation.RsvNo,
                    RsvTime = reservation.RsvTime.ToUniversalTime(),
                    RsvStatusCd = RsvStatusCd.Mnemonic(reservation.RsvStatus),
                    CancellationTypeCd = null,
                    UserId = reservation.User != null ? reservation.User.Id : null,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                };




                var activityDetailNowDb = ActivityTableRepo.GetInstance().Find1(conn, new ActivityTableRecord { Id = reservation.ActivityDetails.ActivityId });
                var activityContentsNowDb = ActivityContentTableRepo.GetInstance().Find(conn, new ActivityContentTableRecord { ActivityId = reservation.ActivityDetails.ActivityId }).ToList();


                var activityRecord = new ActivityReservationTableRecord
                {
                    Id = ActivityReservationIdSequence.GetInstance().GetNext(),
                    RsvNo = reservation.RsvNo,
                    ActivityId = reservation.ActivityDetails.ActivityId,
                    BookingStatusCd = BookingStatusCd.Mnemonic(reservation.ActivityDetails.BookingStatus),
                    Date = reservation.DateTime.Date,
                    SelectedSession = reservation.DateTime.Session,
                    UserId = reservation.User.Id,
                    TicketCount = null,
                    UpdateDate = DateTime.UtcNow,
                };

                var activityDetialReservation = new ActivityDetailReservationTableRecord
                {
                    RsvNo = reservation.RsvNo,
                    ActivityId = activityDetailNowDb.Id,
                    ActivityDuration = activityDetailNowDb.ActivityDuration,
                    ActivityMedia = reservation.ActivityDetails.MediaSrc[0],
                    AdditionalNotes = activityDetailNowDb.AdditionalNotes,
                    Address = activityDetailNowDb.Address,
                    AmountDuration = activityDetailNowDb.AmountDuration,
                    Area = activityDetailNowDb.Area,
                    Cancellation = activityDetailNowDb.Cancellation,
                    Category = activityDetailNowDb.Category,
                    City = activityDetailNowDb.City,
                    Country = activityDetailNowDb.Country,
                    Description = activityDetailNowDb.Description,
                    HasPDFVoucher = activityDetailNowDb.HasPDFVoucher,
                    ImportantNotice = activityDetailNowDb.ImportantNotice,
                    IsDateOfBirthNeeded = activityDetailNowDb.IsDateOfBirthNeeded,
                    IsFixedDate = activityDetailNowDb.IsFixedDate,
                    IsInstantConfirmation = activityDetailNowDb.IsInstantConfirmation,
                    IsNameAccordingToPassport = activityDetailNowDb.IsNameAccordingToPassport,
                    IsPassportIssueDateNeeded = activityDetailNowDb.IsPassportIssueDateNeeded,
                    IsPassportNeeded = activityDetailNowDb.IsPassportNeeded,
                    IsPhoneNumberAccordingToPassport = activityDetailNowDb.IsPhoneNumberAccordingToPassport,
                    IsRedemptionNeeded = activityDetailNowDb.IsRedemptionNeeded,
                    Latitude = activityDetailNowDb.Latitude,
                    Longitude = activityDetailNowDb.Longitude,
                    MustPrinted = activityDetailNowDb.MustPrinted,
                    Name = activityDetailNowDb.Name,
                    OperationTime = activityDetailNowDb.OperationTime,
                    OperatorEmail = activityDetailNowDb.OperatorEmail,
                    OperatorName = activityDetailNowDb.OperatorName,
                    OperatorPhone = activityDetailNowDb.OperatorPhone,
                    PriceDetail = activityDetailNowDb.PriceDetail,
                    Rating = activityDetailNowDb.Rating,
                    RefundRegulationId = activityDetailNowDb.RefundRegulationId,
                    UnitDuration = activityDetailNowDb.UnitDuration,
                    viewCount = activityDetailNowDb.viewCount,
                    Warning = activityDetailNowDb.Warning,
                    Zone = activityDetailNowDb.Zone,
                    HasOperator = activityDetailNowDb.HasOperator
                };

                foreach (var activityContentNow in activityContentsNowDb)
                {
                    var activityContentRsv = new ActivityReservationContentTableRecord
                    {
                        RsvNo = reservation.RsvNo,
                        ActivityId = activityContentNow.ActivityId,
                        Description = activityContentNow.Description,
                        Title = activityContentNow.Title
                    };
                    ActivityReservationContentTableRepo.GetInstance().Insert(conn, activityContentRsv);
                }

                ActivityReservationTableRepo.GetInstance().Insert(conn, activityRecord);
                ReservationTableRepo.GetInstance().Insert(conn, reservationRecord);
                ActivityDetailReservationTableRepo.GetInstance().Insert(conn, activityDetialReservation);
                var package = GetPackagePriceFromDb(reservation.PackageId).ToList();
                foreach (var ticketCount in reservation.TicketCount)
                {
                    var amount = package.Where(a => a.Type == ticketCount.Type).First().Amount;
                    var totalAmount = amount * ticketCount.Count;
                    var activityPackageReservation = new ActivityPackageReservationTableRecord
                    {
                        RsvId = activityRecord.Id,
                        PackageId = reservation.PackageId,
                        ActivityId = reservation.ActivityDetails.ActivityId,
                        Type = ticketCount.Type,
                        Count = ticketCount.Count,
                        RsvNo = reservation.RsvNo,
                        Amount = totalAmount
                    };
                    ActivityPackageReservationTableRepo.GetInstance().Insert(conn, activityPackageReservation);
                }

                reservation.Contact.InsertToDb(reservation.RsvNo);
                reservation.State.InsertToDb(reservation.RsvNo);
                _paymentService.CreateNewPayment(reservation.RsvNo,
                    originalPrice,
                    new Currency(OnlineContext.GetActiveCurrencyCode()));

                if (reservation.Pax != null)
                {
                    foreach (var passenger in reservation.Pax)
                    {

                        var passengerRecord = new PaxTableRecord
                        {
                            Id = PaxIdSequence.GetInstance().GetNext(),
                            RsvNo = reservation.RsvNo,
                            TypeCd = PaxTypeCd.Mnemonic(passenger.Type),
                            GenderCd = GenderCd.Mnemonic(passenger.Gender),
                            TitleCd = TitleCd.Mnemonic(passenger.Title),
                            FirstName = passenger.FirstName,
                            LastName = passenger.LastName,
                            BirthDate = passenger.DateOfBirth.HasValue ? passenger.DateOfBirth.Value.ToUniversalTime() : (DateTime?)null,
                            NationalityCd = passenger.Nationality,
                            PassportNumber = passenger.PassportNumber,
                            PassportExpiryDate = passenger.PassportExpiryDate.HasValue ? passenger.PassportExpiryDate.Value.ToUniversalTime() : (DateTime?)null,
                            PassportCountryCd = passenger.PassportCountry,
                            InsertBy = "LunggoSystem",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "0"
                        };
                        PaxTableRepo.GetInstance().Insert(conn, passengerRecord);
                    }
                }

                InsertStatusHistoryToDb(reservation.RsvNo, reservation.ActivityDetails.BookingStatus);
            }
        }
        #endregion

        #region Update

        private void UpdateRsvStatusDb(string rsvNo, RsvStatus status)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                ReservationTableRepo.GetInstance().Update(conn, new ReservationTableRecord
                {
                    RsvNo = rsvNo,
                    RsvStatusCd = RsvStatusCd.Mnemonic(status)
                });
            }

        }

        private void UpdateActivityDb(ActivityUpdateInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                ActivityTableRepo.GetInstance().Update(conn, new ActivityTableRecord
                {
                    Id = input.ActivityId,
                    Name = input.Name,
                    Category = input.Category,
                    Description = input.ShortDesc,
                    City = input.City,
                    Country = input.Country,
                    Address = input.Address,
                    Latitude = input.Latitude,
                    Longitude = input.Longitude,
                    PriceDetail = input.PriceDetail,
                    AmountDuration = int.Parse(input.Duration.Amount),
                    UnitDuration = input.Duration.Unit,
                    OperationTime = input.OperationTime,
                    //TODO: ImportantNotices, Warning, AdditionalNotes missing
                    Cancellation = input.Cancellation,
                    IsDateOfBirthNeeded = input.IsPaxDoBNeeded,
                    IsPassportIssueDateNeeded = input.IsPassportIssuedDateNeeded,
                    IsPassportNeeded = input.IsPassportNumberNeeded
                });
                UpdatePriceQuery.GetInstance().Execute(conn, new { Price = input.Price, ActivityId = input.ActivityId });
            }

        }
        #endregion

        private void UpdateActivityBookingStatusInDb(string rsvNo, BookingStatus bookingStatus)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var bookingStatusCd = BookingStatusCd.Mnemonic(bookingStatus);
                UpdateActivityBookingStatusQuery.GetInstance().Execute(conn, new { rsvNo, bookingStatusCd, updateDate = DateTime.UtcNow });
            }
        }

        public AddToWishlistOutput InsertActivityIdToWishlistDb(long activityId, string user)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var wishlistRecord = new WishlistTableRecord
                {
                    UserId = user,
                    ActivityId = activityId
                };
                WishlistTableRepo.GetInstance().Insert(conn, wishlistRecord);
            }
            var response = new AddToWishlistOutput { isSuccess = true };
            return response;
        }

        public void InsertStatusHistoryToDb(string rsvNo, BookingStatus bookingStatus)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var bookingStatusCd = BookingStatusCd.Mnemonic(bookingStatus);
                var statusHistory = new ReservationStatusHistoryTableRecord
                {
                    BookingStatusCd = bookingStatusCd,
                    RsvNo = rsvNo,
                    TimeStamp = DateTime.UtcNow
                };
                ReservationStatusHistoryTableRepo.GetInstance().Insert(conn, statusHistory);
            }
        }

        public DeleteFromWishlistOutput DeleteActivityIdFromWishlistDb(long activityId, string user)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var wishlistRecord = new WishlistTableRecord
                {
                    UserId = user,
                    ActivityId = activityId
                };
                WishlistTableRepo.GetInstance().Delete(conn, wishlistRecord);
            }
            var response = new DeleteFromWishlistOutput { isSuccess = true };
            return response;
        }

        public List<long> GetActivityListFromWishlistDb(string user)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var activityList = GetActivityListFromWishlistDbQuery.GetInstance().Execute(conn, new { user });

                var response = activityList.ToList();
                //List<int>responseInt = response.Select(i => (int)i).ToList();
                return response;
            }
        }

        public ActivityAddSessionOutput InsertActivitySessionToDb(ActivityAddSessionInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (input.RegularAvailableDates == null)
                {
                    return new ActivityAddSessionOutput { isSuccess = false };
                }

                foreach (var date in input.RegularAvailableDates)
                {
                    if (date.AvailableHours != null)
                    {
                        foreach (var availableHours in date.AvailableHours)
                        {
                            var activityAddSessionRecord = new ActivityRegularDateTableRecord
                            {
                                ActivityId = input.ActivityId,
                                AvailableDay = date.Day,
                                AvailableHour = availableHours
                            };
                            ActivityRegularDateTableRepo.GetInstance().Insert(conn, activityAddSessionRecord);
                        }
                    }
                    else
                    {
                        var activityAddSessionRecord = new ActivityRegularDateTableRecord
                        {
                            ActivityId = input.ActivityId,
                            AvailableDay = date.Day,
                            AvailableHour = ""
                        };
                        ActivityRegularDateTableRepo.GetInstance().Insert(conn, activityAddSessionRecord);
                    }
                }
            }
            var response = new ActivityAddSessionOutput { isSuccess = true };
            return response;
        }

        public ActivityDeleteSessionOutput DeleteActivitySessionFromDb(ActivityDeleteSessionInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (input.RegularAvailableDates == null)
                {
                    return new ActivityDeleteSessionOutput { isSuccess = false };
                }

                foreach (var date in input.RegularAvailableDates)
                {
                    if (date.AvailableHours == null)
                    {
                        var ActivityId = input.ActivityId;
                        var AvailableDay = date.Day;
                        DeleteDaySessionDbQuery.GetInstance().Execute(conn, new { ActivityId, AvailableDay });
                    }
                    else
                    {
                        foreach (var availableHours in date.AvailableHours)
                        {
                            var activityDeleteSessionRecord = new ActivityRegularDateTableRecord
                            {
                                ActivityId = input.ActivityId,
                                AvailableDay = date.Day,
                                AvailableHour = availableHours ?? "*"
                            };
                            ActivityRegularDateTableRepo.GetInstance().Delete(conn, activityDeleteSessionRecord);
                        }
                    }
                }
            }
            var response = new ActivityDeleteSessionOutput { isSuccess = true };
            return response;
        }

        public GetActivityTicketDetailOutput GetActivityTicketDetailFromDb(GetDetailActivityInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var result = new List<ActivityPackage>();
                var packageIds = ActivityService.GetInstance().GetPackageIdFromDb(input.ActivityId);
                foreach (var packageId in packageIds)
                {
                    var packageAttribute = ActivityService.GetInstance().GetPackageAttributeFromDb(packageId);
                    var packagePrice = ActivityService.GetInstance().GetPackagePriceFromDb(packageId);
                    var package = new ActivityPackage
                    {
                        PackageId = packageId,
                        PackageName = packageAttribute[0].PackageName,
                        Description = packageAttribute[0].Description,
                        MaxCount = packageAttribute[0].MaxCount,
                        MinCount = packageAttribute[0].MinCount,
                        Price = packagePrice,
                    };
                    result.Add(package);
                }
                var output = new GetActivityTicketDetailOutput
                {
                    Package = result
                };
                return output;
            }
        }


        public List<long> GetPackageIdFromDb(long? ActivityId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                return GetActivityPackageIdDbQuery.GetInstance().Execute(conn, new { ActivityId = ActivityId }).ToList();
            }
        }

        public List<ActivityPricePackage> GetPackagePriceFromDb(long PackageId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                return GetActivityTicketDetailDbQuery.GetInstance().Execute(conn, new { PackageId = PackageId }).ToList();
            }
        }

        public List<ActivityPackage> GetPackageAttributeFromDb(long PackageId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                return GetActivityPackageDbQuery.GetInstance().Execute(conn, new { PackageId = PackageId }).ToList();
            }

        }
        public ActivityCustomDateOutput ActivityCustomDateSetOrUnsetDb(ActivityCustomDateInput activityCustomDateInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (activityCustomDateInput.CustomDate == null || activityCustomDateInput.CustomHour == null || activityCustomDateInput.ActivityId == null)
                {
                    return new ActivityCustomDateOutput
                    {
                        isSuccess = false
                    };
                }
                var regularDates = GetAvailableDatesFromDb(new GetAvailableDatesInput { ActivityId = (int)activityCustomDateInput.ActivityId }).AvailableDateTimes;
                var customDateStatus = GetStatusDateDbQuery.GetInstance().Execute(conn, new { ActivityId = activityCustomDateInput.ActivityId, CustomDate = activityCustomDateInput.CustomDate, CustomHour = activityCustomDateInput.CustomHour }).ToList();
                if (customDateStatus.Count() > 0)
                {
                    DeleteCustomDateDbQuery.GetInstance().Execute(conn, new { ActivityId = activityCustomDateInput.ActivityId, CustomDate = activityCustomDateInput.CustomDate, AvailableHour = activityCustomDateInput.CustomHour });
                    return new ActivityCustomDateOutput
                    {
                        isSuccess = true
                    };
                }
                foreach (var regularDate in regularDates)
                {
                    if (regularDate.Date == activityCustomDateInput.CustomDate && regularDate.AvailableHours.Contains(activityCustomDateInput.CustomHour))
                    {
                        var blacklist = new ActivityCustomDateTableRecord
                        {
                            ActivityId = activityCustomDateInput.ActivityId,
                            AvailableHour = activityCustomDateInput.CustomHour,
                            CustomDate = activityCustomDateInput.CustomDate,
                            DateStatus = "blacklisted"
                        };
                        ActivityCustomDateTableRepo.GetInstance().Insert(conn, blacklist);

                        return new ActivityCustomDateOutput
                        {
                            isSuccess = true
                        };
                    }
                }
                var whitelist = new ActivityCustomDateTableRecord
                {
                    ActivityId = activityCustomDateInput.ActivityId,
                    AvailableHour = activityCustomDateInput.CustomHour,
                    CustomDate = activityCustomDateInput.CustomDate,
                    DateStatus = "whitelisted"
                };
                ActivityCustomDateTableRepo.GetInstance().Insert(conn, whitelist);
                return new ActivityCustomDateOutput
                {
                    isSuccess = true
                };
            }
        }

        public CustomDateOutput AddCustomDateToDb(CustomDateInput customDateInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {

                if (customDateInput == null || customDateInput.CustomDate == null)
                {
                    return new CustomDateOutput
                    {
                        isSuccess = false
                    };
                }

                DateTime startHour;
                DateTime endHour;
                if (!string.IsNullOrWhiteSpace(customDateInput.StartCustomHour) && !DateTime.TryParse(customDateInput.StartCustomHour, out startHour))
                {
                    return new CustomDateOutput
                    {
                        isSuccess = false
                    };
                }
                if (!string.IsNullOrWhiteSpace(customDateInput.EndCustomHour) && !DateTime.TryParse(customDateInput.EndCustomHour, out endHour))
                {
                    return new CustomDateOutput
                    {
                        isSuccess = false
                    };
                }
                var customDate = new ActivityCustomDateTableRecord();
                customDate.ActivityId = customDateInput.ActivityId;
                customDate.CustomDate = customDateInput.CustomDate;
                customDate.DateStatus = "whitelisted";
                if (string.IsNullOrWhiteSpace(customDateInput.StartCustomHour) || string.IsNullOrWhiteSpace(customDateInput.EndCustomHour))
                {
                    customDate.AvailableHour = "";
                }
                else
                {
                    customDate.AvailableHour = customDateInput.StartCustomHour + " - " + customDateInput.EndCustomHour;
                }



                ActivityCustomDateTableRepo.GetInstance().Insert(conn, customDate);
                return new CustomDateOutput
                {
                    isSuccess = true
                };
            }

        }

        public CustomDateOutput DeleteCustomDateFromDb(CustomDateInput customDateInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (customDateInput == null || customDateInput.CustomDate == null)
                {
                    return new CustomDateOutput
                    {
                        isSuccess = false
                    };
                }

                var customDate = new ActivityCustomDateTableRecord
                {
                    ActivityId = customDateInput.ActivityId,
                    CustomDate = customDateInput.CustomDate
                };
                ActivityCustomDateTableRepo.GetInstance().Delete(conn, customDate);
                return new CustomDateOutput
                {
                    isSuccess = true
                };
            }
        }

        public CartList GetCartListDataFromDb(string cartId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                decimal totalOriginalPrice = 0;
                decimal totalFinalPrice = 0;
                decimal totalDiscount = 0;
                decimal totalUniqueCode = 0;
                var bookingDetails = new List<BookingDetail>();
                var rsvNoList = GetCartRsvNoListDbQuery.GetInstance().Execute(conn, new { TrxId = cartId }).ToList();
                foreach (var rsvNo in rsvNoList)
                {
                    var payment = GetReservationFromDb(rsvNo).Payment;
                    var bookingDetail = GetMyBookingDetailFromDb(new GetMyBookingDetailInput { RsvNo = rsvNo });
                    bookingDetail.BookingDetail.RsvNo = rsvNo;
                    bookingDetails.Add(bookingDetail.BookingDetail);
                    totalOriginalPrice += payment.OriginalPriceIdr;
                    totalDiscount += payment.DiscountNominal;
                    totalUniqueCode += payment.UniqueCode;
                    totalFinalPrice += payment.FinalPriceIdr;
                }
                var cartList = new CartList
                {
                    CartId = cartId,
                    Activities = bookingDetails,
                    TotalOriginalPrice = totalOriginalPrice,
                    TotalDiscount = totalDiscount,
                    TotalUniqueCode = totalUniqueCode,
                    TotalFinalPrice = totalFinalPrice
                };
                return cartList;
            }
        }
        public List<string> GetCartRsvNoListFromDb(string cartId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                return GetCartRsvNoListDbQuery.GetInstance().Execute(conn, new { TrxId = cartId }).ToList();
            }

        }

        public InsertRatingAndReviewOutput InsertRatingAndReviewToDb(InsertRatingAndReviewInput insertRatingAndReviewInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var activityId = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = insertRatingAndReviewInput.RsvNo }).BookingDetail.ActivityId;
                //var insertParam = new ActivityRatingAndReviewTableRecord
                //{
                //    UserId = insertRatingAndReviewInput.UserId,
                //    ActivityId = activityId,
                //    Rating = insertRatingAndReviewInput.Rating,
                //    Review = insertRatingAndReviewInput.Review,
                //    Date = DateTime.UtcNow
                //};
                //
                //ActivityRatingAndReviewTableRepo.GetInstance().Insert(conn, insertParam);
                return new InsertRatingAndReviewOutput
                {
                    isSuccess = true
                };
            }

        }

        public List<ActivityReview> GetReviewFromDb(long? activityId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                List<ActivityReview> reviews = new List<ActivityReview>();
                var preReviews = GetActivityReviewFromDbQuery.GetInstance().Execute(conn, new { ActivityId = activityId }).ToList();
                foreach (var preReview in preReviews)
                {
                    var preUser = GetUserByIdQuery.GetInstance().Execute(conn, new { Id = preReview.UserId });
                    if (preUser.Count() == 0)
                    {
                        return reviews;
                    }
                    var user = preUser.ToList()[0];
                    var review = new ActivityReview
                    {
                        Name = user.FirstName + " " + user.LastName,
                        Content = preReview.Review,
                        Date = preReview.DateTime
                    };
                    reviews.Add(review);
                }
                return reviews;
            }

        }

        public InsertActivityRatingOutput InsertActivityRatingToDb(InsertActivityRatingInput insertActivityRatingInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                foreach (var answer in insertActivityRatingInput.Answers)
                {
                    var activityRatingInputData = new ActivityRatingTableRecord
                    {
                        ActivityId = insertActivityRatingInput.ActivityId,
                        Date = answer.Date,
                        Question = answer.Question,
                        Rating = answer.Rate,
                        RsvNo = insertActivityRatingInput.RsvNo,
                        UserId = insertActivityRatingInput.UserId
                    };
                    ActivityRatingTableRepo.GetInstance().Insert(conn, activityRatingInputData);
                }

                return new InsertActivityRatingOutput
                {
                    isSuccess = true
                };
            }
        }

        public void InsertActivityReviewToDb(InsertActivityReviewInput insertActivityReviewInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (string.IsNullOrWhiteSpace(insertActivityReviewInput.Review))
                {

                }
                else
                {
                    var activityReviewInputData = new ActivityReviewTableRecord
                    {
                        ActivityId = insertActivityReviewInput.ActivityId,
                        Date = insertActivityReviewInput.Date,
                        Review = insertActivityReviewInput.Review,
                        RsvNo = insertActivityReviewInput.RsvNo,
                        UserId = insertActivityReviewInput.UserId
                    };
                    ActivityReviewTableRepo.GetInstance().Insert(conn, activityReviewInputData);
                }
            }
        }

        public bool CheckReview(string rsvNo, DateTime date, string session)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                DateTime dateCheck;
                if (!string.IsNullOrWhiteSpace(session))
                {
                    var stringSessionHour = session.Substring(8);
                    var sessionHour = DateTime.Parse(stringSessionHour);
                    dateCheck = date.Date.Add(sessionHour.TimeOfDay);
                }
                else
                {
                    TimeSpan time = new TimeSpan(23, 59, 59);
                    dateCheck = date.Date.Add(time);
                }
                var reviews = GetReviewFromDbByRsvNoQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).ToList();
                if (reviews.Count == 0 && DateTime.UtcNow > dateCheck)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CheckRating(string rsvNo, DateTime date, string session)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                DateTime dateCheck;
                if (!string.IsNullOrWhiteSpace(session))
                {
                    var stringSessionHour = session.Substring(8);
                    var sessionHour = DateTime.Parse(stringSessionHour);
                    dateCheck = date.Date.Add(sessionHour.TimeOfDay);
                }
                else
                {
                    TimeSpan time = new TimeSpan(23, 59, 59);
                    dateCheck = date.Date.Add(time);
                }
                var rating = GetRatingFromDbByRsvNoQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).ToList();
                if (rating.Count() == 0 && DateTime.UtcNow > dateCheck)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void UpdateRsvNoPdfFlag(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UpdateRsvNoPdfFlagQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo });
            }
        }

        public bool InsertTransactionStatementToDb(InsertTransactionStatementInput dbInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                InsertTransactionStatementToDbQuery.GetInstance().Execute(conn, new { TrxNo = dbInput.TrxNo, Remarks = dbInput.Remarks, DateTime = dbInput.DateTime, Amount = dbInput.Amount, OperatorId = dbInput.OperatorId });
                return true;
            }
        }

        public GetTransactionStatementOutput GetTransactionStatementFromDb(string operatorId, DateTime startDate, DateTime endDate)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var transactionStatement = GetTransactionStatementOutputFromDbQuery.GetInstance().Execute(conn, new { OperatorId = operatorId, StartDate = startDate.Date, EndDate = endDate.Date }).ToList();
                var getTransactionStatementOutput = new GetTransactionStatementOutput
                {
                    TransactionStatements = transactionStatement
                };
                return getTransactionStatementOutput;
            }
        }

        public void UpdateTicketNumberReservationDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rsvCode = long.Parse(rsvNo).Base36Encode();
                var random = new Random();
                var randomNumberCode = ((long)random.Next(0, 1296)).Base36Encode();
                while (randomNumberCode.Length < 2)
                {
                    randomNumberCode = "0" + randomNumberCode;
                }
                var ticketNumber = rsvCode + randomNumberCode;
                UpdateTicketNumberReservationDbQuery.GetInstance().Execute(conn, new { TicketNumber = ticketNumber, RsvNo = rsvNo });
            }
        }

        public GetUserByAnyQueryRecord GetUserByIdFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetUserByIdQuery.GetInstance().Execute(conn, new { Id = userId }).ToList();
                if (user.Count == 0)
                {
                    return null;
                }
                return user.First();
            }
        }

        public void GeneratePayStepOperatorDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationRecord = ActivityReservationTableRepo.GetInstance().Find1(conn,
                    new ActivityReservationTableRecord { RsvNo = rsvNo });
                var reservationRecord2 = ReservationTableRepo.GetInstance().Find1(conn, new ReservationTableRecord { RsvNo = rsvNo });
                var activityRules = ActivityPayRulesOperatorTableRepo.GetInstance().Find(conn, new ActivityPayRulesOperatorTableRecord { ActivityId = reservationRecord.ActivityId }).ToList();
                var rsvPayment = _paymentService.GetPaymentDetails(rsvNo).OriginalPriceIdr;
                if (activityRules[0].IsCash == true)
                {
                    var stepDate = new DateTime();
                    if (activityRules[0].PayState == "Jalan")
                    {
                        stepDate = (DateTime)reservationRecord.Date;
                        stepDate = stepDate.AddDays((double)activityRules[0].PayDateLimit);
                    }
                    else if (activityRules[0].PayState == "Book")
                    {
                        stepDate = (DateTime)reservationRecord2.RsvTime;
                        stepDate = stepDate.AddDays((double)activityRules[0].PayDateLimit);
                    }
                    if (stepDate < DateTime.UtcNow)
                    {
                        stepDate = DateTime.UtcNow;
                    }
                    var tableRecord = new ActivityReservationStepOperatorTableRecord
                    {
                        RsvNo = rsvNo,
                        ActivityId = reservationRecord.ActivityId,
                        StepAmount = rsvPayment,
                        StepDate = stepDate,
                        StepDescription = activityRules[0].Description,
                        StepName = activityRules[0].RuleName,
                        StepStatus = false
                    };
                    ActivityReservationStepOperatorTableRepo.GetInstance().Insert(conn, tableRecord);
                }
                else
                {
                    var sisaAmount = rsvPayment;
                    foreach (var activityRule in activityRules)
                    {
                        var stepDate = new DateTime();
                        if (activityRule.PayState == "Jalan")
                        {
                            stepDate = (DateTime)reservationRecord.Date;
                            stepDate = stepDate.AddDays((double)activityRule.PayDateLimit);
                        }
                        else if (activityRule.PayState == "Book")
                        {
                            stepDate = (DateTime)reservationRecord2.RsvTime;
                            stepDate = stepDate.AddDays((double)activityRule.PayDateLimit);
                        }
                        if (stepDate < DateTime.UtcNow)
                        {
                            stepDate = DateTime.UtcNow;
                        }
                        decimal stepAmount = 0;
                        if (activityRule.ValuePercentage != null || activityRule.ValueConstant != null)
                        {
                            var valuePercentage = activityRule.ValuePercentage ?? 0M;
                            valuePercentage = valuePercentage / 100M;
                            var valueConstant = activityRule.ValueConstant ?? 0M;

                            stepAmount = valuePercentage * rsvPayment + valueConstant;
                            if (activityRule.MinValue != null && stepAmount < activityRule.MinValue)
                            {
                                var minValue = activityRule.MinValue ?? 0M;
                                stepAmount = minValue;
                            }
                        }
                        else if (activityRule.MinValue != null)
                        {
                            var minValue = activityRule.MinValue ?? 0M;
                            stepAmount = minValue;
                        }
                        else
                        {
                            stepAmount = sisaAmount;
                        }

                        {
                            var tableRecord = new ActivityReservationStepOperatorTableRecord
                            {
                                RsvNo = rsvNo,
                                ActivityId = reservationRecord.ActivityId,
                                StepAmount = stepAmount,
                                StepDate = stepDate,
                                StepDescription = activityRule.Description,
                                StepName = activityRule.RuleName,
                                StepStatus = false
                            };
                            ActivityReservationStepOperatorTableRepo.GetInstance().Insert(conn, tableRecord);
                        }

                        sisaAmount = sisaAmount - stepAmount;
                    }
                }

            }
        }

        public GetReservationListOutput GetReservationListFromDb(GetAppointmentListInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationsOutput = new List<Reservation>();
                var userName = HttpContext.Current.User.Identity.GetUser();

                var savedBookings = GetAppointmentListQuery.GetInstance()
                    .Execute(conn, new { UserId = userName.Id, Page = input.Page, PerPage = input.PerPage, DateLimit = DateTime.UtcNow }).ToList();

                foreach (var savedBooking in savedBookings)
                {
                    var savedPassengers = GetPassengersQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = savedBooking.RsvNo, }, null,
                       (passengers, typeCd, titleCd, genderCd) =>
                       {
                           passengers.Type = PaxTypeCd.Mnemonic(typeCd);
                           passengers.Title = TitleCd.Mnemonic(titleCd);
                           passengers.Gender = GenderCd.Mnemonic(genderCd);
                           return passengers;
                       }, "TypeCd, TitleCd, GenderCd").ToList();
                    var contact = Contact.GetFromDb(savedBooking.RsvNo);
                    var savedPaxCountAndPackageId = GetPaxCountAndPackageIdDbQuery.GetInstance().Execute(conn, new { RsvNo = savedBooking.RsvNo }).ToList();
                    if (savedPaxCountAndPackageId.Count() != 0)
                    {
                        savedBooking.PackageId = savedPaxCountAndPackageId.First().PackageId;
                        savedBooking.PackageName = savedPaxCountAndPackageId.First().PackageName;
                    }
                    var savedPaxCounts = new List<ActivityPricePackageReservation>();
                    var pricePackages = ActivityService.GetInstance().GetPackagePriceFromDb(savedBooking.PackageId);
                    foreach (var savedPaxCount in savedPaxCountAndPackageId)
                    {
                        var saveds = new ActivityPricePackageReservation();
                        saveds.Type = savedPaxCount.Type;
                        saveds.Count = savedPaxCount.Count;
                        var amountType = pricePackages.Where(package => package.Type == savedPaxCount.Type);
                        if (amountType.Count() != 0)
                        {
                            saveds.TotalPrice = savedPaxCount.Count * amountType.First().Amount;
                        }
                        savedPaxCounts.Add(saveds);
                    }

                    var reservationRecord = ReservationTableRepo.GetInstance()
                    .Find1(conn, new ReservationTableRecord { RsvNo = savedBooking.RsvNo });
                    var rsvTime = (DateTime)reservationRecord.RsvTime;
                    var paymentStepsDb = ActivityReservationStepOperatorTableRepo.GetInstance().Find(conn, new ActivityReservationStepOperatorTableRecord() { RsvNo = savedBooking.RsvNo }).ToList();
                    var paymentSteps = paymentStepsDb.Select(a => new PaymentStep { StepDescription = a.StepDescription, StepAmount = a.StepAmount ?? 0, StepDate = a.StepDate }).ToList();

                    var activityReservation = new ReservationActivityDetail
                    {
                        ActivityId = savedBooking.ActivityId,
                        Date = savedBooking.Date,
                        MediaSrc = savedBooking.MediaSrc,
                        Name = savedBooking.Name,
                        Session = savedBooking.Session
                    };

                    var reservation = new Reservation
                    {
                        Activity = activityReservation,
                        Contact = contact,
                        Passengers = ActivityService.GetInstance().ConvertToPaxForDisplay(savedPassengers),
                        PaxCounts = savedPaxCounts,
                        PaymentSteps = paymentSteps,
                        RsvNo = savedBooking.RsvNo,
                        RsvTime = rsvTime,
                    };

                    reservationsOutput.Add(reservation);
                }

                var rsvOutput = reservationsOutput.OrderBy(a => a.RsvTime).ToList();

                var output = new GetReservationListOutput
                {
                    Reservations = rsvOutput,
                    Page = input.Page,
                    PerPage = input.PerPage
                };


                return output;
            }
        }

        public bool VerifyTicketNumberDb(string ticketNumber, string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationTickets = ActivityReservationTableRepo.GetInstance().Find(conn, new ActivityReservationTableRecord
                {
                    TicketNumber = ticketNumber,
                }).ToList();
                if (reservationTickets.Count == 0)
                {
                    return false;
                }
                else
                {
                    var reservation = reservationTickets.Where(a => a.RsvNo == rsvNo).ToList();
                    if (reservation.Count == 0)
                    {
                        return false;
                    }
                    UpdateIsVerifiedDbQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo });
                    return true;
                }
            }
        }

        public void InsertRefundAmountOperatorToDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservation = GetReservationFromDb(rsvNo);
                var rules = RefundRulesOperatorTableRepo.GetInstance().Find(conn, new RefundRulesOperatorTableRecord
                {
                    ActivityId = reservation.ActivityDetails.ActivityId
                }).ToList();
                var selectedRules = new List<RefundRulesOperatorTableRecord>();
                var jalanRules = rules.Where(a => a.PayState == "Jalan" && (DateTime.UtcNow < reservation.DateTime.Date.Value && DateTime.UtcNow > reservation.DateTime.Date.Value.AddDays((double)a.PayDateLimit))).ToList();
                if (jalanRules.Count > 0)
                {
                    selectedRules = jalanRules.OrderBy(a => a.PayDateLimit).ToList();
                }
                else
                {
                    var bookRules = rules.Where(a => a.PayState == "Book" && (DateTime.UtcNow > reservation.RsvTime && DateTime.UtcNow < reservation.RsvTime.AddDays((double)a.PayDateLimit))).ToList();
                    selectedRules = bookRules.OrderByDescending(a => a.PayDateLimit).ToList();
                }
                if (selectedRules.Count < 1)
                {
                    selectedRules = rules.Where(a => a.PayState == "Treshold").ToList();
                }
                var selectedRule = selectedRules.First();
                var amountsReservation = ActivityReservationStepOperatorTableRepo.GetInstance().Find(conn, new ActivityReservationStepOperatorTableRecord
                {
                    RsvNo = rsvNo,
                }).ToList();
                var amounts = amountsReservation.Where(a => a.StepStatus == true).ToList();
                decimal refundAmount = 0;

                if (amounts.Count > 0)
                {
                    var amount = amounts.Sum(a => a.StepAmount);
                    refundAmount = (decimal)(selectedRule.ValuePercentage * amount + selectedRule.ValueConstant);
                    if (refundAmount < selectedRule.MinValue)
                    {
                        refundAmount = (decimal)selectedRule.MinValue;
                    }
                }

                var rule = new RefundHistoryTableRecord()
                {
                    ActivityId = reservation.ActivityDetails.ActivityId,
                    RsvNo = rsvNo,
                    RefundAmount = refundAmount,
                    RefundDate = DateTime.UtcNow.AddMonths(1),
                    RefundDescription = selectedRule.Description,
                    RefundName = selectedRule.RuleName,
                    RefundStatus = false
                };
                RefundHistoryTableRepo.GetInstance().Insert(conn, rule);
            }
        }

        public void InsertRefundAmountCancelByOperatorToDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservation = GetReservation(rsvNo);
                var amountReservations = ActivityReservationStepOperatorTableRepo.GetInstance().Find(conn, new ActivityReservationStepOperatorTableRecord
                {
                    RsvNo = rsvNo
                }).ToList();
                var amounts = amountReservations.Where(a => a.StepStatus == true).ToList();
                decimal refundAmount = 0;
                if (amounts.Count > 0)
                {
                    refundAmount = (decimal)amounts.Sum(a => a.StepAmount);
                }

                var rule = new RefundHistoryTableRecord()
                {
                    ActivityId = reservation.ActivityDetails.ActivityId,
                    RsvNo = rsvNo,
                    RefundAmount = refundAmount,
                    RefundDate = DateTime.UtcNow.AddMonths(1),
                    RefundDescription = "Cancelled By Operator",
                    RefundName = "Cancelled By Operator",
                    RefundStatus = false
                };
                RefundHistoryTableRepo.GetInstance().Insert(conn, rule);
            }
        }

        public List<PendingRefund> GetPendingRefundsFromDb(GetPendingRefundsInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userName = HttpContext.Current.User.Identity.GetUser();
                var refundHistories = GetRefundPendingFromDbQuery.GetInstance()
                    .Execute(conn, new { UserId = userName.Id, Page = input.Page, PerPage = input.PerPage, StartDate = input.StartDate, EndDate = input.EndDate }).ToList();
                var savedBookings = refundHistories.Select(a => new AppointmentDetail { RsvNo = a.RsvNo, RefundAmount = a.RefundAmount, RefundDate = a.RefundDate }).ToList();
                foreach (var savedBooking in savedBookings)
                {
                    var savedPassengers = GetPassengersQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = savedBooking.RsvNo, }, null,
                        (passengers, typeCd, titleCd, genderCd) =>
                        {
                            passengers.Type = PaxTypeCd.Mnemonic(typeCd);
                            passengers.Title = TitleCd.Mnemonic(titleCd);
                            passengers.Gender = GenderCd.Mnemonic(genderCd);
                            return passengers;
                        }, "TypeCd, TitleCd, GenderCd").ToList();
                    var contact = Contact.GetFromDb(savedBooking.RsvNo);
                    var savedPaxCountAndPackageId = GetPaxCountAndPackageIdDbQuery.GetInstance().Execute(conn, new { RsvNo = savedBooking.RsvNo }).ToList();
                    if (savedPaxCountAndPackageId.Count() != 0)
                    {
                        savedBooking.PackageId = savedPaxCountAndPackageId.First().PackageId;
                        savedBooking.PackageName = savedPaxCountAndPackageId.First().PackageName;
                    }
                    var savedPaxCounts = new List<ActivityPricePackageReservation>();
                    var pricePackages = ActivityService.GetInstance().GetPackagePriceFromDb(savedBooking.PackageId);
                    foreach (var savedPaxCount in savedPaxCountAndPackageId)
                    {
                        var saveds = new ActivityPricePackageReservation();
                        saveds.Type = savedPaxCount.Type;
                        saveds.Count = savedPaxCount.Count;
                        var amountType = pricePackages.Where(package => package.Type == savedPaxCount.Type);
                        if (amountType.Count() != 0)
                        {
                            saveds.TotalPrice = savedPaxCount.Count * amountType.First().Amount;
                        }
                        savedPaxCounts.Add(saveds);
                    }
                    var paxGroup = new PaxGroup();
                    paxGroup.Passengers = ActivityService.GetInstance().ConvertToPaxForDisplay(savedPassengers);
                    paxGroup.Contact = contact;
                    paxGroup.PaxCounts = savedPaxCounts;
                    savedBooking.PaxGroup = paxGroup;
                    var reservationRecord = ReservationTableRepo.GetInstance()
                    .Find1(conn, new ReservationTableRecord { RsvNo = savedBooking.RsvNo });
                    savedBooking.RequestTime = (DateTime)reservationRecord.RsvTime;
                    var activityDetail = ActivityDetailReservationTableRepo.GetInstance().Find1(conn, new ActivityDetailReservationTableRecord
                    {
                        RsvNo = savedBooking.RsvNo
                    });
                    var activityReservation = ActivityReservationTableRepo.GetInstance().Find1(conn, new ActivityReservationTableRecord { RsvNo = savedBooking.RsvNo });

                    savedBooking.Name = activityDetail.Name;
                    savedBooking.MediaSrc = activityDetail.ActivityMedia;
                    savedBooking.Date = activityReservation.Date.Value;
                    savedBooking.Session = activityReservation.SelectedSession;
                    savedBooking.RsvStatus = activityReservation.BookingStatusCd;
                    var paymentStepsDb = ActivityReservationStepOperatorTableRepo.GetInstance().Find(conn, new ActivityReservationStepOperatorTableRecord() { RsvNo = savedBooking.RsvNo }).ToList();
                    var paymentSteps = paymentStepsDb.Select(a => new PaymentStep { StepDescription = a.StepDescription, StepAmount = a.StepAmount ?? 0, StepDate = a.StepDate, StepStatus = (a.StepStatus.Value) ? "PAID" : "PENDING" }).ToList();
                    var cancelStatus = savedBooking.RsvStatus.Substring(0, 6);
                    if (cancelStatus == "Cancel")
                    {
                        var newPaymentSteps = paymentSteps.Where(a => a.StepStatus == "PAID").ToList();
                        var halfStatusHistory = ReservationStatusHistoryTableRepo.GetInstance().Find(conn, new ReservationStatusHistoryTableRecord
                        {
                            RsvNo = savedBooking.RsvNo
                        }).ToList();
                        var statusHistory = halfStatusHistory.Where(a => a.BookingStatusCd == savedBooking.RsvStatus).ToList();
                        var amount = RefundHistoryTableRepo.GetInstance().Find1(conn, new RefundHistoryTableRecord
                        {
                            RsvNo = savedBooking.RsvNo
                        }).RefundAmount;

                        newPaymentSteps.Add(new PaymentStep
                        {
                            StepDescription = "Cancel",
                            StepDate = statusHistory[0].TimeStamp,
                            StepAmount = (amount.HasValue ? amount.Value : 0) * -1M,
                            StepStatus = "CANCEL",
                        });
                        paymentSteps = newPaymentSteps;
                    }
                    savedBooking.PaymentSteps = paymentSteps;
                }

                var refundsHalf = savedBookings.Where(a => a.RefundAmount > 0).ToList();
                if (refundsHalf.Count < 1)
                {
                    return new List<PendingRefund>();
                }
                var refunds = savedBookings.Select(appointment => new PendingRefund
                {
                    RsvNo = appointment.RsvNo,
                    Contact = appointment.PaxGroup.Contact,
                    PaxCount = appointment.PaxGroup.PaxCounts,
                    PaymentSteps = appointment.PaymentSteps,
                    ActivityDate = appointment.Date,
                    ActivityName = appointment.Name,
                    MediaSrc = appointment.MediaSrc,
                    RefundAmount = appointment.RefundAmount.Value,
                    Session = appointment.Session,
                    RefundDate = appointment.RefundDate.Value
                });
                var output = refunds.OrderBy(a => a.RefundDate).ToList();
                return output;
            }
        }

        public string GetUserIdByRsvNo(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var activityReservation = ActivityReservationTableRepo.GetInstance().Find1(conn, new ActivityReservationTableRecord { RsvNo = rsvNo });
                return activityReservation.UserId;
            }

        }

        public string GetOperatorIdByActivityId(long activityId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var operatorId = OperatorTableRepo.GetInstance()
                    .Find1(conn, new OperatorTableRecord {ActivityId = activityId});
                return operatorId.UserId;
            }
        }

        public void DecreasePaxSlotFromDb(long activityId, int ticketCount, DateTime date, string session)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var oldPaxSlotTableRecord = new ActivityCustomDateTableRecord
                {
                    ActivityId = activityId,
                    AvailableHour = session ?? "",
                    CustomDate = date
                };
                var oldPaxSlot = ActivityCustomDateTableRepo.GetInstance().Find(conn,oldPaxSlotTableRecord).First().PaxSlot;
               
                var newPaxSlot = oldPaxSlot - ticketCount;
                var newPaxSlotTableRecord = new ActivityCustomDateTableRecord
                {
                    ActivityId = activityId,
                    AvailableHour = session ?? "",
                    CustomDate = date,
                    PaxSlot = newPaxSlot,
                    DateStatus = "whitelisted"
                };
                ActivityCustomDateTableRepo.GetInstance().Update(conn,newPaxSlotTableRecord);
            }
        }
    }
}