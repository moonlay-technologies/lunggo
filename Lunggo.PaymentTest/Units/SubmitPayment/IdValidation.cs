using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.PaymentTest.Units.SubmitPayment
{
    public partial class SubmitPaymentTest
    {
        [TestMethod]
        // TODO GANTI
        public void Should_not_return_failed_when_ID_is_RsvNo___all_numbers_8_12_char()
        {
            object dummy = null;
            string id;
            bool result;

            id = "12345678";
            result = PaymentService.GetInstance().InvokePrivate<bool>("ValidateTrxId", id, dummy);
            Assert.IsTrue(result);

            id = "123456789012";
            result = PaymentService.GetInstance().InvokePrivate<bool>("ValidateTrxId", id, dummy);
            Assert.IsTrue(result);
        }

        [TestMethod]
        // TODO GANTI
        public void Should_not_return_failed_when_ID_is_Cart_ID___alphanumeric_8_12_char()
        {
            object dummy = null;
            string id;
            bool result;

            id = "i23a56j8";
            result = PaymentService.GetInstance().InvokePrivate<bool>("ValidateTrxId", id, dummy);
            Assert.IsTrue(result);

            id = "i23a56j89oi2";
            result = PaymentService.GetInstance().InvokePrivate<bool>("ValidateTrxId", id, dummy);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Should_return_type_rsv_when_ID_is_RsvNo()
        {
            PaymentDetailsType? type = null;
            string id;
            bool result;

            id = "12345678";
            var inputParams = new object[] {id, type};
            result = PaymentService.GetInstance().InvokePrivate<bool>("ValidateTrxId", inputParams);
            Assert.AreEqual(PaymentDetailsType.Rsv, inputParams[1]);
        }

        [TestMethod]
        public void Should_return_type_cart_when_ID_is_CartID()
        {
            PaymentDetailsType? type = null;
            string id;
            bool result;

            id = "12345abc";
            var inputParams = new object[] {id, type};
            result = PaymentService.GetInstance().InvokePrivate<bool>("ValidateTrxId", inputParams);
            Assert.AreEqual(PaymentDetailsType.Cart, inputParams[1]);
        }

        [TestMethod]
        public void Should_return_failed_when_ID_less_than_8_char()
        {
            var id = "i23";
            object dummy = null;
            var result = PaymentService.GetInstance().InvokePrivate<bool>("ValidateTrxId", id, dummy);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Should_return_failed_when_ID_more_than_8_char()
        {
            var id = "i23a56j890abc";
            object dummy = null;
            var result = PaymentService.GetInstance().InvokePrivate<bool>("ValidateTrxId", id, dummy);
            Assert.IsFalse(result);
        }
    }
}