using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database.Query;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Database
{
    internal partial class PaymentDbService
    {
        internal CampaignVoucher GetCampaignVoucher(string voucherCode)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var voucher = GetCampaignVoucherRecordQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode }).FirstOrDefault();
                if (voucher != null && string.IsNullOrWhiteSpace(voucher.DisplayName))
                    voucher.DisplayName = "Discount";
                return voucher;
            }
        }
        internal int CheckVoucherUsage(string voucherCode, string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var voucherUsage = CheckVoucherUsageQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode, Email = email }).FirstOrDefault();
                return voucherUsage;
            }
        }
        internal bool IsEligibleForVoucher(string voucherCode, string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var voucherRecipient = GetVoucherRecipientsRecordQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode, Email = email }).FirstOrDefault();
                return voucherRecipient != null;
            }
        }
        internal bool IsMember(string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userName = GetMemberRecordQuery.GetInstance().Execute(conn, new { Email = email }).FirstOrDefault();
                return string.IsNullOrEmpty(userName);
            }
        }
        internal bool VoucherDecrement(string voucherCode)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                int rowAffected = VoucherDecrementQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode });
                return rowAffected > 0;
            }
        }
        internal bool VoucherIncrement(string voucherCode)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                int rowAffected = VoucherIncrementQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode });
                return rowAffected > 0;
            }
        }

        internal bool UseHotelBudget(string voucherCode, string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                int rowAffected = UseHotelBudgetQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode, RsvNo = rsvNo });
                return rowAffected > 0;
            }
        }
    }
}