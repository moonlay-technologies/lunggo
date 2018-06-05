using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Payment.Database.Query;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Database
{
    internal partial class PaymentDbService
    {
        internal virtual List<string> GetTrxRsvNos(string trxId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var records = TrxRsvTableRepo.GetInstance().Find(conn, new TrxRsvTableRecord { TrxId = trxId });

                var rsvNoList = records?.Select(r => r.RsvNo).Distinct().ToList();
                return rsvNoList ?? new List<string>();
            }
        }

        internal virtual void InsertTrxUser(TrxPaymentDetails trxDetails)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userId = GetUserIdFromActivityReservationDbQuery.GetInstance()
                    .Execute(conn, new { trxDetails.RsvPaymentDetails[0].RsvNo }).First();
                var trxUserRecord = new TrxUserTableRecord
                {
                    TrxId = trxDetails.TrxId,
                    UserId = userId,
                    Time = DateTime.UtcNow
                };
                TrxUserTableRepo.GetInstance().Insert(conn, trxUserRecord);
            }
        }
        
        internal virtual void InsertTrxRsv(string rsvNo, string trxId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                
                var trxRsvRecord = new TrxRsvTableRecord
                {
                    TrxId = trxId,
                    RsvNo = rsvNo,
                };
                TrxRsvTableRepo.GetInstance().Insert(conn, trxRsvRecord);
                
            }
        }

        internal string GetCartIdByRsvNoFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var cartId = GetCartIdFromDbQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).First();
                return cartId;
            }
        }
    }
}