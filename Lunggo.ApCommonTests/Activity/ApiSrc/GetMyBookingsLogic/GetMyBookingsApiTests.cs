using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using System;
using System.Security.Principal;
using System.Web;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.GetMyBookingsLogic.Tests
{
    //[TestClass]
    public partial class GetMyBookingsLogicTest
    {
        [TestMethod]
        public void MyBookings_null_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(String.Empty), new string[0]);
            var expectedResult = new GetMyBookingsApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERAGPR01"
            };
            var actualResult = ActivityLogic.GetMyBookingsCartActive(null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void MyBookings_Null_ReturnBadRequest()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("username"), new string[0]);
            var expectedResult = new GetMyBookingsApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.GetMyBookingsCartActive(null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void MyBookings_ValidInput_ReturnSomething()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("08712345678"), new string[0]);
            var input = new GetMyBookingsApiRequest()
            {
                Page = "1",
                PerPage = "10"
            };
            var actualResult = ActivityLogic.GetMyBookingsCartActive(input);

            Assert.IsNotNull(actualResult);
        }

        //[TestMethod]
        //public void AssembleApiResponse_GetMyBookingsOutput_ReturnGetMyBookingsApiResponse()
        //{
        //    var bookList1 = new BookingDetail()
        //    { Name = "", Price = 2000 };
        //    var test = new GetMyBookingsOutput()
        //    {
        //        MyBookings = new List<BookingDetail>() { bookList1 },
        //        Page = 1,
        //        PerPage = 10
        //    };

        //    var actualResult = ActivityLogic.AssembleApiResponse(test);

        //    var bookListDisplay = new BookingDetailForDisplay()
        //    { Name = "", Price = 2000 };
        //    var expectedResult = new GetMyBookingsApiResponse()
        //    {
        //        MyBookings = new List<BookingDetailForDisplay>() { bookListDisplay },
        //        Page = 1,
        //        PerPage = 10
        //    };
            
        //    Assert.AreEqual(expectedResult.Page, actualResult.Page);
        //    Assert.AreEqual(expectedResult.PerPage, actualResult.PerPage);
        //    Assert.AreEqual(expectedResult.MyBookings[0].Name, actualResult.MyBookings[0].Name);
        //    Assert.AreEqual(expectedResult.MyBookings[0].Price, actualResult.MyBookings[0].Price);
        //}
    }
}
