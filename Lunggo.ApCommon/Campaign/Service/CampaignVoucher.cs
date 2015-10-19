using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        public CampaignVoucher GetCampaignVoucher(string voucherCode)
        {
            return GetDb.GetCampaignVoucher(voucherCode);
        }
        public VoucherResponse UseCampaignVoucher(VoucherRequest voucherRequest)
        {
            VoucherResponse response = new VoucherResponse();
            var voucher = GetDb.GetCampaignVoucher(voucherRequest.VoucherCode);
            VoucherValidationStatusType updateStatus = ValidateVoucherRequest(voucher, voucherRequest);
            if (updateStatus == VoucherValidationStatusType.UpdateSuccess)
            {
                response = CalculateVoucherDiscount(voucher, voucherRequest);
                response.UpdateStatus = DecrementVoucher(response, voucherRequest);
            }
            else
            {
                response = new VoucherResponse()
                {
                    UpdateStatus = updateStatus
                };
            }

            return response;
        }
        private VoucherValidationStatusType ValidateVoucherRequest(CampaignVoucher voucher, VoucherRequest voucherRequest)
        {
            var currentTime = DateTime.Now;
            if (voucher.StartDate >= currentTime)
                return VoucherValidationStatusType.CampaignNotStartedYet;
            else if (voucher.EndDate <= currentTime)
                return VoucherValidationStatusType.CampaignHasEnded;
            else if (voucher.RemainingCount < 1)
                return VoucherValidationStatusType.NoVoucherRemaining;
            else if (voucher.MinSpendValue > voucherRequest.Price)
                return VoucherValidationStatusType.BelowMinimumSpend;
            else if (voucher.CampaignTypeCd == CampaignTypeCd.Mnemonic(CampaignType.Member))
            {
                //TODO check is registered from user table
            }
            else if (voucher.CampaignTypeCd == CampaignTypeCd.Mnemonic(CampaignType.Private))
            {
                if (!GetDb.IsEligibleForVoucher(voucherRequest.VoucherCode, voucherRequest.Email))
                    return VoucherValidationStatusType.EmailNotEligible;
            }
            else if (voucher.CampaignStatus == false)
                return VoucherValidationStatusType.CampaignInactive;

            return VoucherValidationStatusType.UpdateSuccess;
        }
        private VoucherResponse CalculateVoucherDiscount(CampaignVoucher voucher, VoucherRequest voucherRequest)
        {
            VoucherResponse response = new VoucherResponse();
            response.OriginalPrice = voucherRequest.Price;
            response.TotalDiscount = 0;
            if (voucher.ValuePercentage != null && voucher.ValuePercentage > 0)
                response.TotalDiscount += (response.OriginalPrice * (decimal)voucher.ValuePercentage / 100);
            if (voucher.ValueConstant != null && voucher.ValueConstant > 0)
                response.TotalDiscount += (decimal)voucher.ValueConstant;
            if (voucher.MaxDiscountValue != null && response.TotalDiscount > voucher.MaxDiscountValue)
                response.TotalDiscount = (decimal)voucher.MaxDiscountValue;

            response.DiscountedPrice = response.OriginalPrice - response.TotalDiscount;
            return response;
        }
        private VoucherValidationStatusType DecrementVoucher(VoucherResponse response, VoucherRequest voucherRequest)
        {
            //TODO decrement voucher
            return VoucherValidationStatusType.UpdateSuccess;
        }
    }
}
