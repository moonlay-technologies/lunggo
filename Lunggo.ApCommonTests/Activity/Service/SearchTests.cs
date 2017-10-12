using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommonTests.Init;
using Lunggo.Framework.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.Service.Tests
{
    [TestClass()]
    public partial class ActivityServiceTests
    {
        [TestMethod()]
        public void Search_SearchId_Test()
        {
            var input = new SearchActivityInput { SearchActivityType = SearchActivityType.SearchID};

            var ActualResult = ActivityService.GetInstance().Search(input);
            
            Assert.IsNotNull(ActualResult);
        }

        [TestMethod()]
        public void Search_ActivityName_Test()
        { 
            Initializer.Init();

            var input = new SearchActivityInput
            {
                SearchActivityType = SearchActivityType.ActivityName,
                ActivityFilter = new ActivityFilter() { Name = "Marjan"},
                Page = 1,
                PerPage = 10
            };

            var ActualResult = ActivityService.GetInstance().Search(input);

            Assert.IsNotNull(ActualResult);

        }

        [TestMethod()]
        public void Search_ActivityDate_Test()
        {
            Initializer.Init();

            var input = new SearchActivityInput
            {
                SearchActivityType = SearchActivityType.ActivityDate,
                ActivityFilter = new ActivityFilter() { CloseDate = DateTime.Parse("02/18/2017") },
                Page = 1,
                PerPage = 10
            };

            var ActualResult = ActivityService.GetInstance().Search(input);

            Assert.IsNotNull(ActualResult);

        }
    }
}