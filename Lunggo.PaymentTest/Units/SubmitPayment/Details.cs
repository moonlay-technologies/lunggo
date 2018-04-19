using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.TestHelpers;
using Lunggo.Repository.TableRecord;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.PaymentTest.Units.SubmitPayment
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
                LocalCurrencyRounding = 10,
                LocalFinalPrice = 47384123,
                LocalPaidAmount = 47297424,
                InvoiceNo = "asdfg123456",
                InsertBy = "LunggoTester",
                InsertDate = DateTime.UtcNow,
                InsertPgId = "0"
            };
            AppInitializer.InitDatabaseService();

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
            Assert.AreEqual(expected.LocalCurrencyRounding, actual.LocalCurrency.RoundingOrder);
            Assert.AreEqual(expected.LocalFinalPrice, actual.LocalFinalPrice);
            Assert.AreEqual(expected.LocalPaidAmount, actual.LocalPaidAmount);
            Assert.AreEqual(expected.InvoiceNo, actual.InvoiceNo);
        }

        [TestMethod]
        public void Should_convert_valid_data_from_payment_details_to_payment_record()
        {
            var expected = new PaymentDetails
            {
                RsvNo = "123456789",
                Medium = PaymentMediumCd.Mnemonic("VERI"),
                Method = PaymentMethodCd.Mnemonic("BKP"),
                Submethod = PaymentSubmethodCd.Mnemonic("BCA"),
                Status = PaymentStatusCd.Mnemonic("SET"),
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
                LocalCurrency = new Currency("USD", 123, 321),
                LocalFinalPrice = 47384123,
                LocalPaidAmount = 47297424,
                InvoiceNo = "asdfg123456"
            };
            AppInitializer.InitDatabaseService();

            var actual = PaymentService.GetInstance().InvokePrivate<PaymentTableRecord>("ConvertPaymentDetailsToPaymentRecord", expected);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.RsvNo, actual.RsvNo);
            Assert.AreEqual(expected.Medium, PaymentMediumCd.Mnemonic(actual.MediumCd));
            Assert.AreEqual(expected.Method, PaymentMethodCd.Mnemonic(actual.MethodCd));
            Assert.AreEqual(expected.Status, PaymentStatusCd.Mnemonic(actual.StatusCd));
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
            Assert.AreEqual(expected.LocalCurrency, new Currency(
                    actual.LocalCurrencyCd,
                    actual.LocalRate.GetValueOrDefault(),
                    actual.LocalCurrencyRounding.GetValueOrDefault()));
            Assert.AreEqual(expected.LocalFinalPrice, actual.LocalFinalPrice);
            Assert.AreEqual(expected.LocalPaidAmount, actual.LocalPaidAmount);
            Assert.AreEqual(expected.InvoiceNo, actual.InvoiceNo);
            Assert.IsNotNull(actual.InsertBy);
            Assert.IsNotNull(actual.InsertDate);
            Assert.IsNotNull(actual.InsertPgId);
        }

        [TestMethod]
        // Should generate valid data when creating new payment details
        public void Should_generate_valid_data_when_creating_new_payment_details()
        {
            var rsvNo = "12345678";
            var price = 100000M;
            var currency = new Currency("ASD", 100, 200);
            var timelimit = DateTime.Now;

            var actual = PaymentService.GetInstance().InvokePrivate<PaymentDetails>("CreateNewPaymentDetails", rsvNo, price, currency, timelimit);

            Assert.AreEqual(rsvNo, actual.RsvNo);
            Assert.AreEqual(price, actual.OriginalPriceIdr);
            Assert.AreEqual(currency, actual.LocalCurrency);
            Assert.AreEqual(PaymentStatus.Pending, actual.Status);
            Assert.AreEqual(timelimit.AddMinutes(-10), actual.TimeLimit);
        }
	

        [TestMethod]
        public void Should_return_exception_from_DB_when_rsvNo_not_found()
        {
            var rsvNo = Guid.NewGuid().ToString();
            AppInitializer.InitDatabaseService();
            Assert.ThrowsException<InvalidOperationException>(() => PaymentService.GetInstance().GetPaymentDetails(rsvNo));
        }

        [TestMethod]
        // Should return exception when cart does not contain any reservation
        public void Should_return_exception_when_cart_with_reservation_is_not_found()
        {
            AppInitializer.InitDatabaseService();
            AppInitializer.InitRedisService();
            Assert.ThrowsException<Exception>(() => PaymentService.GetInstance().GetCart(Guid.NewGuid().ToString()));
        }


        [TestMethod]
        public void Should_have_sum_of_all_pricing_from_rsv_for_cart_payment_details()
        {
            var a = new PaymentDetails
            {
                OriginalPriceIdr = 12345678,
                FinalPriceIdr = 8765432,
                UniqueCode = -76543,
                LocalFinalPrice = 34567890
            };
            var b = new PaymentDetails
            {
                OriginalPriceIdr = 849574857485,
                FinalPriceIdr = -283283,
                UniqueCode = 4783843,
                LocalFinalPrice = 2738248
            };
            var c = new PaymentDetails
            {
                OriginalPriceIdr = -38433,
                FinalPriceIdr = 21734824,
                UniqueCode = 7684864,
                LocalFinalPrice = -27483
            };

            var actual = PaymentService.GetInstance().InvokePrivate<PaymentDetails>("AggregateRsvPaymentDetails", new List<PaymentDetails> { a, b, c });

            Assert.AreEqual(a.OriginalPriceIdr + b.OriginalPriceIdr + c.OriginalPriceIdr, actual.OriginalPriceIdr);
            Assert.AreEqual(a.FinalPriceIdr + b.FinalPriceIdr + c.FinalPriceIdr, actual.FinalPriceIdr);
            Assert.AreEqual(a.UniqueCode + b.UniqueCode + c.UniqueCode, actual.UniqueCode);
            Assert.AreEqual(a.LocalFinalPrice + b.LocalFinalPrice + c.LocalFinalPrice, actual.LocalFinalPrice);
        }
    }
}