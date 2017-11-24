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

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.AppointmentConfirmationLogic.Tests
{
    //[TestClass]
    public partial class ConfirmAppointmentLogicTest
    {
        [TestMethod]
        public void ConfirmAppointment_null_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            var expectedResult = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERR_UNDEFINED_USER"
            };
            var actualResult = ActivityLogic.ConfirmAppointment(null, null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void GetAppointmentRequest_NotOperator_ReturnUnauthorized()
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
            var actualResult = ActivityLogic.ConfirmAppointment(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void ConfirmAppointment_Null_ReturnBadRequest()
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
            var actualResult = ActivityLogic.ConfirmAppointment(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void ConfirmAppointment_ValidInput_ReturnSomething()
        {
            Initializer.Init();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("12345678900"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            var rsvNo = "36536079";
            var actualResult = ActivityLogic.ConfirmAppointment(rsvNo, userManager);
            var expectedResult = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.Accepted
            };
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void AssembleApiResponse_AppointmentConfirmationOutput_ReturnHttpOK()
        {
            var test = new AppointmentConfirmationOutput()
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
        public void AssembleApiResponse_AppointmentConfirmationOutput_ReturnHttpAccepted()
        {
            var test = new AppointmentConfirmationOutput()
            {
                IsSuccess = false
            };

            var actualResult = ActivityLogic.AssembleApiResponse(test);

            var expectedResult = new ApiResponseBase()
            {
                StatusCode = HttpStatusCode.Accepted,
                ErrorCode = "ERR_CONFIRMATION_FAILED"
            };

            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }
    }
}
