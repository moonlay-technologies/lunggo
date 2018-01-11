using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Redis;
using StackExchange.Redis;
using Lunggo.ApCommon.Payment.Model;
using System.Runtime.InteropServices;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Query;
using Lunggo.ApCommon.Activity.Database;
using Lunggo.ApCommon.Payment.Service;
using System.Net;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Database;
using Lunggo.ApCommon.Activity.Service;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public ViewCartOutput ViewCart(string userId)
        {
            var cart = new ViewCartOutput();
            cart.RsvNoList = new List<string>();
            cart.TotalPrice = 0;
            var idCart = GetUserIdCart(userId);
            var redisService = RedisService.GetInstance();
            var redisKey = "CartId:" + idCart;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            for (long i = 0; i < redisDb.ListLength(redisKey); i++)
            {
                string rsvNo = redisDb.ListGetByIndex(redisKey, i);         
                cart.RsvNoList.Add(rsvNo);
                var paymentDetail = PaymentDetails.GetFromDb(rsvNo);
                cart.TotalPrice += paymentDetail.OriginalPriceIdr;
            }
            cart.StatusCode = HttpStatusCode.OK;
            return cart;
        }

        public AddToCartOutput AddToCart(AddToCartInput input, string userId)
        {
            var response = new AddToCartOutput();
            var idCart = GetUserIdCart(userId);
            if (idCart == null)
            {
                AddIdCart(userId);
                idCart = GetUserIdCart(userId);
            }
            var checkRsv = ActivityService.GetInstance().GetReservation(input.RsvNo);
            if (checkRsv == null)
                return new AddToCartOutput { isSuccess = false };
            var paymentDetail = PaymentDetails.GetFromDb(input.RsvNo);
            if (paymentDetail.FinalPriceIdr != 0 || paymentDetail.OriginalPriceIdr == 0)
                return new AddToCartOutput { isSuccess = false };
            AddRsvNoToIdCart(idCart, input.RsvNo);
            return new AddToCartOutput { isSuccess = true };
        }

        public DeleteRsvFromCartOutput DeleteFromCart(DeleteRsvFromCartInput request)
        {
            var response = new DeleteRsvFromCartOutput();
            var userId = HttpContext.Current.User.Identity.GetId();
            if (userId == null)
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }
            var idCart = GetUserIdCart(userId);
            var redisService = RedisService.GetInstance();
            var redisKey = "CartId:" + idCart;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.ListRemove(redisKey, request.RsvNo);
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        public string GetUserIdCart(string userId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "NameId:" + userId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            try
            {
                var value = redisDb.StringGet(redisKey);
                return value;
            }
            catch
            {

            }
            return null;
        }
        
        internal void AddIdCart(string userId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "NameId:" + userId;
            var redisValue = Guid.NewGuid();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, redisValue.ToString());            
        }

        internal void AddRsvNoToIdCart(string idCart, string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "CartId:" + idCart;
            var redisValue = rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.ListRightPush(redisKey, redisValue);
        }        
    }
}

