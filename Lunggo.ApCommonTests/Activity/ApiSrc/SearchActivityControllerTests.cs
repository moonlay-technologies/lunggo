using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Routing;
using Moq;
using Lunggo.WebAPI.ApiSrc.Activity;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Tests
{
    [TestClass]
    public class SearchActivityControllerTests
    {
        [TestInitialize]
        public void TestInit()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost.com", null), new HttpResponse(null));
        }
        
        [TestMethod]
        public void SearchActivityType_unknown_Test()
        {
            var ExpectedResult = new ActivitySearchApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };

            ActivityController actController = new ActivityController();

            var ActualResult = actController.SearchActivity("123abc","","280217","1","10");
            Assert.AreEqual(ExpectedResult.StatusCode, ActualResult.StatusCode);
            Assert.AreEqual(ExpectedResult.ErrorCode, ActualResult.ErrorCode);
        }

        [TestMethod]
        public void Date_unformatted_Test()
        {
            try
            {
                ActivityController actController = new ActivityController();
                var ActualResult = actController.SearchActivity("ActivityName", "gunung", "abcdef", "1", "10");
                Assert.Fail("An exception should have been thrown");
            }
            catch (NullReferenceException ae)
            {
                Assert.AreEqual("Object reference not set to an instance of an object.", ae.Message);
            }
        }

        [TestMethod]
        public void page_lessThanZero_Test()
        {
            try
            {
                ActivityController actController = new ActivityController();
                var ActualResult = actController.SearchActivity("ActivityDate", "gunung", "180217", "-1", "10");
                Assert.Fail("An exception should have been thrown");
            }
            catch (NullReferenceException ae)
            {
                Assert.AreEqual("Object reference not set to an instance of an object.", ae.Message);
            }
        }

        [TestMethod]
        public void perPage_lessThanZero_Test()
        {
            try
            {
                ActivityController actController = new ActivityController();
                var ActualResult = actController.SearchActivity("ActivityName", "gunung", "180217", "1", "-10");
                Assert.Fail("An exception should have been thrown");
            }
            catch (NullReferenceException ae)
            {
                Assert.AreEqual("Object reference not set to an instance of an object.", ae.Message);
            }
        }
    }
}
