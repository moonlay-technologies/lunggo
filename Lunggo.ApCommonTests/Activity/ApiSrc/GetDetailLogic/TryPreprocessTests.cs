using System;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.GetDetailLogic.Tests
{
    public partial class GetDetailActivityLogicTest
    {
        [TestMethod]
        public void TryPreprocess_Null_ReturnFalse()
        {
            GetDetailActivityApiRequest request = null;
            var result = ActivityLogic.TryPreprocess(request, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_IsntNumeric_ReturnFalse()
        {
            var request = new GetDetailActivityApiRequest(){ActivityId = "abcde"};
            var result = ActivityLogic.TryPreprocess(request, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_LessThanZero_ReturnFalse()
        {
            var request = new GetDetailActivityApiRequest() { ActivityId = "-1" };
            var result = ActivityLogic.TryPreprocess(request, out var serviceRequest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryPreprocess_MoreThanZero_ReturnTrue()
        {
            var request = new GetDetailActivityApiRequest() { ActivityId = "1" };
            var result = ActivityLogic.TryPreprocess(request, out var serviceRequest);
            Assert.IsTrue(result);
        }
    }
}
