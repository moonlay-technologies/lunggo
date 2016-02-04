using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Model;
using Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Query;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Logic
{
    public static class ImlekLogic
    {
        private const int RetryCountPerDay = 2;
        private const int Chance = 5;
        private const int ChanceMax = 100;

        public static ImlekApiResponse Roll(ImlekApiRequest request)
        {
            if (EmailNotExist(request.Email))
                AddEmail(request.Email);

            var retryCount = GetRetryCount(request.Email);
            if (retryCount == 0)
            {
                return new ImlekApiResponse
                {
                    ReturnCode = -1
                };
            }
            else
            {
                DecrementRetryCount(request.Email);
                return RollResult(request.Email);
            }
        }

        private static ImlekApiResponse RollResult(string email)
        {
            if (new Random().Next(ChanceMax) < Chance)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var unusedVouchers = GetUnusedVoucherCodesQuery.GetInstance().Execute(conn, null).ToList();
                    var voucher = unusedVouchers[new Random().Next(unusedVouchers.Count)];
                    var voucherType = voucher.Email.Substring(9);
                    VoucherRecipientsTableRepo.GetInstance().Update(conn, new VoucherRecipientsTableRecord
                    {
                        VoucherRecipientId = voucher.VoucherRecipientId,
                        Email = email
                    });
                    switch (voucherType)
                    {
                        case "50":
                            return new ImlekApiResponse
                            {
                                ReturnCode = 1,
                                VoucherCode = voucher.VoucherCode
                            };
                        case "100":
                            return new ImlekApiResponse
                            {
                                ReturnCode = 2,
                                VoucherCode = voucher.VoucherCode
                            };
                        case "150":
                            return new ImlekApiResponse
                            {
                                ReturnCode = 3,
                                VoucherCode = voucher.VoucherCode
                            };
                        case "200":
                            return new ImlekApiResponse
                            {
                                ReturnCode = 4,
                                VoucherCode = voucher.VoucherCode
                            };
                        default:
                            return new ImlekApiResponse
                            {
                                ReturnCode = 0
                            };
                    }
                }
            }
            else
                return new ImlekApiResponse
                {
                    ReturnCode = 0
                };
        }

        private static void DecrementRetryCount(string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                DecrementRetryCountQuery.GetInstance().Execute(conn, new { Email = email });
            }
        }

        private static int GetRetryCount(string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var imlekTable = GetImlekTableByEmailQuery.GetInstance().Execute(conn, new { Email = email }).Single();
                if (imlekTable.LastTryDate < DateTime.Now.Date)
                {
                    imlekTable.RetryCount = RetryCountPerDay;
                    ResetRetryCountQuery.GetInstance().Execute(conn, imlekTable);
                }
                return imlekTable.RetryCount;
            }
        }

        private static void AddEmail(string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                AddImlekEmailQuery.GetInstance().Execute(conn, new ImlekTable
                {
                    Email = email,
                    RetryCount = RetryCountPerDay,
                    LastTryDate = DateTime.Now.Date
                });
            }
        }

        private static bool EmailNotExist(string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var imlekTable = GetImlekTableByEmailQuery.GetInstance().Execute(conn, new { Email = email }).SingleOrDefault();
                return imlekTable == null;
            }
        }
    }
}