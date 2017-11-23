using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
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
using Lunggo.ApCommon.Flight.Constant;

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

                var savedActivities = GetSearchResultQuery.GetInstance()
                    .ExecuteMultiMap(conn, new { Name = input.ActivityFilter.Name, StartDate = startDate, EndDate = endDate, Page = input.Page, PerPage = input.PerPage },
                    null, (activities, duration) =>
                        {
                            activities.Duration = duration;
                            return activities;
                        }, "Amount").ToList();
                
                //for (int i = 0; i < savedActivities.Count; i++)
                //{
                //    var id = savedActivities[i].Id;
                //    var mediaSrc = GetMediaActivityDetailQuery.GetInstance()
                //        .Execute(conn, new { ActivityId = id }).ToList();
                //    savedActivities[i].MediaSrc = mediaSrc[0];
                //}

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
                        (detail, duration, content) =>
                        {
                            detail.Duration = duration;
                            detail.Contents = content;
                            return detail;
                        }, "Amount, Content1");

                var mediaSrc = GetMediaActivityDetailQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId }).ToList();

                var additionalContentsDetail = GetAdditionalContentActivityDetailQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId });

                var activityDetail = details.First();

                activityDetail.MediaSrc = mediaSrc;
                activityDetail.AdditionalContent = additionalContentsDetail.Select(a => new AdditionalContent()
                {
                    Title = a.Title,
                    Description = a.Description
                }).ToList();

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
                var savedActivities = GetAvailableDatesQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId });
                
                var output = new GetAvailableDatesOutput
                {
                    AvailableDateTimes = savedActivities.Select(a => new DateAndAvailableHour()
                    {
                        Date = a.Date,
                        AvailableHour = a.AvailableHour
                    }).ToList()
                };
                return output;
            }
        }

        private ActivityReservation GetReservationFromDb(string rsvNo)
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

                var actDetail = GetActivityDetailFromDb(new GetDetailActivityInput() {ActivityId = activityDetailRecord.ActivityId });

                activityReservation.ActivityDetails = actDetail.ActivityDetail;

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
                
                var savedBookings = GetMyBookingsQuery.GetInstance()
                    .Execute(conn, new { UserId = userName.Id, Page = input.Page, PerPage = input.PerPage });
                
                var output = new GetMyBookingsOutput
                {
                    MyBookings = savedBookings.Select(a => new BookingDetail()
                    {
                        ActivityId = a.ActivityId,
                        RsvNo = a.RsvNo,
                        Name = a.Name,
                        BookingStatus = a.BookingStatus,
                        TimeLimit = a.TimeLimit.Value.AddHours(1),
                        Date = a.Date,
                        SelectedSession = a.SelectedSession,
                        PaxCount = a.PaxCount,
                        Price = a.Price,
                        MediaSrc = a.MediaSrc
                    }).ToList(),
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
                    .Execute(conn, new { RsvNo = input.RsvNo }).First();

                var savedPassengers = GetPassengersQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = input.RsvNo }, null,
                        (passengers, typeCd, titleCd, genderCd) =>
                        {
                            passengers.Type = PaxTypeCd.Mnemonic(typeCd);
                            passengers.Title = TitleCd.Mnemonic(titleCd);
                            passengers.Gender = GenderCd.Mnemonic(genderCd);
                            return passengers;
                        }, "TypeCd, TitleCd, GenderCd").ToList();

                var output = new GetMyBookingDetailOutput
                {
                    BookingDetail = new BookingDetail()
                    {
                        ActivityId = savedBooking.ActivityId,
                        Name = savedBooking.Name,
                        BookingStatus = savedBooking.BookingStatus,
                        TimeLimit = savedBooking.TimeLimit.Value.AddHours(1),
                        Date = savedBooking.Date,
                        SelectedSession = savedBooking.SelectedSession,
                        PaxCount = savedBooking.PaxCount,
                        Passengers = savedPassengers,
                        Price = savedBooking.Price,
                        MediaSrc = savedBooking.MediaSrc
                    }
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
                    .Execute(conn, new {UserId = userName.Id, Page = input.Page, PerPage = input.PerPage });

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
                    Appointments = savedBookings.Select(a => new AppointmentDetail()
                    {
                        AppointmentId = a.AppointmentId,
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

                var savedBookings = GetAppointmentDetailQuery.GetInstance()
                    .Execute(conn, new { AppointmentId = input.ActivityId }).Single();

                var savedReservation = GetReservationBySessionQuery.GetInstance().Execute(conn, new { });

                var savedPassengers = GetPassengersQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = savedBookings.RsvNo }, null,
                        (passengers, typeCd, titleCd, genderCd) =>
                        {
                            passengers.Type = PaxTypeCd.Mnemonic(typeCd);
                            passengers.Title = TitleCd.Mnemonic(titleCd);
                            passengers.Gender = GenderCd.Mnemonic(genderCd);
                            return passengers;
                        }, "TypeCd, TitleCd, GenderCd").ToList();

                var output = new GetAppointmentDetailOutput
                {
                    AppointmentDetail = new AppointmentDetail()
                    {
                        ActivityId = savedBookings.ActivityId,
                        Name = savedBookings.Name,
                        Date = savedBookings.Date,
                        Session = savedBookings.Session,
                        MediaSrc = savedBookings.MediaSrc,
                        PaxGroups = new List<PaxGroup>()
                    }
                };
                return output;
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
                    Date = reservation.DateTime.Date,
                    SelectedSession = reservation.DateTime.AvailableHour,
                    TicketCount = reservation.TicketCount,
                    UserId = reservation.User.Id
                };

                ActivityReservationTableRepo.GetInstance().Insert(conn, activityRecord);
                ReservationTableRepo.GetInstance().Insert(conn, reservationRecord);
                reservation.Contact.InsertToDb(reservation.RsvNo);
                reservation.State.InsertToDb(reservation.RsvNo);
                reservation.Payment.InsertToDb(reservation.RsvNo);
                if(reservation.Pax != null)
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

        private void InsertAppointmentReqToDb(ActivityReservation rsvData, string request)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var activityRsv = ActivityReservationTableRepo.GetInstance()
                    .Find1(conn, new ActivityReservationTableRecord { RsvNo = rsvData.RsvNo });
                var operatorData = OperatorTableRepo.GetInstance().Find1(conn, new OperatorTableRecord {ActivityId = activityRsv.ActivityId });

                AppointmentTableRepo.GetInstance().Insert(conn, new AppointmentTableRecord()
                {
                    Id = AppointmentRequestIdSequence.GetInstance().GetNext(),
                    ActivityRsvId = activityRsv.Id,
                    RsvNo = rsvData.RsvNo,
                    AppointmentStatus = request,
                    OperatorId = operatorData.UserId,
                    InsertDate = DateTime.UtcNow

                });
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

        private void UpdateAppointmentStatusDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var appointmentDetailRecord = AppointmentTableRepo.GetInstance()
                    .Find1(conn, new AppointmentTableRecord { RsvNo = rsvNo });

                AppointmentTableRepo.GetInstance().Update(conn, new AppointmentTableRecord
                {
                    Id = appointmentDetailRecord.Id,
                    AppointmentStatus = "Approved"
                });
            }

        }
        #endregion
    }
}