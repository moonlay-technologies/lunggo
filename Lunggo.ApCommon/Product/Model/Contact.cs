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
        [JsonProperty("title")]
        public Title Title { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("countryCallCd")]
        public string CountryCallingCode { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("email")]
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

        internal static Contact GetFromDb(string rsvNo)
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

        private class GetContactQuery : QueryBase<GetContactQuery, ContactTableRecord>
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
