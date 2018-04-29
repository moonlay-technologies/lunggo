using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.CustomerWeb.Areas.Payment_v2.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lunggo.CustomerWebTests.Payment
{
    [TestClass]
    public partial class Payment
    {
        [TestMethod]
        // Should return payment view when cart has content and not yet paid
        public void Should_return_payment_view_when_cart_has_content_and_not_yet_paid()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Status = PaymentStatus.Undefined,
                RsvPaymentDetails = new List<PaymentDetails>
                {
                    new PaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.Undefined
                    }
                }
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("Payment", actual?.ViewName);
        }

        [TestMethod]
        // Should redirect to instruction action when method already selected and has instruction
        public void Should_redirect_to_instruction_action_when_method_already_selected_and_has_instruction()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                Method = PaymentMethod.CreditCard,
                Status = PaymentStatus.Pending,
                HasInstruction = true
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as RedirectToRouteResult;

            Assert.AreEqual("Instruction", actual?.RouteValues["action"]);
            Assert.AreEqual(cartId, actual?.RouteValues["cartId"]);
        }



        [TestMethod]
        // Should redirect to 3rd party page action when method already selected and utilize 3rd party page
        public void Should_redirect_to_3rd_party_page_action_when_method_already_selected_and_utilize_3rd_party_page()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                Method = PaymentMethod.CreditCard,
                Status = PaymentStatus.Pending,
                HasThirdPartyPage = true
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as RedirectToRouteResult;

            Assert.AreEqual("ThirdParty", actual?.RouteValues["action"]);
            Assert.AreEqual(cartId, actual?.RouteValues["cartId"]);
        }

        [TestMethod]
        // Should return invalid cart ID page when cart ID is invalid
        public void Should_return_invalid_cart_ID_page_when_cart_ID_is_invalid()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns((CartPaymentDetails)null);
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("Error", actual?.ViewName);
        }

        [TestMethod]
        // Should return empty cart page when cart is still empty
        public void Should_return_empty_cart_page_when_cart_is_still_empty()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                RsvPaymentDetails = new List<PaymentDetails>()
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("EmptyCart", actual?.ViewName);
        }

        [TestMethod]
        // Should redirect to thank you page when cart is already paid
        public void Should_redirect_to_thank_you_page_when_cart_is_already_paid()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                Status = PaymentStatus.Settled,
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as RedirectToRouteResult;

            Assert.AreEqual("ThankYou", actual?.RouteValues["action"]);
            Assert.AreEqual(cartId, actual?.RouteValues["CartId"]);
        }

    }
}
