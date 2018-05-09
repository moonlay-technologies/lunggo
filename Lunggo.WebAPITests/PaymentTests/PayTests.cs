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
    public class PayTests
    {
        [TestMethod]
        // Should return invalid request when rsvNo and cartId are none
        public void Should_return_invalid_request_when_rsvNo_and_cartId_are_none()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should pay for rsv when supplied with rsvNo
        public void Should_pay_for_rsv_when_supplied_with_rsvNo()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should pay for cart when supplied with cartId
        public void Should_pay_for_cart_when_supplied_with_cartId()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should pay for rsv when both rsvNo and cartId are supplied
        public void Should_pay_for_rsv_when_both_rsvNo_and_cartId_are_supplied()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return invalid request when rsvNo is invalid
        public void Should_return_invalid_request_when_rsvNo_is_invalid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return invalid request when cartId is invalid
        public void Should_return_invalid_request_when_cartId_is_invalid()
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
