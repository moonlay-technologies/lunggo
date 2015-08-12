using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Logic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Sequence;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private static readonly List<MarginRule> MarginRules = new List<MarginRule>();
        private static readonly List<MarginRule> DeletedMarginRules = new List<MarginRule>();
        private const decimal RoundingOrder = 100M;

        internal void InitPriceMarginRules()
        {
            MarginRules.Add(new MarginRule
            {
                Name = "Default",
                Description = "This is the default margin rules",
                ConstraintCount = 0,
                Coefficient = 0.07M,
                Constant = 0
            });
        }

        internal void AddPriceMargin(FlightItineraryFare fare)
        {
            var rule = GetFirstMatchingRule(fare);
            ApplyMarginRule(fare, rule);
        }

        public MarginRule GetPriceMarginRule(long ruleId)
        {
            return MarginRules.Single(rule => rule.RuleId == ruleId);
        }

        public List<MarginRule> GetAllPriceMarginRules()
        {
            return MarginRules;
        }

        public List<MarginRule> InsertPriceMarginRuleAndRetrieveConflict(MarginRule newRule)
        {
            AssignRuleId(newRule);
            InsertMarginRule(newRule);
            var isConflicting = CheckForConflict(newRule);
            return isConflicting ? RetrieveConflict(newRule) : null;
        }

        public void DeletePriceMarginRule(long ruleId)
        {
            var obsoleteRule = MarginRules.Single(rule => rule.RuleId == ruleId);
            MarginRules.Remove(obsoleteRule);
            DeletedMarginRules.Add(obsoleteRule);
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

        public void PullRemotePriceMarginRules()
        {
            MarginRules.Clear();
            MarginRules.AddRange(GetFlightDb.PriceMarginRules());
        }

        public void PushRemotePriceMarginRules()
        {
            InsertFlightDb.PriceMarginRules(MarginRules, DeletedMarginRules);
        }

        #region HelperMethods

        private static void AssignRuleId(MarginRule newRule)
        {
            newRule.RuleId = FlightPriceMarginRuleIdSequence.GetInstance().GetNext();
        }

        private static void UpdateConflictingPriceMarginRules(List<MarginRule> updatedRules)
        {
            var constraintCount = updatedRules.First().ConstraintCount;
            var updatedRulesCount = updatedRules.Count();
            var orderedUpdatedRules = updatedRules.OrderBy(rule => rule.Priority);
            var targetIndex = MarginRules.FindIndex(rule => rule.ConstraintCount == constraintCount);
            MarginRules.RemoveRange(targetIndex, updatedRulesCount);
            MarginRules.InsertRange(targetIndex, orderedUpdatedRules);
        }

        private static List<MarginRule> RetrieveConflict(MarginRule newRule)
        {
            return MarginRules.Where(rule => rule.ConstraintCount == newRule.ConstraintCount).ToList();
        }

        private static void InsertMarginRule(MarginRule newRule)
        {
            var index = MarginRules.FindLastIndex(rule => rule.ConstraintCount > newRule.ConstraintCount);
            MarginRules.Insert(index + 1, newRule);
        }

        private static bool CheckForConflict(MarginRule newRule)
        {
            return MarginRules.Count(rule => rule.ConstraintCount == newRule.ConstraintCount) > 1;
        }

        private static void ApplyMarginRule(FlightItineraryFare fare, MarginRule rule)
        {
            fare.MarginId = rule.RuleId;
            var modifiedFare = fare.OriginalIdrPrice*(1M + rule.Coefficient) + rule.Constant;
            var roundingAmount = RoundingOrder - (modifiedFare%RoundingOrder);
            var finalFare = modifiedFare + roundingAmount;
            fare.MarginCoefficient = rule.Coefficient;
            fare.MarginConstant = rule.Constant + roundingAmount;
            fare.MarginNominal = finalFare - fare.OriginalIdrPrice;
            fare.FinalIdrPrice = finalFare;
        }

        private static MarginRule GetFirstMatchingRule(FlightItineraryFare fare)
        {
            var rule = new MarginRule();
            for (var i = 0; i < MarginRules.Count; i++)
            {
                rule = MarginRules[i];
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
            var dateSpanOk = !rule.BookingDateSpans.Any() || rule.BookingDateSpans.Any(dateSpan => dateSpan.Includes(nowDate));
            var dayOk = !rule.BookingDays.Any() || rule.BookingDays.Contains(nowDate.DayOfWeek);
            var dateOk = !rule.BookingDates.Any() || rule.BookingDates.Contains(nowDate);
            return dateSpanOk && dayOk && dateOk;
        }

        private static bool FlightTimeMatches(MarginRule rule, FlightItineraryFare fare)
        {
            var departureOk = DepartureTimeMatches(rule, fare);
            var returnOk = ReturnTimeMatches(rule, fare);
            return departureOk && returnOk;
        }

        private static bool DepartureTimeMatches(MarginRule rule, FlightItineraryFare fare)
        {
            var dates = fare.FlightTrips.First().FlightSegments.Select(segment => segment.DepartureTime.Date).ToList();
            var times = fare.FlightTrips.First().FlightSegments.Select(segment => segment.DepartureTime.TimeOfDay).ToList();
            var dateSpanOk = !rule.DepartureDateSpans.Any() || dates.All(date => rule.DepartureDateSpans.Any(dateSpan => dateSpan.Includes(date)));
            var dayOk = !rule.DepartureDays.Any() || dates.All(date => rule.DepartureDays.Contains(date.DayOfWeek));
            var dateOk = !rule.DepartureDates.Any() || dates.All(date => rule.DepartureDates.Contains(date));
            var timeSpanOk = !rule.DepartureTimeSpans.Any() || times.All(time => rule.DepartureTimeSpans.Any(timeSpan => timeSpan.Includes(time)));
            return dateSpanOk && dayOk && dateOk && timeSpanOk;
        }

        private static bool ReturnTimeMatches(MarginRule rule, FlightItineraryFare fare)
        {
            if (fare.TripType != TripType.Return)
                return true;
            else
            {
                var dates = fare.FlightTrips.Last().FlightSegments.Select(segment => segment.DepartureTime.Date).ToList();
                var times = fare.FlightTrips.Last().FlightSegments.Select(segment => segment.DepartureTime.TimeOfDay).ToList();
                var dateSpanOk = !rule.ReturnDateSpans.Any() || dates.All(date => rule.ReturnDateSpans.Any(dateSpan => dateSpan.Includes(date)));
                var dayOk = !rule.ReturnDays.Any() || dates.All(date => rule.ReturnDays.Contains(date.DayOfWeek));
                var dateOk = !rule.ReturnDates.Any() || dates.All(date => rule.ReturnDates.Contains(date));
                var timeSpanOk = !rule.ReturnTimeSpans.Any() || times.All(time => rule.ReturnTimeSpans.Any(timeSpan => timeSpan.Includes(time)));
                return dateSpanOk && dayOk && dateOk && timeSpanOk;
            }
        }

        private static bool TripTypeMatches(MarginRule rule, FlightItineraryFare fare)
        {
            return !rule.TripTypes.Any() || rule.TripTypes.Contains(fare.TripType);
        }

        private static bool FareTypeMatches(MarginRule rule, FlightItineraryFare fare)
        {
            return !rule.FareTypes.Any() || rule.FareTypes.Contains(fare.FareType);
        }

        private static bool CabinClassMatches(MarginRule rule, FlightItineraryFare fare)
        {
            return !rule.CabinClasses.Any() || rule.CabinClasses.Contains(fare.CabinClass);
        }

        private static bool PassengerCountMatches(MarginRule rule, FlightItineraryFare fare)
        {
            var totalPassenger = fare.AdultCount + fare.ChildCount + fare.InfantCount;
            return totalPassenger >= rule.MinPassengers && totalPassenger <= rule.MaxPassengers;
        }

        private static bool AirlineMatches(MarginRule rule, FlightItineraryFare fare)
        {
            var airlines = fare.FlightTrips.SelectMany(trip => trip.FlightSegments)
                .Select(segment => segment.AirlineCode);

            if (rule.Airlines.Any())
                return rule.AirlinesIsExclusion
                    ? airlines.All(airline => !rule.Airlines.Contains(airline))
                    : airlines.All(airline => rule.Airlines.Contains(airline));
            else
                return true;
        }

        private static bool AirportPairMatches(MarginRule rule, FlightItineraryFare fare)
        {
            var farePairs = fare.FlightTrips.Select(trip => new AirportPairRule
            {
                Origin = trip.OriginAirport,
                Destination = trip.DestinationAirport
            });
            var returnPair = new AirportPairRule
            {
                Origin = fare.FlightTrips.First().OriginAirport,
                Destination = fare.FlightTrips.First().DestinationAirport
            };

            if (rule.AirportPairs.Any())
            {
                if (rule.TripTypes.Contains(TripType.Return) && fare.TripType == TripType.Return)
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

        private static bool CityPairMatches(MarginRule rule, FlightItineraryFare fare)
        {
            var dict = DictionaryService.GetInstance();
            var farePairs = fare.FlightTrips.Select(trip => new AirportPairRule
            {
                Origin = dict.GetAirportCityCode(trip.OriginAirport),
                Destination = dict.GetAirportCityCode(trip.DestinationAirport)
            });
            var returnPair = new AirportPairRule
            {
                Origin = dict.GetAirportCityCode(fare.FlightTrips.First().OriginAirport),
                Destination = dict.GetAirportCityCode(fare.FlightTrips.First().DestinationAirport)
            };

            if (rule.CityPairs.Any())
            {
                if (rule.TripTypes.Contains(TripType.Return) && fare.TripType == TripType.Return)
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

        private static bool CountryPairMatches(MarginRule rule, FlightItineraryFare fare)
        {
            var dict = DictionaryService.GetInstance();
            var farePairs = fare.FlightTrips.Select(trip => new AirportPairRule
            {
                Origin = dict.GetAirportCountryCode(trip.OriginAirport),
                Destination = dict.GetAirportCountryCode(trip.DestinationAirport)
            });
            var returnPair = new AirportPairRule
            {
                Origin = dict.GetAirportCountryCode(fare.FlightTrips.First().OriginAirport),
                Destination = dict.GetAirportCountryCode(fare.FlightTrips.First().DestinationAirport)
            };

            if (rule.CountryPairs.Any())
            {
                if (rule.TripTypes.Contains(TripType.Return) && fare.TripType == TripType.Return)
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





