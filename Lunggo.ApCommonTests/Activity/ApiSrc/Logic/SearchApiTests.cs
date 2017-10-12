using System;
using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Activity.Constant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommonTests.Init;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Logic.Tests
{
    //[TestClass]
    public partial class SearchActivityLogicTest
    {
        [TestMethod]
        public void Search_requestNull_Test()
        {
            ActivitySearchApiRequest input = null;
            var ExpectedResult = new ActivitySearchApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var ActualResult = ActivityLogic.Search(input);
            Assert.AreEqual(ExpectedResult.StatusCode, ActualResult.StatusCode);
            Assert.AreEqual(ExpectedResult.StatusCode, ActualResult.StatusCode);
        }

        [TestMethod]
        public void Search_requestNotNull_Test()
        {
            Initializer.Init();

            var input = new ActivitySearchApiRequest()
            {
                Filter = new ActivityFilter() { Name = "Marjan" },
                Page = 1,
                PerPage = 10,
                SearchType = SearchActivityType.ActivityName
            };

            var ActualResult = ActivityLogic.Search(input);

            Assert.IsNotNull(ActualResult);
        }

        [TestMethod]
        public void Search_PreprocessServiceRequest_Test()
        {
            var test = new ActivitySearchApiRequest()
            {
                Filter = new ActivityFilter(),
                Page = 1,
                PerPage = 10,
                SearchType = SearchActivityType.ActivityName,
                SearchId = ""
            };

            var ActualResult = ActivityLogic.PreprocessServiceRequest(test);

            var ExpectedResult = new SearchActivityInput()
            {
                ActivityFilter = new ActivityFilter(),
                Page = 1,
                PerPage = 10,
                SearchActivityType = SearchActivityType.ActivityName,
                SearchId = ""
            };

            Assert.AreEqual(ExpectedResult.ActivityFilter.Price, ActualResult.ActivityFilter.Price);
            Assert.AreEqual(ExpectedResult.ActivityFilter.CloseDate, ActualResult.ActivityFilter.CloseDate);
            Assert.AreEqual(ExpectedResult.ActivityFilter.Name, ActualResult.ActivityFilter.Name);
            Assert.AreEqual(ExpectedResult.Page, ActualResult.Page);
            Assert.AreEqual(ExpectedResult.PerPage, ActualResult.PerPage);
            Assert.AreEqual(ExpectedResult.SearchActivityType, ActualResult.SearchActivityType);
            Assert.AreEqual(ExpectedResult.SearchId, ActualResult.SearchId);
        }

        [TestMethod]
        public void Search_AssembleApiResponse_Test()
        {
            var ActList1 = new SearchResult()
                { Name = "Marjan", City = "Bandung", Country = "Indonesia", Description = "coba", CloseDate = DateTime.Parse("02/18/2017"), OperationTime = "24 Jam", Price = 2000 };
            var test = new SearchActivityOutput()
            {
                ActivityList = new List<SearchResult>(){ ActList1 },
                Page = 1,
                PerPage = 10,
                SearchId = ""
            };
            var ActualResult = ActivityLogic.AssembleApiResponse(test);

            var ActListDisplay = new SearchResultForDisplay()
                { Name = "Marjan", City = "Bandung", Country = "Indonesia", Description = "coba", CloseDate = DateTime.Parse("02/18/2017"), OperationTime = "24 Jam", Price = 2000 };
            var ExpectedResult = new ActivitySearchApiResponse()
            {
                ActivityList = new List<SearchResultForDisplay>() {ActListDisplay},
                Page = 1,
                PerPage = 10,
                SearchId = ""
            };

            Assert.AreEqual(ExpectedResult.SearchId, ActualResult.SearchId);
            Assert.AreEqual(ExpectedResult.Page, ActualResult.Page);
            Assert.AreEqual(ExpectedResult.PerPage, ActualResult.PerPage);
            Assert.AreEqual(ExpectedResult.ActivityList[0].Name, ActualResult.ActivityList[0].Name);
            Assert.AreEqual(ExpectedResult.ActivityList[0].Description, ActualResult.ActivityList[0].Description);
            Assert.AreEqual(ExpectedResult.ActivityList[0].Price, ActualResult.ActivityList[0].Price);
            Assert.AreEqual(ExpectedResult.ActivityList[0].City, ActualResult.ActivityList[0].City);
            Assert.AreEqual(ExpectedResult.ActivityList[0].CloseDate, ActualResult.ActivityList[0].CloseDate);
            Assert.AreEqual(ExpectedResult.ActivityList[0].Country, ActualResult.ActivityList[0].Country);
            Assert.AreEqual(ExpectedResult.ActivityList[0].OperationTime, ActualResult.ActivityList[0].OperationTime);
        }
    }
}
