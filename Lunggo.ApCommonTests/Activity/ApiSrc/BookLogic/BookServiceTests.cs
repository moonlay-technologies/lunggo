using System;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.BookLogic
{
    [TestClass]
    public class ActivityServiceTests
    {
        [TestMethod]
        public void CreateActivityReservation()
        {
            var input = new BookActivityInput();
            var actInfo = new ActivityDetail();

            var actualResult = ActivityService.GetInstance().CreateActivityReservation(input, actInfo);

        }
    }
}
