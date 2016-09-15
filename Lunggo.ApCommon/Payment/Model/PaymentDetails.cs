using System;
using System.Linq;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentDetailsForDisplay
    {
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentMethod Method { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentStatus Status { get; set; }
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Time { get; set; }
        [JsonProperty("timeLimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TimeLimit { get; set; }
        [JsonProperty("transferAccount", NullValueHandling = NullValueHandling.Ignore)]
        public string TransferAccount { get; set; }
        [JsonProperty("redirectionUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string RedirectionUrl { get; set; }
        [JsonProperty("originalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OriginalPrice { get; set; }
        [JsonProperty("discountCode", NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountCode { get; set; }
        [JsonProperty("discountNominal", NullValueHandling = NullValueHandling.Ignore)]
        public decimal DiscountNominal { get; set; }
        [JsonProperty("discountName", NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountName { get; set; }
        [JsonProperty("uniqueCode", NullValueHandling = NullValueHandling.Ignore)]
        public decimal UniqueCode { get; set; }
        [JsonProperty("transferFee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TransferFee { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }
        [JsonProperty("finalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal FinalPrice { get; set; }
        [JsonProperty("refund", NullValueHandling = NullValueHandling.Ignore)]
        public RefundForDisplay Refund { get; set; }
        [JsonProperty("invoice", NullValueHandling = NullValueHandling.Ignore)]
        public string InvoiceNo { get; set; }
    }

    public class PaymentDetails
    {
        public PaymentMedium Medium { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public FailureReason FailureReason { get; set; }
        public DateTime? Time { get; set; }
        public DateTime TimeLimit { get; set; }
        public PaymentData Data { get; set; }
        public string TransferAccount { get; set; }
        public string RedirectionUrl { get; set; }
        public string ExternalId { get; set; }
        public decimal OriginalPriceIdr { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountNominal { get; set; }
        public UsedDiscount Discount { get; set; }
        public decimal UniqueCode { get; set; }
        public decimal FinalPriceIdr { get; set; }
        public decimal PaidAmountIdr { get; set; }
        public Currency LocalCurrency { get; set; }
        public decimal LocalFinalPrice { get; set; }
        public decimal LocalPaidAmount { get; set; }
        public Refund Refund { get; set; }
        public string InvoiceNo { get; set; }

        internal void InsertToDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                PaymentTableRepo.GetInstance().Insert(conn, new PaymentTableRecord
                {
                    RsvNo = rsvNo,
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
                    UniqueCode = UniqueCode,
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
                var record = GetPaymentQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).SingleOrDefault();

                if (record == null)
                    return null;

                return new PaymentDetails
                {
                    Medium = PaymentMediumCd.Mnemonic(record.MediumCd),
                    Method = PaymentMethodCd.Mnemonic(record.MethodCd),
                    Status = PaymentStatusCd.Mnemonic(record.StatusCd),
                    Time = record.Time,
                    TimeLimit = DateTime.SpecifyKind(record.TimeLimit.GetValueOrDefault(), DateTimeKind.Utc),
                    TransferAccount = record.TransferAccount,
                    RedirectionUrl = record.RedirectionUrl,
                    ExternalId = record.ExternalId,
                    DiscountCode = record.DiscountCode,
                    OriginalPriceIdr = record.OriginalPriceIdr.GetValueOrDefault(),
                    DiscountNominal = record.DiscountNominal.GetValueOrDefault(),
                    UniqueCode = record.UniqueCode.GetValueOrDefault(),
                    FinalPriceIdr = record.FinalPriceIdr.GetValueOrDefault(),
                    PaidAmountIdr = record.PaidAmountIdr.GetValueOrDefault(),
                    LocalCurrency = new Currency(record.LocalCurrencyCd, record.LocalRate.GetValueOrDefault()),
                    LocalFinalPrice = record.LocalFinalPrice.GetValueOrDefault(),
                    LocalPaidAmount = record.LocalPaidAmount.GetValueOrDefault(),
                    InvoiceNo = record.InvoiceNo,
                    Discount = UsedDiscount.GetFromDb(rsvNo),
                    Refund = Refund.GetFromDb(rsvNo)
                };
            }
        }

        private class GetPaymentQuery : QueryBase<GetPaymentQuery, PaymentTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT MediumCd, MethodCd, StatusCd, Time, TimeLimit, TransferAccount, RedirectionUrl, " +
                       "ExternalId, DiscountCode, OriginalPriceIdr, DiscountNominal, UniqueCode, FinalPriceIdr, " +
                       "PaidAmountIdr, LocalCurrencyCd, LocalRate, LocalFinalPrice, LocalPaidAmount, InvoiceNo " +
                       "FROM Payment " +
                       "WHERE RsvNo = @RsvNo";
            }
        }
    }
}
