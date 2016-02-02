using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Framework.Database;
using Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Model;
using Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Query;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Logic
{
    public class ImlekLogic
    {
        private const int RetryCountPerDay = 2;
        private const double Chance = 0.00001;

        public ImlekApiResponse Roll(ImlekApiRequest request)
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
                return RollResult();
            }
        }

        private static ImlekApiResponse RollResult()
        {
            if (new Random().NextDouble() <= Chance)
            {
                throw new NotImplementedException();
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
                return imlekTable != null;
            }
        }
    }
}