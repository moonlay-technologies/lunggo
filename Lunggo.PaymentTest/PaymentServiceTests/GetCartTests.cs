using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Cache;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Processor;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lunggo.PaymentTest.PaymentServiceTests
{
    [TestClass]
    public class GetCartTests
    {
        [TestMethod]
        // TESTNAME
        public void TESTNAME()
        {
            var userId = "8601";
            var cartId = "dawdaw";
            var cacheMock = new Mock<PaymentCacheService>();
            var dbMock = new Mock<PaymentDbService>();

            var rsvNo1 = "1234";
            var rsvNo2 = "5678";
            cacheMock.Setup(m => m.GetCartRsvNos(It.IsAny<string>())).Returns(new List<string> { rsvNo1, rsvNo2 });
            dbMock.Setup(m => m.GetPaymentDetails(rsvNo1)).Returns(new RsvPaymentDetails());
            dbMock.Setup(m => m.GetPaymentDetails(rsvNo2)).Returns(new RsvPaymentDetails());

            var service = new Mock<PaymentService>(null, dbMock.Object, cacheMock.Object);
            service.Setup(m => m.GetSurchargeNominal(It.IsAny<RsvPaymentDetails>())).Returns(272838);
            service.Setup(m => m.GetCartPaymentDetails(cartId)).Returns(new CartPaymentDetails
            {
                RsvPaymentDetails = new List<RsvPaymentDetails>
                {
                    new RsvPaymentDetails
                    {
                        RsvNo = rsvNo1,
                        Medium = PaymentMedium.Veritrans,
                        Method = PaymentMethod.BcaKlikpay,
                        Submethod = PaymentSubmethod.BCA,
                        Status = PaymentStatus.MethodNotSet,
                        Time = DateTime.Now,
                        TimeLimit = DateTime.Now,
                        TransferAccount = "1234567890",
                        RedirectionUrl = "http://234567890",
                        ExternalId = "87654321",
                        DiscountCode = "asdfghjkl",
                        OriginalPriceIdr = 1234567890,
                        DiscountNominal = 987654321,
                        Surcharge = 3456789,
                        UniqueCode = 8765432,
                        FinalPriceIdr = 876543234,
                        PaidAmountIdr = 654345,
                        LocalCurrency = new Currency("USD", 123, 321),
                        LocalFinalPrice = 47384123,
                        LocalPaidAmount = 47297424,
                        InvoiceNo = "asdfg123456"
                    },
                    new RsvPaymentDetails
                    {
                        RsvNo = rsvNo2,
                        Medium = PaymentMedium.Veritrans,
                        Method = PaymentMethod.BcaKlikpay,
                        Submethod = PaymentSubmethod.BCA,
                        Status = PaymentStatus.MethodNotSet,
                        Time = DateTime.Now,
                        TimeLimit = DateTime.Now,
                        TransferAccount = "1234567890",
                        RedirectionUrl = "http://234567890",
                        ExternalId = "87654321",
                        DiscountCode = "asdfghjkl",
                        OriginalPriceIdr = 1234567890,
                        DiscountNominal = 987654321,
                        Surcharge = 3456789,
                        UniqueCode = 8765432,
                        FinalPriceIdr = 876543234,
                        PaidAmountIdr = 654345,
                        LocalCurrency = new Currency("USD", 123, 321),
                        LocalFinalPrice = 47384123,
                        LocalPaidAmount = 47297424,
                        InvoiceNo = "asdfg123456"
                    }
                },
                CartId = cartId,
                Medium = PaymentMedium.Veritrans,
                Method = PaymentMethod.BcaKlikpay,
                Submethod = PaymentSubmethod.BCA,
                Status = PaymentStatus.MethodNotSet,
                Time = DateTime.Now,
                TimeLimit = DateTime.Now,
                TransferAccount = "1234567890",
                RedirectionUrl = "http://234567890",
                ExternalId = "87654321",
                DiscountCode = "aaa",
                OriginalPriceIdr = 1234567890,
                DiscountNominal = 987654321,
                Surcharge = 3456789,
                UniqueCode = 8765432,
                FinalPriceIdr = 876543234,
                PaidAmountIdr = 654345,
                LocalCurrency = new Currency("USD", 123, 321),
                LocalFinalPrice = 47384123,
                LocalPaidAmount = 47297424,
                InvoiceNo = "asdfg123456"
            });

            var actual = service.Object.GetCartByUser(userId);
        }
	
    }
}
