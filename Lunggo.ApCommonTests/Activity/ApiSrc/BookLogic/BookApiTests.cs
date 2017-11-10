using System;
using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommonTests.Init;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Security.Principal;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.BookLogic.Tests
{
    [TestClass]
    public class BookLogicTest
    {
        [TestInitialize]
        public void TestInit()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost.com", null), new HttpResponse(null));
        }

        [TestMethod]
        public void Book_null_ReturnUnauthorized()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal( new GenericIdentity(String.Empty), new string[0] );
            var expectedResult = new ActivityBookApiResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorCode = "ERAGPR01"
            };
            var actualResult = ActivityLogic.BookActivity(null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void Book_Null_ReturnBadRequest()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost.com", ""), new HttpResponse(null));
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("username"), new string[0]);
            var expectedResult = new ActivityBookApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.BookActivity(null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Book_Valid_NullException()
        {
            Initializer.Init();
            
            var pax = new PaxForDisplay(){ Type= PaxType.Adult, Name = "Travo", Nationality = "ID", Title = Title.Mister};
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Date = "2017-02-18",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "62", Phone = "1234567890", Email = "developer@travelmadezy.com"},
                Passengers = paxs
            };
            ActivityLogic.BookActivity(input);
        }

        [TestMethod]
        public void IsValid_Null_ReturnFalse()
        {
            var actualResult = ActivityLogic.IsValid(null);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_EmptyPassengerName_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "", Nationality = "ID", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "62", Phone = "1234567890", Email = "developer@travelmadezy.com" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_EmptyPassengerNationality_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "62", Phone = "1234567890", Email = "developer@travelmadezy.com" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_PassengerNationalityNotEqualTwoChar_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "IDN", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "62", Phone = "1234567890", Email = "developer@travelmadezy.com" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_ContactNull_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "ID", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact(),
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_PassengersNull_ReturnFalse()
        {
            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "62", Phone = "1234567890", Email = "developer@travelmadezy.com" },
                Passengers = null
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_ActvityIdNullOrEmpty_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "ID", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "62", Phone = "1234567890", Email = "developer@travelmadezy.com" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_LanguageCodeNullOrEmpty_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "ID", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "62", Phone = "1234567890", Email = "developer@travelmadezy.com" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_TitleContactUndefined_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "ID", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "Travorama", Title = Title.Undefined, CountryCallingCode = "62", Phone = "1234567890", Email = "developer@travelmadezy.com" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_ContactNameNullOrEmpty_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "ID", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "", Title = Title.Mister, CountryCallingCode = "62", Phone = "1234567890", Email = "developer@travelmadezy.com" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_ContactPhoneNullOrEmpty_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "ID", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "62", Phone = "", Email = "developer@travelmadezy.com" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_ContactEmailNullOrEmpty_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "ID", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "62", Phone = "1234567890", Email = "" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsValid_ContactCallingCdNullOrEmpty_ReturnFalse()
        {
            var pax = new PaxForDisplay() { Type = PaxType.Adult, Name = "Travo", Nationality = "ID", Title = Title.Mister };
            var paxs = new List<PaxForDisplay> {pax};

            var input = new ActivityBookApiRequest
            {
                ActivityId = "1",
                Contact = new Contact() { Name = "Travorama", Title = Title.Mister, CountryCallingCode = "", Phone = "1234567890", Email = "developer@travelmadezy.com" },
                Passengers = paxs
            };
            var actualResult = ActivityLogic.IsValid(input);
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void PreprocessServiceRequest_valid_ReturnObjectWithConvertedPax()
        {
            var PaxsForDisplay = new List<PaxForDisplay>() { new PaxForDisplay() { Name = "abcde" } };
            var Paxs = new List<Pax>() { new Pax() { FirstName = "abcde" } };
            var input = new ActivityBookApiRequest()
            {
                ActivityId = "1",
                Date = "2017-01-01",
                Contact = null,
                Passengers = PaxsForDisplay,
                TicketCount = null
            };

            var expectedResult = new BookActivityInput()
            {
                ActivityId = "1",
                Passengers = Paxs
            };

            var actualResult = ActivityLogic.PreprocessServiceRequest(input);

            Assert.AreEqual(expectedResult.ActivityId, actualResult.ActivityId);
            Assert.AreEqual(expectedResult.Passengers[0].FirstName, actualResult.Passengers[0].FirstName);
        }

        [TestMethod]
        public void AssembleApiResponse_Null_ReturnEmptyObject()
        {
            var expectedResult = new ActivityBookApiResponse();

            var actualResult = ActivityLogic.AssembleApiResponse((BookActivityOutput) null);

            Assert.AreEqual(expectedResult.RsvNo, actualResult.RsvNo);
            Assert.AreEqual(expectedResult.IsValid, actualResult.IsValid);
        }

        [TestMethod]
        public void AssembleApiResponse_Invalid_ReturnStatusandValidationResult()
        {
            var input = new BookActivityOutput(){IsValid = false};

            var expectedResult = new ActivityBookApiResponse(){IsValid = false,StatusCode = HttpStatusCode.OK};

            var actualResult = ActivityLogic.AssembleApiResponse(input);
            
            Assert.AreEqual(expectedResult.IsValid, actualResult.IsValid);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void AssembleApiResponse_Valid_ReturnWithRsvNo()
        {
            var input = new BookActivityOutput()
            {
                IsValid = true,
                RsvNo = "12345",
                TimeLimit = new DateTime(2017, 01, 01)
            };

            var expectedResult = new ActivityBookApiResponse()
            {
                IsValid = true,
                RsvNo = "12345",
                StatusCode = HttpStatusCode.OK,
                TimeLimit = new DateTime(2017, 01, 01)
            };

            var actualResult = ActivityLogic.AssembleApiResponse(input);

            Assert.AreEqual(expectedResult.IsValid, actualResult.IsValid);
            Assert.AreEqual(expectedResult.RsvNo, actualResult.RsvNo);
            Assert.AreEqual(expectedResult.TimeLimit, actualResult.TimeLimit);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }
    }
}
