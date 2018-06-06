using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Lunggo.ApCommon.Account.Service;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Redis;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Product.Constant;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public VoucherDiscount GetVoucherDiscountForCart(string cartId, string voucherCode, out VoucherStatus status)
        {
            var trxPayment = GenerateTrxPaymentDetails(cartId);
            return GetVoucherDiscount(trxPayment, voucherCode, out status);
        }

        public VoucherDiscount GetVoucherDiscount(string rsvNo, string voucherCode, out VoucherStatus status)
        {
            var response = new VoucherDiscount();

            var campaign = _db.GetCampaignVoucher(voucherCode);

            if (campaign == null)
            {
                status = VoucherStatus.VoucherNotFound;
                return response;
            }

            if (campaign.ProductType != null && !campaign.ProductType.Contains(rsvNo[0]))
            {
                status = VoucherStatus.TermsConditionsNotEligible;
                return response;
            }

            //var contact = Contact.GetFromDb(rsvNo);
            //if (contact == null)
            //{
            //    status = VoucherStatus.InternalError;
            //    return response;
            //}

            //if (!_cache.IsPhoneAndEmailEligibleInCache(voucherCode, contact.CountryCallingCode + contact.Phone, contact.Email))
            //{
            //    status = VoucherStatus.EmailNotEligible;
            //    return discount;
            //}

            var paymentDetails = GetPaymentDetails(rsvNo);
            if (paymentDetails == null)
            {
                paymentDetails = GetPaymentDetails(rsvNo);
                if (paymentDetails == null)
                {
                    status = VoucherStatus.InternalError;
                    return response;
                }
            }

            var price = paymentDetails.OriginalPriceIdr * paymentDetails.LocalCurrency.Rate;

            var validationStatus = ValidateVoucher(campaign, price);

            if (validationStatus == VoucherStatus.Success)
            {
                response = CalculateVoucherDiscount(campaign, voucherCode, price);
            }

            //ReservationBase rsv;
            //if (rsvNo.StartsWith("1"))
            //    rsv = FlightService.GetInstance().GetReservation(rsvNo);
            //else
            //    rsv = HotelService.GetInstance().GetReservation(rsvNo);

            ReservationBase rsv;
            if (rsvNo.StartsWith("1"))
                rsv = FlightService.GetInstance().GetReservation(rsvNo);
            else if (rsvNo.StartsWith("2"))
                rsv = HotelService.GetInstance().GetReservation(rsvNo);
            else
                rsv = ActivityService.GetInstance().GetReservation(rsvNo);

            //var cost = rsv.GetTotalSupplierPrice();
            //if (campaign.MaxBudget.HasValue &&
            //    (campaign.MaxBudget - campaign.UsedBudget < cost - paymentDetails.FinalPriceIdr * 0.97M))

            //{
            //    status = VoucherStatus.VoucherDepleted;
            //    return discount;
            //}

            //////////////  HARDCODED VALIDATION /////////////////

            if (campaign.CampaignId == 66 || campaign.CampaignName == "Good Monday") // Good Monday
            {
                var valid = true;
                status = VoucherStatus.InternalError;

                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
                var platform = Client.GetPlatformType(clientId);
                if (platform == PlatformType.AndroidApp || platform == PlatformType.IosApp)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if ((rsv as FlightReservation).Itineraries.SelectMany(i => i.Trips)
                    .Any(t =>
                        FlightService.GetInstance().GetAirportCountryCode(t.OriginAirport) != "ID" ||
                        FlightService.GetInstance().GetAirportCountryCode(t.DestinationAirport) != "ID"))
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if (!(new[] { "JKT", "CGK", "HLP", "SRG", "JOG", "TNJ", "SUB" }
                    .Contains((rsv as FlightReservation).Itineraries[0].Trips[0].DestinationAirport)))
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                var airlines =
                    (rsv as FlightReservation).Itineraries.SelectMany(i => i.Trips)
                        .SelectMany(t => t.Segments)
                        .Select(s => s.AirlineCode);
                foreach (var airline in airlines)
                {
                    if (!(new[] { "QG", "SJ", "IN", "ID" }.Contains(airline)))
                    {
                        status = VoucherStatus.TermsConditionsNotEligible;
                        valid = false;
                    }
                }

                if (DateTime.UtcNow.AddHours(7).DayOfWeek != DayOfWeek.Monday)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if (!valid)
                {
                    return response;
                }
            }

            if (campaign.CampaignId == 71 || campaign.CampaignName == "Selasa Spesial") // Selasa Spesial
            {
                var valid = true;
                status = VoucherStatus.InternalError;

                if (DateTime.UtcNow.AddHours(7).DayOfWeek != DayOfWeek.Tuesday)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if (!valid)
                {
                    return response;
                }
            }

            if (campaign.CampaignId == 67 || campaign.CampaignName == "Promo Rabu") // Promo Rabu
            {
                var valid = true;
                status = VoucherStatus.InternalError;

                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
                var platform = Client.GetPlatformType(clientId);
                if (platform == PlatformType.AndroidApp || platform == PlatformType.IosApp)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if ((rsv as FlightReservation).Itineraries.SelectMany(i => i.Trips)
                    .Any(t =>
                        FlightService.GetInstance().GetAirportCountryCode(t.OriginAirport) != "ID" ||
                        FlightService.GetInstance().GetAirportCountryCode(t.DestinationAirport) != "ID"))
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                var airlines =
                    (rsv as FlightReservation).Itineraries.SelectMany(i => i.Trips)
                        .SelectMany(t => t.Segments)
                        .Select(s => s.AirlineCode);
                foreach (var airline in airlines)
                {
                    if (!(new[] { "SJ", "IN" }.Contains(airline)))
                    {
                        status = VoucherStatus.TermsConditionsNotEligible;
                        valid = false;
                    }
                }

                if (paymentDetails.Method != PaymentMethod.BankTransfer &&
                    paymentDetails.Method != PaymentMethod.VirtualAccount &&
                    paymentDetails.Method != PaymentMethod.Undefined)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }

                if (DateTime.UtcNow.AddHours(7).DayOfWeek != DayOfWeek.Wednesday)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if (!valid)
                {
                    return response;
                }
            }

            if (campaign.CampaignId == 68 || campaign.CampaignName == "Kamis Ceria") // Kamis Ceria
            {
                var valid = true;
                status = VoucherStatus.InternalError;

                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
                var platform = Client.GetPlatformType(clientId);
                if (platform == PlatformType.AndroidApp || platform == PlatformType.IosApp)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if ((rsv as FlightReservation).Itineraries.SelectMany(i => i.Trips)
                    .Any(t =>
                        FlightService.GetInstance().GetAirportCountryCode(t.OriginAirport) != "ID" ||
                        FlightService.GetInstance().GetAirportCountryCode(t.DestinationAirport) != "ID"))
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if (!(new[] { "DPS", "LOP", "LBJ", "BTH", "BTJ" }
                    .Contains((rsv as FlightReservation).Itineraries[0].Trips[0].DestinationAirport)))
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                var airlines =
                    (rsv as FlightReservation).Itineraries.SelectMany(i => i.Trips)
                        .SelectMany(t => t.Segments)
                        .Select(s => s.AirlineCode);
                foreach (var airline in airlines)
                {
                    if (!(new[] { "QG", "SJ", "IN", "ID" }.Contains(airline)))
                    {
                        status = VoucherStatus.TermsConditionsNotEligible;
                        valid = false;
                    }
                }


                if (DateTime.UtcNow.AddHours(7).DayOfWeek != DayOfWeek.Thursday)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if (!valid)
                {
                    return response;
                }
            }

            if (campaign.CampaignId == 72 || campaign.CampaignName == "Jumat Hemat") // Jumat Hemat
            {
                var valid = true;
                status = VoucherStatus.InternalError;

                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var userId = identity.Name == "anonymous" ? null : identity.GetUser().Id;
                var userEmail = identity.Name == "anonymous" ? null : identity.GetEmail();
                //var rsvs1 = FlightService.GetInstance()
                //    .GetOverviewReservationsByUserIdOrEmail(userId, contact.Email, null, null, null, null)
                //    .Where(r => r.Payment.Status == PaymentStatus.Settled);
                var rsvs2 = FlightService.GetInstance()
                    .GetOverviewReservationsByUserIdOrEmail(userId, userEmail, null, null, null, null)
                    .Where(r => r.Payment.Status == PaymentStatus.Settled);
                //if (!rsvs1.Concat(rsvs2).Any())
                //{
                //    status = VoucherStatus.TermsConditionsNotEligible;
                //    valid = false;
                //}

                if (DateTime.UtcNow.AddHours(7).DayOfWeek != DayOfWeek.Friday)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }

                if (!valid)
                {
                    return response;
                }
            }

            if (campaign.CampaignId == 69 || campaign.CampaignName == "Jalan-Jalan Sabtu") // Jalan-Jalan Sabtu
            {
                var valid = true;
                status = VoucherStatus.InternalError;

                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
                var platform = Client.GetPlatformType(clientId);
                if (platform == PlatformType.AndroidApp || platform == PlatformType.IosApp)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if ((rsv as FlightReservation).Itineraries.SelectMany(i => i.Trips)
                    .Any(t =>
                        FlightService.GetInstance().GetAirportCountryCode(t.OriginAirport) != "ID" ||
                        FlightService.GetInstance().GetAirportCountryCode(t.DestinationAirport) != "ID"))
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                var airlines =
                    (rsv as FlightReservation).Itineraries.SelectMany(i => i.Trips)
                        .SelectMany(t => t.Segments)
                        .Select(s => s.AirlineCode);
                foreach (var airline in airlines)
                {
                    if (!(new[] { "QG", "SJ", "IN", "ID" }.Contains(airline)))
                    {
                        status = VoucherStatus.TermsConditionsNotEligible;
                        valid = false;
                    }
                }


                if (paymentDetails.Method != PaymentMethod.BankTransfer &&
                    paymentDetails.Method != PaymentMethod.VirtualAccount &&
                    paymentDetails.Method != PaymentMethod.Undefined)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if (DateTime.UtcNow.AddHours(7).DayOfWeek != DayOfWeek.Saturday)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if (!valid)
                {
                    return response;
                }
            }

            if (campaign.CampaignId == 73 || campaign.CampaignName == "Sunday Funday") // Sunday Funday
            {
                var valid = true;
                status = VoucherStatus.InternalError;

                if (DateTime.UtcNow.AddHours(7).DayOfWeek != DayOfWeek.Sunday)
                {
                    status = VoucherStatus.TermsConditionsNotEligible;
                    valid = false;
                }


                if (!valid)
                {
                    return response;
                }
            }

            //////////////  HARDCODED VALIDATION /////////////////

            status = validationStatus;
            return response;
        }

        private VoucherDiscount GetVoucherDiscount(TrxPaymentDetails trxPayment, string voucherCode, out VoucherStatus status)
        {
            var campaign = _db.GetCampaignVoucher(voucherCode);

            status = ValidateVoucher(campaign, trxPayment.OriginalPriceIdr);
            if (status != VoucherStatus.Success)
                return null;

            var voucherDiscount = CalculateVoucherDiscount(campaign, voucherCode, trxPayment.OriginalPriceIdr);
            return voucherDiscount;
        }

        private bool RealizeVoucher(VoucherDiscount discount, AccountService accountService, PaymentDetails paymentDetails)
        {
            //var isUseBudgetSuccess = !rsvNo.StartsWith("2") || UseHotelBudget(voucherCode, rsvNo);
            var isVoucherDecrementSuccess = VoucherDecrement(discount.VoucherCode);
            if ( /*isUseBudgetSuccess && */isVoucherDecrementSuccess)
            {
                //var userId = ActivityService.GetInstance().GetReservationUserIdFromDb(paymentDetails.RsvNo);
                //accountService.UseReferralCredit(userId, paymentDetails.DiscountNominal);
                return true;
                //var contact = Contact.GetFromDb(rsvNo);
                //SavePhoneAndEmailInCache(voucherCode, contact.CountryCallingCode + contact.Phone, contact.Email);
            }
            else
                return false;

        }

        private VoucherStatus ValidateVoucher(CampaignVoucher voucher, decimal price)
        {
            var currentTime = DateTime.Now;
            if (voucher == null)
                return VoucherStatus.VoucherNotFound;
            if (voucher.StartDate >= currentTime)
                return VoucherStatus.OutsidePeriod;
            if (voucher.EndDate <= currentTime)
                return VoucherStatus.OutsidePeriod;
            if (voucher.RemainingCount < 1)
                return VoucherStatus.VoucherDepleted;
            if (voucher.MinSpendValue > price)
                return VoucherStatus.BelowMinimumSpend;
            //if (campaign.CampaignTypeCd == CampaignTypeCd.Mnemonic(CampaignType.Member))
            //{
            //    if (!_db.IsMember(email))
            //        return VoucherStatus.EmailNotEligible;
            //}
            //if (campaign.CampaignTypeCd == CampaignTypeCd.Mnemonic(CampaignType.Private))
            //{
            //    if (!_db.IsEligibleForVoucher(voucherCode, email))
            //        return VoucherStatus.EmailNotEligible;
            //}
            //if (campaign.CampaignStatus == false)
            //    return VoucherStatus.OutsidePeriod;
            //if (campaign.IsSingleUsage != null && campaign.IsSingleUsage == true)
            //{
            //    if (_db.CheckVoucherUsage(voucherCode, email) > 0)
            //        return VoucherStatus.VoucherAlreadyUsed;
            //}
            return VoucherStatus.Success;
        }

        private VoucherDiscount CalculateVoucherDiscount(CampaignVoucher campaign, string voucherCode, decimal price)
        {
            var discount = new VoucherDiscount();

            if (campaign.ValuePercentage > 0)
                discount.TotalDiscount += (price * campaign.ValuePercentage / 100M);

            if (campaign.ValueConstant > 0)
                discount.TotalDiscount += campaign.ValueConstant;

            if (discount.TotalDiscount > campaign.MaxDiscountValue)
            {
                discount.TotalDiscount = campaign.MaxDiscountValue;
            }

            discount.TotalDiscount = Math.Floor(discount.TotalDiscount);
            discount.DiscountedPrice = price - discount.TotalDiscount;

            if (discount.DiscountedPrice < 50000)
            {
                discount.DiscountedPrice = 50000M;
                discount.TotalDiscount = price - 50000M;
            }

            discount.CampaignId = campaign.CampaignId;
            discount.VoucherCode = voucherCode;
            discount.Discount = new UsedDiscount
            {
                Name = campaign.CampaignName,
                Description = campaign.CampaignDescription,
                DisplayName = campaign.DisplayName,
                Percentage = campaign.ValuePercentage,
                Constant = campaign.ValueConstant,
                Currency = new Currency("IDR"),
                IsFlat = false
            };

            return discount;
        }

        private bool VoucherDecrement(string voucherCode)
        {
            try
            {
                return _db.VoucherDecrement(voucherCode);
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
                return _db.UseHotelBudget(voucherCode, rsvNo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool TryApplyVoucher(string cartId, string discountCode, PaymentDetails paymentDetails,
            out VoucherDiscount discount)
        {
            discount = GetVoucherDiscount(cartId, discountCode, out var status);
            if (status != VoucherStatus.Success || discount.Discount == null)
            {
                paymentDetails.Status = PaymentStatus.Failed;
                paymentDetails.FailureReason = FailureReason.VoucherNoLongerAvailable;
                {
                    return false;
                }
            }

            if (discountCode == "REFERRALCREDIT")
            {
                var referral = AccountService.GetInstance().GetReferral(cartId);
                if (referral.ReferralCredit <= 0)
                {
                    paymentDetails.Status = PaymentStatus.Failed;
                    paymentDetails.FailureReason = FailureReason.VoucherNotEligible;
                    {
                        return false;
                    }
                }

                if (referral.ReferralCredit < discount.TotalDiscount)
                {
                    discount.TotalDiscount = referral.ReferralCredit;
                }
            }

            paymentDetails.FinalPriceIdr -= discount.TotalDiscount;
            paymentDetails.Discount = discount.Discount;
            paymentDetails.DiscountCode = discountCode;
            paymentDetails.DiscountNominal = discount.TotalDiscount;
            return true;
        }
    }
}

