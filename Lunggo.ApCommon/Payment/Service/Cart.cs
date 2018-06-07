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
        public virtual Cart GetCartByUser(string userId)
        {
            var cartId = GetCartId(userId);
            var cart = GetCart(cartId);

            return cart;
        }

        public virtual Cart GetCart(string cartId)
        {
            var rsvNoList = _cache.GetCartRsvNos(cartId);

            if (rsvNoList == null || !rsvNoList.Any())
                return new Cart
                {
                    RsvNoList = new List<string>()
                };

            var cart = ConstructCart(cartId, rsvNoList);
            return cart;
        }

        public virtual bool AddToCart(string userId, string rsvNo)
        {
            var paymentDetails = _db.GetPaymentDetails(rsvNo);
            if (paymentDetails == null)
                return false;

            var cartId = GetCartId(userId);
            _cache.AddRsvToCart(cartId, rsvNo);
            return true;
        }

        public virtual bool RemoveFromCart(string userId, string rsvNo)
        {
            var cartId = GetCartId(userId);
            _cache.RemoveRsvFromCart(cartId, rsvNo);
            return true;
        }

        public string GetCartIdByRsvNo(string rsvNo)
        {
            return _db.GetCartIdByRsvNoFromDb(rsvNo);
        }

        private string GetCartId(string userId)
        {
            var cartId = userId.Base64Encode();
            return cartId;
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

