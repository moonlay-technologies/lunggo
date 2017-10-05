using System;
using Lunggo.ApCommon.Activity.Constant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.ApCommonTests.Activity.ApiSrc.Tests
{
    [TestClass]
    public partial class SearchActivityLogicTest
    {
        [TestMethod]
        public void IsValid_requestNull_Test()
        {
            ActivitySearchApiRequest test = null;
            var result = ActivityLogic.IsValid(test);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_SearchIdNull_Test()
        {
            var test = new ActivitySearchApiRequest()
            {
                Filter = new ActivityFilter(),
                Page = 1,
                PerPage = 10,
                SearchType = SearchActivityType.ActivityName
            };
            var result = ActivityLogic.IsValid(test);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValid_PageandPerPageValidation_Test()
        {
            var test = new ActivitySearchApiRequest()
            {
                Filter = new ActivityFilter(),
                Page = 0,
                PerPage = 0,
                SearchType = SearchActivityType.ActivityName,
                SearchId = ""
            };
            var result = ActivityLogic.IsValid(test);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_PageValidation_Test()
        {
            var test = new ActivitySearchApiRequest()
            {
                Filter = new ActivityFilter(),
                Page = 0,
                PerPage = 10,
                SearchType = SearchActivityType.ActivityName,
                SearchId = ""
            };
            var result = ActivityLogic.IsValid(test);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_PerPageValidation_Test()
        {
            var test = new ActivitySearchApiRequest()
            {
                Filter = new ActivityFilter(),
                Page = 3,
                PerPage = 0,
                SearchType = SearchActivityType.ActivityName,
                SearchId = ""
            };
            var result = ActivityLogic.IsValid(test);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_PerPageandPageValidation_Test()
        {
            var test = new ActivitySearchApiRequest()
            {
                Filter = new ActivityFilter(),
                Page = 3,
                PerPage = 10,
                SearchType = SearchActivityType.ActivityName,
                SearchId = ""
            };
            var result = ActivityLogic.IsValid(test);
            Assert.IsTrue(result);
        }
    }
}
