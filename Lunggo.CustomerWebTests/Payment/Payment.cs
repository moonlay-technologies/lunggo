using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lunggo.CustomerWebTests.Payment
{
    [TestClass]
    public partial class Payment
    {
        [TestMethod]
        // Should return payment view when eligible for payment
        public void Should_return_payment_view_when_eligible_for_payment()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should redirect to instruction action when method already selected and has instruction
        public void Should_redirect_to_instruction_action_when_method_already_selected_and_has_instruction()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should redirect to 3rd party page action when method already selected and utilize 3rd party page
        public void Should_redirect_to_3rd_party_page_action_when_method_already_selected_and_utilize_3rd_party_page()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return invalid cart ID page when cart ID is invalid
        public void Should_return_invalid_cart_ID_page_when_cart_ID_is_invalid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return empty cart page when cart is still empty
        public void Should_return_empty_cart_page_when_cart_is_still_empty()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should redirect to thank you page when cart is already paid
        public void Should_redirect_to_thank_you_page_when_cart_is_already_paid()
        {
            throw new NotImplementedException();
        }
	
    }
}
