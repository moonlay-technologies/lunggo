using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.SearchLogic.Tests
{
    [TestClass]
    public partial class SearchActivityLogicTest
    {
        [TestMethod]
        public void TryPreprocess_Null_ReturnFalse()
        {
            ActivitySearchApiRequest test = null;
            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_PageIsntNumeric_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "",
                EndDate = "",
                Page = "abcde",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_PageLessThanZero_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "",
                EndDate = "",
                Page = "-1",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_PerPageIsntNumeric_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "",
                EndDate = "",
                Page = "1",
                PerPage = "abcde"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_PerPageLessThanZero_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "",
                EndDate = "",
                Page = "1",
                PerPage = "-1"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_StartDateNullorEmpty_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = null,
                EndDate = "",
                Page = "1",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryPreprocess_StartDateValid_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "02-18-2017",
                EndDate = "",
                Page = "1",
                PerPage = "10"
            };
            var expectedResult = new SearchActivityInput()
            {
                ActivityFilter = new ActivityFilter() { StartDate = DateTime.Parse("02-18-2017")}
            };
            var result = ActivityLogic.TryPreprocess(test, out var actualResult);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedResult.ActivityFilter.StartDate, actualResult.ActivityFilter.StartDate);
        }

        [TestMethod]
        public void TryPreprocess_StartDateInvalid_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "02182017",
                EndDate = "",
                Page = "1",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var actualResult);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_EndDateNullorEmpty_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "",
                EndDate = null,
                Page = "1",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryPreprocess_EndDateValid_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "",
                EndDate = "02-18-2017",
                Page = "1",
                PerPage = "10"
            };
            var expectedResult = new SearchActivityInput()
            {
                ActivityFilter = new ActivityFilter() { EndDate = DateTime.Parse("02-18-2017") }
            };
            var result = ActivityLogic.TryPreprocess(test, out var actualResult);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedResult.ActivityFilter.EndDate, actualResult.ActivityFilter.EndDate);
        }

        [TestMethod]
        public void TryPreprocess_EndDateInvalid_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "",
                EndDate = "02182017",
                Page = "1",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var actualResult);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_DateValid_ReturnFalse()
        {
            var test = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "02-18-2017",
                EndDate = "02-18-2022",
                Page = "1",
                PerPage = "10"
            };
            var expectedResult = new SearchActivityInput()
            {
                ActivityFilter = new ActivityFilter()
                {
                    Name = "",
                    StartDate = DateTime.Parse("02-18-2017"),
                    EndDate = DateTime.Parse("02-18-2022") 
                },
                Page = 1,
                PerPage = 10
            };
            var result = ActivityLogic.TryPreprocess(test, out var actualResult);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedResult.ActivityFilter.Name, actualResult.ActivityFilter.Name);
            Assert.AreEqual(expectedResult.ActivityFilter.StartDate, actualResult.ActivityFilter.StartDate);
            Assert.AreEqual(expectedResult.ActivityFilter.EndDate, actualResult.ActivityFilter.EndDate);
            Assert.AreEqual(expectedResult.Page, actualResult.Page);
            Assert.AreEqual(expectedResult.PerPage, actualResult.PerPage);
        }
    }
}
