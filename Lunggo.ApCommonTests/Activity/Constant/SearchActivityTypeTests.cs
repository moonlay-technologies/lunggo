using System;
using Lunggo.ApCommon.Activity.Constant;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.Constant.Tests
{
    [TestClass]
    public class SearchActivityTypeTests
    {
        [TestMethod]
        public void MnemonicByType_SearchID_Test()
        {
            var actualResult = SearchActivityTypeCd.Mnemonic(SearchActivityType.SearchID);
            string expectedResult = "SearchID";
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void MnemonicByType_ActivityName_Test()
        {
            var actualResult = SearchActivityTypeCd.Mnemonic(SearchActivityType.ActivityName);
            string expectedResult = "ActivityName";
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void MnemonicByType_ActivityDate_Test()
        {
            var actualResult = SearchActivityTypeCd.Mnemonic(SearchActivityType.ActivityDate);
            string expectedResult = "ActivityDate";
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void MnemonicByType_Undefined_Test()
        {
            var actualResult = SearchActivityTypeCd.Mnemonic(SearchActivityType.Undefined);
            string expectedResult = null;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void MnemonicByStr_SearchID_Test()
        {
            var actualResult = SearchActivityTypeCd.Mnemonic("SearchID");
            var expectedResult = SearchActivityType.SearchID;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void MnemonicByStr_ActivityName_Test()
        {
            var actualResult = SearchActivityTypeCd.Mnemonic("ActivityName");
            var expectedResult = SearchActivityType.ActivityName;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void MnemonicByStr_ActivityDate_Test()
        {
            var actualResult = SearchActivityTypeCd.Mnemonic("ActivityDate");
            var expectedResult = SearchActivityType.ActivityDate;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void MnemonicByStr_Undefined_Test()
        {
            var actualResult = SearchActivityTypeCd.Mnemonic("Undefined");
            var expectedResult = SearchActivityType.Undefined;
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
