using System;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Database;
using Lunggo.Framework.TestHelpers;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.PaymentTest.Public.SubmitPayment
{
    public partial class SubmitPaymentTest
    {
        [TestMethod]
        public void Should_convert_valid_data_from_payment_record_to_payment_details()
        {
            var expected = new PaymentTableRecord
            {
                RsvNo = "123456789",
                MediumCd = PaymentMediumCd.Mnemonic(PaymentMedium.Veritrans),
                MethodCd = PaymentMethodCd.Mnemonic(PaymentMethod.BcaKlikpay),
                SubMethod = PaymentSubmethodCd.Mnemonic(PaymentSubmethod.BCA),
                StatusCd = PaymentStatusCd.Mnemonic(PaymentStatus.Failed),
                Time = DateTime.Now,
                TimeLimit = DateTime.Now,
                TransferAccount = "1234567890",
                RedirectionUrl = "http://234567890",
                ExternalId = "87654321",
                DiscountCode = "asdfghjkl",
                OriginalPriceIdr = 1234567890,
                DiscountNominal = 987654321,
                Surcharge = 3456789,
                UniqueCode = 8765432,
                FinalPriceIdr = 876543234,
                PaidAmountIdr = 654345,
                LocalCurrencyCd = "USD",
                LocalRate = 12,
                LocalFinalPrice = 47384123,
                LocalPaidAmount = 47297424,
                InvoiceNo = "asdfg123456",
                InsertBy = "LunggoTester",
                InsertDate = DateTime.UtcNow,
                InsertPgId = "0"
            };
            DbService.GetInstance().Init("Server=tcp:travorama-development-sql-server.database.windows.net,1433;Database=travorama-local;User ID=developer@travorama-development-sql-server;Password=Standar1234;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;");

            var actual = PaymentService.GetInstance().InvokePrivate<PaymentDetails>("ConvertPaymentRecordToPaymentDetails", expected);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.RsvNo, actual.RsvNo);
            Assert.AreEqual(expected.MediumCd, PaymentMediumCd.Mnemonic(actual.Medium));
            Assert.AreEqual(expected.MethodCd, PaymentMethodCd.Mnemonic(actual.Method));
            Assert.AreEqual(expected.StatusCd, PaymentStatusCd.Mnemonic(actual.Status));
            Assert.AreEqual(expected.Time, actual.Time);
            Assert.AreEqual(expected.TimeLimit, actual.TimeLimit);
            Assert.AreEqual(expected.TransferAccount, actual.TransferAccount);
            Assert.AreEqual(expected.RedirectionUrl, actual.RedirectionUrl);
            Assert.AreEqual(expected.ExternalId, actual.ExternalId);
            Assert.AreEqual(expected.DiscountCode, actual.DiscountCode);
            Assert.AreEqual(expected.OriginalPriceIdr, actual.OriginalPriceIdr);
            Assert.AreEqual(expected.DiscountNominal, actual.DiscountNominal);
            Assert.AreEqual(expected.Surcharge, actual.Surcharge);
            Assert.AreEqual(expected.UniqueCode, actual.UniqueCode);
            Assert.AreEqual(expected.FinalPriceIdr, actual.FinalPriceIdr);
            Assert.AreEqual(expected.PaidAmountIdr, actual.PaidAmountIdr);
            Assert.AreEqual(expected.LocalCurrencyCd, actual.LocalCurrency.Symbol);
            Assert.AreEqual(expected.LocalRate, actual.LocalCurrency.Rate);
            Assert.AreEqual(expected.LocalFinalPrice, actual.LocalFinalPrice);
            Assert.AreEqual(expected.LocalPaidAmount, actual.LocalPaidAmount);
            Assert.AreEqual(expected.InvoiceNo, actual.InvoiceNo);
        }

        [TestMethod]
        public void Should_return_exception_from_DB_when_rsvNo_not_found()
        {
            var rsvNo = Guid.NewGuid().ToString();
            DbService.GetInstance().Init("Server=tcp:travorama-development-sql-server.database.windows.net,1433;Database=travorama-local;User ID=developer@travorama-development-sql-server;Password=Standar1234;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;");
            Assert.ThrowsException<InvalidOperationException>(() => PaymentService.GetInstance().GetPaymentDetails(rsvNo));
        }

        [TestMethod]
        public void Should_have_sum_of_all_prices_from_rsv_for_cart_details()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Should_have_sum_of_all_unique_codes_from_rsv_for_cart_details()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Should_return_not_found_when_ID_does_not_match_rsv_or_cart()
        {
            throw new NotImplementedException();
        }
    }
}