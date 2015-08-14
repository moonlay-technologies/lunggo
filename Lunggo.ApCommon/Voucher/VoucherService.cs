using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Voucher.Query;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

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
            var code = "NSC9T" + Base36Encoder.Base36.Encode(sequence);
            var voucherRecord = new VoucherTableRecord
            {
                VoucherId = code,
                ExpiryDate = DateTime.Now.AddDays(30),
                ValidEmail = email,
                IsUsed = false
            };
            using (var conn = DbService.GetInstance().GetOpenConnection())
                VoucherTableRepo.GetInstance().Insert(conn, voucherRecord);
            return code;
        }

        internal List<long> GetFlightDiscountRules(string code, string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var voucherRecord = GetVoucherRecordQuery.GetInstance().Execute(conn, new {VoucherCode = code}).Single();
                if (voucherRecord.ValidEmail == email &&
                    voucherRecord.ExpiryDate <= DateTime.UtcNow &&
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
    }
}
