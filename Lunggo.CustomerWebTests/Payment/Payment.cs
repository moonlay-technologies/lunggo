using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.CustomerWeb.Areas.Payment_v2.Controllers;
using Lunggo.CustomerWebTests.Utils;
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
            var cartId = It.IsAny<string>();
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                CartId = It.IsAny<string>(),
                OriginalPriceIdr = It.IsAny<decimal>(),
                Status = PaymentStatus.Undefined,
                RsvPaymentDetails = new List<PaymentDetails>
                {
                    new PaymentDetails
                    {
                        RsvNo = It.IsAny<string>(),
                        OriginalPriceIdr = It.IsAny<decimal>(),
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
            var cartId = It.IsAny<string>();
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                Method = It.IsNotIn(PaymentMethod.Undefined),
                Status = PaymentStatus.Pending,
                HasInstruction = true
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as RedirectToRouteResult;
            var actualData = actual.GetRouteValues();

            Assert.AreEqual("Instruction", actual?.RouteName);
            Assert.IsTrue(actualData.TryGetValue("cartId", out var actualCartId));
            Assert.AreEqual(cartId, actualCartId);
        }



        [TestMethod]
        // Should redirect to 3rd party page action when method already selected and utilize 3rd party page
        public void Should_redirect_to_3rd_party_page_action_when_method_already_selected_and_utilize_3rd_party_page()
        {
            var cartId = It.IsAny<string>();
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                Method = It.IsNotIn(PaymentMethod.Undefined),
                Status = PaymentStatus.Pending,
                HasThirdPartyPage = true
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as RedirectToRouteResult;
            var actualData = actual.GetRouteValues();

            Assert.AreEqual("ThirdParty", actual?.RouteName);
            Assert.IsTrue(actualData.TryGetValue("cartId", out var actualCartId));
            Assert.AreEqual(cartId, actualCartId);
        }

        [TestMethod]
        // Should return invalid cart ID page when cart ID is invalid
        public void Should_return_invalid_cart_ID_page_when_cart_ID_is_invalid()
        {
            var cartId = It.IsAny<string>();
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns((CartPaymentDetails)null);
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("Erro", actual?.ViewName);
        }

        [TestMethod]
        // Should return empty cart page when cart is still empty
        public void Should_return_empty_cart_page_when_cart_is_still_empty()
        {
            var cartId = It.IsAny<string>();
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                RsvPaymentDetails = It.IsIn(new List<PaymentDetails>(), null)
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("EmptyCart", actual?.ViewName);
        }

        [TestMethod]
        // Should redirect to thank you page when cart is already paid
        public void Should_redirect_to_thank_you_page_when_cart_is_already_paid()
        {
            var cartId = It.IsAny<string>();
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                Status = PaymentStatus.Settled,
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as RedirectToRouteResult;
            var actualData = actual.GetRouteValues();

            Assert.AreEqual("ThankYou", actual?.RouteName);
            Assert.IsTrue(actualData.TryGetValue("cartId", out var actualCartId));
            Assert.AreEqual(cartId, actualCartId);
        }

    }
}
