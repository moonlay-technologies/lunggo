using System;
using System.Linq;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.ProductBase.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentDetails
    {
        [JsonProperty("med")]
        public PaymentMedium Medium { get; set; }
        [JsonProperty("met")]
        public PaymentMethod Method { get; set; }
        [JsonProperty("st")]
        public PaymentStatus Status { get; set; }
        [JsonProperty("tm")]
        public DateTime? Time { get; set; }
        [JsonProperty("lim")]
        public DateTime TimeLimit { get; set; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentData Data { get; set; }
        [JsonProperty("account")]
        public string TransferAccount { get; set; }
        [JsonProperty("url")]
        public string RedirectionUrl { get; set; }
        [JsonProperty("id")]
        public string ExternalId { get; set; }
        [JsonProperty("curr")]
        public decimal OriginalPriceIdr { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountNominal { get; set; }
        public Discount Discount { get; set; }
        public decimal TransferFee { get; set; }
        public decimal FinalPriceIdr { get; set; }
        [JsonProperty("paid")]
        public decimal PaidAmountIdr { get; set; }
        [JsonProperty("id")]
        public Currency LocalCurrency { get; set; }
        public decimal LocalFinalPrice { get; set; }
        public decimal LocalPaidAmount { get; set; }
        [JsonProperty("ref", NullValueHandling = NullValueHandling.Ignore)]
        public Refund Refund { get; set; }
        public string InvoiceNo { get; set; }

        internal void InsertToDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                PaymentTableRepo.GetInstance().Insert(conn, new PaymentTableRecord
                {
                    RsvNo = rsvNo,
                    Id = PaymentIdSequence.GetInstance().GetNext(),
                    DiscountId = Discount.Id,
                    MediumCd = PaymentMediumCd.Mnemonic(Medium),
                    MethodCd = PaymentMethodCd.Mnemonic(Method),
                    StatusCd = PaymentStatusCd.Mnemonic(Status),
                    Time = Time,
                    TimeLimit = TimeLimit,
                    TransferAccount = TransferAccount,
                    RedirectionUrl = RedirectionUrl,
                    ExternalId = ExternalId,
                    DiscountCode = DiscountCode,
                    OriginalPriceIdr = OriginalPriceIdr,
                    DiscountNominal = DiscountNominal,
                    TransferFee = TransferFee,
                    FinalPriceIdr = FinalPriceIdr,
                    PaidAmountIdr = PaidAmountIdr,
                    LocalCurrencyCd = LocalCurrency,
                    LocalRate = LocalCurrency.Rate,
                    LocalFinalPrice = LocalFinalPrice,
                    LocalPaidAmount = LocalPaidAmount,
                    InvoiceNo = InvoiceNo,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                });
            }
        }

        internal static PaymentDetails GetFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetPaymentQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).Single();
                return new PaymentDetails
                {
                    Medium = PaymentMediumCd.Mnemonic(record.MediumCd),
                    Method = PaymentMethodCd.Mnemonic(record.MethodCd),
                    Status = PaymentStatusCd.Mnemonic(record.StatusCd),
                    Time = record.Time,
                    TimeLimit = record.TimeLimit.GetValueOrDefault(),
                    TransferAccount = record.TransferAccount,
                    RedirectionUrl = record.RedirectionUrl,
                    ExternalId = record.ExternalId,
                    DiscountCode = record.DiscountCode,
                    OriginalPriceIdr = record.OriginalPriceIdr.GetValueOrDefault(),
                    DiscountNominal = record.DiscountNominal.GetValueOrDefault(),
                    TransferFee = record.TransferFee.GetValueOrDefault(),
                    FinalPriceIdr = record.FinalPriceIdr.GetValueOrDefault(),
                    PaidAmountIdr = record.PaidAmountIdr.GetValueOrDefault(),
                    LocalCurrency = new Currency(record.LocalCurrencyCd, record.LocalRate.GetValueOrDefault()),
                    LocalFinalPrice = record.LocalFinalPrice.GetValueOrDefault(),
                    LocalPaidAmount = record.LocalPaidAmount.GetValueOrDefault(),
                    InvoiceNo = record.InvoiceNo,
                    Discount = Discount.GetFromDb(record.DiscountId.GetValueOrDefault())
                };
            }
        }

        private class GetPaymentQuery : QueryBase<GetPaymentQuery, PaymentTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT Id, DiscountId, MediumCd, DiscountId, MethodCd, StatusCd, Time, TimeLimit, Account, " +
                       "Url, ExternalId, DiscountCode, OriginalPrice, DiscountNominal, TransferFee, FinalPrice, " +
                       "PaidAmount, LocalCurrencyCd, LocalRate, LocalFinalPrice, LocalPaidAmount, InvoiceNo " +
                       "FROM Payment " +
                       "WHERE RsvNo = @RsvNo";
            }
        }
    }
}
