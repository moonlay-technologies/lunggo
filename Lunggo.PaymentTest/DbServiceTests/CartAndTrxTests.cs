using System;
using Dapper;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.Framework.TestHelpers;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.PaymentTest.DbServiceTests
{
    [TestClass]
    public class CartAndTrxTests
    {
        [TestMethod]
        // Should return rsvNo list when retrieving by trxId
        public void Should_return_rsvNo_list_when_retrieving_by_trxId()
        {
            TestHelper.UseDb(conn =>
            {

                var trxId = "1234567890";
                var rsvNo1 = "12345";
                var rsvNo2 = "43233";
                var rsvNo3 = "65765";
                conn.Execute("DELETE FROM TrxRsv WHERE TrxId = @trxId", new {trxId});
                TrxRsvTableRepo.GetInstance().Insert(conn, new TrxRsvTableRecord {TrxId = trxId, RsvNo = rsvNo1});
                TrxRsvTableRepo.GetInstance().Insert(conn, new TrxRsvTableRecord {TrxId = trxId, RsvNo = rsvNo2});
                TrxRsvTableRepo.GetInstance().Insert(conn, new TrxRsvTableRecord {TrxId = trxId, RsvNo = rsvNo3});

                var actual = new PaymentDbService().GetTrxRsvNos(trxId);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Contains(rsvNo1));
                Assert.IsTrue(actual.Contains(rsvNo2));
                Assert.IsTrue(actual.Contains(rsvNo3));

                conn.Execute("DELETE FROM TrxRsv WHERE TrxId = @trxId", new {trxId});
            });
        }

        [TestMethod]
        // Should return empty list when there is no rsv associated with trxId
        public void Should_return_empty_list_when_there_is_no_rsv_associated_with_trxId()
        {
            TestHelper.UseDb(conn =>
            {
                var trxId = "1234567890";
                conn.Execute("DELETE FROM TrxRsv WHERE TrxId = @trxId", new {trxId});

                var actual = new PaymentDbService().GetTrxRsvNos(trxId);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Count == 0);

                conn.Execute("DELETE FROM TrxRsv WHERE TrxId = @trxId", new {trxId});
            });
        }

        [TestMethod]
        // Should return empty list when trxId does not exist
        public void Should_return_empty_list_when_trxId_does_not_exist()
        {
            TestHelper.UseDb(conn =>
            {
                var trxId = "1234567890";
                conn.Execute("DELETE FROM TrxRsv WHERE TrxId = @trxId", new {trxId});

                var actual = new PaymentDbService().GetTrxRsvNos(trxId);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Count == 0);

                conn.Execute("DELETE FROM TrxRsv WHERE TrxId = @trxId", new {trxId});
            });
        }
	
    }
}
