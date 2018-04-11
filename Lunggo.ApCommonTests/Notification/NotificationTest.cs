using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommonTests.Notification
{
    [TestClass]
    class NotificationTest
    {
        [TestMethod]
        public void Notification_RegisterDeviceIfDeviceHaventRegisteredBefore()
        {
            var expectedResult = new GetAvailableDatesApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERASEA01"
            };
            var actualResult = RegisterDevice(handle, deviceId, userId, tags);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

        [TestMethod]
        public void GetAvailableDates_ValidInput_ReturnSomething()
        {
            Initializer.Init();

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
