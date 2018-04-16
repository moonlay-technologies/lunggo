using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Net;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.WishlistLogic
{
    [TestClass]
    public partial class WishlistApiTests
    {
        [TestMethod]
        public void AddToWishlist_Null_ReturnBadRequest()
        {
            var expectedResult = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERR_INVALID_REQUEST"
            };

            var actualResult = ActivityLogic.AddToWishlist(null);
            Assert.AreEqual(expectedResult, actualResult);
        }

        /*public void AddToWishlist_Null_ReturnBadRequest()
        {
            var expectedResult = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERR_INVALID_REQUEST"
            };

            var actualResult = ActivityLogic.AddToWishlist(null);
            Assert.AreEqual(expectedResult, actualResult);
        }*/
    }
}
