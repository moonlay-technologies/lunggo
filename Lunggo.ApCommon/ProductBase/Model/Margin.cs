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
    public sealed class Margin
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public decimal Constant { get; set; }
        public Currency Currency { get; set; }
        public bool IsFlat { get; set; }
        public bool IsActive { get; set; }
        public long RuleId { get; set; }

        internal void InsertToDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                MarginTableRepo.GetInstance().Insert(conn, new MarginTableRecord
                {
                    Id = MarginIdSequence.GetInstance().GetNext(),
                    Name = Name,
                    Description = Description,
                    Percentage = Percentage,
                    Constant = Constant,
                    CurrencyCd = Currency,
                    IsFlat = IsFlat,
                    IsActive = IsActive

                });
            }
        }

        internal static Margin GetFromDb(long marginId, out long orderRuleId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetMarginQuery.GetInstance().Execute(conn, new { MarginId = marginId }).Single();
                orderRuleId = record.OrderRuleId.GetValueOrDefault();
                return new Margin
                {
                    Name = record.Name,
                    Description = record.Description,
                    Percentage = record.Percentage.GetValueOrDefault(),
                    Constant = record.Constant.GetValueOrDefault(),
                    Currency = new Currency(record.CurrencyCd),
                    IsFlat = record.IsFlat.GetValueOrDefault(),
                    IsActive = record.IsActive.GetValueOrDefault()
                };
            }
        }

        internal static List<Margin> GetFromDb(ProductType productType, out List<long> orderRuleIds)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var records = GetMarginsQuery.GetInstance().Execute(conn, new {ProductType = (int) productType});
                var margins = new List<Margin>();
                orderRuleIds = new List<long>();
                foreach (var record in records)
                {
                    margins.Add(new Margin
                    {
                        Name = record.Name,
                        Description = record.Description,
                        Percentage = record.Percentage.GetValueOrDefault(),
                        Constant = record.Constant.GetValueOrDefault(),
                        Currency = new Currency(record.CurrencyCd),
                        IsFlat = record.IsFlat.GetValueOrDefault(),
                        IsActive = record.IsActive.GetValueOrDefault()
                    });
                    orderRuleIds.Add(record.OrderRuleId.GetValueOrDefault());
                }
                return margins;
            }
        }

        private class GetMarginQuery : QueryBase<GetMarginQuery, MarginTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT OrderRuleId, Name, Description, Percentage, Constant, CurrencyCd, IsFlat, IsActive " +
                       "FROM Margin " +
                       "WHERE Id = @MarginId";
            }
        }

        private class GetMarginsQuery : QueryBase<GetMarginsQuery, MarginTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT Id, OrderRuleId, Name, Description, Percentage, Constant, CurrencyCd, IsFlat, IsActive " +
                       "FROM Margin " +
                       "WHERE CAST(OrderRuleId AS NVARCHAR) LIKE @ProductType + '%'";
            }
        }


        //public static Margin GetMatchingMargin(OrderBase order)
        //{
        //    var margins = GetAllOrderMargins(order.Type);
        //    var margin = GetFirstMatch(margins, order);
        //}

        //private static List<Margin> GetAllOrderMargins(ProductType type)
        //{
        //    var margins = GetAllOrderMarginsFromDb(type);
        //}

        //private static List<Margin> GetAllOrderMarginsFromDb(ProductType type)
        //{
        //    using (var conn = DbService.GetInstance().GetOpenConnection())
        //    {
        //        var marginRecords = GetAllOrderMarginsQuery.GetInstance().Execute(conn, new {ProductType = (int) type}).ToList();
        //        var margins = marginRecords.Select(record =>
        //        {
        //            GetOrderRuleFromDb(ruleId)
        //        }).ToList();
        //        {
                    
        //        }
        //    }
        //}
    }
}
