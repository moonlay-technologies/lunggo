using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Payment.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Lunggo.Framework.TestHelpers.TestHelper;
using static Lunggo.WebAPI.ApiSrc.Payment.Logic.PaymentLogic;

namespace Lunggo.WebAPITests.PaymentTests
{
    [TestClass]
    public class GetUserBankAccountsTests
    {
        [TestMethod]
        // Should return unauthorized when not logged in
        public void Should_return_unauthorized_when_not_logged_in()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return list of bank accounts when data exists under supplied userId
        public void Should_return_list_of_bank_accounts_when_data_exists_under_supplied_userId()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return empty list when there is no data existing under supplier userId
        public void Should_return_empty_list_when_there_is_no_data_existing_under_supplier_userId()
        {
            throw new NotImplementedException();
        }
	
	
    }
}
