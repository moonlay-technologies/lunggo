using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Model
{
    public class Discount
    {
        public long Id { get; set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string DisplayName { get; internal set; }
        public decimal Percentage { get; internal set; }
        public decimal Constant { get; internal set; }
        public Currency Currency { get; internal set; }
        public bool IsFlat { get; internal set; }
        public bool IsActive { get; internal set; }
        public List<long> RsvRuleIds { get; internal set; }

        internal void InsertToDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var discountId = DiscountIdSequence.GetInstance().GetNext();
                DiscountTableRepo.GetInstance().Insert(conn, new DiscountTableRecord
                {
                    Id = discountId,
                    RsvRuleIds = string.Join(",", RsvRuleIds),
                    Name = Name,
                    Description = Description,
                    DisplayName = DisplayName,
                    Percentage = Percentage,
                    Constant = Constant,
                    CurrencyCd = Currency,
                    IsFlat = IsFlat,
                    IsActive = IsActive,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                });
            }
        }

        internal static Discount GetFromDb(long discountId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetDiscountQuery.GetInstance().Execute(conn, new { DiscountId = discountId }).Single();
                return new Discount
                {
                    Name = record.Name,
                    Description = record.Description,
                    DisplayName = record.DisplayName,
                    Percentage = record.Percentage.GetValueOrDefault(),
                    Constant = record.Constant.GetValueOrDefault(),
                    Currency = new Currency(record.CurrencyCd),
                    IsFlat = record.IsFlat.GetValueOrDefault(),
                    IsActive = record.IsActive.GetValueOrDefault(),
                    RsvRuleIds = record.RsvRuleIds.Split(',').Select(long.Parse).ToList()
                };
            }
        }

        private class GetDiscountQuery : DbQueryBase<GetDiscountQuery, DiscountTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT RsvRuleIds, Name, Description, DisplayName, Percentage, Constant, CurrencyCd, " +
                       "IsFlat, IsActive " +
                       "FROM Discount " +
                       "WHERE Id = @DiscountId";
            }
        }
    }
}