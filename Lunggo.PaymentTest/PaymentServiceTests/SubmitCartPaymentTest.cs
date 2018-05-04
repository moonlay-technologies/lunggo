using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Cache;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Model.Data;
using Lunggo.ApCommon.Payment.Processor;
using Lunggo.ApCommon.Payment.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lunggo.PaymentTest.PaymentServiceTests
{
    [TestClass]
    public class SubmitCartPaymentTest
    {
        [TestMethod]
        // Should return final payment details and isUpdated as true when have all required valid data
        public void Should_return_final_payment_details_and_isUpdated_as_true_when_have_all_required_valid_data()
        {
            var cartId = "abc";
            var method = PaymentMethod.CreditCard;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = new PaymentData { CreditCard = new CreditCard { TokenId = "asbdcd" } };
            var discCd = "acc";

            var procMock = new Mock<PaymentProcessorService>();
            var dbMock = new Mock<PaymentDbService>();
            var cacheMock = new Mock<PaymentCacheService>();

            var rsvNo1 = "1234";
            var rsvNo2 = "5678";
            dbMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string> { rsvNo1, rsvNo2 });
            cacheMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string> { rsvNo1, rsvNo2 });
            dbMock.Setup(m => m.GetCartRsvNos(cartId)).Returns(new List<string> { rsvNo1, rsvNo2 });
            dbMock.Setup(m => m.GetPaymentDetails(rsvNo1)).Returns(new PaymentDetails());
            dbMock.Setup(m => m.GetPaymentDetails(rsvNo2)).Returns(new PaymentDetails());
            procMock.Setup(m => m.ProcessPayment(It.IsAny<PaymentDetails>(), It.IsAny<TransactionDetails>())).Returns(true);

            var service = new Mock<PaymentService>(procMock.Object, dbMock.Object, cacheMock.Object);

            service.Setup(m => m.TryApplyVoucher(cartId, discCd, It.IsAny<PaymentDetails>())).Returns(true);
            //service.Setup(m => m.GetCartPaymentDetails(cartId)).Returns();

            var result = service.Object.SubmitCartPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated);

            Assert.AreEqual(cartId, result.CartId);
            Assert.AreEqual(result.Method, method);
            Assert.IsTrue(result.RsvPaymentDetails.TrueForAll(d => d.Method == method));
            Assert.AreEqual(result.Submethod, submethod);
            Assert.IsTrue(result.RsvPaymentDetails.TrueForAll(d => d.Submethod == submethod));
            Assert.AreEqual(result.DiscountCode, discCd);
            Assert.IsTrue(result.RsvPaymentDetails.TrueForAll(d => d.DiscountCode == discCd));
            Assert.IsTrue(isUpdated);
        }


        [TestMethod]
        // Should return exception when cartId is not valid
        public void Should_return_exception_when_cartId_is_not_valid()
        {
            var cartId = "abc";
            var method = PaymentMethod.CreditCard;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = new PaymentData();
            var discCd = "acc";

            var service = new PaymentService();

            Assert.ThrowsException<ArgumentException>(() =>
                service.SubmitPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));
        }

        [TestMethod]
        // Should return exception when cart id does not contain any reservation
        public void Should_return_exception_when_cart_id_does_not_contain_any_reservation()
        {
            var cartId = "abc";
            var method = PaymentMethod.CreditCard;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = new PaymentData();
            var discCd = "acc";

            var service = new PaymentService();

            Assert.ThrowsException<ArgumentException>(() =>
                service.SubmitPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));
        }

        [TestMethod]
        // Should return exception when transaction status is settled
        public void Should_return_exception_when_transaction_status_is_settled()
        {
            var cartId = "abc";
            var method = PaymentMethod.CreditCard;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = new PaymentData();
            var discCd = "acc";

            var service = new PaymentService();

            Assert.ThrowsException<ArgumentException>(() =>
                service.SubmitPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));
        }

        [TestMethod]
        // Should return exception when transaction status is verifying
        public void Should_return_exception_when_transaction_status_is_verifying()
        {
            var cartId = "abc";
            var method = PaymentMethod.CreditCard;
            var submethod = PaymentSubmethod.BRI;
            var paymentData = new PaymentData();
            var discCd = "acc";

            var service = new PaymentService();

            Assert.ThrowsException<ArgumentException>(() =>
                service.SubmitPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));
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

            var service = new PaymentService();

            Assert.ThrowsException<ArgumentException>(() =>
                service.SubmitPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));
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

            var service = new PaymentService();

            Assert.ThrowsException<ArgumentException>(() =>
                service.SubmitPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));
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

            var service = new PaymentService();

            Assert.ThrowsException<ArgumentException>(() =>
                service.SubmitPayment(cartId, method, submethod, paymentData, discCd, out var isUpdated));
        }
    }
}
