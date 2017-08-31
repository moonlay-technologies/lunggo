using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Product.Query;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Framework.Pattern;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Product.Service
{
    public abstract class ProductServiceBase<T> : SingletonBase<T> where T : SingletonBase<T>
    {
        internal abstract void Issue(string rsvNo);

        private static Dictionary<ProductType, Type> _serviceList;

        //protected ProductServiceBase()
        //{
        //    _serviceList =
        //        Assembly.GetAssembly(typeof(ProductServiceBase<>)).GetTypes()
        //            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof (ProductServiceBase<T>)))
        //            .ToDictionary(
        //                type => (ProductType) type.GetProperty("Type").GetConstantValue(),
        //                type => type);
        //}


        public static void SavePassenger(string email, List<Pax> paxs)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                foreach (var pax in paxs)
                {
                    var paxRecord = new SavedPassengerTableRecord
                    {
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
                }
            }
        }

        public List<PaxForDisplay> GetSavedPassenger(string email)
        {
            try
            {
                var paxList = GetSavedPassengerFromDb(email);
                return ConvertToPaxForDisplay(paxList);
            }
            catch
            {
                return null;
            }
            
        } 

        internal static List<Pax> GetSavedPassengerFromDb(string email)
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

        private static void UpdateDetailsToDb(List<Pax> paxList, string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                foreach (var pax in paxList)
                {
                    UpdateDetailsQuery.GetInstance().Execute(conn, new
                    {
                        Id = pax.Id,
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
                    });
                }
            }
        }

        internal static Type GetService(ProductType type)
        {
            return _serviceList[type];
        }

        public List<PaxForDisplay> ConvertToPaxForDisplay(List<Pax> pax)
        {
            return pax != null
                ? pax.Select(ConvertToPaxForDisplay).ToList()
                : null;
        }

        public PaxForDisplay ConvertToPaxForDisplay(Pax pax)
        {
            if (pax == null)
                return null;

            var name = pax.FirstName == pax.LastName 
                ? pax.LastName 
                : pax.FirstName + " " + pax.LastName;

            return new PaxForDisplay
            {
                Type = pax.Type,
                Title = pax.Title,
                Name = name,
                Gender = pax.Gender,
                DateOfBirth = pax.DateOfBirth,
                Nationality = pax.Nationality,
                PassportNumber = pax.PassportNumber,
                PassportExpiryDate = pax.PassportExpiryDate,
                PassportCountry = pax.PassportCountry
            };
        }

        public List<Pax> ConvertToPax(List<PaxForDisplay> pax)
        {
            return pax != null
                ? pax.Select(ConvertToPax).ToList()
                : null;
        }
        
        public Pax ConvertToPax(PaxForDisplay pax)
        {
            if (pax == null)
                return null;

            string first, last;
            if (pax.Name == null)
            {
                first = null;
                last = null;
            }
            else
            {
                var splittedName = pax.Name.Trim().Split(' ');
                if (splittedName.Length == 1)
                {
                    first = pax.Name;
                    last = pax.Name;
                }
                else
                {
                    first = pax.Name.Substring(0, pax.Name.LastIndexOf(' '));
                    last = splittedName[splittedName.Length - 1];
                }
            }

            return new Pax
            {
                Type = pax.Type.GetValueOrDefault(),
                Title = pax.Title.GetValueOrDefault(),
                FirstName = first,
                LastName = last,
                Gender = pax.Title.GetValueOrDefault() == Title.Mister ? Gender.Male : Gender.Female,
                DateOfBirth = pax.DateOfBirth,
                Nationality = pax.Nationality,
                PassportNumber = pax.PassportNumber,
                PassportExpiryDate = pax.PassportExpiryDate,
                PassportCountry = pax.PassportCountry
            };
        }
    }
}
