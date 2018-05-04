using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Redis;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Activity.Database.Query;

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
            var rsvNoList = _cache.GetCartRsvNos(cartId) ?? _db.GetCartRsvNos(cartId);

            if (rsvNoList == null || !rsvNoList.Any())
                return null;

            var cart = ConstructCartFronRsvNoList(rsvNoList);

            return cart;
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
            var paymentDetail = _db.GetPaymentDetails(rsvNo);
            if (paymentDetail.FinalPriceIdr != 0 || paymentDetail.OriginalPriceIdr == 0)
                return false;

            var cartId = GetCartId(userId);
            _cache.AddRsvToCart(cartId, rsvNo);
            return true;
        }

        public bool RemoveFromCart(string rsvNo)
        {
            var userId = HttpContext.Current.User.Identity.GetId();
            return RemoveFromCart(userId, rsvNo);
        }

        public bool RemoveFromCart(string userId, string rsvNo)
        {
            var cartId = GetCartId(userId);
            _cache.RemoveRsvFromCart(cartId, rsvNo);
            return true;
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

        public string GetCartIdByRsvNo(string rsvNo)
        {
            return _db.GetCartIdByRsvNoFromDb(rsvNo);
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

        private Cart ConstructCartFronRsvNoList(List<string> rsvNoList)
        {
            var cart = new Cart();
            cart.RsvNoList = rsvNoList;
            cart.TotalPrice = rsvNoList.Select(_db.GetPaymentDetails).Sum(d => d.OriginalPriceIdr);
            return cart;
        }
    }
}

