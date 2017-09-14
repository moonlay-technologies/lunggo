using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Product.Query;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Product.Service
{
    public class SavePax
    {
        public static int Create(string email, Pax pax)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var id = Convert.ToInt32(SavedPaxIdSequence.GetInstance().GetNext());
                var paxRecord = new SavedPassengerTableRecord
                {
                    Id = id,
                    Email = email,
                    TypeCd = PaxTypeCd.Mnemonic(pax.Type),
                    GenderCd = GenderCd.Mnemonic(pax.Gender),
                    TitleCd = TitleCd.Mnemonic(pax.Title),
                    FirstName = pax.FirstName,
                    LastName = pax.LastName,
                    BirthDate = pax.DateOfBirth.HasValue ? pax.DateOfBirth.Value.ToUniversalTime() : (DateTime?)null,
                    NationalityCd = pax.Nationality,
                    PassportNumber = pax.PassportNumber,
                    PassportExpiryDate = pax.PassportExpiryDate.HasValue ? pax.PassportExpiryDate.Value.ToUniversalTime() : (DateTime?)null,
                    PassportCountryCd = pax.PassportCountry,
                };

                SavedPassengerTableRepo.GetInstance().Insert(conn, paxRecord);
                return id;
            }

        }

        public static List<Pax> Read(string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var passengerRecords =
                    GetSavedPassengerQuery.GetInstance().Execute(conn, new { Email = email }).ToList();
                return passengerRecords.Select(record => new Pax
                {
                    Id = record.Id,
                    Email = record.Email,
                    Type = PaxTypeCd.Mnemonic(record.TypeCd),
                    Title = TitleCd.Mnemonic(record.TitleCd),
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    Gender = GenderCd.Mnemonic(record.GenderCd),
                    DateOfBirth = record.BirthDate,
                    Nationality = record.NationalityCd,
                    PassportNumber = record.PassportNumber,
                    PassportExpiryDate = record.PassportExpiryDate,
                    PassportCountry = record.PassportCountryCd
                }).ToList();
            }
        }

        public static void Update(string email, int id, Pax pax)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UpdateSavedPassengerQuery.GetInstance().Execute(conn, new SavedPassengerTableRecord
                    {
                        Id = id,
                        Email = email,
                        TypeCd = PaxTypeCd.Mnemonic(pax.Type),
                        GenderCd = GenderCd.Mnemonic(pax.Gender),
                        TitleCd = TitleCd.Mnemonic(pax.Title),
                        FirstName = pax.FirstName,
                        LastName = pax.LastName,
                        BirthDate = pax.DateOfBirth.HasValue ? pax.DateOfBirth.Value.ToUniversalTime() : (DateTime?)null,
                        NationalityCd = pax.Nationality,
                        PassportNumber = pax.PassportNumber,
                        PassportExpiryDate = pax.PassportExpiryDate.HasValue ? pax.PassportExpiryDate.Value.ToUniversalTime() : (DateTime?)null,
                        PassportCountryCd = pax.PassportCountry
                    });
            }
        }

        public static void Delete(string email, int id)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var passengerRecords =
                    GetSavedPassengerQuery.GetInstance().Execute(conn, new { Email = email }).ToList();
                if (passengerRecords.Exists(r => r.Id == id))
                {
                    SavedPassengerTableRepo.GetInstance().Delete(conn, new SavedPassengerTableRecord
                    {
                        Id = id
                    });
                }

            }

        }
    }


}
