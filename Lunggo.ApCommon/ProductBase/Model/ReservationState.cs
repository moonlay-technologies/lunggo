using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.Framework.Context;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.ProductBase.Model
{
    public class ReservationState
    {
        public PlatformType Platform { get; set; }
        public string Device { get; set; }
        public string Language { get; set; }
        public Currency Currency { get; set; }

        internal void InsertToDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                ReservationStateTableRepo.GetInstance().Insert(conn, new ReservationStateTableRecord
                {
                    RsvNo = rsvNo,
                    PlatformCd = PlatformTypeCd.Mnemonic(PlatformTypeCd.FrameworkCode(OnlineContext.GetActivePlatformCode())),
                    DeviceCd = OnlineContext.GetActiveDeviceCode(),
                    LanguageCd = OnlineContext.GetActiveLanguageCode(),
                    CurrencyCd = OnlineContext.GetActiveCurrencyCode(),
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                });
            }
        }

        internal static ReservationState GetFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetReservationStateQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).Single();
                return new ReservationState
                {
                    Platform = PlatformTypeCd.FrameworkCode(OnlineContext.GetActivePlatformCode()),
                    Device = OnlineContext.GetActiveDeviceCode(),
                    Language = OnlineContext.GetActiveLanguageCode().ToUpper(),
                    Currency = new Currency(OnlineContext.GetActiveCurrencyCode())
                };
            }
        }

        private class GetReservationStateQuery : QueryBase<GetReservationStateQuery, ReservationStateTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT PlatformCd, DeviceCd, LanguageCd, CurrencyCd " +
                       "FROM ReservationState " +
                       "WHERE RsvNo = @RsvNo";
            }
        }
    }
}
