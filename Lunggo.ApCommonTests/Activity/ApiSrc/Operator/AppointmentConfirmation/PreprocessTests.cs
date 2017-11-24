using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.AppointmentConfirmationLogic.Tests
{
    [TestClass]
    public partial class ConfirmAppointmentLogicTest
    {
        [TestMethod]
        public void IsValid_Null_ReturnFalse()
        {
            string rsvNo = "123";
            var actualResult = ActivityLogic.PreprocessServiceRequest(rsvNo);
            var expectedResult = new AppointmentConfirmationInput() { RsvNo = "123" };
            Assert.AreEqual(expectedResult.RsvNo, actualResult.RsvNo);
        }
        
    }
}
