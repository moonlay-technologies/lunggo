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
using Lunggo.WebAPI;
using Lunggo.ApCommon.Identity.UserStore;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.GetAppointmentListLogic.Tests
{
    [TestClass]
    public partial class GetAppointmentListLogicTest
    {

        [TestMethod]
        public void GetAppointmentList_null_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            var expectedResult = new GetAppointmentListApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERR_UNDEFINED_USER"
            };
            var actualResult = ActivityLogic.GetAppointmentList(null, null);
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
            var expectedResult = new GetAppointmentListApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERR_NOT_OPERATOR"
            };
            var actualResult = ActivityLogic.GetAppointmentList(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void GetAppointmentList_Null_ReturnBadRequest()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("12345678900"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            var expectedResult = new GetAppointmentListApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.GetAppointmentList(null, userManager);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        [TestMethod]
        public void GetAppointmentList_ValidInput_ReturnSomething()
        {
            Initializer.Init();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("12345678900"), new string[0]);
            var userManager = new ApplicationUserManager(new DapperUserStore<User>());
            var input = new GetAppointmentListApiRequest()
            {
                Page = "1",
                PerPage = "10"
            };
            var actualResult = ActivityLogic.GetAppointmentList(input, userManager);

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void AssembleApiResponse_GetAppointmentListOutput_ReturnGetAppointmentListApiResponse()
        {
            var appointment1 = new AppointmentDetail()
            { Name = "" };
            var test = new GetAppointmentListOutput()
            {
                Appointments = new List<AppointmentDetail>() { appointment1 },
                Page = 1,
                PerPage = 10
            };

            var actualResult = ActivityLogic.AssembleApiResponse(test);

            var appointmentListDisplay = new AppointmentDetailForDisplay()
            { Name = "" };

            var expectedResult = new GetAppointmentListApiResponse()
            {
                Appointments = new List<AppointmentDetailForDisplay>() { appointmentListDisplay },
                Page = 1,
                PerPage = 10
            };
            
            Assert.AreEqual(expectedResult.Page, actualResult.Page);
            Assert.AreEqual(expectedResult.PerPage, actualResult.PerPage);
            Assert.AreEqual(expectedResult.Appointments[0].Name, actualResult.Appointments[0].Name);
        }
    }
}
