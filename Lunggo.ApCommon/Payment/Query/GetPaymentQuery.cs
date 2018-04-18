using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Query
{
    public class GetPaymentQuery : DbQueryBase<GetPaymentQuery, PaymentTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT RsvNo, MediumCd, MethodCd, SubMethod, StatusCd, Time, TimeLimit, TransferAccount, RedirectionUrl, " +
                   "ExternalId, DiscountCode, OriginalPriceIdr, DiscountNominal, UniqueCode, FinalPriceIdr, " +
                   "PaidAmountIdr, LocalCurrencyCd, LocalRate, LocalFinalPrice, LocalPaidAmount, InvoiceNo " +
                   "FROM Payment " +
                   "WHERE RsvNo = @RsvNo";
        }
    }
}