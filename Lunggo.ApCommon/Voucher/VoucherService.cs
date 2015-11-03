using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Voucher.Query;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Voucher
{
    public class VoucherService
    {
        private static readonly VoucherService Instance = new VoucherService();
        private bool _isInitialized;
        private VoucherService()
        {
            
        }

        public static VoucherService GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }

        public string GenerateVoucherCode(string email)
        {
            var sequence = VoucherSequence.GetInstance().GetNext();
            var code = "NSC9T" + Base36Encoder.Base36.Encode(sequence) + "AA";
            var voucherRecord = new VoucherTableRecord
            {
                VoucherId = code,
                ExpiryDate = DateTime.Now.AddDays(30),
                ValidEmail = email,
                IsUsed = false
            };
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                VoucherTableRepo.GetInstance().Insert(conn, voucherRecord);
            }
            return code;
        }

        internal List<long> GetFlightDiscountRules(string code, string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var voucherRecords = GetVoucherRecordQuery.GetInstance().Execute(conn, new {VoucherCode = code}).ToList();
                VoucherTableRecord voucherRecord;
                if (voucherRecords.Count() == 1)
                {
                    voucherRecord = voucherRecords.Single();
                }
                else
                {
                    return null;
                }
                if (voucherRecord.ValidEmail == email &&
                    DateTime.UtcNow <= voucherRecord.ExpiryDate &&
                    voucherRecord.IsUsed == false)
                {
                    return new List<long> {0};
                }
                else
                {
                    return null;
                }
            }

        }

        public void InvalidateVoucher(string code)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
                VoucherTableRepo.GetInstance().Update(conn, new VoucherTableRecord
                {
                    VoucherId = code,
                    IsUsed = true
                });
        }

        public void SendVoucherEmailToCustomer(string email, string voucherCode)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("VoucherEmail");
            var model = new VoucherEmailModel
            {
                Email = email,
                VoucherCode = voucherCode
            };
            queue.AddMessage(new CloudQueueMessage(model.Serialize()));
        }

        public decimal CheckVoucherDiscount(string token, string code, string email)
        {
            if (token.IsFlightCache())
            {
                var flight = FlightService.GetInstance();
                var itinerary = flight.GetItineraryForDisplay(token);
                var rules = GetFlightDiscountRules(code, email);
                var rule = flight.GetMatchingDiscountRule(rules);
                if (rule != null)
                    return (itinerary.TotalFare*rule.Coefficient) + rule.Constant;
                else
                    return 0;
            }
            else 
                return 0;
        }

    }
}
