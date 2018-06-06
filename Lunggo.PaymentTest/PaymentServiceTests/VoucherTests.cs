using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class GetVoucherDiscountForCartTests
    {
        //TODO: unit test ancur gara2 ganti struktur. Semua yg mock GetTrxPaymentDetails harus diganti jadi mock GetCart

        //[TestMethod]
        //// Should return status success and discount details when voucher code is valid and eligible
        //public void Should_return_status_success_and_discount_details_when_voucher_code_is_valid_and_eligible()
        //{
        //    var cartId = RandomString();
        //    var voucherCode = RandomString();
        //    var constant = RandomDecimal(0, 10_000);
        //    var percentage = RandomDecimal(0, 20);
        //    var name = RandomString();
        //    var displayName = RandomString();
        //    var description = RandomString();
        //    var minSpend = RandomDecimal(0, 100_000);
        //    var price = RandomDecimal(minSpend, decimal.MaxValue);
        //    var expectedCampaign = new CampaignVoucher
        //    {
        //        CampaignId = RandomLong(),
        //        CampaignDescription = description,
        //        CampaignName = name,
        //        DisplayName = displayName,
        //        StartDate = RandomDateTime(DateTime.MinValue, DateTime.Now),
        //        EndDate = RandomDateTime(DateTime.Now, DateTime.MaxValue),
        //        ValuePercentage = percentage,
        //        ValueConstant = constant,
        //        MinSpendValue = minSpend,
        //        MaxDiscountValue = RandomDecimal(1_000_000, decimal.MaxValue),
        //        RemainingCount = RandomInt(0, Int32.MaxValue),
        //        ProductType = RandomString()
        //    };
        //    var expectedDiscount = new UsedDiscount
        //    {
        //        Constant = constant,
        //        Percentage = percentage,
        //        Name = name,
        //        DisplayName = displayName,
        //        Description = description
        //    };
        //    var dbService = new Mock<PaymentDbService>();
        //    dbService.Setup(m => m.GetCampaignVoucher(voucherCode)).Returns(expectedCampaign);
        //    var trxDetails = new TrxPaymentDetails
        //    {
        //        TrxId = "TRX1234567890",
        //        OriginalPriceIdr = price
        //    };
        //    var expectedDiscountNominal = Math.Floor(price * percentage / 100 + constant);
        //    var expectedDiscountedPrice = price - expectedDiscountNominal;
        //    var service = new Mock<PaymentService>(null, dbService.Object, null);
        //    service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(trxDetails);

        //    var actual = service.Object.GetVoucherDiscountForCart(cartId, voucherCode, out var actualStatus);

        //    Assert.AreEqual(VoucherStatus.Success, actualStatus);
        //    Assert.AreEqual(expectedCampaign.CampaignId, actual.CampaignId);
        //    Assert.AreEqual(voucherCode, actual.VoucherCode);
        //    Assert.IsNotNull(actual.Discount);
        //    Assert.AreEqual(expectedDiscount.Constant, actual.Discount?.Constant);
        //    Assert.AreEqual(expectedDiscount.Percentage, actual.Discount?.Percentage);
        //    Assert.AreEqual(expectedDiscount.Name, actual.Discount?.Name);
        //    Assert.AreEqual(expectedDiscount.DisplayName, actual.Discount?.DisplayName);
        //    Assert.AreEqual(expectedDiscount.Description, actual.Discount?.Description);
        //    Assert.AreEqual(expectedDiscountNominal, actual.TotalDiscount);
        //    Assert.AreEqual(expectedDiscountedPrice, actual.DiscountedPrice);
        //}

        //[TestMethod]
        //// Should return status not found when voucher code does not match any campaign
        //public void Should_return_status_not_found_when_voucher_code_does_not_match_any_campaign()
        //{
        //    var cartId = RandomString();
        //    var voucherCode = RandomString();
        //    var constant = RandomDecimal(0, 10000000);
        //    var percentage = RandomDecimal(0, 100);
        //    var minSpend = RandomDecimal(0, decimal.MaxValue);
        //    var dbService = new Mock<PaymentDbService>();
        //    dbService.Setup(m => m.GetCampaignVoucher(voucherCode)).Returns((CampaignVoucher)null);
        //    var price = RandomDecimal(minSpend, decimal.MaxValue);
        //    var trxDetails = new TrxPaymentDetails
        //    {
        //        TrxId = "TRX1234567890",
        //        OriginalPriceIdr = price
        //    };
        //    var expectedDiscountNominal = Math.Floor(price * percentage / 100 + constant);
        //    var service = new Mock<PaymentService>(null, dbService.Object, null);
        //    service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(trxDetails);

        //    service.Object.GetVoucherDiscountForCart(cartId, voucherCode, out var actualStatus);

        //    Assert.AreEqual(VoucherStatus.VoucherNotFound, actualStatus);
        //}

        //[TestMethod]
        //// Should return status outside period when campaign has not started
        //public void Should_return_status_outside_period_when_campaign_has_not_started()
        //{
        //    var cartId = RandomString();
        //    var voucherCode = RandomString();
        //    var constant = RandomDecimal(0, 10000000);
        //    var percentage = RandomDecimal(0, 100);
        //    var name = RandomString();
        //    var displayName = RandomString();
        //    var description = RandomString();
        //    var minSpend = RandomDecimal(0, decimal.MaxValue);
        //    var expectedCampaign = new CampaignVoucher
        //    {
        //        CampaignId = RandomLong(),
        //        CampaignDescription = description,
        //        CampaignName = name,
        //        DisplayName = displayName,
        //        StartDate = RandomDateTime(DateTime.Now, DateTime.Now.AddDays(10)),
        //        EndDate = RandomDateTime(DateTime.Now.AddDays(11), DateTime.MaxValue),
        //        ValuePercentage = percentage,
        //        ValueConstant = constant,
        //        MinSpendValue = minSpend,
        //        MaxDiscountValue = RandomDecimal(),
        //        RemainingCount = RandomInt(0, Int32.MaxValue),
        //        ProductType = RandomString()
        //    };
        //    var dbService = new Mock<PaymentDbService>();
        //    dbService.Setup(m => m.GetCampaignVoucher(voucherCode)).Returns(expectedCampaign);
        //    var price = RandomDecimal(minSpend, decimal.MaxValue);
        //    var trxDetails = new TrxPaymentDetails
        //    {
        //        TrxId = "TRX1234567890",
        //        OriginalPriceIdr = price
        //    };
        //    var service = new Mock<PaymentService>(null, dbService.Object, null);
        //    service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(trxDetails);

        //    service.Object.GetVoucherDiscountForCart(cartId, voucherCode, out var actualStatus);

        //    Assert.AreEqual(VoucherStatus.OutsidePeriod, actualStatus);
        //}

        //[TestMethod]
        //// Should return status outside period when campaign has ended
        //public void Should_return_status_outside_period_when_campaign_has_ended()
        //{
        //    var cartId = RandomString();
        //    var voucherCode = RandomString();
        //    var constant = RandomDecimal(0, 10000000);
        //    var percentage = RandomDecimal(0, 100);
        //    var name = RandomString();
        //    var displayName = RandomString();
        //    var description = RandomString();
        //    var minSpend = RandomDecimal(0, decimal.MaxValue);
        //    var expectedCampaign = new CampaignVoucher
        //    {
        //        CampaignId = RandomLong(),
        //        CampaignDescription = description,
        //        CampaignName = name,
        //        DisplayName = displayName,
        //        StartDate = RandomDateTime(DateTime.MinValue, DateTime.Now.AddDays(-10)),
        //        EndDate = RandomDateTime(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-4)),
        //        ValuePercentage = percentage,
        //        ValueConstant = constant,
        //        MinSpendValue = minSpend,
        //        MaxDiscountValue = RandomDecimal(),
        //        RemainingCount = RandomInt(0, Int32.MaxValue),
        //        ProductType = RandomString()
        //    };
        //    var dbService = new Mock<PaymentDbService>();
        //    dbService.Setup(m => m.GetCampaignVoucher(voucherCode)).Returns(expectedCampaign);
        //    var price = RandomDecimal(minSpend, decimal.MaxValue);
        //    var trxDetails = new TrxPaymentDetails
        //    {
        //        TrxId = "TRX1234567890",
        //        OriginalPriceIdr = price
        //    };
        //    var service = new Mock<PaymentService>(null, dbService.Object, null);
        //    service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(trxDetails);

        //    service.Object.GetVoucherDiscountForCart(cartId, voucherCode, out var actualStatus);

        //    Assert.AreEqual(VoucherStatus.OutsidePeriod, actualStatus);
        //}

        //[TestMethod]
        //// Should return status voucher depleted when no more voucher remaining
        //public void Should_return_status_voucher_depleted_when_no_more_voucher_remaining()
        //{
        //    var cartId = RandomString();
        //    var voucherCode = RandomString();
        //    var constant = RandomDecimal(0, 10000000);
        //    var percentage = RandomDecimal(0, 100);
        //    var name = RandomString();
        //    var displayName = RandomString();
        //    var description = RandomString();
        //    var minSpend = RandomDecimal(0, decimal.MaxValue);
        //    var expectedCampaign = new CampaignVoucher
        //    {
        //        CampaignId = RandomLong(),
        //        CampaignDescription = description,
        //        CampaignName = name,
        //        DisplayName = displayName,
        //        StartDate = RandomDateTime(DateTime.MinValue, DateTime.Now),
        //        EndDate = RandomDateTime(DateTime.Now, DateTime.MaxValue),
        //        ValuePercentage = percentage,
        //        ValueConstant = constant,
        //        MinSpendValue = minSpend,
        //        MaxDiscountValue = RandomDecimal(),
        //        RemainingCount = RandomInt(int.MinValue, 0),
        //        ProductType = RandomString()
        //    };
        //    var dbService = new Mock<PaymentDbService>();
        //    dbService.Setup(m => m.GetCampaignVoucher(voucherCode)).Returns(expectedCampaign);
        //    var price = RandomDecimal(minSpend, decimal.MaxValue);
        //    var trxDetails = new TrxPaymentDetails
        //    {
        //        TrxId = "TRX1234567890",
        //        OriginalPriceIdr = price
        //    };
        //    var service = new Mock<PaymentService>(null, dbService.Object, null);
        //    service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(trxDetails);

        //    service.Object.GetVoucherDiscountForCart(cartId, voucherCode, out var actualStatus);

        //    Assert.AreEqual(VoucherStatus.VoucherDepleted, actualStatus);
        //}

        //[TestMethod]
        //// Should return status belom minimum spend when transaction value is below minimum voucher eligibility
        //public void Should_return_status_belom_minimum_spend_when_transaction_value_is_below_minimum_voucher_eligibility()
        //{
        //    var cartId = RandomString();
        //    var voucherCode = RandomString();
        //    var constant = RandomDecimal(0, 10000000);
        //    var percentage = RandomDecimal(0, 100);
        //    var name = RandomString();
        //    var displayName = RandomString();
        //    var description = RandomString();
        //    var minSpend = RandomDecimal(10000, decimal.MaxValue);
        //    var expectedCampaign = new CampaignVoucher
        //    {
        //        CampaignId = RandomLong(),
        //        CampaignDescription = description,
        //        CampaignName = name,
        //        DisplayName = displayName,
        //        StartDate = RandomDateTime(DateTime.MinValue, DateTime.Now),
        //        EndDate = RandomDateTime(DateTime.Now, DateTime.MaxValue),
        //        ValuePercentage = percentage,
        //        ValueConstant = constant,
        //        MinSpendValue = minSpend,
        //        MaxDiscountValue = RandomDecimal(),
        //        RemainingCount = RandomInt(0, Int32.MaxValue),
        //        ProductType = RandomString()
        //    };
        //    var dbService = new Mock<PaymentDbService>();
        //    dbService.Setup(m => m.GetCampaignVoucher(voucherCode)).Returns(expectedCampaign);
        //    var price = RandomDecimal(0, minSpend - 100);
        //    var trxDetails = new TrxPaymentDetails
        //    {
        //        TrxId = "TRX1234567890",
        //        OriginalPriceIdr = price
        //    };
        //    var service = new Mock<PaymentService>(null, dbService.Object, null);
        //    service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(trxDetails);

        //    service.Object.GetVoucherDiscountForCart(cartId, voucherCode, out var actualStatus);

        //    Assert.AreEqual(VoucherStatus.BelowMinimumSpend, actualStatus);
        //}

        //[TestMethod]
        //// Should limit discount nominal to max when calculated discount is above max
        //public void Should_limit_discount_nominal_to_max_when_calculated_discount_is_above_max()
        //{
        //    var cartId = RandomString();
        //    var voucherCode = RandomString();
        //    var maxDiscount = RandomDecimal(1_000_000, decimal.MaxValue);
        //    var constant = RandomDecimal(maxDiscount, decimal.MaxValue);
        //    var percentage = 0;
        //    var name = RandomString();
        //    var displayName = RandomString();
        //    var description = RandomString();
        //    var minSpend = RandomDecimal(0, 100_000);
        //    var price = RandomDecimal(maxDiscount + 1_000_000, decimal.MaxValue);
        //    var expectedCampaign = new CampaignVoucher
        //    {
        //        CampaignId = RandomLong(),
        //        CampaignDescription = description,
        //        CampaignName = name,
        //        DisplayName = displayName,
        //        StartDate = RandomDateTime(DateTime.MinValue, DateTime.Now),
        //        EndDate = RandomDateTime(DateTime.Now, DateTime.MaxValue),
        //        ValuePercentage = percentage,
        //        ValueConstant = constant,
        //        MinSpendValue = minSpend,
        //        MaxDiscountValue = maxDiscount,
        //        RemainingCount = RandomInt(0, Int32.MaxValue),
        //        ProductType = RandomString()
        //    };
        //    var expectedDiscount = new UsedDiscount
        //    {
        //        Constant = constant,
        //        Percentage = percentage,
        //        Name = name,
        //        DisplayName = displayName,
        //        Description = description
        //    };
        //    var dbService = new Mock<PaymentDbService>();
        //    dbService.Setup(m => m.GetCampaignVoucher(voucherCode)).Returns(expectedCampaign);
        //    var trxDetails = new TrxPaymentDetails
        //    {
        //        TrxId = "TRX1234567890",
        //        OriginalPriceIdr = price
        //    };
        //    var expectedDiscountNominal = maxDiscount;
        //    var expectedDiscountedPrice = price - expectedDiscountNominal;
        //    var service = new Mock<PaymentService>(null, dbService.Object, null);
        //    service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(trxDetails);

        //    var actual = service.Object.GetVoucherDiscountForCart(cartId, voucherCode, out var actualStatus);

        //    Assert.AreEqual(VoucherStatus.Success, actualStatus);
        //    Assert.AreEqual(expectedCampaign.CampaignId, actual.CampaignId);
        //    Assert.AreEqual(voucherCode, actual.VoucherCode);
        //    Assert.IsNotNull(actual.Discount);
        //    Assert.AreEqual(expectedDiscount.Constant, actual.Discount?.Constant);
        //    Assert.AreEqual(expectedDiscount.Percentage, actual.Discount?.Percentage);
        //    Assert.AreEqual(expectedDiscount.Name, actual.Discount?.Name);
        //    Assert.AreEqual(expectedDiscount.DisplayName, actual.Discount?.DisplayName);
        //    Assert.AreEqual(expectedDiscount.Description, actual.Discount?.Description);
        //    Assert.AreEqual(expectedDiscountNominal, actual.TotalDiscount);
        //    Assert.AreEqual(expectedDiscountedPrice, actual.DiscountedPrice);
        //}

        //[TestMethod]
        //// Should limit discount value to 50_000 when calculated discount is belom 50_000
        //public void Should_limit_discount_value_to_50_000_when_calculated_discount_is_belom_50_000()
        //{
        //    var cartId = RandomString();
        //    var voucherCode = RandomString();
        //    var constant = RandomDecimal(0, 10_000);
        //    var percentage = RandomDecimal(90, 100);
        //    var name = RandomString();
        //    var displayName = RandomString();
        //    var description = RandomString();
        //    var minSpend = RandomDecimal(0, 10_000);
        //    var price = RandomDecimal(minSpend, 500_000);
        //    var expectedCampaign = new CampaignVoucher
        //    {
        //        CampaignId = RandomLong(),
        //        CampaignDescription = description,
        //        CampaignName = name,
        //        DisplayName = displayName,
        //        StartDate = RandomDateTime(DateTime.MinValue, DateTime.Now),
        //        EndDate = RandomDateTime(DateTime.Now, DateTime.MaxValue),
        //        ValuePercentage = percentage,
        //        ValueConstant = constant,
        //        MinSpendValue = minSpend,
        //        MaxDiscountValue = RandomDecimal(1_000_000, decimal.MaxValue),
        //        RemainingCount = RandomInt(0, Int32.MaxValue),
        //        ProductType = RandomString()
        //    };
        //    var expectedDiscount = new UsedDiscount
        //    {
        //        Constant = constant,
        //        Percentage = percentage,
        //        Name = name,
        //        DisplayName = displayName,
        //        Description = description
        //    };
        //    var dbService = new Mock<PaymentDbService>();
        //    dbService.Setup(m => m.GetCampaignVoucher(voucherCode)).Returns(expectedCampaign);
        //    var trxDetails = new TrxPaymentDetails
        //    {
        //        TrxId = "TRX1234567890",
        //        OriginalPriceIdr = price
        //    };
        //    var expectedDiscountNominal = price - 50_000;
        //    var expectedDiscountedPrice = 50_000;
        //    var service = new Mock<PaymentService>(null, dbService.Object, null);
        //    service.Setup(m => m.GetTrxPaymentDetails(cartId)).Returns(trxDetails);

        //    var actual = service.Object.GetVoucherDiscountForCart(cartId, voucherCode, out var actualStatus);

        //    Assert.AreEqual(VoucherStatus.Success, actualStatus);
        //    Assert.AreEqual(expectedCampaign.CampaignId, actual.CampaignId);
        //    Assert.AreEqual(voucherCode, actual.VoucherCode);
        //    Assert.IsNotNull(actual.Discount);
        //    Assert.AreEqual(expectedDiscount.Constant, actual.Discount?.Constant);
        //    Assert.AreEqual(expectedDiscount.Percentage, actual.Discount?.Percentage);
        //    Assert.AreEqual(expectedDiscount.Name, actual.Discount?.Name);
        //    Assert.AreEqual(expectedDiscount.DisplayName, actual.Discount?.DisplayName);
        //    Assert.AreEqual(expectedDiscount.Description, actual.Discount?.Description);
        //    Assert.AreEqual(expectedDiscountNominal, actual.TotalDiscount);
        //    Assert.AreEqual(expectedDiscountedPrice, actual.DiscountedPrice);
        //}
    }
}
