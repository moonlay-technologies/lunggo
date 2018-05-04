using System;
using System.Linq;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Product.Model
{
    public sealed class Contact
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public Title Title { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("countryCallCd", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCallingCode { get; set; }
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        internal void InsertToDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                ContactTableRepo.GetInstance().Insert(conn, new ContactTableRecord
                {
                    RsvNo = rsvNo,
                    TitleCd = TitleCd.Mnemonic(Title),
                    Name = Name,
                    CountryCallCd = CountryCallingCode,
                    Phone = Phone,
                    Email = Email,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                });
            }
        }

        public static Contact GetFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetContactQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).SingleOrDefault();

                if (record == null)
                    return null;

                return new Contact
                {
                    Title = TitleCd.Mnemonic(record.TitleCd),
                    Name = record.Name,
                    Email = record.Email,
                    CountryCallingCode = record.CountryCallCd,
                    Phone = record.Phone
                };
            }
        }

        private class GetContactQuery : DbQueryBase<GetContactQuery, ContactTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT TitleCd, Name, Email, CountryCallCd, Phone " +
                       "FROM Contact " +
                       "WHERE RsvNo = @RsvNo";
            }
        }
    }
}
