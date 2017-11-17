using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.GetAppointmentRequestLogic.Tests
{
    [TestClass]
    public partial class GetAppointmentRequestLogicTest
    {
        [TestMethod]
        public void TryPreprocess_Null_ReturnFalse()
        {
            GetAppointmentRequestApiRequest test = null;
            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_ValidInput_ReturnTrue()
        {
            var test = new GetAppointmentRequestApiRequest()
            {
                Page = "1",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_PageIsntNumeric_ReturnFalse()
        {
            var test = new GetAppointmentRequestApiRequest()
            {
                Page = "abcde",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_PageLessThanZero_ReturnFalse()
        {
            var test = new GetAppointmentRequestApiRequest()
            {
                Page = "-1",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_PerPageIsntNumeric_ReturnFalse()
        {
            var test = new GetAppointmentRequestApiRequest()
            {
                Page = "1",
                PerPage = "abcde"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_PerPageLessThanZero_ReturnFalse()
        {
            var test = new GetAppointmentRequestApiRequest()
            {
                Page = "1",
                PerPage = "-1"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }
    }
}
