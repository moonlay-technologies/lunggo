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
using System.Web.Http;
using Lunggo.WebAPI;
using Lunggo.ApCommon.Identity.Users;
using Microsoft.AspNet.Identity;
using Lunggo.ApCommon.Identity.UserStore;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.GetListActivityLogic.Tests
{
    [TestClass]
    public partial class GetListActivityLogicTest
    {
        [TestMethod]
        public void GetListActivity_null_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            var expectedResult = new GetListActivityApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERR_UNDEFINED_USER"
            };
            var actualResult = ActivityLogic.GetListActivity(null, null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void GetListActivity_NotOperator_ReturnUnauthorized()
        {
            Initializer.Init();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("08712345678"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());

            var expectedResult = new GetListActivityApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERR_NOT_OPERATOR"
            };
            var actualResult = ActivityLogic.GetListActivity(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void GetListActivity_Null_ReturnBadRequest()
        {
            Initializer.Init();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("12345678900"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            
            var expectedResult = new GetListActivityApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.GetListActivity(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void GetListActivity_ValidInput_ReturnSomething()
        {
            Initializer.Init();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("12345678900"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());

            var input = new GetListActivityApiRequest()
            {
                Page = "1",
                PerPage = "10"
            };
            var actualResult = ActivityLogic.GetListActivity(input, userManager);

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void AssembleApiResponse_GetListActivityOutput_ReturnGetListActivityApiResponse()
        {
            var actList1 = new SearchResult()
            { Name = "" };
            var test = new GetListActivityOutput()
            {
                ActivityList = new List<SearchResult>() { actList1 },
                Page = 1,
                PerPage = 10
            };

            var actualResult = ActivityLogic.AssembleApiResponse(test);

            var actListDisplay = new SearchResultForDisplay()
            { Name = "" };

            var expectedResult = new GetListActivityApiResponse()
            {
                ActivityList = new List<SearchResultForDisplay>() { actListDisplay },
                Page = 1,
                PerPage = 10
            };
            
            Assert.AreEqual(expectedResult.Page, actualResult.Page);
            Assert.AreEqual(expectedResult.PerPage, actualResult.PerPage);
            Assert.AreEqual(expectedResult.ActivityList[0].Name, actualResult.ActivityList[0].Name);
        }
    }
}
