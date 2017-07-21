using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Context;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void InitPriceMarginRules()
        {
            PullPriceMarginRulesFromDatabaseToCache();
        }

        internal void AddPriceMargin(List<HotelDetail> hotelDetails)
        {
            var localCurr = new Currency(OnlineContext.GetActiveCurrencyCode());
            AddPriceMargin(hotelDetails, localCurr);
        }

        internal void AddPriceMargin(List<HotelDetail> hotelDetails, Currency localCurrency)
        {
            if (hotelDetails == null || !hotelDetails.Any())
            {
                return;
            }

            var rules = GetAllActiveMarginRulesFromCache();
            foreach (var hotelDetail in hotelDetails)
            {
                AddPriceMargin(hotelDetail, rules);
            }
            
            hotelDetails.SelectMany(h => h.Rooms).SelectMany(r => r.Rates).ToList().ForEach(r => r.Price.CalculateFinalAndLocal(localCurrency));
        }

        internal void AddPriceMargin(HotelDetail hotelDetail, List<HotelMarginRule> marginRules)
        {
            if (!hotelDetail.Rooms.Any())
            {
                return;
            }

            foreach (var room in hotelDetail.Rooms)
            {
                AddPriceMargin(room, hotelDetail, marginRules);
            }
        }

        internal void AddPriceMargin(HotelRoom room, HotelDetail hotelDetail, List<HotelMarginRule> marginRules)
        {
            if (!room.Rates.Any())
            {
                return;
            }

            foreach (var rate in room.Rates)
            {
                
                AddPriceMargin(rate, room, hotelDetail, marginRules);
            }
        }

        internal void AddPriceMargin(HotelRate hotelRate, HotelRoom room, HotelDetail hotelDetail, List<HotelMarginRule> marginRules)
        {
            if (hotelRate.HotelSellingRate > 0)
            {
                hotelRate.Price.SetMargin(new Margin
                {
                    Constant = hotelRate.HotelSellingRate - hotelRate.Price.Supplier,
                    Currency = hotelRate.Price.SupplierCurrency,
                    Name = "HotelSellingRate"
                });
            }
            else
            {
                var rule = GetFirstMatchingRule(hotelRate, room, hotelDetail, marginRules);
                ApplyMarginRule(hotelRate, rule);                
            }

        }

        //private void RoundFinalAndLocalPrice(HotelRate rate)
        //{
        //    var initLocalPrice = rate.Price.Local;
        //    var adultCount = rate.AdultCount;
        //    var childCount = rate.ChildCount;;
        //    var initAdultPortion = rate.Price.AdultPricePortion;
        //    var initChildPortion = rate.ChildPricePortion;
        //    var roundingOrder = rate.Price.LocalCurrency.RoundingOrder;

        //    var adultAdjustment = adultCount != 0
        //        ? (initLocalPrice * initAdultPortion / adultCount) % roundingOrder * adultCount
        //        : 0M;
        //    var childAdjustment = childCount != 0
        //        ? roundingOrder - (initLocalPrice * initChildPortion / childCount) % roundingOrder * childCount
        //        : 0M;
        //    var infantAdjustment = infantCount != 0
        //        ? roundingOrder - (initLocalPrice * initInfantPortion / infantCount) % roundingOrder * infantCount
        //        : 0M;
        //    var adjustment = -adultAdjustment + childAdjustment + infantAdjustment;

        //    var initAdultPrice = initAdultPortion * initLocalPrice;
        //    var adultPrice = initAdultPrice - adultAdjustment;
        //    var initChildPrice = initChildPortion * initLocalPrice;
        //    var childPrice = initChildPrice + childAdjustment;
        //    var initInfantPrice = initInfantPortion * initLocalPrice;
        //    var infantPrice = initInfantPrice + infantAdjustment;

        //    rate.Price.Local += adjustment;
        //    rate.Price.Rounding += adjustment;
        //    rate.Price.FinalIdr = rate.Price.Local * rate.Price.LocalCurrency.Rate;
        //    rate.NetAdultPricePortion = adultPrice / rate.Price.Local;
        //    rate.NetChildPricePortion = childPrice / rate.Price.Local;
        //    rate.NetInfantPricePortion = infantPrice / rate.Price.Local;
        //}

//        
        public void PullPriceMarginRulesFromDatabaseToCache()
        {
            var dbRules = GetActivePriceMarginRulesFromDb();
            SaveActiveMarginRulesToCache(dbRules);
            SaveActiveMarginRulesInBufferCache(dbRules);
            SaveDeletedMarginRulesInBufferCache(new List<HotelMarginRule>());
        }

        public void PushPriceMarginRulesFromCacheBufferToDatabase()
        {
            var rules = GetActiveMarginRulesFromBufferCache();
            var deletedRules = GetDeletedMarginRulesFromBufferCache();
            InsertPriceMarginRulesToDb(rules, deletedRules);
            PullPriceMarginRulesFromDatabaseToCache();
        }

        #region HelperMethods

        private static void ApplyMarginRule(HotelRate rate, HotelMarginRule marginRule)
        {
            rate.Price.SetMargin(marginRule.Margin);
        }

        private HotelMarginRule GetFirstMatchingRule(HotelRate rate, HotelRoom room, HotelDetail hotelDetail, List<HotelMarginRule> rules)
        {
            foreach (var marginRule in rules)
            {
                var rule = marginRule.Rule;
                if (!BookingDateMatches(rule)) continue;
                if (!StayDurationMatches(rule, rate)) continue;
                if (!StayDateMatches(rule, rate)) continue;
                if (!HotelStarMatches(rule, hotelDetail)) continue;
                if (!CountryMatches(rule, hotelDetail)) continue;
                if (!DestinationMatches(rule, hotelDetail)) continue;
                if (!BoardMatches(rule, rate)) continue;
                if (!RoomTypeMatches(rule, room)) continue;
                if (!PaxMatches(rule, rate)) continue;
                return marginRule;
            }
            return null;
        }
        
        private static bool BookingDateMatches(HotelRateRule rule)
        {
            var nowDate = DateTime.UtcNow.Date;
            var dateSpanOk = !rule.BookingDates.Any() || rule.BookingDates.Any(dateSpan => dateSpan.Contains(nowDate));
            return dateSpanOk;
        }

        private static bool StayDateMatches(HotelRateRule rule, HotelRate hotelRate)
        {
            var cidate = hotelRate.RateKey.Split('|')[0];
            var cekin = new DateTime(Convert.ToInt32(cidate.Substring(0, 4)), Convert.ToInt32(cidate.Substring(4, 2)),
                Convert.ToInt32(cidate.Substring(6, 2)));
            var dateSpanOk = !rule.StayDates.Any() || rule.StayDates.Any(dateSpan => dateSpan.Contains(cekin));
            return dateSpanOk;
        }

        private static bool StayDurationMatches(HotelRateRule rule, HotelRate hotelRate)
        {
            var cidate = hotelRate.RateKey.Split('|')[0];
            var codate = hotelRate.RateKey.Split('|')[1];
            var cekin = new DateTime(Convert.ToInt32(cidate.Substring(0, 4)), Convert.ToInt32(cidate.Substring(4, 2)),
                Convert.ToInt32(cidate.Substring(6, 2)));
            var cekout = new DateTime(Convert.ToInt32(codate.Substring(0, 4)), Convert.ToInt32(codate.Substring(4, 2)),
                Convert.ToInt32(codate.Substring(6, 2)));
            var stayduration = (cekout - cekin).Days;
            var stayDurOk = !rule.StayDurations.Any() || rule.StayDurations.Contains(stayduration);
            return stayDurOk;
        }

        private static bool HotelStarMatches(HotelRateRule rule, HotelDetail hotelDetail)
        {
            var rating = hotelDetail.StarRating;
            var ratingOk = !rule.HotelStars.Any() || rule.HotelStars.Contains(rating);
            return ratingOk;
        }

        private static bool CountryMatches(HotelRateRule rule, HotelDetail hotelDetail)
        {
            var countryCd = hotelDetail.CountryCode;
            var countryOk = !rule.Countries.Any() || rule.Countries.Contains(countryCd);
            return countryOk;
        }

        private static bool DestinationMatches(HotelRateRule rule, HotelDetail hotelDetail)
        {
            var destCd = hotelDetail.DestinationCode;
            var destOk = !rule.Destinations.Any() || rule.Destinations.Contains(destCd);
            return destOk;
        }

        private static bool BoardMatches(HotelRateRule rule, HotelRate rate)
        {
            var board = rate.Board;
            var boardOk = !rule.Boards.Any() || rule.Boards.Contains(board);
            return boardOk;
        }

        private static bool RoomTypeMatches(HotelRateRule rule, HotelRoom room)
        {
            var roomtype = room.Type;
            var roomTypeOk = !rule.RoomTypes.Any() || rule.RoomTypes.Contains(roomtype);
            return roomTypeOk;
        }

        private static bool PaxMatches(HotelRateRule rule, HotelRate rate)
        {
            var childCt = rate.ChildCount;
            var adultCt = rate.AdultCount;
            return rule.MinAdult <= adultCt && rule.MaxAdult >= adultCt && rule.MinChild <= childCt && rule.MaxChild >= childCt;
        }

       #endregion

    }
}





