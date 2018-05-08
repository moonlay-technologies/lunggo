using System;
using Dapper;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.Framework.TestHelpers;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.PaymentTest.DbServiceTests
{
    [TestClass]
    public class GetCampaignVoucherTests
    {
        [TestMethod]
        // Should return voucher with supplied voucherCode
        public void Should_return_voucher_with_supplied_voucherCode()
        {
            TestHelper.UseDb(conn =>
                {
                    var voucherCode = "3w2ertyuiy6tr";
                    var campaignId = 1234567890123456789L;
                    var expectedCampaign = new CampaignTableRecord
                    {
                        CampaignId = campaignId,
                        Name = "asdfghjkl",
                        DisplayName = "dnhahdakw",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(100),
                        ValuePercentage = 12,
                        ValueConstant = 34,
                        VoucherCount = 1234,
                        MinSpendValue = 100,
                        MaxDiscountValue = 12345,
                        Description = "dnwjdkjwandja",
                        ProductType = "asd"
                    };
                    var expectedCampaignVoucher = new CampaignVoucherTableRecord
                    {
                        CampaignId = campaignId,
                        RemainingCount = 1234567,
                        VoucherCode = voucherCode
                    };
                    CampaignTableRepo.GetInstance().Delete(conn, expectedCampaign);
                    CampaignVoucherTableRepo.GetInstance().Delete(conn, expectedCampaignVoucher);
                    CampaignTableRepo.GetInstance().Insert(conn, expectedCampaign);
                    CampaignVoucherTableRepo.GetInstance().Insert(conn, expectedCampaignVoucher);

                    var actual = new PaymentDbService().GetCampaignVoucher(voucherCode);

                    Assert.IsNotNull(actual);
                    Assert.AreEqual(expectedCampaign.CampaignId, actual.CampaignId);
                    Assert.AreEqual(expectedCampaign.Name, actual.CampaignName);
                    Assert.AreEqual(expectedCampaign.DisplayName, actual.DisplayName);
                    Assert.AreEqual(expectedCampaign.Description, actual.CampaignDescription);
                    Assert.AreEqual(expectedCampaign.StartDate.Value.ToString("G"), actual.StartDate.ToString("G"));
                    Assert.AreEqual(expectedCampaign.EndDate.Value.ToString("G"), actual.EndDate.ToString("G"));
                    Assert.AreEqual(expectedCampaign.ValuePercentage, actual.ValuePercentage);
                    Assert.AreEqual(expectedCampaign.ValueConstant, actual.ValueConstant);
                    Assert.AreEqual(expectedCampaign.MinSpendValue, actual.MinSpendValue);
                    Assert.AreEqual(expectedCampaign.MaxDiscountValue, actual.MaxDiscountValue);
                    Assert.AreEqual(expectedCampaignVoucher.RemainingCount, actual.RemainingCount);
                    Assert.AreEqual(expectedCampaign.ProductType, actual.ProductType);

                    CampaignTableRepo.GetInstance().Delete(conn, expectedCampaign);
                    CampaignVoucherTableRepo.GetInstance().Delete(conn, expectedCampaignVoucher);
                }
            );
        }

        [TestMethod]
        // Should return null when there is no campaign mathing with supplier voucherCode
        public void Should_return_null_when_there_is_no_campaign_mathing_with_supplier_voucherCode()
        {
            TestHelper.UseDb(conn =>
            {
                var voucherCode = "db398rn08n8wr9uw3";
                var campaignId = 987654321987654321L;
                var expectedCampaign = new CampaignTableRecord
                {
                    CampaignId = campaignId
                };
                var expectedCampaignVoucher = new CampaignVoucherTableRecord
                {
                    VoucherCode = voucherCode,
                    CampaignId = campaignId
                };
                CampaignTableRepo.GetInstance().Delete(conn, expectedCampaign);
                CampaignVoucherTableRepo.GetInstance().Delete(conn, expectedCampaignVoucher);

                var actual = new PaymentDbService().GetCampaignVoucher(voucherCode);

                Assert.IsNull(actual);
            });
        }
    }
}
