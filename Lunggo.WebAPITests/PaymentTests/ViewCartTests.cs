using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.Framework.TestHelpers;
using Lunggo.WebAPI.ApiSrc.Cart.Logic;
using Lunggo.WebAPI.ApiSrc.Cart.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.WebAPITests.PaymentTests
{
    [TestClass]
    public class ViewCartTests
    {
        [TestMethod]
        // TESTNAME
        public void TESTNAME()
        {
            HttpContext.Current = TestHelper.LoginUser("08428373826:petermort@wasasa.com");

            CartLogic.ViewCart();
        }
	
    }
}
