using System;
using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommonTests.Init;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Logic;
using Lunggo.WebAPI.ApiSrc.Cart.Logic;

namespace Lunggo.ApCommonTests.Cart
{
    [TestClass]
    public partial class GetAvailableDatesLogicTest
    {
        [TestMethod]
        public void AddToCart_Null_ReturnBadRequest()
        {
            var expectedResult = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERR_INVALID_REQUEST"
            };
            var actualResult = CartLogic.AddCart(null);
            Assert.AreEqual(expectedResult, actualResult);
        }

        
    }
}
