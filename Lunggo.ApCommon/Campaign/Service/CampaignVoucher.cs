using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using System;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
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

            if (voucher.ProductType != null && !voucher.ProductType.Contains(rsvNo[0]))
            {
                response.VoucherStatus = VoucherStatus.ProductNotEligible;
                return response;
            }

            if (user.Identity.IsAuthenticated && user.Identity.IsUserAuthorized())
                response.Email = user.Identity.GetEmail();
            response.VoucherCode = voucherCode;

            var contact = Contact.GetFromDb(rsvNo);
            if (contact == null)
            {
                response.VoucherStatus = VoucherStatus.ReservationNotFound;
                return response;
            }

            if (!IsPhoneAndEmailEligibleInCache(voucherCode, contact.CountryCallingCode + contact.Phone, contact.Email))
            {
                response.VoucherStatus = VoucherStatus.EmailNotEligible;
                return response;
            }

            var paymentDetails = PaymentDetails.GetFromDb(rsvNo);
            if (paymentDetails == null)
            {
                response.VoucherStatus = VoucherStatus.ReservationNotFound;
                return response;
            }

            var price = paymentDetails.OriginalPriceIdr*paymentDetails.LocalCurrency.Rate;

            var validationStatus = ValidateVoucher(voucher, price, response.Email, voucherCode);

            if (validationStatus == VoucherStatus.Success)
            {
                CalculateVoucherDiscount(voucher, price, response);
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
            }

            ReservationBase rsv;
            if (rsvNo.StartsWith("1"))
                rsv = FlightService.GetInstance().GetReservation(rsvNo);
            else
                rsv = HotelService.GetInstance().GetReservation(rsvNo);

            var cost = rsv.GetTotalSupplierPrice();
            if (voucher.MaxBudget.HasValue &&
                (voucher.MaxBudget - voucher.UsedBudget < cost - paymentDetails.FinalPriceIdr*0.97M))
            {
                response.VoucherStatus = VoucherStatus.NoBudgetRemaining;
                return response;
            }

            response.VoucherStatus = validationStatus;
            return response;
        }

        public VoucherResponse UseVoucherRequest(string rsvNo, string voucherCode)
        {
            var response = ValidateVoucherRequest(rsvNo, voucherCode);
            if (response.VoucherStatus == VoucherStatus.Success)
            {
                var isUseBudgetSuccess = !rsvNo.StartsWith("2") || UseHotelBudget(voucherCode, rsvNo);
                var isVoucherDecrementSuccess = VoucherDecrement(voucherCode);
                if (isUseBudgetSuccess && isVoucherDecrementSuccess)
                {
                    response.VoucherStatus = VoucherStatus.Success;
                    var contact = Contact.GetFromDb(rsvNo);
                    SavePhoneAndEmailInCache(voucherCode, contact.CountryCallingCode + contact.Phone, contact.Email);
                }
                else
                    response.VoucherStatus = VoucherStatus.UpdateError;
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
            if (response.DiscountedPrice < 50000)
            {
                response.DiscountedPrice = 50000M;
            }
        }
        private bool VoucherDecrement(string voucherCode)
        {
            try
            {
                return UpdateDb.VoucherDecrement(voucherCode);
            }
            catch (Exception)
            {
                return false;
        }
        private bool VoucherDecrement(string voucherCode)
        {
            try
            {
                return UpdateDb.VoucherDecrement(voucherCode);
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool VoucherIncrement(string voucherCode)
        {
            try
            {
                return UpdateDb.VoucherIncrement(voucherCode);
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool UseHotelBudget(string voucherCode, string rsvNo)
        {
            try
            {
                return UpdateDb.UseHotelBudget(voucherCode, rsvNo);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
