using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Model;

namespace Lunggo.ApCommon.Activity.Service.Tests
{
    [TestClass()]
    public class ActivityServiceTests
    {
        [TestMethod()]
        public void SearchTest()
        {   var input = new SearchActivityInput{Name = "Marjan"};
            var activity = ActivityService.GetInstance().Search(input);
            Assert.IsNotNull(activity);
        }
    }
}