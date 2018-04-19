using System;
using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.GetAvailableLogic.Tests
{
    [TestClass]
    public partial class GetAvailableDatesLogicTest
    {
        [TestMethod]
        public void GetAvailableDates_Null_ReturnBadRequest()
        {
            var expectedResult = new GetAvailableDatesApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = ActivityLogic.GetAvailable(null);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void GetAvailableDates_ValidInput_ReturnSomething()
        {
            var input = new GetAvailableDatesApiRequest()
            {
                ActivityId = "1"
            };
            
            var actualResult = ActivityLogic.GetAvailable(input);
            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void AssembleApiResponse_GetAvailableDatesOutput_ReturnGetAvailableDatesApiResponse()
        {
            var activityDateTimes = new DateAndAvailableHour()
            {
                Date = DateTime.Parse("2017/02/18")
            };
            var input = new GetAvailableDatesOutput()
            {
                AvailableDateTimes = new List<DateAndAvailableHour>() { activityDateTimes }
            };

            var actualResult = ActivityLogic.AssembleApiResponse(input);

            var expectedResult = new GetAvailableDatesApiResponse()
            {
               // AvailableDateTimes = new List<DateAndAvailableHour>() { activityDateTimes }
            };

            Assert.AreEqual(expectedResult.AvailableDateTimes[0].Date, actualResult.AvailableDateTimes[0].Date);
        }
    }
}
