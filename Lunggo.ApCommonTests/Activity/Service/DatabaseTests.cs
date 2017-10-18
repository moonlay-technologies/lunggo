using System;
using System.Collections.Generic;
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
        public void GetActivityFromDbByName_BasicTest()
        {
            Initializer.Init();
            var actFilter = new ActivityFilter()
            {
                Name = "",
                EndDate = DateTime.Parse("02/18/2022"),
                StartDate = DateTime.Parse("02/18/2017")
            };

            var input = new SearchActivityInput
            {
                ActivityFilter = actFilter,
                Page = 1,
                PerPage = 1
            };

            var actualResult = ActivityService.GetInstance().Search(input);
            
            Assert.IsNotNull(actualResult);
        }
        
    }
}