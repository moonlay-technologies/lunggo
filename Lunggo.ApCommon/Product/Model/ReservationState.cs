using System;
using System.Linq;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Context;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Product.Model
{
    public class ReservationState
    {
        public PlatformType Platform { get; set; }
        public string Device { get; set; }
        public string Language { get; set; }
        public Currency Currency { get; set; }

        public ReservationState()
        {
            Platform = PlatformTypeCd.Mnemonic(OnlineContext.GetActivePlatformCode());
            Device = OnlineContext.GetActiveDeviceCode();
            Language = OnlineContext.GetActiveLanguageCode();
            Currency = new Currency(OnlineContext.GetActiveCurrencyCode());
        }

        internal void InsertToDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                ReservationStateTableRepo.GetInstance().Insert(conn, new ReservationStateTableRecord
                {
                    RsvNo = rsvNo,
                    PlatformCd = PlatformTypeCd.Mnemonic(Platform),
                    DeviceCd = Device,
                    LanguageCd = Language,
                    CurrencyCd = Currency,
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
                    Platform = PlatformTypeCd.Mnemonic(OnlineContext.GetActivePlatformCode()),
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
