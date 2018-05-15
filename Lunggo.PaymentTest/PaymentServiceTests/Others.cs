using System;
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
}
