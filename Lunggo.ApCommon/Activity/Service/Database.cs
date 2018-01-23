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

                activityDetail.Package = GetActivityTicketDetailFromDb(input).Package;
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
                var blacklistedDates = GetBlacklistedDateDbQuery.GetInstance().Execute(conn, new { ActivityId = input.ActivityId }).ToList();
                var allDates = new List<DateTime>();
                var result = new List<DateAndAvailableHour>();
                var allAvailableDates = new List<DateTime>();
                var startDate = DateTime.Today;
                var endDate = startDate.AddMonths(3);
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    allDates.Add(date);
                }
                var savedDay = GetAvailableDatesQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId });
                var availableDays = savedDay.ToList();
                var dayEnum = new List<DayOfWeek>();
                foreach(var day in availableDays)
                {
                    DayOfWeek myDays = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day);
                    dayEnum.Add(myDays);
                }

                foreach (var date in allDates)
                {
                    if (dayEnum.Contains(date.DayOfWeek))
                    {
                        var savedHours = GetAvailableSessionQuery.GetInstance()
                         .Execute(conn, new { ActivityId = input.ActivityId, AvailableDay = date.DayOfWeek.ToString() }).ToList();
               
                        if (whitelistedDates.Contains(date))
                        {
                            var customSavedHours = GetCustomAvailableHoursWhitelistDbQuery.GetInstance().Execute(conn, new { CustomDate = date, ActivityId = input.ActivityId }).ToList();
                            foreach(var customSavedHour in customSavedHours)
                            {
                                savedHours.Add(customSavedHour);
                                whitelistedDates.Remove(date);
                            }
                        }

                        if (blacklistedDates.Contains(date))
                        {
                            var customSavedHours = GetCustomAvailableHoursBlacklistDbQuery.GetInstance().Execute(conn, new { CustomDate = date, ActivityId = input.ActivityId }).ToList();
                            foreach (var customSavedHour in customSavedHours)
                            {
                                savedHours.Remove(customSavedHour);
                            }
                        }

                        if (savedHours.Count > 0)
                        {
                            var savedDateAndHour = new DateAndAvailableHour
                            {
                                AvailableHours = savedHours,
                                Date = date
                            };
                            result.Add(savedDateAndHour);
                        }                      
                    }                   
                }

                
                foreach (var whitelistedDate in whitelistedDates)
                {
                    var customSavedHours = GetCustomAvailableHoursWhitelistDbQuery.GetInstance().Execute(conn, new { CustomDate = whitelistedDate, ActivityId = input.ActivityId }).ToList();
                    var customSavedDateAndHour = new DateAndAvailableHour
                    {
                        AvailableHours = customSavedHours,
                        Date = whitelistedDate
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
                    Pax = new List<Pax>(),
                    Payment = PaymentDetails.GetFromDb(rsvNo),
                    State = ReservationState.GetFromDb(rsvNo),
                    ActivityDetails = new ActivityDetail(),
                    RsvTime = reservationRecord.RsvTime.GetValueOrDefault(),
                    RsvStatus = RsvStatusCd.Mnemonic(reservationRecord.RsvStatusCd)
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
                        activityReservation.Pax.Add(passenger);
                    }

                return activityReservation;
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
                    decimal totalOriginalPrice = 0;
                    decimal totalFinalPrice = 0;
                    decimal totalDiscount = 0;
                    decimal totalUniqueCode = 0; 
                    var bookingDetails = new List<BookingDetail>();
                    var rsvNoList = GetCartRsvNoListDbQuery.GetInstance().Execute(conn, new { CartId = cartId }).ToList();
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
                    savedBookings.Add(cartList);
                }

                var output = new GetMyBookingsOutput
                {
                    MyBookings = savedBookings,
                    Page = input.Page,
                    PerPage = input.PerPage
                };
                return output;
            }
        }

        public GetMyBookingDetailOutput GetMyBookingDetailFromDb(GetMyBookingDetailInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {                          
                var savedBooking = GetMyBookingDetailQuery.GetInstance()
                    .Execute(conn, new { input.RsvNo }).First();

                var savedPassengers = GetPassengersQuery.GetInstance().ExecuteMultiMap(conn, new { input.RsvNo }, null,
                        (passengers, typeCd, titleCd, genderCd) =>
                        {
                            passengers.Type = PaxTypeCd.Mnemonic(typeCd);
                            passengers.Title = TitleCd.Mnemonic(titleCd);
                            passengers.Gender = GenderCd.Mnemonic(genderCd);
                            return passengers;
                        }, "TypeCd, TitleCd, GenderCd").ToList();

                var savedPaxCountAndPackageId = GetPaxCountAndPackageIdDbQuery.GetInstance().Execute(conn, new { RsvNo = input.RsvNo }).ToList();
                savedBooking.PackageId = savedPaxCountAndPackageId.First().PackageId;
                savedBooking.PackageName = savedPaxCountAndPackageId.First().PackageName;
                var savedPaxCounts = new List<ActivityPricePackageReservation>();
                var pricePackages = ActivityService.GetInstance().GetPackagePriceFromDb(savedBooking.PackageId);
                foreach (var savedPaxCount in savedPaxCountAndPackageId)
                {
                    var saved = new ActivityPricePackageReservation
                    {
                        Type = savedPaxCount.Type,
                        Count = savedPaxCount.Count,
                        TotalPrice = savedPaxCount.Count * pricePackages.Where(package => package.Type == savedPaxCount.Type).First().Amount
                    };
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
                savedBooking.Price = priceBook;
                savedBooking.Passengers = savedPassengers;


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
                    .Execute(conn, new { UserId = userName.Id, Page = input.Page, PerPage = input.PerPage });

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
                        PaxCount = a.PaxCount,
                        MediaSrc = a.MediaSrc
                    }).ToList(),
                    Page = input.Page,
                    PerPage = input.PerPage
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
                    .Execute(conn, new { UserId = userName.Id, Page = input.Page, PerPage = input.PerPage });

                var output = new GetAppointmentListOutput
                {
                    Appointments = savedBookings.ToList(),
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
                        MediaSrc = a.MediaSrc
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
                    PaxGroup = new PaxGroup()
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

                    appointmentDetail.PaxGroup = paxgroup;
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

        private void InsertActivityRsvToDb(ActivityReservation reservation)
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

                var activityRecord = new ActivityReservationTableRecord
                {
                    Id = ActivityReservationIdSequence.GetInstance().GetNext(),
                    RsvNo = reservation.RsvNo,
                    ActivityId = reservation.ActivityDetails.ActivityId,
                    BookingStatusCd = BookingStatusCd.Mnemonic(reservation.ActivityDetails.BookingStatus),
                    Date = reservation.DateTime.Date,
                    SelectedSession = reservation.DateTime.Session,
                    UserId = reservation.User.Id,
                    TicketCount = null
                };

                ActivityReservationTableRepo.GetInstance().Insert(conn, activityRecord);
                ReservationTableRepo.GetInstance().Insert(conn, reservationRecord);
                foreach(var ticketCount in reservation.TicketCount)
                {
                    var activityPackageReservation = new ActivityPackageReservationTableRecord
                    {
                        RsvId = activityRecord.Id,
                        PackageId = reservation.PackageId,
                        ActivityId = reservation.ActivityDetails.ActivityId,
                        Type = ticketCount.Type,
                        Count = ticketCount.Count
                    };
                    ActivityPackageReservationTableRepo.GetInstance().Insert(conn,activityPackageReservation);
                }
                reservation.Contact.InsertToDb(reservation.RsvNo);
                reservation.State.InsertToDb(reservation.RsvNo);
                reservation.Payment.InsertToDb(reservation.RsvNo);
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
                    IsPaxDoBNeeded = input.IsPaxDoBNeeded,
                    IsPassportIssuedDateNeeded = input.IsPassportIssuedDateNeeded,
                    IsPassportNumberNeeded = input.IsPassportNumberNeeded
                });
                UpdatePriceQuery.GetInstance().Execute(conn, new { Price = input.Price, ActivityId = input.ActivityId });
            }

        }
        #endregion

        private void UpdateActivityBookingStatusInDb(string rsvNo, BookingStatus bookingStatus)
        {
            var bookingStatusCd = BookingStatusCd.Mnemonic(bookingStatus);
            using (var conn = DbService.GetInstance().GetOpenConnection())
                UpdateActivityBookingStatusQuery.GetInstance().Execute(conn, new { rsvNo, bookingStatusCd });
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
                try
                {
                    WishlistTableRepo.GetInstance().Insert(conn, wishlistRecord);
                }
                catch
                {

                }
            }
            var response = new AddToWishlistOutput { isSuccess = true };
            return response;
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
                    foreach(var availableHours in date.AvailableHours)
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
                    if(date.AvailableHours == null)
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

        public List<ActivityPackage> GetPackageAttributeFromDb (long PackageId)
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
                if (!DateTime.TryParse(customDateInput.StartCustomHour, out startHour))
                {
                    return new CustomDateOutput
                    {
                        isSuccess = false
                    };
                }
                if (!DateTime.TryParse(customDateInput.EndCustomHour, out endHour))
                {
                    return new CustomDateOutput
                    {
                        isSuccess = false
                    };
                }
                var customDate = new ActivityCustomDateTableRecord
                {
                    ActivityId = customDateInput.ActivityId,
                    CustomDate = customDateInput.CustomDate,
                    AvailableHour = customDateInput.StartCustomHour + " - " + customDateInput.EndCustomHour,
                    DateStatus = "whitelisted"
                };
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
    }
}