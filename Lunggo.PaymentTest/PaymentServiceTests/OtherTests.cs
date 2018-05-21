using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Lunggo.Framework.TestHelpers.TestHelper;

namespace Lunggo.PaymentTest.PaymentServiceTests
{
    [TestClass]
    public class CreateNewPaymentTests
    {
        [TestMethod]
        // Should generate paymentdetails with right data
        public void Should_generate_paymentdetails_with_right_data()
        {
            var rsvNo = "8134819004";
            var price = 123456789;
            var currency = new Currency("ASD", 100, 123);
            DateTime? timeLimit = RandomDateTime(DateTime.Now, DateTime.MaxValue);

            var dbMock = new Mock<PaymentDbService>();

            var expected = new RsvPaymentDetails
            {
                RsvNo = rsvNo,
                Status = PaymentStatus.MethodNotSet,
                OriginalPriceIdr = price,
                LocalCurrency = currency,
                TimeLimit = timeLimit
            };

            var service = new PaymentService(null, dbMock.Object, null);

            service.CreateNewPayment(rsvNo, price, currency, timeLimit);

            dbMock.Verify(m => m.InsertPaymentDetails(It.Is<RsvPaymentDetails>(actual =>
                actual.RsvNo == expected.RsvNo &&
                actual.Status == expected.Status &&
                actual.OriginalPriceIdr == expected.OriginalPriceIdr &&
                actual.LocalCurrency.Equals(expected.LocalCurrency) &&
                actual.TimeLimit == expected.TimeLimit.Value.AddMinutes(-10)
                )));
        }
    }

    [TestClass]
    public class GetUserBankAccountsTests
    {
        [TestMethod]
        // Should return list of bank accounts when data exists under supplied userId
        public void Should_return_list_of_bank_accounts_when_data_exists_under_supplied_userId()
        {
            var userId = RandomString();
            var expected1 = new BankAccount
            {
                AccountNumber = RandomString(),
                BankName = RandomString(),
                Branch = RandomString(),
                OwnerName = RandomString()
            };
            var expected2 = new BankAccount
            {
                AccountNumber = RandomString(),
                BankName = RandomString(),
                Branch = RandomString(),
                OwnerName = RandomString()
            };
            var dbMock = new Mock<PaymentDbService>();
            dbMock.Setup(m => m.GetUserBankAccounts(userId)).Returns(new List<BankAccount> {expected1, expected2});

            var service = new PaymentService(paymentDbService: dbMock.Object);
            var actual = service.GetUserBankAccounts(userId);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 2);
            Assert.IsTrue(actual.Contains(expected1));
            Assert.IsTrue(actual.Contains(expected2));
        }

        [TestMethod]
        // Should return empty list when there is no data existing under supplier userId
        public void Should_return_empty_list_when_there_is_no_data_existing_under_supplier_userId()
        {
            var userId = RandomString();
            var dbMock = new Mock<PaymentDbService>();
            dbMock.Setup(m => m.GetUserBankAccounts(userId)).Returns(new List<BankAccount> ());

            var service = new PaymentService(paymentDbService: dbMock.Object);
            var actual = service.GetUserBankAccounts(userId);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 0);
        }
	
    }
}
