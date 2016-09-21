using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;

using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper;
using Lunggo.ApCommon.Sequence;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        internal void InitPriceMarginRules()
        {
            PullPriceMarginRulesFromDatabaseToCache();
        }

        internal void AddPriceMargin(List<FlightItinerary> itins)
        {
            if (!itins.Any())
                return;

            var rules = GetAllActiveMarginRulesFromCache();
            foreach (var itin in itins)
            {
                AddPriceMargin(itin, rules);
            }
        }

        internal void AddPriceMargin(FlightItinerary itin, List<FlightMarginRule> marginRules)
        {
            var rule = GetFirstMatchingRule(itin, marginRules);
            ApplyMarginRule(itin, rule);
        }

        private void RoundFinalAndLocalPrice(FlightItinerary itin)
        {
            var initLocalPrice = itin.Price.Local;
            var adultCount = itin.AdultCount;
            var childCount = itin.ChildCount;
            var infantCount = itin.InfantCount;
            var initAdultPortion = itin.AdultPricePortion;
            var initChildPortion = itin.ChildPricePortion;
            var initInfantPortion = itin.InfantPricePortion;
            var roundingOrder = itin.Price.LocalCurrency.RoundingOrder;

            var adultAdjustment = adultCount != 0
                ? (initLocalPrice * initAdultPortion / adultCount) % roundingOrder * adultCount
                : 0M;
            var childAdjustment = childCount != 0
                ? roundingOrder - (initLocalPrice * initChildPortion / childCount) % roundingOrder * childCount
                : 0M;
            var infantAdjustment = infantCount != 0
                ? roundingOrder - (initLocalPrice * initInfantPortion / infantCount) % roundingOrder * infantCount
                : 0M;
            var adjustment = -adultAdjustment + childAdjustment + infantAdjustment;

            var initAdultPrice = initAdultPortion * initLocalPrice;
            var adultPrice = initAdultPrice - adultAdjustment;
            var initChildPrice = initChildPortion * initLocalPrice;
            var childPrice = initChildPrice + childAdjustment;
            var initInfantPrice = initInfantPortion * initLocalPrice;
            var infantPrice = initInfantPrice + infantAdjustment;

            itin.Price.Local += adjustment;
            itin.Price.Rounding += adjustment;
            itin.Price.FinalIdr = itin.Price.Local * itin.Price.LocalCurrency.Rate;
            itin.NetAdultPricePortion = adultPrice / itin.Price.Local;
            itin.NetChildPricePortion = childPrice / itin.Price.Local;
            itin.NetInfantPricePortion = infantPrice / itin.Price.Local;
        }

        //public FlightMarginRule GetPriceMarginRule(long ruleId)
        //{
        //    var rules = GetAllActiveMarginRulesFromCache();
        //    return rules.Single(rule => rule.RuleId == ruleId);
        //}

        //public List<MarginRule> GetAllPriceMarginRules()
        //{
        //    var rules = GetAllActiveMarginRulesFromCache();
        //    rules =
        //        rules.OrderByDescending(rule => rule.ConstraintCount).ThenBy(rule => rule.Priority).ToList();
        //    return rules;
        //}

        //public List<MarginRule> InsertPriceMarginRuleAndRetrieveConflict(MarginRule newRule)
        //{
        //    AssignRuleId(newRule);
        //    InsertMarginRule(newRule);
        //    var conflictingRules = RetrieveConflict(newRule);
        //    return conflictingRules;
        //}

        //public void DeletePriceMarginRule(long ruleId)
        //{
        //    var rules = GetActiveMarginRulesFromBufferCache();
        //    var deletedRules = GetDeletedMarginRulesFromBufferCache();
        //    var obsoleteRule = rules.Single(rule => rule.RuleId == ruleId);
        //    rules.Remove(obsoleteRule);
        //    deletedRules.Add(obsoleteRule);
        //    SaveActiveMarginRulesInBufferCache(rules);
        //    SaveDeletedMarginRulesInBufferCache(deletedRules);
        //    PushPriceMarginRulesFromCacheBufferToDatabase();
        //}

        //public List<MarginRule> UpdatePriceMarginRuleAndRetrieveConflict(MarginRule updatedRule)
        //{
        //    DeletePriceMarginRule(updatedRule.RuleId);
        //    return InsertPriceMarginRuleAndRetrieveConflict(updatedRule);
        //}

        //public void UpdateResolvedPriceMarginRulesConflict(List<MarginRule> updatedRules)
        //{
        //    UpdateConflictingPriceMarginRules(updatedRules);
        //}

        public void PullPriceMarginRulesFromDatabaseToCache()
        {
            var dbRules = GetActivePriceMarginRulesFromDb();
            SaveActiveMarginRulesToCache(dbRules);
            SaveActiveMarginRulesInBufferCache(dbRules);
            SaveDeletedMarginRulesInBufferCache(new List<FlightMarginRule>());
        }

        public void PushPriceMarginRulesFromCacheBufferToDatabase()
        {
            var rules = GetActiveMarginRulesFromBufferCache();
            var deletedRules = GetDeletedMarginRulesFromBufferCache();
            InsertPriceMarginRulesToDb(rules, deletedRules);
            PullPriceMarginRulesFromDatabaseToCache();
        }

        #region HelperMethods

        //private static void AssignRuleId(MarginRule newRule)
        //{
        //    newRule.RuleId = FlightPriceMarginRuleIdSequence.GetInstance().GetNext();
        //}

        //private void UpdateConflictingPriceMarginRules(List<MarginRule> updatedRules)
        //{
        //    var rules = GetActiveMarginRulesFromBufferCache();
        //    var constraintCount = updatedRules.First().ConstraintCount;
        //    var updatedRulesCount = updatedRules.Count();
        //    var orderedUpdatedRules = updatedRules.OrderBy(rule => rule.Priority);
        //    var targetIndex = rules.FindIndex(rule => rule.ConstraintCount == constraintCount);
        //    rules.RemoveRange(targetIndex, updatedRulesCount);
        //    rules.InsertRange(targetIndex, orderedUpdatedRules);
        //    SaveActiveMarginRulesInBufferCache(rules);
        //    PushPriceMarginRulesFromCacheBufferToDatabase();
        //}

        //private List<MarginRule> RetrieveConflict(MarginRule newRule)
        //{
        //    var rules = GetActiveMarginRulesFromBufferCache();
        //    rules = rules.Where(rule => rule.ConstraintCount == newRule.ConstraintCount)
        //            .OrderByDescending(rule => rule.Priority)
        //            .ToList();
        //    return rules;
        //}

        //private void InsertMarginRule(MarginRule newRule)
        //{
        //    AssignConstraintCount(newRule);
        //    var rules = GetAllActiveMarginRulesFromCache();
        //    var index = rules.FindLastIndex(rule => rule.ConstraintCount > newRule.ConstraintCount);
        //    rules.Insert(index + 1, newRule);
        //    SaveActiveMarginRulesInBufferCache(rules);
        //}

        //private void AssignConstraintCount(MarginRule newRule)
        //{
        //    var count = 0;
        //    if (newRule.Airlines.Any())
        //        count++;
        //    if (newRule.AirportPairs.Any())
        //        count++;
        //    if (newRule.CityPairs.Any())
        //        count++;
        //    if (newRule.CountryPairs.Any())
        //        count++;
        //    if (newRule.BookingDateSpans.Any())
        //        count++;
        //    if (newRule.BookingDays.Any())
        //        count++;
        //    if (newRule.BookingDates.Any())
        //        count++;
        //    if (newRule.DepartureDateSpans.Any())
        //        count++;
        //    if (newRule.DepartureDays.Any())
        //        count++;
        //    if (newRule.DepartureDates.Any())
        //        count++;
        //    if (newRule.DepartureTimeSpans.Any())
        //        count++;
        //    if (newRule.ReturnDateSpans.Any())
        //        count++;
        //    if (newRule.ReturnDays.Any())
        //        count++;
        //    if (newRule.ReturnDates.Any())
        //        count++;
        //    if (newRule.ReturnTimeSpans.Any())
        //        count++;
        //    if (newRule.CabinClasses.Any())
        //        count++;
        //    if (newRule.TripTypes.Any())
        //        count++;
        //    if (newRule.FareTypes.Any())
        //        count++;
        //    newRule.ConstraintCount = count;
        //}

        private static void ApplyMarginRule(FlightItinerary itin, FlightMarginRule marginRule)
        {
            itin.Price.SetMargin(marginRule.Margin);
        }

        private FlightMarginRule GetFirstMatchingRule(FlightItinerary itin, List<FlightMarginRule> rules)
        {
           foreach (var marginRule in rules)
            {
                var rule = marginRule.Rule;
                if (!BookingDateMatches(rule)) continue;
                if (!FareTypeMatches(rule, itin)) continue;
                if (!CabinClassMatches(rule, itin)) continue;
                if (!AirlineTypeMatches(rule, itin)) continue;
                if (!TripTypeMatches(rule, itin)) continue;
                if (!FlightTimeMatches(rule, itin)) continue;
                if (!PassengerCountMatches(rule, itin)) continue;
                if (!AirlineMatches(rule, itin)) continue;
                if (!AirportPairMatches(rule, itin)) continue;
                if (!CityPairMatches(rule, itin)) continue;
                if (!CountryPairMatches(rule, itin)) continue;
                return marginRule;
            }
            return null;
        }

        private static bool BookingDateMatches(FlightItineraryRule rule)
        {
            var nowDate = DateTime.UtcNow.Date;
            var dateSpanOk = !rule.BookingDateSpans.Any() || rule.BookingDateSpans.Any(dateSpan => dateSpan.Contains(nowDate));
            var dayOk = !rule.BookingDays.Any() || rule.BookingDays.Contains(nowDate.DayOfWeek);
            var dateOk = !rule.BookingDates.Any() || rule.BookingDates.Contains(nowDate);
            return dateSpanOk && dayOk && dateOk;
        }

        private static bool FlightTimeMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            var departureOk = DepartureTimeMatches(rule, fare);
            var returnOk = ReturnTimeMatches(rule, fare);
            return departureOk && returnOk;
        }

        private static bool DepartureTimeMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            var dates = fare.Trips.First().Segments.Select(segment => segment.DepartureTime.Date).ToList();
            var times = fare.Trips.First().Segments.Select(segment => segment.DepartureTime.TimeOfDay).ToList();
            var dateSpanOk = !rule.DepartureDateSpans.Any() || dates.All(date => rule.DepartureDateSpans.Any(dateSpan => dateSpan.Contains(date)));
            var dayOk = !rule.DepartureDays.Any() || dates.All(date => rule.DepartureDays.Contains(date.DayOfWeek));
            var dateOk = !rule.DepartureDates.Any() || dates.All(date => rule.DepartureDates.Contains(date));
            var timeSpanOk = !rule.DepartureTimeSpans.Any() || times.All(time => rule.DepartureTimeSpans.Any(timeSpan => timeSpan.Contains(time)));
            return dateSpanOk && dayOk && dateOk && timeSpanOk;
        }

        private static bool ReturnTimeMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            if (fare.TripType != TripType.RoundTrip)
                return true;
            else
            {
                var dates = fare.Trips.Last().Segments.Select(segment => segment.DepartureTime.Date).ToList();
                var times = fare.Trips.Last().Segments.Select(segment => segment.DepartureTime.TimeOfDay).ToList();
                var dateSpanOk = !rule.ReturnDateSpans.Any() || dates.All(date => rule.ReturnDateSpans.Any(dateSpan => dateSpan.Contains(date)));
                var dayOk = !rule.ReturnDays.Any() || dates.All(date => rule.ReturnDays.Contains(date.DayOfWeek));
                var dateOk = !rule.ReturnDates.Any() || dates.All(date => rule.ReturnDates.Contains(date));
                var timeSpanOk = !rule.ReturnTimeSpans.Any() || times.All(time => rule.ReturnTimeSpans.Any(timeSpan => timeSpan.Contains(time)));
                return dateSpanOk && dayOk && dateOk && timeSpanOk;
            }
        }

        private static bool TripTypeMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            return !rule.TripTypes.Any() || rule.TripTypes.Contains(fare.RequestedTripType);
        }

        private static bool FareTypeMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            return !rule.FareTypes.Any() || rule.FareTypes.Contains(fare.FareType);
        }

        private static bool CabinClassMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            return !rule.CabinClasses.Any() || rule.CabinClasses.Contains(fare.RequestedCabinClass);
        }

        private static bool AirlineTypeMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            var airlineTypes = fare.Trips.SelectMany(trip => trip.Segments)
                .Select(segment => segment.AirlineType);

            return !rule.AirlineTypes.Any() || airlineTypes.All(airlineType => rule.AirlineTypes.Contains(airlineType));
        }

        private static bool PassengerCountMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            var totalPassenger = fare.AdultCount + fare.ChildCount + fare.InfantCount;
            return totalPassenger >= rule.MinPassengers && totalPassenger <= rule.MaxPassengers;
        }

        private static bool AirlineMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            var airlines = fare.Trips.SelectMany(trip => trip.Segments)
                .Select(segment => segment.AirlineCode);

            if (rule.Airlines.Any())
                return rule.AirlinesIsExclusion
                    ? airlines.All(airline => !rule.Airlines.Contains(airline))
                    : airlines.All(airline => rule.Airlines.Contains(airline));
            else
                return true;
        }

        private static bool AirportPairMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            var farePairs = fare.Trips.Select(trip => new AirportPairRule
            {
                Origin = trip.OriginAirport,
                Destination = trip.DestinationAirport
            });
            var returnPair = new AirportPairRule
            {
                Origin = fare.Trips.First().OriginAirport,
                Destination = fare.Trips.First().DestinationAirport
            };

            if (rule.AirportPairs.Any())
            {
                if (rule.TripTypes.Contains(TripType.RoundTrip) && fare.TripType == TripType.RoundTrip)
                    return rule.AirportPairsIsExclusion
                        ? !rule.AirportPairs.Contains(returnPair)
                        : rule.AirportPairs.Contains(returnPair);
                else
                    return rule.AirportPairsIsExclusion
                        ? farePairs.All(pair => !rule.AirportPairs.Contains(pair))
                        : farePairs.All(pair => rule.AirportPairs.Contains(pair));
            }
            else
                return true;
        }

        private bool CityPairMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            var farePairs = fare.Trips.Select(trip => new AirportPairRule
            {
                Origin = GetAirportCityCode(trip.OriginAirport),
                Destination = GetAirportCityCode(trip.DestinationAirport)
            });
            var returnPair = new AirportPairRule
            {
                Origin = GetAirportCityCode(fare.Trips.First().OriginAirport),
                Destination = GetAirportCityCode(fare.Trips.First().DestinationAirport)
            };

            if (rule.CityPairs.Any())
            {
                if (rule.TripTypes.Contains(TripType.RoundTrip) && fare.TripType == TripType.RoundTrip)
                    return rule.CityPairsIsExclusion
                        ? !rule.CityPairs.Contains(returnPair)
                        : rule.CityPairs.Contains(returnPair);
                else
                    return rule.CityPairsIsExclusion
                        ? farePairs.All(pair => !rule.CityPairs.Contains(pair))
                        : farePairs.All(pair => rule.CityPairs.Contains(pair));
            }
            else
                return true;
        }

        private bool CountryPairMatches(FlightItineraryRule rule, FlightItinerary fare)
        {
            var farePairs = fare.Trips.Select(trip => new AirportPairRule
            {
                Origin = GetAirportCountryCode(trip.OriginAirport),
                Destination = GetAirportCountryCode(trip.DestinationAirport)
            });
            var returnPair = new AirportPairRule
            {
                Origin = GetAirportCountryCode(fare.Trips.First().OriginAirport),
                Destination = GetAirportCountryCode(fare.Trips.First().DestinationAirport)
            };

            if (rule.CountryPairs.Any())
            {
                if (rule.TripTypes.Contains(TripType.RoundTrip) && fare.TripType == TripType.RoundTrip)
                    return rule.CountryPairsIsExclusion
                        ? !rule.CountryPairs.Contains(returnPair)
                        : rule.CountryPairs.Contains(returnPair);
                else
                    return rule.CountryPairsIsExclusion
                        ? farePairs.All(pair => !rule.CountryPairs.Contains(pair))
                        : farePairs.All(pair => rule.CountryPairs.Contains(pair));
            }
            else
                return true;
        }

        #endregion

    }
}





