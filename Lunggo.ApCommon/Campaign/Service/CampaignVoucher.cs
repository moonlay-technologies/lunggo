using System.Runtime.InteropServices;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using System;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        public CampaignVoucher GetCampaignVoucher(string voucherCode)
        {
            return GetDb.GetCampaignVoucher(voucherCode);
        }
        public VoucherResponse ValidateVoucherRequest(ValidateVoucherRequest request)
        {
            var response = new VoucherResponse();

            var voucher = GetDb.GetCampaignVoucher(request.VoucherCode);
            response.Email = request.Email;
            response.VoucherCode = request.VoucherCode;
            response.Discount = new UsedDiscount
            {
                Name = voucher.CampaignName,
                Description = voucher.CampaignDescription,
                DisplayName = voucher.DisplayName,
                Percentage = voucher.ValuePercentage.GetValueOrDefault(),
                Constant = voucher.ValueConstant.GetValueOrDefault(),
                Currency = new Currency("IDR"),
                IsFlat = false
            };

            var price = GetFlightPrice(request.Token);

            var validationStatus = ValidateVoucher(voucher, price, request.Email, request.VoucherCode);

            if (validationStatus == VoucherStatus.Success)
            {
                CalculateVoucherDiscount(voucher, price, response);
            }
            response.VoucherStatus = validationStatus;
            return response;
        }
        public VoucherResponse UseVoucherRequest(UseVoucherRequest request)
        {
            var response = new VoucherResponse();

            var voucher = GetDb.GetCampaignVoucher(request.VoucherCode);

            if (request.RsvNo.IsFlightRsvNo())
            {
                var reservation = FlightService.GetInstance().GetReservation(request.RsvNo);
                response.Email = reservation.Contact.Email;
                response.VoucherCode = request.VoucherCode;

                var validationStatus = ValidateVoucher(voucher, reservation.Payment.FinalPriceIdr, reservation.Contact.Email, request.VoucherCode);

                if (validationStatus == VoucherStatus.Success)
                {
                    CalculateVoucherDiscount(voucher, reservation.Payment.FinalPriceIdr, response);
                    validationStatus = VoucherDecrement(request.VoucherCode);
                    response.Discount = new UsedDiscount
                    {
                        Name = voucher.CampaignName,
                        Description = voucher.CampaignDescription,
                        DisplayName = voucher.DisplayName,
                        Percentage = voucher.ValuePercentage.GetValueOrDefault(),
                        Constant = voucher.ValueConstant.GetValueOrDefault(),
                        Currency = new Currency("IDR"),
                        IsFlat = false,
                    };
                }

                response.VoucherStatus = validationStatus;
            }
            return response;
        }
        private VoucherStatus ValidateVoucher(CampaignVoucher voucher, decimal price, string email, string voucherCode)
        {
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
                if (!GetDb.IsMember(email))
                    return VoucherStatus.EmailNotEligible;
            }
            if (voucher.CampaignTypeCd == CampaignTypeCd.Mnemonic(CampaignType.Private))
            {
                if (!GetDb.IsEligibleForVoucher(voucherCode, email))
                    return VoucherStatus.EmailNotEligible;
            }
            if (voucher.CampaignStatus == false)
                return VoucherStatus.CampaignInactive;
            if (voucher.IsSingleUsage!=null && voucher.IsSingleUsage == true)
            {
                if (GetDb.CheckVoucherUsage(voucherCode, email) > 0)
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

        private void CalculateVoucherDiscount(CampaignVoucher voucher, decimal price, VoucherResponse response)
        {
            response.OriginalPrice = price;
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
