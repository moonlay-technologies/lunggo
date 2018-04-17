using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.Framework.Database;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Redis;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
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
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public Cart GetCart()
        {
            var userId = HttpContext.Current.User.Identity.GetId();
            var cartId = GetCartId(userId);
            return GetCart(cartId);
        }

        public Cart GetCart(string cartId)
        {
            var cart = new Cart
            {
                Id = cartId,
            };

            GetCartContentFromCache(cart);
            if (cart.RsvNoList == null || !cart.RsvNoList.Any())
                GetCartContentFromDb(cart);
            if (cart.RsvNoList == null || !cart.RsvNoList.Any())
                return cart;

            cart.Contact = Contact.GetFromDb(cart.RsvNoList[0]);
            
            return cart;
        }

        private void GetCartContentFromDb(Cart cart)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = CartsTableRepo.GetInstance().Find(conn, new CartsTableRecord {CartId = cart.Id});
                cart.RsvNoList = record.Select(r => r.RsvNoList).ToList();
            }
        }

        private static void GetCartContentFromCache(Cart cart)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:RsvNoList:" + cart.Id;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            if (redisDb.ListLength(redisKey) == 0)
                return;

            var rsvNoList = redisDb.ListRange(redisKey).Select(val => val.ToString()).Distinct().ToList();
            cart.RsvNoList = rsvNoList;
            cart.TotalPrice = rsvNoList.Select(PaymentDetails.GetFromDb).Sum(p => p.OriginalPriceIdr);
        }

        public bool AddToCart(string rsvNo)
        {
            var userId = HttpContext.Current.User.Identity.GetId();
            return AddToCart(userId, rsvNo);
        }

        public bool AddToCart(string userId, string rsvNo)
        {
            var checkRsv = ActivityService.GetInstance().GetReservation(rsvNo);
            if (checkRsv == null)
                return false;
            var paymentDetail = PaymentDetails.GetFromDb(rsvNo);
            if (paymentDetail.FinalPriceIdr != 0 || paymentDetail.OriginalPriceIdr == 0)
                return false;

            var cartId = GetCartId(userId);
            AddRsvNoToCartCache(cartId, rsvNo);
            return true;
        }

        public void DeleteFromCart(string rsvNo)
        {
            var userId = HttpContext.Current.User.Identity.GetId();
            var cartId = GetCartId(userId);
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:RsvNoList:" + cartId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.ListRemove(redisKey, rsvNo);
        }

        public string GetCartId(string userId)
        {
            var hash = userId.Sha1Encode();
            var stringBuilder = new StringBuilder();
            foreach (var hashByte in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", hashByte);
            }
            return stringBuilder.ToString();
        }
        
        internal void AddCartCache(string userId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:UserCartId:" + userId;
            var redisValue = Guid.NewGuid();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, redisValue.ToString());            
        }

        internal void AddRsvNoToCartCache(string cartId, string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:RsvNoList:" + cartId;
            var redisValue = rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.ListRightPush(redisKey, redisValue);
        }

        public void DeleteCartCache(string cartId)
        {
            var redisService = RedisService.GetInstance();
            var redisKeyCartId = "Cart:RsvNoList:" + cartId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.KeyDelete(redisKeyCartId);
        }

        public void InsertCartToDb(string cartRecordId, List<string> rsvNoList)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userId = GetUserIdFromActivityReservationDbQuery.GetInstance().Execute(conn, new { RsvNo = rsvNoList[0] }).First();
                rsvNoList = rsvNoList.Distinct().ToList();
                foreach (var rsvNo in rsvNoList)
                {
                    var cartsRecord = new CartsTableRecord
                    {
                        CartId = cartRecordId,
                        RsvNoList = rsvNo,
                        UserId = userId
                    };
                    CartsTableRepo.GetInstance().Insert(conn, cartsRecord);
                }
            }
        }

        public string GetCartIdByRsvNo(string rsvNo)
        {
            return GetCartIdByRsvNoFromDb(rsvNo);
        }

        public decimal GetSurchargeNominal(PaymentDetails payment)
        {
            var surchargeList = GetSurchargeList();
            var surcharge =
                surchargeList.SingleOrDefault(
                    sur =>
                        payment.Method == sur.PaymentMethod &&
                        (sur.PaymentSubMethod == null || payment.Submethod == sur.PaymentSubMethod));
            return surcharge == null
                ? 0
                : Math.Ceiling((payment.OriginalPriceIdr - payment.DiscountNominal) * surcharge.Percentage / 100) +
                  surcharge.Constant;
        }
    }
}

