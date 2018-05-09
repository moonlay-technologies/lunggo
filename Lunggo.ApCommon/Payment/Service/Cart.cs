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
        public Cart GetCart(string userId)
        {
            var cartId = GetCartId(userId);
            var rsvNoList = _cache.GetCartRsvNos(cartId);

            if (rsvNoList == null || !rsvNoList.Any())
                return new Cart
                {
                    RsvNoList = new List<string>()
                };

            var cart = ConstructCart(cartId, rsvNoList);

            return cart;
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

        public bool RemoveFromCart(string userId, string rsvNo)
        {
            var cartId = GetCartId(userId);
            _cache.RemoveRsvFromCart(cartId, rsvNo);
            return true;
        }

        public string GetCartIdByRsvNo(string rsvNo)
        {
            return _db.GetCartIdByRsvNoFromDb(rsvNo);
        }

        public Cart GetTrx(string trxId)
        {
            var rsvNoList = _db.GetTrxRsvNos(trxId);

            if (rsvNoList == null || !rsvNoList.Any())
                return null;

            var cart = ConstructCart(trxId, rsvNoList);

            return cart;
        }

        private string GetCartId(string userId)
        {
            var hash = userId.Sha1Encode();
            var stringBuilder = new StringBuilder();
            foreach (var hashByte in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", hashByte);
            }
            return stringBuilder.ToString();
        }

        private Cart ConstructCart(string cartId, List<string> rsvNoList)
        {
            var cart = new Cart();
            cart.Id = cartId;
            cart.RsvNoList = rsvNoList;
            cart.TotalPrice = rsvNoList.Select(_db.GetPaymentDetails).Sum(d => d.OriginalPriceIdr);
            return cart;
        }
    }
}

