using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Currency.Service;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;

using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper;
using Lunggo.ApCommon.Sequence;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private const decimal RoundingOrder = 100M;

        internal void InitPriceMarginRules()
        {
            PullPriceMarginRulesFromDatabaseToCache();
        }

        internal void AddPriceMargin(List<FlightItinerary> itins)
        {
            if (!itins.Any())
                return;

            var rules = GetAllActiveMarginRulesFromCache();
            var currency = CurrencyService.GetInstance();
            var supplierRate = currency.GetSupplierExchangeRate(itins[0].Supplier);
            foreach (var itin in itins)
            {
                itin.SupplierRate = supplierRate;
                itin.OriginalIdrPrice = itin.SupplierPrice * itin.SupplierRate;
                AddPriceMargin(itin, rules);
                itin.LocalCurrency = "IDR";
                itin.LocalRate = 1;
                itin.LocalPrice = itin.FinalIdrPrice * itin.LocalRate;
            }
        }

        internal void AddPriceMargin(FlightItinerary itin, List<MarginRule> rules)
        {
            var rule = GetFirstMatchingRule(itin, rules);   
            ApplyMarginRule(itin, rule);
        }

        public MarginRule GetPriceMarginRule(long ruleId)
        {
            var rules = GetAllActiveMarginRulesFromCache();
            return rules.Single(rule => rule.RuleId == ruleId);
        }

        public List<MarginRule> GetAllPriceMarginRules()
        {
            var rules = GetAllActiveMarginRulesFromCache();
            rules =
                rules.OrderByDescending(rule => rule.ConstraintCount).ThenBy(rule => rule.Priority).ToList();
            return rules;
        }

        public List<MarginRule> InsertPriceMarginRuleAndRetrieveConflict(MarginRule newRule)
        {
            AssignRuleId(newRule);
            InsertMarginRule(newRule);
            var conflictingRules = RetrieveConflict(newRule);
            return conflictingRules;
        }

        public void DeletePriceMarginRule(long ruleId)
        {
            var rules = GetActiveMarginRulesFromBufferCache();
            var deletedRules = GetDeletedMarginRulesFromBufferCache();
            var obsoleteRule = rules.Single(rule => rule.RuleId == ruleId);
            rules.Remove(obsoleteRule);
            deletedRules.Add(obsoleteRule);
            SaveActiveMarginRulesInBufferCache(rules);
            SaveDeletedMarginRulesInBufferCache(deletedRules);
            PushPriceMarginRulesFromCacheBufferToDatabase();
        }

        public List<MarginRule> UpdatePriceMarginRuleAndRetrieveConflict(MarginRule updatedRule)
        {
            DeletePriceMarginRule(updatedRule.RuleId);
            return InsertPriceMarginRuleAndRetrieveConflict(updatedRule);
        }

        public void UpdateResolvedPriceMarginRulesConflict(List<MarginRule> updatedRules)
        {
            UpdateConflictingPriceMarginRules(updatedRules);
        }

        public void PullPriceMarginRulesFromDatabaseToCache()
        {
            var dbRules = GetDb.ActivePriceMarginRules();
            SaveActiveMarginRulesToCache(dbRules);
            SaveActiveMarginRulesInBufferCache(dbRules);
            SaveDeletedMarginRulesInBufferCache(new List<MarginRule>());
        }

        public void PushPriceMarginRulesFromCacheBufferToDatabase()
        {
            var rules = GetActiveMarginRulesFromBufferCache();
            var deletedRules = GetDeletedMarginRulesFromBufferCache();
            InsertDb.PriceMarginRules(rules, deletedRules);
            PullPriceMarginRulesFromDatabaseToCache();
        }

        #region HelperMethods

        private static void AssignRuleId(MarginRule newRule)
        {
            newRule.RuleId = FlightPriceMarginRuleIdSequence.GetInstance().GetNext();
        }

        private void UpdateConflictingPriceMarginRules(List<MarginRule> updatedRules)
        {
            var rules = GetActiveMarginRulesFromBufferCache();
            var constraintCount = updatedRules.First().ConstraintCount;
            var updatedRulesCount = updatedRules.Count();
            var orderedUpdatedRules = updatedRules.OrderBy(rule => rule.Priority);
            var targetIndex = rules.FindIndex(rule => rule.ConstraintCount == constraintCount);
            rules.RemoveRange(targetIndex, updatedRulesCount);
            rules.InsertRange(targetIndex, orderedUpdatedRules);
            SaveActiveMarginRulesInBufferCache(rules);
            PushPriceMarginRulesFromCacheBufferToDatabase();
        }

        private List<MarginRule> RetrieveConflict(MarginRule newRule)
        {
            var rules = GetActiveMarginRulesFromBufferCache();
            rules = rules.Where(rule => rule.ConstraintCount == newRule.ConstraintCount)
                    .OrderByDescending(rule => rule.Priority)
                    .ToList();
            return rules;
        }

        private void InsertMarginRule(MarginRule newRule)
        {
            AssignConstraintCount(newRule);
            var rules = GetAllActiveMarginRulesFromCache();
            var index = rules.FindLastIndex(rule => rule.ConstraintCount > newRule.ConstraintCount);
            rules.Insert(index + 1, newRule);
            SaveActiveMarginRulesInBufferCache(rules);
        }

        private void AssignConstraintCount(MarginRule newRule)
        {
            var count = 0;
            if (newRule.Airlines.Any())
                count++;
            if (newRule.AirportPairs.Any())
                count++;
            if (newRule.CityPairs.Any())
                count++;
            if (newRule.CountryPairs.Any())
                count++;
            if (newRule.BookingDateSpans.Any())
                count++;
            if (newRule.BookingDays.Any())
                count++;
            if (newRule.BookingDates.Any())
                count++;
            if (newRule.DepartureDateSpans.Any())
                count++;
            if (newRule.DepartureDays.Any())
                count++;
            if (newRule.DepartureDates.Any())
                count++;
            if (newRule.DepartureTimeSpans.Any())
                count++;
            if (newRule.ReturnDateSpans.Any())
                count++;
            if (newRule.ReturnDays.Any())
                count++;
            if (newRule.ReturnDates.Any())
                count++;
            if (newRule.ReturnTimeSpans.Any())
                count++;
            if (newRule.CabinClasses.Any())
                count++;
            if (newRule.TripTypes.Any())
                count++;
            if (newRule.FareTypes.Any())
                count++;
            newRule.ConstraintCount = count;
        }

        private static void ApplyMarginRule(FlightItinerary fare, MarginRule rule)
        {
            fare.MarginId = rule.RuleId;
            fare.MarginIsFlat = rule.IsFlat;
            if (rule.IsFlat)
            {
                fare.FinalIdrPrice = rule.Constant;
                fare.MarginCoefficient = 0M;
                fare.MarginConstant = rule.Constant;
                fare.MarginNominal = fare.OriginalIdrPrice - fare.FinalIdrPrice;    
            }
            else
            {
                var modifiedFare = fare.OriginalIdrPrice*(1M + rule.Coefficient) + rule.Constant;
                var roundingAmount = RoundingOrder - (modifiedFare%RoundingOrder);
                var finalFare = modifiedFare + roundingAmount;
                fare.MarginCoefficient = rule.Coefficient;
                fare.MarginConstant = rule.Constant + roundingAmount;
                fare.MarginNominal = finalFare - fare.OriginalIdrPrice;
                fare.FinalIdrPrice = finalFare;
            }
        }

        private static MarginRule GetFirstMatchingRule(FlightItinerary fare, List<MarginRule> rules)
        {
            var rule = new MarginRule();
            for (var i = 0; i < rules.Count; i++)
            {
                rule = rules[i];
                if (!BookingDateMatches(rule)) continue;
                if (!FareTypeMatches(rule, fare)) continue;
                if (!CabinClassMatches(rule, fare)) continue;
                if (!TripTypeMatches(rule, fare)) continue;
                if (!FlightTimeMatches(rule, fare)) continue;
                if (!PassengerCountMatches(rule, fare)) continue;
                if (!AirlineMatches(rule, fare)) continue;
                if (!AirportPairMatches(rule, fare)) continue;
                if (!CityPairMatches(rule, fare)) continue;
                if (CountryPairMatches(rule, fare)) break;
            }
            return rule;
        }

        private static bool BookingDateMatches(MarginRule rule)
        {
            var nowDate = DateTime.UtcNow.Date;
            var dateSpanOk = !rule.BookingDateSpans.Any() || rule.BookingDateSpans.Any(dateSpan => dateSpan.Contains(nowDate));
            var dayOk = !rule.BookingDays.Any() || rule.BookingDays.Contains(nowDate.DayOfWeek);
            var dateOk = !rule.BookingDates.Any() || rule.BookingDates.Contains(nowDate);
            return dateSpanOk && dayOk && dateOk;
        }

        private static bool FlightTimeMatches(MarginRule rule, FlightItinerary fare)
        {
            var departureOk = DepartureTimeMatches(rule, fare);
            var returnOk = ReturnTimeMatches(rule, fare);
            return departureOk && returnOk;
        }

        private static bool DepartureTimeMatches(MarginRule rule, FlightItinerary fare)
        {
            var dates = fare.Trips.First().Segments.Select(segment => segment.DepartureTime.Date).ToList();
            var times = fare.Trips.First().Segments.Select(segment => segment.DepartureTime.TimeOfDay).ToList();
            var dateSpanOk = !rule.DepartureDateSpans.Any() || dates.All(date => rule.DepartureDateSpans.Any(dateSpan => dateSpan.Contains(date)));
            var dayOk = !rule.DepartureDays.Any() || dates.All(date => rule.DepartureDays.Contains(date.DayOfWeek));
            var dateOk = !rule.DepartureDates.Any() || dates.All(date => rule.DepartureDates.Contains(date));
            var timeSpanOk = !rule.DepartureTimeSpans.Any() || times.All(time => rule.DepartureTimeSpans.Any(timeSpan => timeSpan.Contains(time)));
            return dateSpanOk && dayOk && dateOk && timeSpanOk;
        }

        private static bool ReturnTimeMatches(MarginRule rule, FlightItinerary fare)
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

        private static bool TripTypeMatches(MarginRule rule, FlightItinerary fare)
        {
            return !rule.TripTypes.Any() || rule.TripTypes.Contains(fare.RequestedTripType);
        }

        private static bool FareTypeMatches(MarginRule rule, FlightItinerary fare)
        {
            return !rule.FareTypes.Any() || rule.FareTypes.Contains(fare.FareType);
        }

        private static bool CabinClassMatches(MarginRule rule, FlightItinerary fare)
        {
            return !rule.CabinClasses.Any() || rule.CabinClasses.Contains(fare.RequestedCabinClass);
        }

        private static bool PassengerCountMatches(MarginRule rule, FlightItinerary fare)
        {
            var totalPassenger = fare.AdultCount + fare.ChildCount + fare.InfantCount;
            return totalPassenger >= rule.MinPassengers && totalPassenger <= rule.MaxPassengers;
        }

        private static bool AirlineMatches(MarginRule rule, FlightItinerary fare)
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

        private static bool AirportPairMatches(MarginRule rule, FlightItinerary fare)
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

        private static bool CityPairMatches(MarginRule rule, FlightItinerary fare)
        {
            var dict = DictionaryService.GetInstance();
            var farePairs = fare.Trips.Select(trip => new AirportPairRule
            {
                Origin = dict.GetAirportCityCode(trip.OriginAirport),
                Destination = dict.GetAirportCityCode(trip.DestinationAirport)
            });
            var returnPair = new AirportPairRule
            {
                Origin = dict.GetAirportCityCode(fare.Trips.First().OriginAirport),
                Destination = dict.GetAirportCityCode(fare.Trips.First().DestinationAirport)
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

        private static bool CountryPairMatches(MarginRule rule, FlightItinerary fare)
        {
            var dict = DictionaryService.GetInstance();
            var farePairs = fare.Trips.Select(trip => new AirportPairRule
            {
                Origin = dict.GetAirportCountryCode(trip.OriginAirport),
                Destination = dict.GetAirportCountryCode(trip.DestinationAirport)
            });
            var returnPair = new AirportPairRule
            {
                Origin = dict.GetAirportCountryCode(fare.Trips.First().OriginAirport),
                Destination = dict.GetAirportCountryCode(fare.Trips.First().DestinationAirport)
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





