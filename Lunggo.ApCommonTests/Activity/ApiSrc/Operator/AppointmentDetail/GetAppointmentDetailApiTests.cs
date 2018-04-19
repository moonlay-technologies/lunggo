using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using System;
using System.Security.Principal;
using System.Web;
using Lunggo.WebAPI;
using Lunggo.ApCommon.Identity.UserStore;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.GetAppointmentDetailLogic.Tests
{
    [TestClass]
    public partial class GetAppointmentDetailLogicTest
    {

        [TestMethod]
        public void GetAppointmentDetail_null_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            var expectedResult = new GetAppointmentDetailApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERR_UNDEFINED_USER"
            };
            var actualResult = ActivityLogic.GetAppointmentDetail(null, null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void GetAppointmentRequest_NotOperator_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("08712345678"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            var expectedResult = new GetAppointmentDetailApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERR_NOT_OPERATOR"
            };
            var actualResult = ActivityLogic.GetAppointmentDetail(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void GetAppointmentDetail_Null_ReturnBadRequest()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("12345678900"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            var expectedResult = new GetAppointmentDetailApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.GetAppointmentDetail(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void GetAppointmentDetail_ValidInput_ReturnSomething()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("12345678900"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            var input = new GetAppointmentDetailApiRequest()
            {
                ActivityId = "8",
                Date = "2017-01-09"
            };
            var actualResult = ActivityLogic.GetAppointmentDetail(input, userManager);

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void AssembleApiResponse_GetAppointmentDetailOutput_ReturnGetAppointmentDetailApiResponse()
        {
            var test = new GetAppointmentDetailOutput()
            {
                AppointmentDetail = new AppointmentDetail() { Name = "" }
            };

            var actualResult = ActivityLogic.AssembleApiResponse(test);

            var appointmentListDisplay = new AppointmentDetailForDisplay()
            { Name = "" };

            var expectedResult = new GetAppointmentDetailApiResponse()
            {
                AppointmentDetail = new AppointmentDetailForDisplay()
                { Name = "" }
        };
            
            Assert.AreEqual(expectedResult.AppointmentDetail.Name, actualResult.AppointmentDetail.Name);
        }
    }
}
