using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.GetListActivityLogic.Tests
{
    
    public partial class GetListActivityLogicTest
    {
        [TestMethod]
        public void TryPreprocess_Null_ReturnFalse()
        {
            GetListActivityApiRequest test = null;
            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_ValidInput_ReturnTrue()
        {
            var test = new GetListActivityApiRequest()
            {
                Page = "1",
                PerPage = "10"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryPreprocess_PageIsntNumeric_ReturnFalse()
        {
            var test = new GetListActivityApiRequest()
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
            var test = new GetListActivityApiRequest()
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
            var test = new GetListActivityApiRequest()
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
            var test = new GetListActivityApiRequest()
            {
                Page = "1",
                PerPage = "-1"
            };

            var result = ActivityLogic.TryPreprocess(test, out var serviceRequest);
            Assert.IsFalse(result);
        }
    }
}
