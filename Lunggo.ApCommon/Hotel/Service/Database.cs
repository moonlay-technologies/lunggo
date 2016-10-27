﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

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
                //|| hotelReservation.State == null
                if (hotelReservation.Contact == null || hotelReservation.Payment == null )
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
                    .Find(conn, new HotelRoomTableRecord { DetailsId = hotelDetailRecord.Id }).ToList();

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
                            .Find(conn, new HotelRateTableRecord { RoomId =  hotelRoomRecord.Id }).ToList();

                        if (rateRecords.Count == 0)
                            return null;

                        foreach (var rateRecord in rateRecords)
                        {
                            var rate = new HotelRate
                            {
                                RateKey = rateRecord.RateKey,
                                AdultCount = rateRecord.AdultCount.GetValueOrDefault(),
                                ChildCount = rateRecord.ChildCount.GetValueOrDefault(),
                                Boards = rateRecord.Board,
                                Cancellation = rateRecord.Cancellation.Deserialize<List<Cancellation>>(),
                                PaymentType = PaymentTypeCd.Mnemonic(rateRecord.PaymentType),
                                RoomCount = rateRecord.RoomCount.GetValueOrDefault(),
                                Price = Price.GetFromDb(rateRecord.PriceId.GetValueOrDefault()) 
                            };
                            hotelRoom.Rates.Add(rate);
                        }

                        hotelDetail.Rooms.Add(hotelRoom);
                    }

                    hotelReservation.HotelDetails = hotelDetail;
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

        private static List<HotelMarginRule> GetActivePriceMarginRulesFromDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var activeRules = GetActiveHotelPriceMarginRuleQuery.GetInstance().ExecuteMultiMap(conn, null, null,
                    (marginRecord, ruleRecord) =>
                    {
                        var margin = new Margin
                        {
                            Id = marginRecord.Id.GetValueOrDefault(),
                            RuleId = marginRecord.OrderRuleId.GetValueOrDefault(),
                            Name = marginRecord.Name,
                            Description = marginRecord.Description,
                            Percentage = marginRecord.Percentage.GetValueOrDefault(),
                            Constant = marginRecord.Constant.GetValueOrDefault(),
                            Currency = new Currency(marginRecord.CurrencyCd),
                            IsFlat = marginRecord.IsFlat.GetValueOrDefault(),
                            IsActive = marginRecord.IsActive.GetValueOrDefault(),
                        };
                        var rule = new HotelRateRule
                        {
                            BookingDays = ruleRecord.BookingDays.Deserialize<List<DayOfWeek>>(),
                            ConstraintCount = ruleRecord.ConstraintCount.GetValueOrDefault(),
                            Priority = ruleRecord.Priority.GetValueOrDefault(),
                            StayDates = ruleRecord.StayDates.Deserialize<List<HotelRateRule.DateSpanRule>>(),
                            StayDurations = ruleRecord.StayDurations.Deserialize<List<int>>(),
                            Boards = ruleRecord.Boards.Deserialize<List<string>>(),
                            BookingDates = ruleRecord.BookingDays.Deserialize<List<HotelRateRule.DateSpanRule>>(),
                            Countries = ruleRecord.Countries.Deserialize<List<string>>(),
                            Destinations = ruleRecord.Destinations.Deserialize<List<string>>(),
                            HotelStars = ruleRecord.HotelStars.Deserialize<List<string>>(),
                            MaxAdult = ruleRecord.MaxAdult.GetValueOrDefault(),
                            MaxChild = ruleRecord.MaxChild.GetValueOrDefault(),
                            MinAdult = ruleRecord.MinAdult.GetValueOrDefault(),
                            MinChild = ruleRecord.MinChild.GetValueOrDefault(),
                            RoomTypes = ruleRecord.RoomTypes.Deserialize<List<string>>()
                        };
                        return new HotelMarginRule(margin, rule);
                    }, "ConstraintCount").ToList();
                return activeRules.ToList();
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
                
                //reservation.State.InsertToDb(reservation.RsvNo);
                reservation.Payment.InsertToDb(reservation.RsvNo);
                
                var hotelRsvDetailsRecord = new HotelReservationDetailsTableRecord
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
                        Type = room.Type,
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
                            Cancellation = rate.Cancellation.Serialize(),
                            InsertDate = DateTime.UtcNow,
                            InsertBy = "LunggoSystem",
                            InsertPgId = "0",
                            PriceId = rate.Price.InsertToDb(),
                            RoomId = roomId,
                            RoomCount = rate.RoomCount,
                            RateKey =  rate.RateKey,
                            PaymentType = PaymentTypeCd.MnemonicToString(rate.PaymentType),
                        };

                        HotelRateTableRepo.GetInstance().Insert(conn, rateRecord);
                    }
                }
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

        private static void InsertPriceMarginRulesToDb(IEnumerable<HotelMarginRule> rules, IEnumerable<HotelMarginRule> deletedRules)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var activeRules = GetActivePriceMarginRulesFromDb();
                foreach (var deletedRule in deletedRules)
                {
                    if (activeRules.Any(activeRule => activeRule.Margin.Id == deletedRule.Margin.Id))
                    {
                        HotelRateRuleTableRepo.GetInstance()
                            .Update(conn, new HotelRateRuleTableRecord
                            {
                                Id = deletedRule.Margin.RuleId,
                                Priority = deletedRule.Rule.Priority,
                            });
                        MarginTableRepo.GetInstance()
                            .Update(conn, new MarginTableRecord
                            {
                                Id = deletedRule.Margin.Id,
                                IsActive = deletedRule.Margin.IsActive
                            });
                    }
                }

                foreach (var marginRule in rules)
                {
                    if (activeRules.Any(activeRule => activeRule.Margin.RuleId == marginRule.Margin.RuleId))
                    {
                        HotelRateRuleTableRepo.GetInstance()
                            .Update(conn, new HotelRateRuleTableRecord
                            {
                                Id = marginRule.Margin.RuleId,
                                Priority = marginRule.Rule.Priority,
                            });
                    }
                    else
                    {
                        var margin = marginRule.Margin;
                        var marginRecord = new MarginTableRecord
                        {
                            Id = margin.Id,
                            Name = margin.Name,
                            Description = margin.Description,
                            Percentage = margin.Percentage,
                            Constant = margin.Constant,
                            IsFlat = margin.IsFlat,
                            IsActive = true,
                            InsertBy = "LunggoSystem",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "0"
                        };
                        MarginTableRepo.GetInstance().Insert(conn, marginRecord);

                        var rule = marginRule.Rule;
                        var ruleRecord = new HotelRateRuleTableRecord
                        {
                            Id = marginRule.Margin.RuleId,
                            BookingDays = rule.BookingDays.Serialize(),
                            BookingDates = rule.BookingDates.Serialize(),
                            Boards = rule.ToString().Serialize(),
                            Countries = rule.Countries.Serialize(),
                            Destinations = rule.Countries.Serialize(),
                            HotelStars = rule.HotelStars.Serialize(),
                            MaxAdult = rule.MaxAdult,
                            MaxChild = rule.MaxChild,
                            MinAdult = rule.MinAdult,
                            MinChild = rule.MinChild,
                            RoomTypes = rule.RoomTypes.Serialize(),
                            StayDates = rule.StayDates.Serialize(),
                            StayDurations = rule.StayDurations.Serialize(),
                            ConstraintCount = rule.ConstraintCount,
                            Priority = rule.Priority,
                            InsertBy = "LunggoSystem",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "0"
                        };
                        HotelRateRuleTableRepo.GetInstance().Insert(conn, ruleRecord);
                    }
                }
            }
        }
        #endregion

        #region Update
        
        #endregion
    }
}
