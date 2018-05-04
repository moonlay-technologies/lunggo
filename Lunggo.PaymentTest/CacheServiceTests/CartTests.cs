﻿using System;
using System.Linq;
using Lunggo.ApCommon.Payment.Cache;
using Lunggo.Framework.Redis;
using Lunggo.Framework.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;

namespace Lunggo.PaymentTest.CacheServiceTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        // Should get all rsvNo from associated cartId
        public void Should_get_all_rsvNo_from_associated_cartId()
        {
            TestHelper.UseRedis(redis =>
            {
                var rsvNo1 = "12345678";
                var rsvNo2 = "64234634";
                var rsvNo3 = "32335325";
                var cartId = Guid.NewGuid().ToString();
                var key = "Cart:RsvNoList:" + cartId;
                redis.ListLeftPush(key, rsvNo1);
                redis.ListLeftPush(key, rsvNo2);
                redis.ListLeftPush(key, rsvNo3);

                var actual = new PaymentCacheService().GetCartRsvNos(cartId);

                Assert.IsTrue(actual.Contains(rsvNo1));
                Assert.IsTrue(actual.Contains(rsvNo2));
                Assert.IsTrue(actual.Contains(rsvNo3));

                redis.KeyDelete(key);
            });
        }

        [TestMethod]
        // Should return empty list when there is no rsvNo with the associated cartId
        public void Should_return_empty_list_when_there_is_no_rsvNo_with_the_associated_cartId()
        {
            TestHelper.UseRedis(redis =>
            {
                var cartId = Guid.NewGuid().ToString();
                var key = "Cart:RsvNoList:" + cartId;

                var actual = new PaymentCacheService().GetCartRsvNos(cartId);

                Assert.IsTrue(actual != null);
                Assert.IsTrue(actual.Count == 0);

                redis.KeyDelete(key);
            });
        }

        [TestMethod]
        // Should return empty list when key does not exist
        public void Should_return_empty_list_when_key_does_not_exist()
        {
            TestHelper.UseRedis(redis =>
            {
                var cartId = Guid.NewGuid().ToString();
                var key = "Cart:RsvNoList:" + cartId;
                redis.KeyDelete(key);

                var actual = new PaymentCacheService().GetCartRsvNos(cartId);

                Assert.IsTrue(actual != null);
                Assert.IsTrue(actual.Count == 0);
            });
        }


        [TestMethod]
        // Should contains rsvNo after adding rsvNo
        public void Should_contains_rsvNo_after_adding_rsvNo()
        {
            TestHelper.UseRedis(redis =>
            {
                var cartId = Guid.NewGuid().ToString();
                var rsvNo = "123456";
                var key = "Cart:RsvNoList:" + cartId;

                new PaymentCacheService().AddRsvToCart(cartId, rsvNo);

                var actual = redis.ListRange(key);
                Assert.IsTrue(actual != null);
                Assert.IsTrue(actual.Length > 0);
                Assert.IsTrue(actual.Contains<RedisValue>(rsvNo));

                redis.KeyDelete(key);
            });
        }

        [TestMethod]
        // Should not contains rsvNo after removing rsvNo
        public void Should_not_contains_rsvNo_after_removing_rsvNo()
        {
            TestHelper.UseRedis(redis =>
            {
                var cartId = Guid.NewGuid().ToString();
                var rsvNo = "123456";
                var key = "Cart:RsvNoList:" + cartId;

                redis.ListLeftPush(key, rsvNo);

                new PaymentCacheService().RemoveRsvFromCart(cartId, rsvNo);

                var actual = redis.ListRange(key);
                Assert.IsTrue(actual == null || !actual.Contains<RedisValue>(rsvNo));
            });
        }

        [TestMethod]
        // Should not contains anything after removing cart
        public void Should_not_contains_anything_after_removing_cart()
        {
            TestHelper.UseRedis(redis =>
            {
                var cartId = Guid.NewGuid().ToString();
                var rsvNo = "123456";
                var key = "Cart:RsvNoList:" + cartId;

                redis.ListLeftPush(key, rsvNo);

                new PaymentCacheService().RemoveRsvFromCart(cartId, rsvNo);

                var actual = redis.ListRange(key);
                Assert.IsTrue(actual == null || actual.Length == 0);
            });
        }
	

    }
}