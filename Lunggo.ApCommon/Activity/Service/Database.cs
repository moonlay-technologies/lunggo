using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System.Linq;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

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
                    .Execute(conn, new { Name = input.ActivityFilter.Name, StartDate = startDate, EndDate = endDate, Page = input.Page, PerPage = input.PerPage });

                var output = new SearchActivityOutput
                {
                    ActivityList = savedActivities.Select(a => new SearchResult()
                    {
                        Id = a.Id,
                        Name = a.Name,
                        City = a.City,
                        Country = a.Country,
                        Price = a.Price,
                        ImgSrc = a.ImgSrc
                    }).ToList(),
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
                //var details = GetActivityDetailQuery.GetInstance()
                //    .ExecuteMultiMap(conn, new { ActivityId = input.ActivityId}, null, (detail, mediaSrc) =>
                //    {
                //        detail.MediaSrc = new List<string> {mediaSrc};
                //        return detail;
                //    }, "MediaSrc").ToList();

                var details = GetActivityDetailQuery.GetInstance().Execute(conn, new { ActivityId = input.ActivityId })
                    .ToList();

                var mediaSrc = GetMediaActivityDetailQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId }).ToList();

                var contentsDetail = GetContentActivityDetailQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId });

                var activityDetail = details.First();

                activityDetail.MediaSrc = mediaSrc;
                activityDetail.Content = contentsDetail.Select(a => new ContentActivityDetail()
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
                    AvailableDates = savedActivities.Select(a => new ActivityDetail()
                    {
                        Date = a.Date
                    }).ToList()
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

                ReservationTableRepo.GetInstance().Insert(conn, reservationRecord);
                reservation.Contact.InsertToDb(reservation.RsvNo);
                reservation.State.InsertToDb(reservation.RsvNo);
                reservation.Payment.InsertToDb(reservation.RsvNo);
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

        #endregion
    }
}