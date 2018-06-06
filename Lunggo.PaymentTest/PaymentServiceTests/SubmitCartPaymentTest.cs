using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Account.Service;
using Lunggo.ApCommon.Payment.Cache;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Model.Data;
using Lunggo.ApCommon.Payment.Processor;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lunggo.PaymentTest.PaymentServiceTests
{
    [TestClass]
    public class SubmitCartPaymentTest
    {
        [TestMethod]
        // Should return final payment details and isUpdated as true when have all required valid data and without voucher
        public void Should_return_final_payment_details_and_isUpdated_as_true_when_have_all_required_valid_data_and_without_voucher()
        {
            // TODO: should mock GetCart instead

            //var cartId = "abc";
            //var method = PaymentMethod.CreditCard;
            //var submethod = PaymentSubmethod.BRI;
            //var paymentData = new PaymentData { CreditCard = new CreditCard { TokenId = "asbdcd" } };
            //var discCd = (string)null;

            //var procMock = new Mock<PaymentProcessorService>();
            //var dbMock = new Mock<PaymentDbService>();
            //var cacheMock = new Mock<PaymentCacheService>();

            //var rsvNo1 = "1234";
            //var rsvNo2 = "5678";
            //cacheMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string> { rsvNo1, rsvNo2 });
            //dbMock.Setup(m => m.GetTrxRsvNos(cartId)).Returns(new List<string> { rsvNo1, rsvNo2 });
            //dbMock.Setup(m => m.GetPaymentDetails(rsvNo1)).Returns(new RsvPaymentDetails());
            //dbMock.Setup(m => m.GetPaymentDetails(rsvNo2)).Returns(new RsvPaymentDetails());
            //dbMock.Setup(m => m.GetRsvContact(rsvNo1)).Returns(new Contact()
            //{
            //    Name = "dwajhgdyuaw",
            //    CountryCallingCode = "4783",
            //    Title = Title.Mister,
            //    Email = "fnwfnje@nfenj.com",
            //    Phone = "74837483"
            //});
            //procMock.Setup(m => m.ProcessPayment(It.IsAny<RsvPaymentDetails>(), It.IsAny<TransactionDetails>())).Returns(true);

            //var service = new Mock<PaymentService>(procMock.Object, dbMock.Object, cacheMock.Object);
            //service.Setup(m => m.GetSurchargeNominal(It.IsAny<RsvPaymentDetails>())).Returns(272838);
            //service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(new TrxPaymentDetails
            //{
            //    RsvPaymentDetails = new List<RsvPaymentDetails>
            //    {
            //        new RsvPaymentDetails
            //        {
            //            RsvNo = rsvNo1,
            //            Medium = PaymentMedium.Veritrans,
            //            Method = PaymentMethod.BcaKlikpay,
            //            Submethod = PaymentSubmethod.BCA,
            //            Status = PaymentStatus.MethodNotSet,
            //            Time = DateTime.Now,
            //            TimeLimit = DateTime.Now,
            //            TransferAccount = "1234567890",
            //            RedirectionUrl = "http://234567890",
            //            ExternalId = "87654321",
            //            DiscountCode = "asdfghjkl",
            //            OriginalPriceIdr = 1234567890,
            //            DiscountNominal = 987654321,
            //            Surcharge = 3456789,
            //            UniqueCode = 8765432,
            //            FinalPriceIdr = 876543234,
            //            PaidAmountIdr = 654345,
            //            LocalCurrency = new Currency("USD", 123, 321),
            //            LocalFinalPrice = 47384123,
            //            LocalPaidAmount = 47297424,
            //            InvoiceNo = "asdfg123456"
            //        },
            //        new RsvPaymentDetails
            //        {
            //            RsvNo = rsvNo2,
            //            Medium = PaymentMedium.Veritrans,
            //            Method = PaymentMethod.BcaKlikpay,
            //            Submethod = PaymentSubmethod.BCA,
            //            Status = PaymentStatus.MethodNotSet,
            //            Time = DateTime.Now,
            //            TimeLimit = DateTime.Now,
            //            TransferAccount = "1234567890",
            //            RedirectionUrl = "http://234567890",
            //            ExternalId = "87654321",
            //            DiscountCode = "asdfghjkl",
            //            OriginalPriceIdr = 1234567890,
            //            DiscountNominal = 987654321,
            //            Surcharge = 3456789,
            //            UniqueCode = 8765432,
            //            FinalPriceIdr = 876543234,
            //            PaidAmountIdr = 654345,
            //            LocalCurrency = new Currency("USD", 123, 321),
            //            LocalFinalPrice = 47384123,
            //            LocalPaidAmount = 47297424,
            //            InvoiceNo = "asdfg123456"
            //        }
            //    },
            //    TrxId = "TRX123457890",
            //    Medium = PaymentMedium.Veritrans,
            //    Method = PaymentMethod.BcaKlikpay,
            //    Submethod = PaymentSubmethod.BCA,
            //    Status = PaymentStatus.MethodNotSet,
            //    Time = DateTime.Now,
            //    TimeLimit = DateTime.Now,
            //    TransferAccount = "1234567890",
            //    RedirectionUrl = "http://234567890",
            //    ExternalId = "87654321",
            //    DiscountCode = discCd,
            //    OriginalPriceIdr = 1234567890,
            //    DiscountNominal = 987654321,
            //    Surcharge = 3456789,
            //    UniqueCode = 8765432,
            //    FinalPriceIdr = 876543234,
            //    PaidAmountIdr = 654345,
            //    LocalCurrency = new Currency("USD", 123, 321),
            //    LocalFinalPrice = 47384123,
            //    LocalPaidAmount = 47297424,
            //    InvoiceNo = "asdfg123456"
            //});

            //var result = service.Object.SubmitCartPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated);

            //Assert.AreEqual(result.Method, method);
            //Assert.IsTrue(result.RsvPaymentDetails.TrueForAll(d => d.Method == method));
            //Assert.AreEqual(result.Submethod, submethod);
            //Assert.IsTrue(result.RsvPaymentDetails.TrueForAll(d => d.Submethod == submethod));
            //Assert.AreEqual(result.DiscountCode, discCd);
            //Assert.IsTrue(result.RsvPaymentDetails.TrueForAll(d => d.DiscountCode == discCd));
            //Assert.IsTrue(isUpdated);
        }

        [TestMethod]
        // Should return null when cartId is not valid
        public void Should_return_null_when_cartId_is_not_valid()
        {
            var cartId = "abc";
            var method = PaymentMethod.CimbClicks;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = new PaymentData();
            var discCd = "acc";

            var procMock = new Mock<PaymentProcessorService>();
            var dbMock = new Mock<PaymentDbService>();
            var cacheMock = new Mock<PaymentCacheService>();

            cacheMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string>());

            var service = new Mock<PaymentService>(procMock.Object, dbMock.Object, cacheMock.Object);
            service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns((TrxPaymentDetails)null);

            var actual = service.Object.SubmitCartPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated);

            Assert.IsNull(actual);
        }

        [TestMethod]
        // Should return null when cart id does not contain any reservation
        public void Should_return_null_when_cart_id_does_not_contain_any_reservation()
        {
            var cartId = "abc";
            var method = PaymentMethod.CimbClicks;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = new PaymentData();
            var discCd = "acc";

            var procMock = new Mock<PaymentProcessorService>();
            var dbMock = new Mock<PaymentDbService>();
            var cacheMock = new Mock<PaymentCacheService>();

            cacheMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string>());

            var service = new Mock<PaymentService>(procMock.Object, dbMock.Object, cacheMock.Object);
            service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(new TrxPaymentDetails
            {
                RsvPaymentDetails = new List<RsvPaymentDetails>(),
                TrxId = "TRX123457890",
                Medium = PaymentMediumCd.Mnemonic("VERI"),
                Method = PaymentMethodCd.Mnemonic("BKP"),
                Submethod = PaymentSubmethodCd.Mnemonic("BCA"),
                Status = PaymentStatusCd.Mnemonic("SET"),
                Time = DateTime.Now,
                TimeLimit = DateTime.Now,
                TransferAccount = "1234567890",
                RedirectionUrl = "http://234567890",
                ExternalId = "87654321",
                DiscountCode = discCd,
                OriginalPriceIdr = 0,
                DiscountNominal = 0,
                Surcharge = 0,
                UniqueCode = 0,
                FinalPriceIdr = 0,
                PaidAmountIdr = 0,
                LocalCurrency = new Currency("USD", 123, 321),
                LocalFinalPrice = 0,
                LocalPaidAmount = 0,
                InvoiceNo = "asdfg123456"
            });

            var actual = service.Object.SubmitCartPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated);

            Assert.IsNull(actual);
        }

        [TestMethod]
        // Should_return_payment_details_with_settled_status_when_transaction_status_is_settled
        public void Should_return_payment_details_with_settled_status_when_transaction_status_is_settled()
        {
            //TODO: should mock GetCart instead

            //var cartId = "abc";
            //var method = PaymentMethod.CimbClicks;
            //var submethod = PaymentSubmethod.BRI;
            //var paymentData = new PaymentData();
            //var discCd = "acc";

            //var procMock = new Mock<PaymentProcessorService>();
            //var dbMock = new Mock<PaymentDbService>();
            //var cacheMock = new Mock<PaymentCacheService>();

            //var rsvNo = "1234";
            //cacheMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string> { rsvNo });
            //dbMock.Setup(m => m.GetTrxRsvNos(cartId)).Returns(new List<string> { rsvNo });
            //dbMock.Setup(m => m.GetPaymentDetails(rsvNo)).Returns(new RsvPaymentDetails());
            //procMock.Setup(m => m.ProcessPayment(It.IsAny<RsvPaymentDetails>(), It.IsAny<TransactionDetails>())).Returns(true);

            //var service = new Mock<PaymentService>(procMock.Object, dbMock.Object, cacheMock.Object);
            //service.Setup(m => m.GetSurchargeNominal(It.IsAny<RsvPaymentDetails>())).Returns(272838);
            //service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(new TrxPaymentDetails
            //{
            //    RsvPaymentDetails = new List<RsvPaymentDetails>
            //    {
            //        new RsvPaymentDetails
            //        {
            //            RsvNo = rsvNo,
            //            Medium = PaymentMediumCd.Mnemonic("VERI"),
            //            Method = PaymentMethodCd.Mnemonic("BKP"),
            //            Submethod = PaymentSubmethodCd.Mnemonic("BCA"),
            //            Status = PaymentStatusCd.Mnemonic("SET"),
            //            Time = DateTime.Now,
            //            TimeLimit = DateTime.Now,
            //            TransferAccount = "1234567890",
            //            RedirectionUrl = "http://234567890",
            //            ExternalId = "87654321",
            //            DiscountCode = "asdfghjkl",
            //            OriginalPriceIdr = 1234567890,
            //            DiscountNominal = 987654321,
            //            Surcharge = 3456789,
            //            UniqueCode = 8765432,
            //            FinalPriceIdr = 876543234,
            //            PaidAmountIdr = 654345,
            //            LocalCurrency = new Currency("USD", 123, 321),
            //            LocalFinalPrice = 47384123,
            //            LocalPaidAmount = 47297424,
            //            InvoiceNo = "asdfg123456"
            //        }
            //    },
            //    TrxId = "TRX123457890",
            //    Medium = PaymentMediumCd.Mnemonic("VERI"),
            //    Method = PaymentMethodCd.Mnemonic("BKP"),
            //    Submethod = PaymentSubmethodCd.Mnemonic("BCA"),
            //    Status = PaymentStatusCd.Mnemonic("SET"),
            //    Time = DateTime.Now,
            //    TimeLimit = DateTime.Now,
            //    TransferAccount = "1234567890",
            //    RedirectionUrl = "http://234567890",
            //    ExternalId = "87654321",
            //    DiscountCode = "asdfghjkl",
            //    OriginalPriceIdr = 1234567890,
            //    DiscountNominal = 987654321,
            //    Surcharge = 3456789,
            //    UniqueCode = 8765432,
            //    FinalPriceIdr = 876543234,
            //    PaidAmountIdr = 654345,
            //    LocalCurrency = new Currency("USD", 123, 321),
            //    LocalFinalPrice = 47384123,
            //    LocalPaidAmount = 47297424,
            //    InvoiceNo = "asdfg123456"
            //});

            //var actual = service.Object.SubmitCartPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated);

            //Assert.AreEqual(PaymentStatus.Settled, actual.Status);

        }

        [TestMethod]
        // Should_return_payment_details_with_verifying_status_when_transaction_status_is_verifying
        public void Should_return_payment_details_with_verifying_status_when_transaction_status_is_verifying()
        {
            //TODO: should mock GetCart instead

            //var cartId = "abc";
            //var method = PaymentMethod.CimbClicks;
            //var submethod = PaymentSubmethod.BRI;
            //var paymentData = new PaymentData();
            //var discCd = "acc";

            //var procMock = new Mock<PaymentProcessorService>();
            //var dbMock = new Mock<PaymentDbService>();
            //var cacheMock = new Mock<PaymentCacheService>();

            //var rsvNo = "1234";
            //cacheMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string> { rsvNo });
            //dbMock.Setup(m => m.GetTrxRsvNos(cartId)).Returns(new List<string> { rsvNo });
            //dbMock.Setup(m => m.GetPaymentDetails(rsvNo)).Returns(new RsvPaymentDetails());
            //procMock.Setup(m => m.ProcessPayment(It.IsAny<RsvPaymentDetails>(), It.IsAny<TransactionDetails>())).Returns(true);

            //var service = new Mock<PaymentService>(procMock.Object, dbMock.Object, cacheMock.Object);
            //service.Setup(m => m.GetSurchargeNominal(It.IsAny<RsvPaymentDetails>())).Returns(272838);
            //service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(new TrxPaymentDetails
            //{
            //    RsvPaymentDetails = new List<RsvPaymentDetails>
            //    {
            //        new RsvPaymentDetails
            //        {
            //            RsvNo = rsvNo,
            //            Medium = PaymentMediumCd.Mnemonic("VERI"),
            //            Method = PaymentMethodCd.Mnemonic("BKP"),
            //            Submethod = PaymentSubmethodCd.Mnemonic("BCA"),
            //            Status = PaymentStatusCd.Mnemonic("VER"),
            //            Time = DateTime.Now,
            //            TimeLimit = DateTime.Now,
            //            TransferAccount = "1234567890",
            //            RedirectionUrl = "http://234567890",
            //            ExternalId = "87654321",
            //            DiscountCode = "asdfghjkl",
            //            OriginalPriceIdr = 1234567890,
            //            DiscountNominal = 987654321,
            //            Surcharge = 3456789,
            //            UniqueCode = 8765432,
            //            FinalPriceIdr = 876543234,
            //            PaidAmountIdr = 654345,
            //            LocalCurrency = new Currency("USD", 123, 321),
            //            LocalFinalPrice = 47384123,
            //            LocalPaidAmount = 47297424,
            //            InvoiceNo = "asdfg123456"
            //        }
            //    },
            //    TrxId = "TRX123457890",
            //    Medium = PaymentMediumCd.Mnemonic("VERI"),
            //    Method = PaymentMethodCd.Mnemonic("BKP"),
            //    Submethod = PaymentSubmethodCd.Mnemonic("BCA"),
            //    Status = PaymentStatusCd.Mnemonic("VER"),
            //    Time = DateTime.Now,
            //    TimeLimit = DateTime.Now,
            //    TransferAccount = "1234567890",
            //    RedirectionUrl = "http://234567890",
            //    ExternalId = "87654321",
            //    DiscountCode = "asdfghjkl",
            //    OriginalPriceIdr = 1234567890,
            //    DiscountNominal = 987654321,
            //    Surcharge = 3456789,
            //    UniqueCode = 8765432,
            //    FinalPriceIdr = 876543234,
            //    PaidAmountIdr = 654345,
            //    LocalCurrency = new Currency("USD", 123, 321),
            //    LocalFinalPrice = 47384123,
            //    LocalPaidAmount = 47297424,
            //    InvoiceNo = "asdfg123456"
            //});

            //var actual = service.Object.SubmitCartPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated);

            //Assert.AreEqual(PaymentStatus.Verifying, actual.Status);

        }

        [TestMethod]
        // Should return exception when method is credit card but payment data is null
        public void Should_return_exception_when_method_is_credit_card_but_payment_data_is_null()
        {
            var cartId = "abc";
            var method = PaymentMethod.CreditCard;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = (PaymentData)null;
            var discCd = "acc";

            var procMock = new Mock<PaymentProcessorService>();
            var dbMock = new Mock<PaymentDbService>();
            var cacheMock = new Mock<PaymentCacheService>();

            var rsvNo = "1234";
            cacheMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string> { rsvNo });
            dbMock.Setup(m => m.GetTrxRsvNos(cartId)).Returns(new List<string> { rsvNo });
            dbMock.Setup(m => m.GetPaymentDetails(rsvNo)).Returns(new RsvPaymentDetails());
            procMock.Setup(m => m.ProcessPayment(It.IsAny<RsvPaymentDetails>(), It.IsAny<TransactionDetails>())).Returns(true);

            var service = new Mock<PaymentService>(procMock.Object, dbMock.Object, cacheMock.Object);
            service.Setup(m => m.GetSurchargeNominal(It.IsAny<RsvPaymentDetails>())).Returns(272838);
            service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(new TrxPaymentDetails
            {
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = rsvNo,
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
                    }
                },
                TrxId = "TRX123457890",
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
            });

            Assert.ThrowsException<ArgumentException>(() =>
                service.Object.SubmitCartPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));
        }

        [TestMethod]
        // Should return exception when method is credit card but payment data does not include credit card data
        public void Should_return_exception_when_method_is_credit_card_but_payment_data_does_not_include_credit_card_data()
        {
            var cartId = "abc";
            var method = PaymentMethod.CreditCard;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = new PaymentData();
            var discCd = "acc";

            var procMock = new Mock<PaymentProcessorService>();
            var dbMock = new Mock<PaymentDbService>();
            var cacheMock = new Mock<PaymentCacheService>();

            var rsvNo = "1234";
            cacheMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string> { rsvNo });
            dbMock.Setup(m => m.GetTrxRsvNos(cartId)).Returns(new List<string> { rsvNo });
            dbMock.Setup(m => m.GetPaymentDetails(rsvNo)).Returns(new RsvPaymentDetails());
            procMock.Setup(m => m.ProcessPayment(It.IsAny<RsvPaymentDetails>(), It.IsAny<TransactionDetails>())).Returns(true);

            var service = new Mock<PaymentService>(procMock.Object, dbMock.Object, cacheMock.Object);
            service.Setup(m => m.GetSurchargeNominal(It.IsAny<RsvPaymentDetails>())).Returns(272838);
            service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(new TrxPaymentDetails
            {
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = rsvNo,
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
                    }
                },
                TrxId = "TRX123457890",
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
            });

            Assert.ThrowsException<ArgumentException>(() =>
                service.Object.SubmitCartPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));

        }

        [TestMethod]
        // Should return exception when method is credit card but credit card data does not contains token
        public void Should_return_exception_when_method_is_credit_card_but_credit_card_data_does_not_contains_token()
        {
            var cartId = "abc";
            var method = PaymentMethod.CreditCard;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = new PaymentData
            {
                CreditCard = new CreditCard()
            };
            var discCd = "acc";

            var procMock = new Mock<PaymentProcessorService>();
            var dbMock = new Mock<PaymentDbService>();
            var cacheMock = new Mock<PaymentCacheService>();

            var rsvNo = "1234";
            cacheMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string> { rsvNo });
            dbMock.Setup(m => m.GetTrxRsvNos(cartId)).Returns(new List<string> { rsvNo });
            dbMock.Setup(m => m.GetPaymentDetails(rsvNo)).Returns(new RsvPaymentDetails());
            procMock.Setup(m => m.ProcessPayment(It.IsAny<RsvPaymentDetails>(), It.IsAny<TransactionDetails>())).Returns(true);

            var service = new Mock<PaymentService>(procMock.Object, dbMock.Object, cacheMock.Object);
            service.Setup(m => m.GetSurchargeNominal(It.IsAny<RsvPaymentDetails>())).Returns(272838);
            service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(new TrxPaymentDetails
            {
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = rsvNo,
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
                    }
                },
                TrxId = "TRX123457890",
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
            });

            Assert.ThrowsException<ArgumentException>(() =>
                service.Object.SubmitCartPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));
        }
    }
}
