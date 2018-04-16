using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.GetAppointmentDetailLogic.Tests
{
    
    public partial class GetAppointmentDetailLogicTest
    {
        [TestMethod]
        public void TryPreprocess_Null_ReturnFalse()
        {
            GetAppointmentDetailApiRequest test = null;
            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_ValidInput_ReturnTrue()
        {
            var test = new GetAppointmentDetailApiRequest()
            {
                ActivityId = "1",
                Date = "2017/01/01"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryPreprocess_ActIdLessThanZero_ReturnFalse()
        {
            var test = new GetAppointmentDetailApiRequest()
            {
                ActivityId = "0",
                Date = "2017/01/01"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_ActIdInvalid_ReturnFalse()
        {
            var test = new GetAppointmentDetailApiRequest()
            {
                ActivityId = "abcd",
                Date = "2017/01/01"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_DateNull_ReturnFalse()
        {
            var test = new GetAppointmentDetailApiRequest()
            {
                ActivityId = "1",
                Date = ""
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_DateInvalid_ReturnFalse()
        {
            var test = new GetAppointmentDetailApiRequest()
            {
                ActivityId = "1",
                Date = "abcde"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_SessionValid_ReturnFalse()
        {
            var test = new GetAppointmentDetailApiRequest()
            {
                ActivityId = "1",
                Date = "2017/01/01",
                Session = "1200-1700"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsTrue(result);
        }
    }
}
