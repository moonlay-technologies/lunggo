using System.Runtime.InteropServices;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using System;
using Lunggo.ApCommon.Flight.Service;

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
            var response = new VoucherResponse();

            var voucher = GetDb.GetCampaignVoucher(voucherRequest.VoucherCode);
            response.Email = voucherRequest.Email;
            response.VoucherCode = voucherRequest.VoucherCode;
            response.CampaignVoucher = voucher;

            var validationStatus = ValidateVoucher(voucher, voucherRequest);

            if (validationStatus == VoucherStatus.Success)
            {
                CalculateVoucherDiscount(voucher, voucherRequest, response);
            }
            response.VoucherStatus = validationStatus;
            return response;
        }
        public VoucherResponse UseVoucherRequest(VoucherRequest voucherRequest)
        {
            var response = new VoucherResponse();

            var voucher = GetDb.GetCampaignVoucher(voucherRequest.VoucherCode);
            response.Email = voucherRequest.Email;
            response.VoucherCode = voucherRequest.VoucherCode;

            var validationStatus = ValidateVoucher(voucher, voucherRequest);

            if (validationStatus == VoucherStatus.Success)
            {
                CalculateVoucherDiscount(voucher, voucherRequest, response);
                validationStatus = VoucherDecrement(voucherRequest.VoucherCode);
                response.CampaignVoucher = voucher;
            }

            response.VoucherStatus = validationStatus;
            return response;
        }
        private VoucherStatus ValidateVoucher(CampaignVoucher voucher, VoucherRequest voucherRequest)
        {
            var price = GetFlightPrice(voucherRequest.Token);

            var currentTime = DateTime.Now;
            if (voucher == null)
                return VoucherStatus.VoucherNotFound;
            if (voucher.StartDate >= currentTime)
                return VoucherStatus.CampaignNotStartedYet;
            if (voucher.EndDate <= currentTime)
                return VoucherStatus.CampaignHasEnded;
            if (voucher.RemainingCount < 1)
                return VoucherStatus.NoVoucherRemaining;
            if (voucher.MinSpendValue > price)
                return VoucherStatus.BelowMinimumSpend;
            if (voucher.CampaignTypeCd == CampaignTypeCd.Mnemonic(CampaignType.Member))
            {
                if (!GetDb.IsMember(voucherRequest.Email))
                    return VoucherStatus.EmailNotEligible;
            }
            if (voucher.CampaignTypeCd == CampaignTypeCd.Mnemonic(CampaignType.Private))
            {
                if (!GetDb.IsEligibleForVoucher(voucherRequest.VoucherCode, voucherRequest.Email))
                    return VoucherStatus.EmailNotEligible;
            }
            if (voucher.CampaignStatus == false)
                return VoucherStatus.CampaignInactive;
            if (voucher.IsSingleUsage!=null && voucher.IsSingleUsage == true)
            {
                if (GetDb.CheckVoucherUsage(voucherRequest.VoucherCode, voucherRequest.Email) > 0)
                    return VoucherStatus.VoucherAlreadyUsed;
            }
            return VoucherStatus.Success;
        }

        private decimal GetFlightPrice(string token)
        {
            var flight = FlightService.GetInstance();
            var itin = flight.GetItineraryForDisplay(token);
            return itin.TotalFare;
        }

        private void CalculateVoucherDiscount(CampaignVoucher voucher, VoucherRequest voucherRequest, VoucherResponse response)
        {
            response.OriginalPrice = GetFlightPrice(voucherRequest.Token);;
            response.TotalDiscount = 0;

            if (voucher.ValuePercentage != null && voucher.ValuePercentage > 0)
                response.TotalDiscount += (response.OriginalPrice * (decimal)voucher.ValuePercentage / 100M);

            if (voucher.ValueConstant != null && voucher.ValueConstant > 0)
                response.TotalDiscount += (decimal)voucher.ValueConstant;

            if (voucher.MaxDiscountValue != null && voucher.MaxDiscountValue > 0
                && response.TotalDiscount > voucher.MaxDiscountValue)
                response.TotalDiscount = (decimal)voucher.MaxDiscountValue;

            response.TotalDiscount = Math.Floor(response.TotalDiscount);

            response.DiscountedPrice = response.OriginalPrice - response.TotalDiscount;
        }
        private VoucherStatus VoucherDecrement(string voucherCode)
        {
            try
            {
                if (UpdateDb.VoucherDecrement(voucherCode))
                    return VoucherStatus.Success;
                else
                    return VoucherStatus.NoVoucherRemaining;
            }
            catch (Exception)
            {
                return VoucherStatus.UpdateError;
            }
        }
        private VoucherStatus VoucherIncrement(string voucherCode)
        {
            try
            {
                if (UpdateDb.VoucherIncrement(voucherCode))
                    return VoucherStatus.Success;
                else
                    return VoucherStatus.UpdateError;
            }
            catch (Exception)
            {
                return VoucherStatus.UpdateError;
            }
        }
    }
}
