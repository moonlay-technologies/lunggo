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
        public void Issue_ReturnException()
        {
            var result = ActivityService.GetInstance().Issue("");
        }
        
    }
}