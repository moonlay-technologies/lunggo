using System;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Model.Data;
using Lunggo.ApCommon.Payment.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.PaymentTest.Public.SubmitPayment.BankTransfer
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
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

        [TestMethod]
        public void Should_failed_when_method_not_selected()
        {
            var method = PaymentMethod.Undefined;
            var submethod = PaymentSubmethod.Undefined;
            PaymentData paymentData = null;
            var result = PaymentService.GetInstance().InvokePrivate<bool>("ValidatePaymentMethod", method, submethod, paymentData);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Should_failed_when_Mandiri_Clickpay_does_not_include_Mandiri_Clickpay_data()
        {
            var method = PaymentMethod.MandiriClickPay;
            var submethod = PaymentSubmethod.Undefined;
            var paymentData = new PaymentData
            {
                MandiriClickPay = null
            };
            var result = PaymentService.GetInstance().InvokePrivate<bool>("ValidatePaymentMethod", method, submethod, paymentData);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Should_failed_when_Mandiri_Clickpay_does_not_include_card_number()
        {
            var method = PaymentMethod.MandiriClickPay;
            var submethod = PaymentSubmethod.Undefined;
            var paymentData = new PaymentData
            {
                MandiriClickPay = new MandiriClickPay
                {
                    CardNumber = null,
                    Token = "123456"
                }
            };
            var result = PaymentService.GetInstance().InvokePrivate<bool>("ValidatePaymentMethod", method, submethod, paymentData);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Should_failed_when_Mandiri_Clickpay_does_not_include_token()
        {
            var method = PaymentMethod.MandiriClickPay;
            var submethod = PaymentSubmethod.Undefined;
            var paymentData = new PaymentData
            {
                MandiriClickPay = new MandiriClickPay
                {
                    CardNumber = "1234567890123456",
                    Token = null
                }
            };
            var result = PaymentService.GetInstance().InvokePrivate<bool>("ValidatePaymentMethod", method, submethod, paymentData);
            Assert.IsFalse(result);
        }



        ////[TestMethod]
        ////public void Should_return_invalid_voucher_when_voucher_is_not_valid()
        ////{
        ////    var voucherCode = '1234567890';
        ////    var expectedResult = false;
        ////    bool actualResult = JalaninFungsiApalahNamanya(voucherCode);
        ////    Assert.AreEqual(expectedResult, actualResult.Status);
        ////}

        ////[TestMethod]
        ////public void Should_not_return_failed_when_voucher_is_valid()
        ////{
        ////    var voucherCode = '1234567890';
        ////    bool actualResult = JalaninFungsiApalahNamanya(voucherCode);
        ////    Assert.AreNotEqual('Failed', actualResult.Status);
        ////}

        //[TestMethod]
        //public void Should_return_failed_when_no_method_is_selected()
        //{
        //    var p = new PaymentDetails() { };
        //    Assert.ThrowsException<Exception>(() => JalaninFungsiApalahNamanya());
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_Bank_Transfer_Mandiri()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_success_on_valid_Credit_Card_Visa()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.CreditCard,
        //        Status = PaymentStatus.Undefined,
        //        Data = new PaymentData(),
        //    };
        //    var actualResult = JalaninFungsiApalahNamanya(p);
        //    p.Status = PaymentStatus.Settled;
        //    var expectedResult = p;
        //    Assert.AreEqual(expectedResult, actualResult);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_denied_Credit_Card_Visa()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_success_on_valid_Credit_Card_Mastercard()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_denied_Credit_Card_Mastercard()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_CIMB_Clicks()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_CIMB_Clicks()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_Virtual_Account_BCA()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_Virtual_Account_BCA()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_BCA_KlikPay()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_BCA_KlikPay()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_Virtual_Account_Bank_BRI()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_Virtual_Account_Bank_BRI()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_Virtual_Account_Danamon()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_Virtual_Account_Danamon()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_Virtual_Account_CIMB_NIAGA()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_Virtual_Account_CIMB_NIAGA()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_Virtual_Account_Permata_Bank()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_Virtual_Account_Permata_Bank()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_Virtual_Account_Others()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_Virtual_Account_Others()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_Mandiri_ClickPay()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_Mandiri_ClickPay()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_BTN_Mobile_Banking()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_BTN_Mobile_Banking()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_IB_Muamalat()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_failed_on_unsuccessful_IB_Muamalat()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}

        //[TestMethod]
        //public void Should_return_pending_on_successful_SBII_Online_Shopping()
        //{
        //    var p = new PaymentDetails()
        //    {
        //        Medium = PaymentMedium.Direct,
        //        Method = PaymentMethod.BankTransfer,
        //        Submethod = PaymentSubmethod.Mandiri,
        //    };
        //    PaymentDetails actualResult = JalaninFungsiApalahNamanya(p);
        //    Assert.AreEqual(PaymentStatus.Settled, actualResult.Status);
        //}
    }
}
