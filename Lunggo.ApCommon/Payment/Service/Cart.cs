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
        public ViewCartOutput ViewCart(string user)
        {
            
            var cartList = new ViewCartOutput();
            cartList.RsvNoList = new List<string>();
            cartList.TotalPrice = 0;
            var idCart = GetUserIdCart(user);
            var redisService = RedisService.GetInstance();
            var redisKey = "CartId:" + idCart;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            for (long i = 0; i < redisDb.ListLength(redisKey); i++)
            {
                string rsvNo = redisDb.ListGetByIndex(redisKey, i);         
                cartList.RsvNoList.Add(rsvNo);
                var paymentDetail = PaymentDetails.GetFromDb(rsvNo);
                cartList.TotalPrice += paymentDetail.OriginalPriceIdr;
            }
            cartList.StatusCode = HttpStatusCode.OK;
            return cartList;
        }

        public AddToCartOutput AddToCart(AddToCartInput input, string user)
        {
            var response = new AddToCartOutput();
            var idCart = GetUserIdCart(user);
            if (idCart == null)
            {
                AddIdCart(user);
                idCart = GetUserIdCart(user);
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
            var user = HttpContext.Current.User.Identity.GetId();
            if (user == null)
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }
            var idCart = GetUserIdCart(user);
            var redisService = RedisService.GetInstance();
            var redisKey = "CartId:" + idCart;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.ListRemove(redisKey, request.RsvNo);
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        internal string GetUserIdCart(string user)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "NameId:" + user;
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
        
        internal void AddIdCart(string user)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "NameId:" + user;
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

