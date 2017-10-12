using System;
using System.Net;
using Lunggo.WebAPI.ApiSrc.Activity;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Tests
{
    [TestClass]
    public class SearchActivityControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void SearchActivityType_unknown_Test()
        {
            var ExpectedResult = new ActivitySearchApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actController = new ActivityController();
            var ActualResult = actController.SearchActivity("123abc","","280217","1","10");
            
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Date_unformatted_Test()
        {
            var actController = new ActivityController();
            ApiResponseBase ActualResult = actController.SearchActivity("ActivityName", "gunung", "abcdef","1","10");
        }
    }
}
