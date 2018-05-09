using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Payment.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Lunggo.Framework.TestHelpers.TestHelper;
using static Lunggo.WebAPI.ApiSrc.Payment.Logic.PaymentLogic;

namespace Lunggo.WebAPITests.PaymentTests
{
    [TestClass]
    public class CheckOutTests
    {
        [TestMethod]
        // Should throw exception when not logged in
        public void Should_throw_exception_when_not_logged_in()
        {
            HttpContext.Current = NoLogin();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                CheckOut(new CheckOutApiRequest());
            });
        }

        [TestMethod]
        // Should throw exception when logged in as guest
        public void Should_throw_exception_when_logged_in_as_guest()
        {
            HttpContext.Current = LoginGuest();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                CheckOut(new CheckOutApiRequest());
            });
        }

        [TestMethod]
        // Should throw exception when logged in invalid user
        public void Should_throw_exception_when_logged_in_as_invalid_user()
        {
            HttpContext.Current = LoginInvalidUser();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                CheckOut(new CheckOutApiRequest());
            });
        }

        [TestMethod]
        // Should return invalid request when cart ID is invalid
        public void Should_return_invalid_request_when_cart_ID_is_invalid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return voucher not available when trying to use voucher but not available
        public void Should_return_voucher_not_available_when_trying_to_use_voucher_but_not_available()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return not success when data not updated in the backend
        public void Should_return_not_success_when_data_not_updated_in_the_backend()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return success with instruction when payment result has so
        public void Should_return_success_with_instruction_when_payment_result_has_so()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return success with third party url when payment result has so
        public void Should_return_success_with_third_party_url_when_payment_result_has_so()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return invalid request when cartID is none
        public void Should_return_invalid_request_when_cartID_is_none()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return invalid request when method is none
        public void Should_return_invalid_request_when_method_is_none()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return invalid request when method is invalid
        public void Should_return_invalid_request_when_method_is_invalid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return invalid request when submethod is invalid
        public void Should_return_invalid_request_when_submethod_is_invalid()
        {
            throw new NotImplementedException();
        }	

        [TestMethod]
        // Should return invalid request when request is none
        public void Should_return_invalid_request_when_request_is_none()
        {
            throw new NotImplementedException();
        }
	
    }
}
