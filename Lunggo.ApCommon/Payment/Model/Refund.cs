using System;
using System.Linq;
using System.Xml.Linq;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class RefundForDisplay
    {
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Time { get; set; }
        [JsonProperty("bank", NullValueHandling = NullValueHandling.Ignore)]
        public string BeneficiaryBank { get; set; }
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string BeneficiaryAccount { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }
    }

    public class Refund
    {
        public DateTime Time { get; set; }
        public string BeneficiaryBank { get; set; }
        public string BeneficiaryAccount { get; set; }
        public string RemitterBank { get; set; }
        public string RemitterAccount { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountIdr { get; set; }

        internal void InsertToDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                RefundTableRepo.GetInstance().Insert(conn, new RefundTableRecord
                {
                    RsvNo = rsvNo,
                    Time = Time,
                    BeneficiaryBank = BeneficiaryBank,
                    BeneficiaryAccount = BeneficiaryAccount,
                    RemitterBank = RemitterBank,
                    RemitterAccount = RemitterAccount,
                    CurrencyCd = Currency,
                    Rate = Currency.Rate,
                    Amount = Amount,
                    AmountIdr = AmountIdr,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                });
            }
        }

        internal static Refund GetFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetRefundQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).SingleOrDefault();
                if (record == null)
                    return null;
                return new Refund
                {
                    Time = record.Time.GetValueOrDefault(),
                    BeneficiaryBank = record.BeneficiaryBank,
                    BeneficiaryAccount = record.BeneficiaryAccount,
                    RemitterBank = record.RemitterBank,
                    RemitterAccount = record.RemitterAccount,
                    Currency = new Currency(record.CurrencyCd, record.Rate.GetValueOrDefault()),
                    Amount = record.Amount.GetValueOrDefault(),
                    AmountIdr = record.AmountIdr.GetValueOrDefault()
                };
            }
        }

        private class GetRefundQuery : QueryBase<GetRefundQuery, RefundTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT Time, BeneficiaryBank, BeneficiaryAccount, RemitterBank, RemitterAccount, " +
                       "CurrencyCd, Rate, Amount, AmountIdr " +
                       "FROM Refund " +
                       "WHERE RsvNo = @RsvNo";
            }
        }
    }
}