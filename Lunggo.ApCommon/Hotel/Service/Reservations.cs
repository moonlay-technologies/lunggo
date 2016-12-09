using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public HotelDetailForDisplay GetSelectionFromCache(string token)
        {
            try
            {
                var rsv = GetSelectedHotelDetailsFromCache(token);
                return ConvertToHotelDetailForDisplay(rsv);
            }
            catch
            {
                return null;
            }
        }

        public HotelReservationForDisplay GetReservationForDisplay(string rsvNo)
        {
            try
            {
                var rsv = GetReservation(rsvNo);
                rsv.HotelDetails.PostalCode = GetHotelDetailFromTableStorage(rsv.HotelDetails.HotelCode).PostalCode;
                foreach (var r in rsv.HotelDetails.Rooms)
                {
                    foreach (var rate in r.Rates)
                    {
                        var cid = rate.RateKey != null ? rate.RateKey.Split('|')[0] : rate.RegsId.Split('|')[0];
                        var cod = rate.RateKey != null ? rate.RateKey.Split('|')[1] : rate.RegsId.Split('|')[1];
                        var checkInDate = new DateTime(Convert.ToInt32(cid.Substring(0, 4)),
                            Convert.ToInt32(cid.Substring(4, 2)), Convert.ToInt32(cid.Substring(6, 2)));
                        var checkOutDate = new DateTime(Convert.ToInt32(cod.Substring(0, 4)),
                            Convert.ToInt32(cod.Substring(4, 2)), Convert.ToInt32(cod.Substring(6, 2)));
                        rate.NightCount = Convert.ToInt32((checkOutDate - checkInDate).TotalDays);
                        rate.TermAndCondition = GetRateCommentFromTableStorage(rate.RateCommentsId,
                        checkInDate).Select(x => x.Description).ToList();
                    }
                }
                
                var rsvfordisplay = ConvertToReservationForDisplay(rsv);
                return rsvfordisplay;
            }
            catch
            {
                return null;
            }
        }

        public HotelReservation GetReservation(string rsvNo)
        {
            try
            {
                return GetReservationFromDb(rsvNo);
            }
            catch
            {
                return null;
            }
        }

        public List<HotelReservationForDisplay> GetOverviewReservationsByUserIdOrEmail(string userId, string email, string filter, string sort, int? page, int? itemsPerPage)
        {
            var filters = filter != null ? filter.Split(',') : null;
            var rsvs = GetOverviewReservationsByUserIdOrEmailFromDb(userId, email, filters, sort, page, itemsPerPage) ?? new List<HotelReservation>();
            return rsvs.Select(ConvertToReservationForDisplay).Where(r => r.RsvDisplayStatus != RsvDisplayStatus.Expired).ToList().ToList();
        }

        private List<HotelReservation> GetOverviewReservationsByUserIdOrEmailFromDb(string userId, string email, string[] filters, string sort, int? page, int? itemsPerPage)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rsvNos =
                    GetRsvNosByUserIdQuery.GetInstance()
                        .Execute(conn, new { UserId = userId, ContactEmail = email }, new { Filters = filters, Sort = sort, Page = page, ItemsPerPage = itemsPerPage })
                        .Distinct().ToList();
                if (!rsvNos.Any())
                    return null;
                else
                {
                    return rsvNos.Select(GetOverviewReservationFromDb).Where(rsv => rsv != null).ToList();
                }
            }
        }

        private HotelReservation GetOverviewReservationFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationRecord = ReservationTableRepo.GetInstance()
                    .Find1(conn, new ReservationTableRecord { RsvNo = rsvNo });

                if (reservationRecord == null)
                    return null;

                var reservation = new HotelReservation
                {
                    RsvNo = rsvNo,
                    RsvTime = reservationRecord.RsvTime.GetValueOrDefault().SpecifyUtc(),
                    Contact = Contact.GetFromDb(rsvNo),
                    Pax = new List<Pax>(),
                    HotelDetails = new HotelDetail(),
                    Payment = PaymentDetails.GetFromDb(rsvNo),
                    State = ReservationState.GetFromDb(rsvNo),
                    User = User.GetFromDb(reservationRecord.UserId)
                };

                if (reservation.Contact == null || reservation.Payment == null || reservation.State == null)
                    return null;

                var hotelDetailRecord = HotelReservationDetailsTableRepo.GetInstance().Find(conn,
                    new HotelReservationDetailsTableRecord { RsvNo = rsvNo }).ToList();

                if (hotelDetailRecord.Count == 0)
                    return null;

                var hotelDetail = new HotelDetail
                {
                    HotelName = hotelDetailRecord[0].HotelName,
                    CheckInDate = hotelDetailRecord[0].CheckInDate.GetValueOrDefault(),
                    CheckOutDate = hotelDetailRecord[0].CheckOutDate.GetValueOrDefault(),
                    TotalAdult = hotelDetailRecord[0].AdultCount.GetValueOrDefault(),
                    TotalChildren = hotelDetailRecord[0].ChildCount.GetValueOrDefault(),
                    StarRating = hotelDetailRecord[0].HotelRating,
                    Address = hotelDetailRecord[0].HotelAddress,
                    ZoneCode = hotelDetailRecord[0].HotelZone,
                    DestinationCode = hotelDetailRecord[0].HotelDestination,
                    City = hotelDetailRecord[0].HotelCity,
                    CountryCode = hotelDetailRecord[0].HotelCountry,
                    Rooms = new List<HotelRoom>(),
                    ImageUrl = GetHotelDetailFromDb(hotelDetailRecord[0].HotelCd.GetValueOrDefault()).ImageUrl
                };

                var roomRecords = HotelRoomTableRepo.GetInstance().
                    Find(conn, new HotelRoomTableRecord { DetailsId = hotelDetailRecord[0].Id }).ToList();

                if (roomRecords.Count == 0)
                    return null;

                foreach (var roomRecord in roomRecords)
                {
                    var room = new HotelRoom
                    {
                        RoomCode = roomRecord.Code,
                        Rates = new List<HotelRate>(),
                    };

                    var rateRecords = HotelRateTableRepo.GetInstance().
                        Find(conn, new HotelRateTableRecord { RoomId = roomRecord.Id }).ToList();

                    if (rateRecords.Count == 0)
                    {
                        return null;
                    }

                    foreach (var rateRecord in rateRecords)
                    {
                        var rate = new HotelRate
                        {
                            Boards = rateRecord.Board,
                            RateCount = rateRecord.RoomCount.GetValueOrDefault(),
                            AdultCount = rateRecord.AdultCount.GetValueOrDefault(),
                            ChildCount = rateRecord.ChildCount.GetValueOrDefault()

                        };
                        room.Rates.Add(rate);
                    }
                    hotelDetail.Rooms.Add(room);
                }

                reservation.HotelDetails = hotelDetail;

                var paxRecords = PaxTableRepo.GetInstance()
                    .Find(conn, new PaxTableRecord { RsvNo = rsvNo }).ToList();

                if (paxRecords.Count == 0)
                    return null;

                foreach (var passengerRecord in paxRecords)
                {
                    var passenger = new Pax
                    {
                        Title = TitleCd.Mnemonic(passengerRecord.TitleCd),
                        FirstName = passengerRecord.FirstName,
                        LastName = passengerRecord.LastName,
                        Type = PaxTypeCd.Mnemonic(passengerRecord.TypeCd)
                    };
                    reservation.Pax.Add(passenger);
                }

                return reservation;

            }
        }

        public List<HotelReservationForDisplay> GetOverviewReservationsByDeviceId(string deviceId, string filter, string sort, int? page, int? itemsPerPage)
        {
            var filters = filter != null ? filter.Split(',') : null;
            var rsvs = GetOverviewReservationsByDeviceIdFromDb(deviceId, filters, sort, page, itemsPerPage) ?? new List<HotelReservation>();
            return rsvs.Select(ConvertToReservationForDisplay).Where(r => r.RsvDisplayStatus != RsvDisplayStatus.Expired).ToList();
        }

        private List<HotelReservation> GetOverviewReservationsByDeviceIdFromDb(string deviceId, string[] filters, string sort, int? page, int? itemsPerPage)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rsvNos =
                    GetRsvNosByDeviceIdQuery.GetInstance()
                        .Execute(conn, new { DeviceId = deviceId }, new { Filters = filters, Sort = sort, Page = page, ItemsPerPage = itemsPerPage })
                        .ToList();
                if (!rsvNos.Any())
                    return null;
                else
                {
                    return rsvNos.Select(GetOverviewReservationFromDb).Where(rsv => rsv != null && rsv.User == null).ToList();
                }
            }
        }
        
        public void ExpireReservationWhenTimeout(string rsvNo, DateTime timeLimit)
        {
            var queue = QueueService.GetInstance().GetQueueByReference("HotelExpireReservation");
            var timeOut = timeLimit - DateTime.UtcNow;
            queue.AddMessage(new CloudQueueMessage(rsvNo), initialVisibilityDelay: timeOut);
        }

        public void ExpireReservation(string rsvNo)
        {
            var payment = PaymentDetails.GetFromDb(rsvNo);
            if (payment.Status == PaymentStatus.Pending || payment.Status == PaymentStatus.Verifying ||
                payment.Status == PaymentStatus.Challenged || payment.Status == PaymentStatus.Undefined)
            {
                payment.Status = PaymentStatus.Expired;
                PaymentService.GetInstance().UpdatePayment(rsvNo, payment);
            }
        }
        
        public byte[] GetEticket(string rsvNo)
        {
            var azureConnString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var storageName = azureConnString.Split(';')[1].Split('=')[1];
            var url = @"https://" + storageName + @".blob.core.windows.net/voucher/" + rsvNo + ".pdf";
            var client = new WebClient();
            try
            {
                return client.DownloadData(url);
            }
            catch
            {
                return null;
            }
        }
    }
}
