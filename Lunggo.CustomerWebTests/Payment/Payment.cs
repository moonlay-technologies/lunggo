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
        // Should return payment view with cart data when cart has content and status is undefined
        public void Should_return_payment_view_with_cart_data_when_cart_has_content_and_status_is_undefined()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            var cartData = new CartPaymentDetails
            {
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Status = PaymentStatus.MethodNotSet,
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.MethodNotSet
                    }
                }
            };
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(cartData);
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("Payment", actual?.ViewName);
            Assert.AreEqual(cartData, actual?.Model);
        }

        [TestMethod]
        // Should_redirect_to_instruction_action_when_status_is_pending_and_has_instruction
        public void Should_redirect_to_instruction_action_when_status_is_pending_and_has_instruction()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Method = PaymentMethod.CreditCard,
                Status = PaymentStatus.Pending,
                HasInstruction = true,
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.Expired
                    }
                }
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as RedirectToRouteResult;

            Assert.AreEqual("Instruction", actual?.RouteValues["action"]);
            Assert.AreEqual(cartId, actual?.RouteValues["cartId"]);
        }
        
        [TestMethod]
        // Should_redirect_to_3rd_party_page_action_when_status_is_pending_and_utilize_3rd_party_page
        public void Should_redirect_to_3rd_party_page_action_when_status_is_pending_and_utilize_3rd_party_page()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Method = PaymentMethod.CreditCard,
                Status = PaymentStatus.Pending,
                HasThirdPartyPage = true,
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.Expired
                    }
                } 
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
                RsvPaymentDetails = new List<RsvPaymentDetails>()
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
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Status = PaymentStatus.Settled,
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.Expired
                    }
                }
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as RedirectToRouteResult;

            Assert.AreEqual("ThankYou", actual?.RouteValues["action"]);
            Assert.AreEqual(cartId, actual?.RouteValues["CartId"]);
        }

        [TestMethod]
        // Should_return_error_page_when_status_pending_but_no_instruction_or_third_party_page
        public void Should_return_error_page_when_status_pending_but_no_instruction_or_third_party_page()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Status = PaymentStatus.Pending,
                HasInstruction = false,
                HasThirdPartyPage = false,
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.Expired
                    }
                }
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("Error", actual?.ViewName);
        }

        [TestMethod]
        // Should return payment view with cart data when cart has content and status is failed
        public void Should_return_payment_view_with_cart_data_when_cart_has_content_and_status_is_failed()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            var cartData = new CartPaymentDetails
            {
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Status = PaymentStatus.MethodNotSet,
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.Failed
                    }
                }
            };
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(cartData);
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("Payment", actual?.ViewName);
            Assert.AreEqual(cartData, actual?.Model);
        }

        [TestMethod]
        // Should return payment view with cart data when cart has content and status is denied
        public void Should_return_payment_view_with_cart_data_when_cart_has_content_and_status_is_denied()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            var cartData = new CartPaymentDetails
            {
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Status = PaymentStatus.MethodNotSet,
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.Denied
                    }
                }
            };
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(cartData);
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("Payment", actual?.ViewName);
            Assert.AreEqual(cartData, actual?.Model);
        }

        [TestMethod]
        // Should return payment view with cart data when cart has content and status is expired
        public void Should_return_payment_view_with_cart_data_when_cart_has_content_and_status_is_expired()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            var cartData = new CartPaymentDetails
            {
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Status = PaymentStatus.MethodNotSet,
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.Expired
                    }
                }
            };
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(cartData);
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as ViewResult;

            Assert.AreEqual("Payment", actual?.ViewName);
            Assert.AreEqual(cartData, actual?.Model);
        }

        [TestMethod]
        // Should_return_varifying_view_when_cart_has_content_and_status_is_verifying
        public void Should_return_verifying_view_when_cart_has_content_and_status_is_verifying()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                CartId = "abcj23r4",
                OriginalPriceIdr = 123456789,
                Status = PaymentStatus.Verifying,
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = "123456789",
                        OriginalPriceIdr = 123456789,
                        Status = PaymentStatus.Expired
                    }
                }
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Payment(cartId) as RedirectToRouteResult;

            Assert.AreEqual("Verifying", actual?.RouteValues["action"]);
            Assert.AreEqual(cartId, actual?.RouteValues["CartId"]);
        }
	
    }
}
