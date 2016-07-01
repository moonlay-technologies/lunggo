using System.Runtime.InteropServices;
using System.Web;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using System;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Http;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        public CampaignVoucher GetCampaignVoucher(string voucherCode)
        {
            return GetDb.GetCampaignVoucher(voucherCode);
        }
        public VoucherResponse ValidateVoucherRequest(string rsvNo, string voucherCode)
        {
            var response = new VoucherResponse();
            var user = HttpContext.Current.User;

            var voucher = GetDb.GetCampaignVoucher(voucherCode);

            if (voucher == null)
            {
                response.VoucherStatus = VoucherStatus.VoucherNotFound;
                return response;
            }

            if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity.IsUserAuthorized())
                response.Email = HttpContext.Current.User.Identity.GetEmail();
            response.VoucherCode = voucherCode;
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

            var paymentDetails = PaymentDetails.GetFromDb(rsvNo);
            if (paymentDetails == null)
            {
                response.VoucherStatus = VoucherStatus.ReservationNotFound;
                return response;
            }

            var price = paymentDetails.OriginalPriceIdr*paymentDetails.LocalCurrency.Rate;

            var validationStatus = ValidateVoucher(voucher, price, user.Identity.GetEmail(), voucherCode);

            if (validationStatus == VoucherStatus.Success)
            {
                CalculateVoucherDiscount(voucher, price, response);
            }
            response.VoucherStatus = validationStatus;
            return response;
        }
        public VoucherResponse UseVoucherRequest(string rsvNo, string voucherCode)
        {
            var response = new VoucherResponse();

            var voucher = GetDb.GetCampaignVoucher(voucherCode);

            var reservation = FlightService.GetInstance().GetReservation(rsvNo);
            response.Email = reservation.Contact.Email;
            response.VoucherCode = voucherCode;

            var paymentDetails = reservation.Payment;
            var price = paymentDetails.OriginalPriceIdr * paymentDetails.LocalCurrency.Rate;

            var validationStatus = ValidateVoucher(voucher, reservation.Payment.FinalPriceIdr, reservation.Contact.Email, voucherCode);

            if (validationStatus == VoucherStatus.Success)
            {
                CalculateVoucherDiscount(voucher, price, response);
                validationStatus = VoucherDecrement(voucherCode);
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
            if (voucher.IsSingleUsage != null && voucher.IsSingleUsage == true)
            {
                if (GetDb.CheckVoucherUsage(voucherCode, email) > 0)
                    return VoucherStatus.VoucherAlreadyUsed;
            }
            return VoucherStatus.Success;
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
