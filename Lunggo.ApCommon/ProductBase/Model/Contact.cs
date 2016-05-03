using System;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.ProductBase.Model
{
    public sealed class Contact
    {
        [JsonProperty("tit")]
        public Title Title { get; set; }
        [JsonProperty("nm")]
        public string Name { get; set; }
        [JsonProperty("ctycd")]
        public string CountryCallingCode { get; set; }
        [JsonProperty("ph")]
        public string Phone { get; set; }
        [JsonProperty("em")]
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
                var record = GetContactQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).Single();
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
