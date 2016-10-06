using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Model
{
    public class UsedDiscount
    {
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string DisplayName { get; internal set; }
        public decimal Percentage { get; internal set; }
        public decimal Constant { get; internal set; }
        public Currency Currency { get; internal set; }
        public bool IsFlat { get; internal set; }

        internal void InsertToDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UsedDiscountTableRepo.GetInstance().Insert(conn, new UsedDiscountTableRecord
                {
                    RsvNo = rsvNo,
                    Name = Name,
                    Description = Description,
                    DisplayName = DisplayName,
                    Percentage = Percentage,
                    Constant = Constant,
                    CurrencyCd = Currency,
                    IsFlat = IsFlat,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                });
            }
        }

        internal static UsedDiscount GetFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetUsedDiscountQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).SingleOrDefault();

                if (record == null)
                    return null;

                return new UsedDiscount
                {
                    Name = record.Name,
                    Description = record.Description,
                    DisplayName = record.DisplayName,
                    Percentage = record.Percentage.GetValueOrDefault(),
                    Constant = record.Constant.GetValueOrDefault(),
                    Currency = new Currency(record.CurrencyCd),
                    IsFlat = record.IsFlat.GetValueOrDefault()
                };
            }
        }

        private class GetUsedDiscountQuery : DbQueryBase<GetUsedDiscountQuery, UsedDiscountTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT Name, Description, DisplayName, Percentage, Constant, CurrencyCd, IsFlat " +
                       "FROM UsedDiscount " +
                       "WHERE RsvNo = @RsvNo";
            }
        }
    }
}