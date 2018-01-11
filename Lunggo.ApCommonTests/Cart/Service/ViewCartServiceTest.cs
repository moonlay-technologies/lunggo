using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommonTests.Cart.Service
{
    [TestClass]
    public class ViewCartServiceTest
    {
        [TestMethod]
        public void ViewCart_Null_ReturnIsSuccessFalse()
        {
            var expectedResult = new ViewCartOutput
            {

            };

        }
    }
}
