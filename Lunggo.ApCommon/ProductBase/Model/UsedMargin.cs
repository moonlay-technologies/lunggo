using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.ProductBase.Query;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.ProductBase.Model
{
    public sealed class UsedMargin
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public decimal Constant { get; set; }
        public Currency Currency { get; set; }
        public bool IsFlat { get; set; }

        internal void InsertToDb(long orderId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UsedMarginTableRepo.GetInstance().Insert(conn, new UsedMarginTableRecord
                {
                    OrderId = orderId,
                    Name = Name,
                    Description = Description,
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

        internal static UsedMargin GetFromDb(long orderId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetUsedMarginQuery.GetInstance().Execute(conn, new { OrderId = orderId }).Single();
                return new UsedMargin
                {
                    Name = record.Name,
                    Description = record.Description,
                    Percentage = record.Percentage.GetValueOrDefault(),
                    Constant = record.Constant.GetValueOrDefault(),
                    Currency = new Currency(record.CurrencyCd),
                    IsFlat = record.IsFlat.GetValueOrDefault()
                };
            }
        }

        private class GetUsedMarginQuery : QueryBase<GetUsedMarginQuery, UsedMarginTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT Name, Description, Percentage, Constant, CurrencyCd, IsFlat " +
                       "FROM UsedMargin " +
                       "WHERE OrderId = @OrderId";
            }
        }
    }
}
