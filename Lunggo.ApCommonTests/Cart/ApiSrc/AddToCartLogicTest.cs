using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.WebAPI.ApiSrc.Common.Model;
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
