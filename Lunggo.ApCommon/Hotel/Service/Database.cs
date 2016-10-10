using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Pax = Lunggo.ApCommon.Product.Model.Pax;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        #region Get
        private HotelReservation GetReservationFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationRecord = ReservationTableRepo.GetInstance()
                    .Find1(conn, new ReservationTableRecord { RsvNo = rsvNo });

                if (reservationRecord == null)
                    return null;
                

                var hotelReservation = new HotelReservation
                {
                    RsvNo = rsvNo,
                    Contact = Contact.GetFromDb(rsvNo),
                    Pax = new List<Pax>(),
                    Payment = PaymentDetails.GetFromDb(rsvNo),
                    State = ReservationState.GetFromDb(rsvNo),
                    HotelDetails = new HotelDetail()
                };

                //HotelDetails = new HotelDetail
                //    {
                //        HotelCode = reservationRecord.HotelCd.GetValueOrDefault(),
                //        HotelName = reservationRecord.HotelName,
                //        CheckInDate = reservationRecord.CheckInDate.GetValueOrDefault(),
                //        CheckOutDate = reservationRecord.CheckOutDate.GetValueOrDefault(),
                //        TotalAdult = reservationRecord.AdultCount.GetValueOrDefault(),
                //        TotalChildren = reservationRecord.ChildCount.GetValueOrDefault(),
                //        SpecialRequest = reservationRecord.SpecialRequest,
                //        Rooms    = new List<HotelRoom>(),
                        
                //    },
                if (hotelReservation.Contact == null || hotelReservation.Payment == null || hotelReservation.State == null)
                    return null;

                var hotelDetailRecords = HotelReservationDetailsTableRepo.GetInstance()
                    .Find(conn, new HotelReservationDetailsTableRecord { RsvNo = rsvNo }).ToList();

                if (hotelDetailRecords.Count == 0)
                    return null;

                foreach (var hotelDetailRecord in hotelDetailRecords)
                {
                    var hotelDetail = new HotelDetail
                    {
                        HotelCode = hotelDetailRecord.HotelCd.GetValueOrDefault(),
                        HotelName = hotelDetailRecord.HotelName,
                        CheckInDate = hotelDetailRecord.CheckInDate.GetValueOrDefault(),
                        CheckOutDate = hotelDetailRecord.CheckOutDate.GetValueOrDefault(),
                        TotalAdult = hotelDetailRecord.AdultCount.GetValueOrDefault(),
                        TotalChildren = hotelDetailRecord.ChildCount.GetValueOrDefault(),
                        SpecialRequest = hotelDetailRecord.SpecialRequest,
                        Rooms = new List<HotelRoom>(),
                    };

                    var hotelRoomRecords = HotelRoomTableRepo.GetInstance()
                    .Find(conn, new HotelRoomTableRecord { Id = hotelDetailRecord.Id }).ToList();

                    if (hotelRoomRecords.Count == 0)
                        return null;

                    foreach (var hotelRoomRecord in hotelRoomRecords)
                    {
                        var hotelRoom = new HotelRoom
                        {
                            RoomCode = hotelRoomRecord.Code,
                            Type = hotelRoomRecord.Type,
                            Rates = new List<HotelRate>(),
                        };

                        if (hotelRoom.RoomCode == null)
                            return null;

                        var rateRecords = HotelRateTableRepo.GetInstance()
                            .Find(conn, new HotelRateTableRecord { Id = hotelRoomRecord.Id }).ToList();

                        if (rateRecords.Count == 0)
                            return null;

                        foreach (var rateRecord in rateRecords)
                        {
                            var rate = new HotelRate
                            {
                                RateKey = rateRecord.Key,
                                AdultCount = rateRecord.AdultCount.GetValueOrDefault(),
                                ChildCount = rateRecord.ChildCount.GetValueOrDefault(),
                                Boards = rateRecord.Board,
                                Cancellation = new Cancellation
                                {
                                    StartTime = rateRecord.CancellationStartTime.GetValueOrDefault(),
                                    Fee = rateRecord.CancellationFee.GetValueOrDefault()
                                },
                                PaymentType = rateRecord.PaymentType,
                                RoomCount = rateRecord.RoomCount.GetValueOrDefault(),
                                Price = Price.GetFromDb(rateRecord.PriceId.GetValueOrDefault()) 
                            };
                            hotelRoom.Rates.Add(rate);
                        }

                        hotelReservation.HotelDetails.Rooms.Add(hotelRoom);
                    }              
                }
                

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
                    hotelReservation.Pax.Add(passenger);
                }
                return hotelReservation;

            }
        }
        #endregion

        #region Insert
        private void InsertHotelRsvToDb(HotelReservation reservation)
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
                
                var hotelRsvDetailsRecord = new HotelReservationDetailsTableRecord()
                {
                    Id = HotelReservationIdSequence.GetInstance().GetNext(),
                    RsvNo = reservation.RsvNo,
                    SpecialRequest = reservation.HotelDetails.SpecialRequest,
                    AdultCount = reservation.HotelDetails.TotalAdult,
                    ChildCount = reservation.HotelDetails.TotalChildren,
                    CheckInDate = reservation.HotelDetails.CheckInDate,
                    CheckOutDate = reservation.HotelDetails.CheckOutDate,
                    HotelName = reservation.HotelDetails.HotelName,
                    HotelCd = reservation.HotelDetails.HotelCode,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0",                 
                };

                HotelReservationDetailsTableRepo.GetInstance().Insert(conn, hotelRsvDetailsRecord);

                foreach (var room in reservation.HotelDetails.Rooms)
                {
                    var roomId = HotelRoomIdSequence.GetInstance().GetNext();
                    var roomRecord = new HotelRoomTableRecord
                    {
                        Id = roomId,
                        Code = room.RoomCode,
                        DetailsId = hotelRsvDetailsRecord.Id,
                        InsertDate = DateTime.UtcNow,
                        InsertBy = "LunggoSystem",
                        InsertPgId = "0"
                    };

                    HotelRoomTableRepo.GetInstance().Insert(conn, roomRecord);

                    foreach (var rate in room.Rates)
                    {
                        var rateId = HotelRateIdSequence.GetInstance().GetNext();
                        var rateRecord = new HotelRateTableRecord
                        {
                            Id = rateId,
                            AdultCount = rate.AdultCount,
                            ChildCount = rate.ChildCount,
                            Board = rate.Boards,
                            CancellationFee = rate.Cancellation.Fee,
                            CancellationStartTime = rate.Cancellation.StartTime,
                            InsertDate = DateTime.UtcNow,
                            InsertBy = "LunggoSystem",
                            InsertPgId = "0",
                            PriceId = rate.Price.InsertToDb(),
                        };

                        HotelRateTableRepo.GetInstance().Insert(conn, rateRecord);
                    }
                }
            }
        }
        
        #endregion

        #region Update
        
        #endregion
    }
}
