using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.PaymentTest.Integration
{
    [TestClass]
    public class SubmitPayment
    {
        [TestMethod]
        // Should update all constituent payment DB for cart input
        public void Should_update_all_constituent_payment_DB_for_cart_input()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return success and update DB as success when input is all valid for CC
        public void Should_return_success_and_update_DB_as_success_when_input_is_all_valid_for_CC()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return and update DB as pending with redirection URL when input is all valid for CIMB Clicks
        public void Should_return_and_update_DB_as_pending_with_redirection_URL_when_input_is_all_valid_for_CIMB_Clicks()
        {
            throw new NotImplementedException();
        }
	
        [TestMethod]
        // Should return and update DB as succes with redirection URL when input is all valid for Mandiri ClickPay
        public void Should_return_and_update_DB_as_pending_with_redirection_URL_when_input_is_all_valid_for_Mandiri_ClickPay()
        {
            throw new NotImplementedException();
        }
    }
}
