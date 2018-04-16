using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommonTests.Init;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using System;
using System.Security.Principal;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI;
using Lunggo.ApCommon.Identity.UserStore;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.UpdateActivityLogic.Tests
{
    //[TestClass]
    public partial class UpdateActivityLogicTest
    {
        [TestMethod]
        public void UpdateActivity_null_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            var expectedResult = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERR_UNDEFINED_USER"
            };
            var actualResult = ActivityLogic.UpdateActivity(null, null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void UpdateActivity_NotOperator_ReturnUnauthorized()
        {
            Initializer.Init();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("08712345678"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            var expectedResult = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERR_NOT_OPERATOR"
            };
            var actualResult = ActivityLogic.UpdateActivity(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void UpdateActivity_Null_ReturnBadRequest()
        {
            Initializer.Init();

            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("12345678900"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            var expectedResult = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.UpdateActivity(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void UpdateActivity_ValidInput_ReturnSomething()
        {
            Initializer.Init();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("12345678900"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            var input = new ActivityUpdateApiRequest()
            {
                ActivityId = 2,
                Name = "abcde",
                Price = 2000,
                Duration = new DurationActivity() {Amount = "1", Unit = "day" },
                RequiredPaxData = new List<string>() { "abc", "cde", "efg" }
            };
            var actualResult = ActivityLogic.UpdateActivity(input, userManager);
            var expectedResult = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.Accepted
            };
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void AssembleApiResponse_ActivityUpdateOutput_ReturnHttpOK()
        {
            var test = new ActivityUpdateOutput()
            {
                IsSuccess = true
            };

            var actualResult = ActivityLogic.AssembleApiResponse(test);
            
            var expectedResult = new ApiResponseBase()
            {
                StatusCode = HttpStatusCode.OK
            };
            
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void AssembleApiResponse_ActivityUpdateOutput_ReturnHttpAccepted()
        {
            var test = new ActivityUpdateOutput()
            {
                IsSuccess = false
            };

            var actualResult = ActivityLogic.AssembleApiResponse(test);

            var expectedResult = new ApiResponseBase()
            {
                StatusCode = HttpStatusCode.Accepted,
                ErrorCode = "ERR_UPDATE_FAILED"
            };

            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }
    }
}
