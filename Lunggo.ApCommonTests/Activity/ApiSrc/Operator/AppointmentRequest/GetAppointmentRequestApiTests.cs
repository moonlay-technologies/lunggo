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

namespace Lunggo.ApCommonTests.Activity.ApiSrc.GetAppointmentRequestLogic.Tests
{
    [TestClass]
    public partial class GetAppointmentRequestLogicTest
    {

        [TestMethod]
        public void MyBookings_null_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            var expectedResult = new GetAppointmentRequestApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERAGPR01"
            };
            var actualResult = ActivityLogic.GetAppointmentRequest(null, null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void MyBookings_Null_ReturnBadRequest()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("username"), new string[0]);
            var expectedResult = new GetAppointmentRequestApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.GetAppointmentRequest(null, null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void Search_ValidInput_ReturnSomething()
        {
            Initializer.Init();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("username"), new string[0]);
            var input = new GetAppointmentRequestApiRequest()
            {
                Page = "1",
                PerPage = "10"
            };
            var actualResult = ActivityLogic.GetAppointmentRequest(input, null);

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void AssembleApiResponse_GetAppointmentRequestOutput_ReturnGetAppointmentRequestApiResponse()
        {
            var AppointmentList1 = new AppointmentDetail()
            { Name = "", RequestTime = "2017-01-01", Date = new DateTime() };
            var test = new GetAppointmentRequestOutput()
            {
                Appointments = new List<AppointmentDetail>() { AppointmentList1 },
                Page = 1,
                PerPage = 10
            };

            var actualResult = ActivityLogic.AssembleApiResponse(test);

            var AppointmentListDisplay = new AppointmentDetailForDisplay()
            { Name = "", RequestTime = new DateTime(2017,01,01), Date = new DateTime() };

            var expectedResult = new GetAppointmentRequestApiResponse()
            {
                Appointments = new List<AppointmentDetailForDisplay>() { AppointmentListDisplay },
                Page = 1,
                PerPage = 10
            };
            
            Assert.AreEqual(expectedResult.Page, actualResult.Page);
            Assert.AreEqual(expectedResult.PerPage, actualResult.PerPage);
            Assert.AreEqual(expectedResult.Appointments[0].Name, actualResult.Appointments[0].Name);
        }
    }
}
