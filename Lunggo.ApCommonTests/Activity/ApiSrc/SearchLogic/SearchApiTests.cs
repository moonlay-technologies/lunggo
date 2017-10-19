using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommonTests.Init;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.SearchLogic.Tests
{
    //[TestClass]
    public partial class SearchActivityLogicTest
    {
        [TestMethod]
        public void Search_Null_ReturnBadRequest()
        {
            var expectedResult = new ActivitySearchApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.Search(null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void Search_ValidInput_ReturnSomething()
        {
            Initializer.Init();

            var input = new ActivitySearchApiRequest()
            {
                Name = "",
                StartDate = "",
                EndDate = "",
                Page = "1",
                PerPage = "10"
            };
            var actualResult = ActivityLogic.Search(input);

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void AssembleApiResponse_SearchActivityOutput_ReturnActivitySearchApiResponse()
        {
            var actList1 = new SearchResult()
            { Name = "", City = "Bandung", Country = "Indonesia", Description = "", OperationTime = "24 Jam", Price = 2000 };
            var test = new SearchActivityOutput()
            {
                ActivityList = new List<SearchResult>() { actList1 },
                Page = 1,
                PerPage = 10
            };

            var actualResult = ActivityLogic.AssembleApiResponse(test);

            var actListDisplay = new SearchResultForDisplay()
            { Name = "", City = "Bandung", Country = "Indonesia", Description = "", OperationTime = "24 Jam", Price = 2000 };
            var expectedResult = new ActivitySearchApiResponse()
            {
                ActivityList = new List<SearchResultForDisplay>() { actListDisplay },
                Page = 1,
                PerPage = 10
            };
            
            Assert.AreEqual(expectedResult.Page, actualResult.Page);
            Assert.AreEqual(expectedResult.PerPage, actualResult.PerPage);
            Assert.AreEqual(expectedResult.ActivityList[0].Name, actualResult.ActivityList[0].Name);
            Assert.AreEqual(expectedResult.ActivityList[0].Description, actualResult.ActivityList[0].Description);
            Assert.AreEqual(expectedResult.ActivityList[0].Price, actualResult.ActivityList[0].Price);
            Assert.AreEqual(expectedResult.ActivityList[0].City, actualResult.ActivityList[0].City);
            Assert.AreEqual(expectedResult.ActivityList[0].Country, actualResult.ActivityList[0].Country);
            Assert.AreEqual(expectedResult.ActivityList[0].OperationTime, actualResult.ActivityList[0].OperationTime);
        }
    }
}
