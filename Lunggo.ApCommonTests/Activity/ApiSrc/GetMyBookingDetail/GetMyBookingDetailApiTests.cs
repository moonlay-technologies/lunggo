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

namespace Lunggo.ApCommonTests.Activity.ApiSrc.GetMyBookingDetailLogic.Tests
{
    //[TestClass]
    public partial class GetMyBookingDetailLogicTest
    {
        [TestMethod]
        public void MyBookingDetail_null_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            var expectedResult = new GetMyBookingDetailApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERAGPR01"
            };
            var actualResult = ActivityLogic.GetMyBookingDetail(null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void MyBookingDetail_Null_ReturnBadRequest()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("username"), new string[0]);
            var expectedResult = new GetMyBookingDetailApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.GetMyBookingDetail(null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void MyBookingDetail_ValidInput_ReturnSomething()
        {
            Initializer.Init();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("08712345678"), new string[0]);
            var input = new GetMyBookingDetailApiRequest()
            {
                RsvNo = "36536079"
            };
            var actualResult = ActivityLogic.GetMyBookingDetail(input);

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void AssembleApiResponse_GetMyBookingDetailOutput_ReturnGetMyBookingDetailApiResponse()
        {
            var book = new BookingDetail()
            { Name = "", Price = 2000 };
            var test = new GetMyBookingDetailOutput()
            {
                BookingDetail = book
            };

            var actualResult = ActivityLogic.AssembleApiResponse(test);

            var bookDisplay = new BookingDetailForDisplay()
            { Name = "", Price = 2000 };
            var expectedResult = new GetMyBookingDetailApiResponse()
            {
                BookingDetail = bookDisplay
            };
            
            Assert.AreEqual(expectedResult.BookingDetail.Price, actualResult.BookingDetail.Price);
            Assert.AreEqual(expectedResult.BookingDetail.Name, actualResult.BookingDetail.Name);
        }
    }
}
