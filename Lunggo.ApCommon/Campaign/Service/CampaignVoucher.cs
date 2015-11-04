using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using System;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        public CampaignVoucher GetCampaignVoucher(string voucherCode)
        {
            return GetDb.GetCampaignVoucher(voucherCode);
        }
        public VoucherResponse ValidateVoucherRequest(VoucherRequest voucherRequest)
        {
            VoucherResponse response = new VoucherResponse();
            VoucherValidationStatusType validationStatus = VoucherValidationStatusType.Undefined;

            var voucher = GetDb.GetCampaignVoucher(voucherRequest.VoucherCode);
            response.Email = voucherRequest.Email;
            response.VoucherCode = voucherRequest.VoucherCode;

            validationStatus = ValidateVoucher(voucher, voucherRequest);

            if (validationStatus == VoucherValidationStatusType.Success)
            {
                CalculateVoucherDiscount(voucher, voucherRequest, response);
            }
            response.UpdateStatus = VoucherValidationStatusTypeMessage.Mnemonic(validationStatus);
            return response;
        }
        public VoucherResponse UseVoucherRequest(VoucherRequest voucherRequest)
        {
            VoucherResponse response = new VoucherResponse();
            VoucherValidationStatusType validationStatus = VoucherValidationStatusType.Undefined;

            var voucher = GetDb.GetCampaignVoucher(voucherRequest.VoucherCode);
            response.Email = voucherRequest.Email;
            response.VoucherCode = voucherRequest.VoucherCode;

            validationStatus = ValidateVoucher(voucher, voucherRequest);

            if (validationStatus == VoucherValidationStatusType.Success)
            {
                CalculateVoucherDiscount(voucher, voucherRequest, response);
                validationStatus = VoucherDecrement(voucherRequest.VoucherCode);
            }

            response.UpdateStatus = VoucherValidationStatusTypeMessage.Mnemonic(validationStatus);
            return response;
        }
        private VoucherValidationStatusType ValidateVoucher(CampaignVoucher voucher, VoucherRequest voucherRequest)
        {
            var currentTime = DateTime.Now;
            if (voucher == null)
                return VoucherValidationStatusType.VoucherNotFound;
            if (voucher.StartDate >= currentTime)
                return VoucherValidationStatusType.CampaignNotStartedYet;
            if (voucher.EndDate <= currentTime)
                return VoucherValidationStatusType.CampaignHasEnded;
            if (voucher.RemainingCount < 1)
                return VoucherValidationStatusType.NoVoucherRemaining;
            if (voucher.MinSpendValue > voucherRequest.Price)
                return VoucherValidationStatusType.BelowMinimumSpend;
            if (voucher.CampaignTypeCd == CampaignTypeCd.Mnemonic(CampaignType.Member))
            {
                //TODO check is registered from user table
            }
            if (voucher.CampaignTypeCd == CampaignTypeCd.Mnemonic(CampaignType.Private))
            {
                if (!GetDb.IsEligibleForVoucher(voucherRequest.VoucherCode, voucherRequest.Email))
                    return VoucherValidationStatusType.EmailNotEligible;
            }
            if (voucher.CampaignStatus == false)
                return VoucherValidationStatusType.CampaignInactive;
            if (voucher.IsSingleUsage!=null && voucher.IsSingleUsage == true)
            {
                if (GetDb.CheckVoucherUsage(voucherRequest.VoucherCode, voucherRequest.Email) > 0)
                    return VoucherValidationStatusType.VoucherAlreadyUsed;
            }
            return VoucherValidationStatusType.Success;
        }
        private void CalculateVoucherDiscount(CampaignVoucher voucher, VoucherRequest voucherRequest, VoucherResponse response)
        {
            response.OriginalPrice = voucherRequest.Price;
            response.TotalDiscount = 0;

            if (voucher.ValuePercentage != null && voucher.ValuePercentage > 0)
                response.TotalDiscount += (response.OriginalPrice * (decimal)voucher.ValuePercentage / 100);

            if (voucher.ValueConstant != null && voucher.ValueConstant > 0)
                response.TotalDiscount += (decimal)voucher.ValueConstant;

            if (voucher.MaxDiscountValue != null && voucher.MaxDiscountValue > 0
                && response.TotalDiscount > voucher.MaxDiscountValue)
                response.TotalDiscount = (decimal)voucher.MaxDiscountValue;

            response.DiscountedPrice = response.OriginalPrice - response.TotalDiscount;
        }
        private VoucherValidationStatusType VoucherDecrement(string voucherCode)
        {
            try
            {
                if (UpdateDb.VoucherDecrement(voucherCode))
                    return VoucherValidationStatusType.Success;
                else
                    return VoucherValidationStatusType.NoVoucherRemaining;
            }
            catch (Exception)
            {
                return VoucherValidationStatusType.UpdateError;
            }
        }
    }
}
