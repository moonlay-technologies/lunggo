using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Cache;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Processor;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lunggo.PaymentTest.PaymentServiceTests
{
    [TestClass]
    public class GetCartTests
    {
        [TestMethod]
        // Should return cart when there is filled cart
        public void Should_return_cart_when_there_is_filled_cart()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return empty cart when there is cart but contains nothing
        public void Should_return_empty_cart_when_there_is_cart_but_contains_nothing()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return null when cartId is invalid
        public void Should_return_null_when_cartId_is_invalid()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class GetCartByUserTests
    {
        [TestMethod]
        // Should return null when userId is invalid
        public void Should_return_null_when_userId_is_invalid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return cart when userId is valid
        public void Should_return_cart_when_userId_is_valid()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class AddToCartTests
    {
        [TestMethod]
        // Should return false when userId is invalid
        public void Should_return_false_when_userId_is_invalid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return false when rsvNo is invalid
        public void Should_return_false_when_rsvNo_is_invalid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return false when failed to add rsvNo to cart
        public void Should_return_false_when_failed_to_add_rsvNo_to_cart()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return true when everything is valid
        public void Should_return_true_when_everything_is_valid()
        {
            throw new NotImplementedException();
        }

    }

    [TestClass]
    public class RemoveFromCartTests
    {
        [TestMethod]
        // Should return false when userId is invalid
        public void Should_return_false_when_userId_is_invalid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return true when userId is valid and rsvNo is whatever
        public void Should_return_true_when_userId_is_valid_and_rsvNo_is_whatever()
        {
            throw new NotImplementedException();
        }
	
    }
}
