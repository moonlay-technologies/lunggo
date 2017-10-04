using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommonTests.Init;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Service.Tests
{
    [TestClass()]
    public class ActivityServiceTests
    {
        [TestMethod()]
        public void GetActivityFromDbByName_BasicTest()
        {
            Initializer.Init();
            var input = new SearchActivityInput { Name = "Marjan" };

            var ActualResult = ActivityService.GetInstance().Search(input);
            
            Assert.IsNotNull(ActualResult);
        }
        [TestMethod()]
        public void GetActivityFromDbByNameTest()
        {
            Initializer.Init();

            var input = new SearchActivityInput { Name = "Marjan" };

            var ActList1 = new ActivityDetail()
            { Name = "Marjan", City = "Bandung", Country = "Indonesia", Description = "coba", OperationTime = "24 Jam", Price = 2000 };
            var ActList2 = new ActivityDetail()
            { Name = "Marjan", City = "Bandung", Country = "Indonesia", Description = "coba", OperationTime = "24 Jam", Price = 3500 };
            var ActList3 = new ActivityDetail()
            { Name = "Marjan aja", City = "Jakarta", Country = "Indonesia", Description = "apapun", OperationTime = "2 Hari", Price = 4500 };
            var ActList4 = new ActivityDetail()
            { Name = "Marjan", City = "Stockholm", Country = "Swedia", Description = "123coba", OperationTime = "3 Jam", Price = 3500 };
            var ActList5 = new ActivityDetail()
            { Name = "Marjan", City = "Stockholm", Country = "Swedia", Description = "123coba", OperationTime = "3 Jam", Price = 4500 };
            var ActList6 = new ActivityDetail()
            { Name = "Marjan aja", City = "Jakarta", Country = "Indonesia", Description = "apapun", OperationTime = "2 Hari", Price = 2000 };
            var ActList7 = new ActivityDetail()
            { Name = "Marjan", City = "Stockholm", Country = "Swedia", Description = "123coba", OperationTime = "3 Jam", Price = 3500 };
            var ActList8 = new ActivityDetail()
            { Name = "Marjan", City = "Stockholm", Country = "Swedia", Description = "123coba", OperationTime = "3 Jam", Price = 2000 };
            var ActList9 = new ActivityDetail()
            { Name = "Marjan", City = "Stockholm", Country = "Swedia", Description = "123coba", OperationTime = "3 Jam", Price = 2000 };
            var ActList = new List<ActivityDetail>();
            ActList.Add(ActList1);
            ActList.Add(ActList2);
            ActList.Add(ActList3);
            ActList.Add(ActList4);
            ActList.Add(ActList5);
            ActList.Add(ActList6);
            ActList.Add(ActList7);
            ActList.Add(ActList8);
            ActList.Add(ActList9);
            var expectedResult = new SearchActivityOutput
            {
                ActivityList = ActList
            };
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var actualResult = ActivityService.GetInstance().GetActivityFromDbByName(input);
                var name = actualResult.ActivityList[0];
                
                Assert.AreEqual(expectedResult.ActivityList, actualResult.ActivityList);
            }

        }
    }
}