using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using System.Collections.Generic;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Operator.UpdateActivityLogic.Tests
{
    [TestClass]
    public partial class UpdateActivityLogicTest
    {
        [TestMethod]
        public void TryPreprocess_Null_ReturnFalse()
        {
            var input = new ActivityUpdateApiRequest();
            var actualResult = ActivityLogic.TryPreprocess(input, out var serviceRequest);

            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryPreprocess_ActivityIdLessThanZero_ReturnFalse()
        {
            var input = new ActivityUpdateApiRequest()
            {
                ActivityId = -1,
                Name = "abcde",
                Price = 2000,
                RequiredPaxData = new List<string>() { "abc", "cde", "efg" }
            };
            var actualResult = ActivityLogic.TryPreprocess(input, out var serviceRequest);

            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryPreprocess_ActivityNameEmpty_ReturnFalse()
        {
            var input = new ActivityUpdateApiRequest()
            {
                ActivityId = 1,
                Name = "",
                Price = 2000
            };
            var actualResult = ActivityLogic.TryPreprocess(input, out var serviceRequest);

            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryPreprocess_PriceLessThanZero_ReturnFalse()
        {
            var input = new ActivityUpdateApiRequest()
            {
                ActivityId = 1,
                Name = "abcde",
                Price = -2000
            };
            var actualResult = ActivityLogic.TryPreprocess(input, out var serviceRequest);

            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryPreprocess_AmountDurationIsntNumeric_ReturnFalse()
        {
            var input = new ActivityUpdateApiRequest()
            {
                ActivityId = 1,
                Name = "abcde",
                Price = 2000,
                Duration = "abc days"
            };
            var actualResult = ActivityLogic.TryPreprocess(input, out var serviceRequest);

            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryPreprocess_ValidInput_ReturnTrue()
        {
            var input = new ActivityUpdateApiRequest()
            {
                ActivityId = 1,
                Name = "abcde",
                Price = 2000,
                Duration = "1 day",
                RequiredPaxData = new List<string>() { "abc", "cde", "efg" }
            };
            var actualResult = ActivityLogic.TryPreprocess(input, out var serviceRequest);

            Assert.IsTrue(actualResult);
        }
    }
}
