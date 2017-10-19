using System;
using Lunggo.ApCommon.Activity.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.ApCommonTests.Activity.Service.Tests
{
    [TestClass]
    public partial class ActivityServiceTests
    {
        [TestMethod()]
        public void Init_False_Test()
        {
            ActivityService.GetInstance().Init("");
        }
    }
}
