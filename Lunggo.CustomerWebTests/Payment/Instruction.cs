using System;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.CustomerWeb.Areas.Payment_v2.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lunggo.CustomerWebTests.Payment
{
    [TestClass]
    public class Instruction
    {
        [TestMethod]
        // Should return error when transaction has no instruction
        public void Should_return_error_when_transaction_has_no_instruction()
        {
            var cartId = "abcde12345";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                HasInstruction = false
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Instruction(cartId) as ViewResult;

            Assert.AreEqual("Error", actual?.ViewName);
        }

        [TestMethod]
        // Should return instruction page when transaction has instruction
        public void Should_return_instruction_page_when_transaction_has_instruction()
        {
            var cartId = "abcde12345";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                HasInstruction = true
            });
            var controller = new PaymentController(mock.Object);

            var actual = controller.Instruction(cartId) as ViewResult;

            Assert.AreEqual("Instruction", actual?.ViewName);
        }

        [TestMethod]
        // Should return error when cartId is invalid
        public void Should_return_error_when_cartId_is_invalid()
        {
            var cartId = "abcjhd124";
            var mock = new Mock<PaymentService>();
            mock.Setup(m => m.GetCartPaymentDetails(cartId)).Returns((CartPaymentDetails)null);
            var controller = new PaymentController(mock.Object);

            var actual = controller.Instruction(cartId) as ViewResult;

            Assert.AreEqual("Error", actual?.ViewName);
        }
    }
}
