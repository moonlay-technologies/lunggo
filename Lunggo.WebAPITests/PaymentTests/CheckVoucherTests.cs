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
    public class CheckVoucherTests
    {
        [TestMethod]
        // Should return invalid request when request is none
        public void Should_return_invalid_request_when_request_is_none()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return invalid request when voucher code is none
        public void Should_return_invalid_request_when_voucher_code_is_none()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return invalid request when rsvNo and cartId are none
        public void Should_return_invalid_request_when_rsvNo_and_cartId_are_none()
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
        // Should return invalid request when cardId is invalid
        public void Should_return_invalid_request_when_cardId_is_invalid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return voucher no longer available when voucher is already depleted
        public void Should_return_voucher_no_longer_available_when_voucher_is_already_depleted()
        {
            throw new NotImplementedException();
        }


        [TestMethod]
        // Should return can only be used inside promo period when outside promo period
        public void Should_return_can_only_be_used_inside_promo_period_when_outside_promo_period()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return transaction value not enough when below minimum spending for promo
        public void Should_return_transaction_value_not_enough_when_below_minimum_spending_for_promo()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return terms and conditions are not met when not eligible according to promo terms and conditions
        public void Should_return_terms_and_conditions_are_not_met_when_not_eligible_according_to_promo_terms_and_conditions()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return internal server error when so
        public void Should_return_internal_server_error_when_so()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return name of the discount and discount amount when succcess
        public void Should_return_name_of_the_discount_and_discount_amount_when_succcess()
        {
            throw new NotImplementedException();
        }
		
    }
}
