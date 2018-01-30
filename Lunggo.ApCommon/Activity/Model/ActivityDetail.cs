using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{

    public class ActivityDetailForDisplay
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long ActivityId { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
        [JsonProperty("shortDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDesc { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("zone", NullValueHandling = NullValueHandling.Ignore)]
        public string Zone { get; set; }
        [JsonProperty("area", NullValueHandling = NullValueHandling.Ignore)]
        public string Area { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Latitude { get; set; }
        [JsonProperty("long", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Longitude { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Price { get; set; }
        [JsonProperty("priceDetail", NullValueHandling = NullValueHandling.Ignore)]
        public string PriceDetail { get; set; }
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public DurationActivity Duration { get; set; }
        [JsonProperty("operationTime", NullValueHandling = NullValueHandling.Ignore)]
        public string OperationTime { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> MediaSrc { get; set; }
        [JsonProperty("contents", NullValueHandling = NullValueHandling.Ignore)]
        public List<Content> Contents { get; set; }
        [JsonProperty("additionalContents", NullValueHandling = NullValueHandling.Ignore)]
        public AdditionalContent AdditionalContents { get; set; }
        [JsonProperty("cancellation", NullValueHandling = NullValueHandling.Ignore)]
        public string Cancellation { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }
        [JsonProperty("requiredPaxData", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> RequiredPaxData { get; set; }
        [JsonProperty("package", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPackage> Package { get; set; }
        [JsonProperty("wishlisted", NullValueHandling = NullValueHandling.Ignore)]
        public bool Wishlisted { get; set; }
        

    }
    public class ActivityDetail
    {
        public long ActivityId { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ShortDesc { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Zone { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal Price { get; set; }
        public string PriceDetail { get; set; }
        public DurationActivity Duration { get; set; }
        public string OperationTime { get; set; }
        public List<string> MediaSrc { get; set; }
        public string ImportantNotice { get; set; }
        public string Warning { get; set; }
        public string AdditionalNotes { get; set; }
        public string Cancellation { get; set; }
        public DateTime Date { get; set; }
        public bool IsPassportNeeded { get; set; }
        public bool IsPassportIssueDateNeeded { get; set; }
        public bool IsDateOfBirthNeeded { get; set; }
        public bool IsIdCardNoRequired { get; set; }
        public string OperatorName { get; set; }
        public string OperatorEmail { get; set; }
        public string OperatorPhone { get; set; }
        public AdditionalContent AdditionalContents { get; set; }
        public List<ActivityPackage> Package { get; set; }
        public bool Wishlisted { get; set; }
    }

    public class AdditionalContent
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("contents", NullValueHandling = NullValueHandling.Ignore)]
        public List<Content> Contents { get; set; }
    }

    public class Content
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

    public class DateAndAvailableHour
    {
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }
        [JsonProperty("availableHours", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AvailableHours { get; set; }
    }

    public class AvailableDayAndHours
    {
        [JsonProperty("availableDay", NullValueHandling = NullValueHandling.Ignore)]
        public string Day { get; set; }
        [JsonProperty("availableHours", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AvailableHours { get; set; }
    }

    public class ActivityPackage
    {
        [JsonProperty("packageId", NullValueHandling = NullValueHandling.Ignore)]
        public long PackageId { get; set; }
        [JsonProperty("packageName", NullValueHandling = NullValueHandling.Ignore)]
        public string PackageName { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description{ get; set; }
        [JsonProperty("maxCount", NullValueHandling = NullValueHandling.Ignore)]
        public int MaxCount { get; set; }
        [JsonProperty("minCount", NullValueHandling = NullValueHandling.Ignore)]
        public int MinCount { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPricePackage> Price { get; set; }
    }

    public class ActivityPackageId
    {
        [JsonProperty("packageId", NullValueHandling = NullValueHandling.Ignore)]
        public long PackageId { get; set; }
    }

    public class ActivityPricePackage
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }
        [JsonProperty("minCount", NullValueHandling = NullValueHandling.Ignore)]
        public int MinCount { get; set; }
    }

    public class CustomDate
    {
        [JsonProperty("availableDay", NullValueHandling = NullValueHandling.Ignore)]
        public string Day { get; set; }
        [JsonProperty("availableHours", NullValueHandling = NullValueHandling.Ignore)]
        public string AvailableHours { get; set; }
    }

    public class ActivityPricePackageReservation
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
        [JsonProperty("totalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalPrice { get; set; }
    }
}
