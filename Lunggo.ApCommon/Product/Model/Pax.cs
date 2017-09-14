using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Product.Model
{
    public class PaxForDisplay
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public PaxType? Type { get; set; }
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public Title? Title { get; set; }
        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        public Gender? Gender { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("dob", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateOfBirth { get; set; }
        [JsonProperty("nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Nationality { get; set; }
        [JsonProperty("passportNo", NullValueHandling = NullValueHandling.Ignore)]
        public string PassportNumber { get; set; }
        [JsonProperty("passportExp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PassportExpiryDate { get; set; }
        [JsonProperty("passportCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string PassportCountry { get; set; }
    }

    public class Pax
    {
        public int Id { get; set; }
        public PaxType Type { get; set; }
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Nationality { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public string PassportCountry { get; set; }
    }

    public static class PaxUtil
    {
        public static List<PaxForDisplay> ConvertToPaxForDisplay(this List<Pax> pax)
        {
            return pax != null
                ? pax.Select(ConvertToPaxForDisplay).ToList()
                : null;
        }

        public static PaxForDisplay ConvertToPaxForDisplay(this Pax pax)
        {
            if (pax == null)
                return null;

            var name = pax.FirstName == pax.LastName
                ? pax.LastName
                : pax.FirstName + " " + pax.LastName;

            return new PaxForDisplay
            {
                Type = pax.Type,
                Title = pax.Title,
                Name = name,
                Gender = pax.Gender,
                DateOfBirth = pax.DateOfBirth,
                Nationality = pax.Nationality,
                PassportNumber = pax.PassportNumber,
                PassportExpiryDate = pax.PassportExpiryDate,
                PassportCountry = pax.PassportCountry
            };
        }

        public static List<Pax> ConvertToPax(this List<PaxForDisplay> pax)
        {
            return pax != null
                ? pax.Select(ConvertToPax).ToList()
                : null;
        }

        public static Pax ConvertToPax(this PaxForDisplay pax)
        {
            if (pax == null)
                return null;

            string first, last;
            if (pax.Name == null)
            {
                first = null;
                last = null;
            }
            else
            {
                var splittedName = pax.Name.Trim().Split(' ');
                if (splittedName.Length == 1)
                {
                    first = pax.Name;
                    last = pax.Name;
                }
                else
                {
                    first = pax.Name.Substring(0, pax.Name.LastIndexOf(' '));
                    last = splittedName[splittedName.Length - 1];
                }
            }

            return new Pax
            {
                Type = pax.Type.GetValueOrDefault(),
                Title = pax.Title.GetValueOrDefault(),
                FirstName = first,
                LastName = last,
                Gender = pax.Title.GetValueOrDefault() == Title.Mister ? Gender.Male : Gender.Female,
                DateOfBirth = pax.DateOfBirth,
                Nationality = pax.Nationality,
                PassportNumber = pax.PassportNumber,
                PassportExpiryDate = pax.PassportExpiryDate,
                PassportCountry = pax.PassportCountry
            };
        }
    }
}