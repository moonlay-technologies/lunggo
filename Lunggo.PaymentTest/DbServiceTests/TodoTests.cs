using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.PaymentTest.DbServiceTests
{
    public class TodoTests
    {
        [TestMethod]
        // Should generate valid data when creating new payment details
        public void Should_generate_valid_data_when_creating_new_payment_details()
        {
            var rsvNo = "12345678";
            var price = 100000M;
            var currency = new Currency("ASD", 100, 200);
            var timelimit = DateTime.Now;

            var actual = new PaymentService().InvokePrivate<PaymentDetails>("CreateNewPaymentDetails", rsvNo, price, currency, timelimit);

            Assert.AreEqual(rsvNo, actual.RsvNo);
            Assert.AreEqual(price, actual.OriginalPriceIdr);
            Assert.AreEqual(currency, actual.LocalCurrency);
            Assert.AreEqual(PaymentStatus.MethodNotSet, actual.Status);
            Assert.AreEqual(timelimit.AddMinutes(-10), actual.TimeLimit);
        }

        [TestMethod]
        public void Should_have_sum_of_all_pricing_from_rsv_for_cart_payment_details()
        {
            var cart = new CartPaymentDetails
            {
                OriginalPriceIdr = 23456789,
                RsvPaymentDetails = new List<PaymentDetails>
                {
                    new PaymentDetails {OriginalPriceIdr = 12345678},
                    new PaymentDetails {OriginalPriceIdr = 849574857485},
                    new PaymentDetails {OriginalPriceIdr = -38433}
                }
            };

            new PaymentService().InvokePrivate("AggregateRsvPaymentDetails", cart);

            Assert.AreEqual(cart.RsvPaymentDetails.Sum(d => d.OriginalPriceIdr), cart.OriginalPriceIdr);
        }

        [TestMethod]
        // Should distribute and clone status to every reservation payment details
        public void Should_distribute_and_clone_status_to_every_reservation_payment_details()
        {
            var cartDetails = new CartPaymentDetails
            {
                DiscountCode = "ABCDE12345",
                Discount = new UsedDiscount(),
                LocalCurrency = new Currency("ASD", 100, 123),
                Status = PaymentStatus.Challenged,
                DiscountNominal = 450_000,
                UniqueCode = 35_000,
                Surcharge = 130_000,
                FinalPriceIdr = 6_715_000,
                LocalFinalPrice = 671_500_000,
                PaidAmountIdr = 6_715_000,
                LocalPaidAmount = 671_500_000,
                OriginalPriceIdr = 7_000_000,
                RsvPaymentDetails = new List<PaymentDetails>
                {
                    new PaymentDetails{OriginalPriceIdr = 200_000},
                    new PaymentDetails{OriginalPriceIdr = 1_000_000},
                    new PaymentDetails{OriginalPriceIdr = 1_300_000},
                    new PaymentDetails{OriginalPriceIdr = 2_000_000},
                    new PaymentDetails{OriginalPriceIdr = 1_800_000},
                    new PaymentDetails{OriginalPriceIdr = 700_000}
                }
            };

            new PaymentService().InvokePrivate("DistributeRsvPaymentDetails", cartDetails);

            var actual1 = cartDetails.RsvPaymentDetails[0];
            var actual2 = cartDetails.RsvPaymentDetails[1];
            var actual3 = cartDetails.RsvPaymentDetails[2];
            var actual4 = cartDetails.RsvPaymentDetails[3];
            var actual5 = cartDetails.RsvPaymentDetails[4];
            var actual6 = cartDetails.RsvPaymentDetails[5];

            Assert.AreEqual(cartDetails.Discount, actual1.Discount);
            Assert.AreEqual(cartDetails.Discount, actual2.Discount);
            Assert.AreEqual(cartDetails.Discount, actual3.Discount);
            Assert.AreEqual(cartDetails.Discount, actual4.Discount);
            Assert.AreEqual(cartDetails.Discount, actual5.Discount);
            Assert.AreEqual(cartDetails.Discount, actual6.Discount);

            Assert.AreEqual(cartDetails.DiscountCode, actual1.DiscountCode);
            Assert.AreEqual(cartDetails.DiscountCode, actual2.DiscountCode);
            Assert.AreEqual(cartDetails.DiscountCode, actual3.DiscountCode);
            Assert.AreEqual(cartDetails.DiscountCode, actual4.DiscountCode);
            Assert.AreEqual(cartDetails.DiscountCode, actual5.DiscountCode);
            Assert.AreEqual(cartDetails.DiscountCode, actual6.DiscountCode);

            Assert.AreEqual(cartDetails.LocalCurrency, actual1.LocalCurrency);
            Assert.AreEqual(cartDetails.LocalCurrency, actual2.LocalCurrency);
            Assert.AreEqual(cartDetails.LocalCurrency, actual3.LocalCurrency);
            Assert.AreEqual(cartDetails.LocalCurrency, actual4.LocalCurrency);
            Assert.AreEqual(cartDetails.LocalCurrency, actual5.LocalCurrency);
            Assert.AreEqual(cartDetails.LocalCurrency, actual6.LocalCurrency);

            Assert.AreEqual(cartDetails.Status, actual1.Status);
            Assert.AreEqual(cartDetails.Status, actual2.Status);
            Assert.AreEqual(cartDetails.Status, actual3.Status);
            Assert.AreEqual(cartDetails.Status, actual4.Status);
            Assert.AreEqual(cartDetails.Status, actual5.Status);
            Assert.AreEqual(cartDetails.Status, actual6.Status);

            Assert.AreEqual(12857, actual1.DiscountNominal);
            Assert.AreEqual(64286, actual2.DiscountNominal);
            Assert.AreEqual(83571, actual3.DiscountNominal);
            Assert.AreEqual(128571, actual4.DiscountNominal);
            Assert.AreEqual(115714, actual5.DiscountNominal);
            Assert.AreEqual(45001, actual6.DiscountNominal);
            Assert.AreEqual(cartDetails.DiscountNominal, cartDetails.RsvPaymentDetails.Sum(d => d.DiscountNominal));

            Assert.AreEqual(1000, actual1.UniqueCode);
            Assert.AreEqual(5000, actual2.UniqueCode);
            Assert.AreEqual(6500, actual3.UniqueCode);
            Assert.AreEqual(10000, actual4.UniqueCode);
            Assert.AreEqual(9000, actual5.UniqueCode);
            Assert.AreEqual(3500, actual6.UniqueCode);
            Assert.AreEqual(cartDetails.UniqueCode, cartDetails.RsvPaymentDetails.Sum(d => d.UniqueCode));

            Assert.AreEqual(3714, actual1.Surcharge);
            Assert.AreEqual(18571, actual2.Surcharge);
            Assert.AreEqual(24143, actual3.Surcharge);
            Assert.AreEqual(37143, actual4.Surcharge);
            Assert.AreEqual(33429, actual5.Surcharge);
            Assert.AreEqual(13000, actual6.Surcharge);
            Assert.AreEqual(cartDetails.Surcharge, cartDetails.RsvPaymentDetails.Sum(d => d.Surcharge));

            Assert.AreEqual(191857, actual1.FinalPriceIdr);
            Assert.AreEqual(959285, actual2.FinalPriceIdr);
            Assert.AreEqual(1247072, actual3.FinalPriceIdr);
            Assert.AreEqual(1918572, actual4.FinalPriceIdr);
            Assert.AreEqual(1726715, actual5.FinalPriceIdr);
            Assert.AreEqual(671499, actual6.FinalPriceIdr);
            Assert.AreEqual(cartDetails.FinalPriceIdr, cartDetails.RsvPaymentDetails.Sum(d => d.FinalPriceIdr));

            Assert.AreEqual(19185700, actual1.LocalFinalPrice);
            Assert.AreEqual(95928500, actual2.LocalFinalPrice);
            Assert.AreEqual(124707200, actual3.LocalFinalPrice);
            Assert.AreEqual(191857200, actual4.LocalFinalPrice);
            Assert.AreEqual(172671500, actual5.LocalFinalPrice);
            Assert.AreEqual(67149900, actual6.LocalFinalPrice);
            Assert.AreEqual(cartDetails.LocalFinalPrice, cartDetails.RsvPaymentDetails.Sum(d => d.LocalFinalPrice));

            Assert.AreEqual(191857, actual1.PaidAmountIdr);
            Assert.AreEqual(959285, actual2.PaidAmountIdr);
            Assert.AreEqual(1247072, actual3.PaidAmountIdr);
            Assert.AreEqual(1918572, actual4.PaidAmountIdr);
            Assert.AreEqual(1726715, actual5.PaidAmountIdr);
            Assert.AreEqual(671499, actual6.PaidAmountIdr);
            Assert.AreEqual(cartDetails.PaidAmountIdr, cartDetails.RsvPaymentDetails.Sum(d => d.PaidAmountIdr));

            Assert.AreEqual(19185700, actual1.LocalPaidAmount);
            Assert.AreEqual(95928500, actual2.LocalPaidAmount);
            Assert.AreEqual(124707200, actual3.LocalPaidAmount);
            Assert.AreEqual(191857200, actual4.LocalPaidAmount);
            Assert.AreEqual(172671500, actual5.LocalPaidAmount);
            Assert.AreEqual(67149900, actual6.LocalPaidAmount);
            Assert.AreEqual(cartDetails.LocalPaidAmount, cartDetails.RsvPaymentDetails.Sum(d => d.LocalPaidAmount));
        }

        [TestMethod]
        // Should return exception when transaction is already paid
        public void Should_return_exception_when_transaction_is_already_paid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should return exception when transaction is already expired
        public void Should_return_exception_when_transaction_is_already_expired()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Should create new payment details with supplied data
        public void Should_create_new_payment_details_with_supplied_data()
        {
            var rsvNo = "123456789";
            var price = 123456789M;
            var currency = new Currency("ASD", 100, 123);

            var actual = new PaymentService().InvokePrivate<PaymentDetails>("CreateNewPaymentDetails", rsvNo, price, currency, null);

            Assert.AreEqual(rsvNo, actual.RsvNo);
            Assert.AreEqual(price, actual.OriginalPriceIdr);
            Assert.AreEqual(currency.Symbol, actual.LocalCurrency.Symbol);
            Assert.AreEqual(currency.Rate, actual.LocalCurrency.Rate);
            Assert.AreEqual(currency.RoundingOrder, actual.LocalCurrency.RoundingOrder);
        }

        [TestMethod]
        // Should create new payment details with undefined status
        public void Should_create_new_payment_details_with_undefined_status()
        {
            var rsvNo = "123456789";
            var price = 123456789M;
            var currency = new Currency("ASD", 100, 123);

            var actual = new PaymentService().InvokePrivate<PaymentDetails>("CreateNewPaymentDetails", rsvNo, price, currency, null);

            Assert.AreEqual(PaymentStatus.MethodNotSet, actual.Status);
        }

        [TestMethod]
        // Should create new payment details with 10 minutes less time limit
        public void Should_create_new_payment_details_with_10_minutes_less_time_limit()
        {
            var rsvNo = "123456789";
            var price = 123456789M;
            var currency = new Currency("ASD", 100, 123);
            var timelimit = DateTime.Now;

            var actual = new PaymentService().InvokePrivate<PaymentDetails>("CreateNewPaymentDetails", rsvNo, price, currency, timelimit);

            Assert.AreEqual(timelimit.AddMinutes(-10), actual.TimeLimit);
        }
    }
}