using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommonTests.Init;
using Lunggo.Framework.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.Service.Tests
{
    //[TestClass()]
    public partial class ActivityServiceTests
    {
        [TestMethod()]
        public void GetActivityFromDbByName_BasicTest()
        {
            Initializer.Init();
            var reqName = new ActivityFilter() { Name = "Marjan" };
            var input = new SearchActivityInput { ActivityFilter = reqName };

            var ActualResult = ActivityService.GetInstance().Search(input);
            
            Assert.IsNotNull(ActualResult);
        }
        
    }
}