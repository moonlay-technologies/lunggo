using System;
using System.Net;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommonTests.Init;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.GetDetailLogic.Tests
{
    [TestClass]
    public partial class GetDetailActivityLogicTest
    {
        [TestMethod]
        public void GetDetail_Null_ReturnBadRequest()
        {
            var expectedResult = new GetDetailActivityApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.GetDetail(null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void GetDetail_ValidInput_ReturnSomething()
        {
            Initializer.Init();

            var input = new GetDetailActivityApiRequest()
            {
                ActivityId = "1"
            };
            
            var actualResult = ActivityLogic.GetDetail(input);
            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void AssembleApiResponse_GetDetailActivityOutput_ReturnGetDetailActivityApiResponse()
        {
            var activityDetail = new ActivityDetail()
            {
                ActivityId = 1,
                Name = "tiket",
                Description = "",
                City = "Jakarta",
                Country = "Indonesia",
                OperationTime = "24 Jam",
                ImportantNotice = "",
                Warning = "",
                AdditionalNotes = "",
                Price = 2000
            };
            var input = new GetDetailActivityOutput()
            {
                ActivityDetail = activityDetail
            };

            var actualResult = ActivityLogic.AssembleApiResponse(input);

            var activityDetailForDisplay = new ActivityDetailForDisplay()
            {
                ActivityId = 1,
                Name = "tiket",
                Description = "",
                City = "Jakarta",
                Country = "Indonesia",
                OperationTime = "24 Jam",
                ImportantNotice = "",
                Warning = "",
                AdditionalNotes = "",
                Price = 2000
            };
            var expectedResult = new GetDetailActivityApiResponse()
            {
                ActivityDetail = activityDetailForDisplay
            };

            Assert.AreEqual(expectedResult.ActivityDetail.ActivityId, actualResult.ActivityDetail.ActivityId);
            Assert.AreEqual(expectedResult.ActivityDetail.Name, actualResult.ActivityDetail.Name);
            Assert.AreEqual(expectedResult.ActivityDetail.Description, actualResult.ActivityDetail.Description);
            Assert.AreEqual(expectedResult.ActivityDetail.City, actualResult.ActivityDetail.City);
            Assert.AreEqual(expectedResult.ActivityDetail.Country, actualResult.ActivityDetail.Country);
            Assert.AreEqual(expectedResult.ActivityDetail.OperationTime, actualResult.ActivityDetail.OperationTime);
            Assert.AreEqual(expectedResult.ActivityDetail.ImportantNotice, actualResult.ActivityDetail.ImportantNotice);
            Assert.AreEqual(expectedResult.ActivityDetail.Warning, actualResult.ActivityDetail.Warning);
            Assert.AreEqual(expectedResult.ActivityDetail.AdditionalNotes, actualResult.ActivityDetail.AdditionalNotes);
            Assert.AreEqual(expectedResult.ActivityDetail.Price, actualResult.ActivityDetail.Price);
        }
    }
}
