using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.GetMyBookingDetailLogic.Tests
{
    [TestClass]
    public partial class GetMyBookingDetailLogicTest
    {
        [TestMethod]
        public void IsValid_Null_ReturnFalse()
        {
            GetMyBookingDetailApiRequest test = null;
            var result = ActivityLogic.IsValid(test);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_RsvNoEmpty_ReturnFalse()
        {
            GetMyBookingDetailApiRequest test = new GetMyBookingDetailApiRequest() {RsvNo = "" };
            var result = ActivityLogic.IsValid(test);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_RsvNo_ReturnTrue()
        {
            GetMyBookingDetailApiRequest test = new GetMyBookingDetailApiRequest() { RsvNo = "12345" };
            var result = ActivityLogic.IsValid(test);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Preprocess_GetMyBookingDetailApiRequest_ReturnGetMyBookingDetailInput()
        {
            GetMyBookingDetailApiRequest test = new GetMyBookingDetailApiRequest() { RsvNo = "12345" };
            var actualResult = ActivityLogic.PreprocessServiceRequest(test);

            var expectedResult = new GetMyBookingDetailInput() { RsvNo = "12345" };
            Assert.AreEqual(expectedResult.RsvNo, actualResult.RsvNo);
        }
    }
}
