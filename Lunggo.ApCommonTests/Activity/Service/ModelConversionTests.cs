using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Product.Constant;
using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommonTests.Activity.Service
{
    [TestClass]
    public partial class ActivityServiceTests
    {
        [TestMethod]
        public void ConvertToReservationForDisplay_null_returnNull()
        {
            ActivityReservation input = null;
            var result = ActivityService.GetInstance().ConvertToReservationForDisplay(input);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ConvertToReservationForDisplay_valid_returnConvertedRsv()
        {
            var input = new ActivityReservation()
            {
                RsvNo = "1234",
                RsvTime = new DateTime(2000 - 01 - 01),
                Payment = null,
                ActivityDetails = new ActivityDetail(),
                Pax = new List<Pax>(),
                Contact = new Contact(),
                RsvStatus = RsvStatus.Undefined
            };

            var actualResult = ActivityService.GetInstance().ConvertToReservationForDisplay(input);

            var expectedResult = new ActivityReservationForDisplay()
            {
                RsvNo = "1234",
                RsvTime = new DateTime(2000 - 01 - 01),
                RsvDisplayStatus = RsvDisplayStatus.Undefined
            };

            Assert.AreEqual(expectedResult.RsvNo, actualResult.RsvNo);
            Assert.AreEqual(expectedResult.RsvTime, actualResult.RsvTime);
        }

        [TestMethod]
        public void ConvertToActivityDetailForDisplay_null_returnNull()
        {
            ActivityDetail input = null;
            var result = ActivityService.GetInstance().ConvertToActivityDetailForDisplay(input);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ConvertToActivityDetailForDisplay_valid_returnConvertedAct()
        {
            var input = new ActivityDetail()
            {
                ActivityId = 1
            };

            var actualResult = ActivityService.GetInstance().ConvertToActivityDetailForDisplay(input);

            var expectedResult = new ActivityDetailForDisplay()
            {
                ActivityId = 1
            };

            Assert.AreEqual(expectedResult.ActivityId, actualResult.ActivityId);
        }

        [TestMethod]
        public void ConvertToActivityDetailForDisplay_ListofNull_returnNull()
        {
            List<ActivityDetail> input = null;
            var result = ActivityService.GetInstance().ConvertToActivityDetailForDisplay(input);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ConvertToActivityDetailForDisplay_ListofValidInput_returnListofConvertedAct()
        {
            var input = new List<ActivityDetail>()
            {
                new ActivityDetail(){ActivityId = 1},
                new ActivityDetail(){ActivityId = 2}
            };

            var actualResults = ActivityService.GetInstance().ConvertToActivityDetailForDisplay(input);

            var expectedResult = new List<ActivityDetailForDisplay>()
            {
                new ActivityDetailForDisplay(){ActivityId = 1},
                new ActivityDetailForDisplay(){ActivityId = 2}
            };

            for (var i = 0; i < actualResults.Count ; i++)
            {
                Assert.AreEqual(expectedResult[i].ActivityId, actualResults[i].ActivityId);
            }
        }

        //[TestMethod]
        //public void MapReservationStatus_RsvStatusUndefined_returnUndefined()
        //{
        //    ActivityReservation input = new ActivityReservation()
        //    {
        //        RsvStatus = RsvStatus.MethodNotSet,
        //        Payment = new PaymentDetails()
        //        {
        //            Status = PaymentStatus.MethodNotSet,
        //            Method = PaymentMethod.Indomaret
        //        }
        //    };

        //    var actualResult = ActivityService.MapReservationStatus(input);
        //    Assert.AreEqual(RsvDisplayStatus.MethodNotSet, actualResult);
        //}

        [TestMethod]
        public void MapReservationStatus_RsvStatusCancelled_returnCancelled()
        {
            ActivityReservation input = new ActivityReservation()
            {
                RsvStatus = RsvStatus.Cancelled,
                Payment = new PaymentDetails() { Status = PaymentStatus.MethodNotSet }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.Cancelled, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_PaymentStatusCancelled_returnCancelled()
        {
            ActivityReservation input = new ActivityReservation()
            {
                Payment = new PaymentDetails() { Status = PaymentStatus.Cancelled }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.Cancelled, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_RsvStatusExpired_returnExpired()
        {
            ActivityReservation input = new ActivityReservation()
            {
                RsvStatus = RsvStatus.Expired,
                Payment = new PaymentDetails() { Status = PaymentStatus.MethodNotSet }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.Expired, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_PaymentStatusExpired_returnExpired()
        {
            ActivityReservation input = new ActivityReservation()
            {
                Payment = new PaymentDetails() { Status = PaymentStatus.Expired }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.Expired, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_PaymentStatusDenied_returnPaymentDenied()
        {
            ActivityReservation input = new ActivityReservation()
            {
                Payment = new PaymentDetails() { Status = PaymentStatus.Denied }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.PaymentDenied, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_PaymentStatusFailed_returnFailedUnpaid()
        {
            ActivityReservation input = new ActivityReservation()
            {
                Payment = new PaymentDetails() { Status = PaymentStatus.Failed }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.FailedUnpaid, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_RsvStatusFailedPaymentStatusSettled_returnFailedPaid()
        {
            ActivityReservation input = new ActivityReservation()
            {
                RsvStatus = RsvStatus.Failed,
                Payment = new PaymentDetails()
                {
                    Status = PaymentStatus.Settled
                }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.FailedPaid, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_RsvStatusFailedPaymentStatusNotSettled_returnFailedUnpaid()
        {
            ActivityReservation input = new ActivityReservation()
            {
                RsvStatus = RsvStatus.Failed,
                Payment = new PaymentDetails()
                {
                    Status = PaymentStatus.Failed                }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.FailedUnpaid, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_PaymentMethodUndefined_returnReserved()
        {
            ActivityReservation input = new ActivityReservation()
            {
                Payment = new PaymentDetails() { Method = PaymentMethod.Undefined }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.Reserved, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_PaymentStatusSettledRsvStatusCompleted_returnIssued()
        {
            ActivityReservation input = new ActivityReservation()
            {
                RsvStatus = RsvStatus.Completed,
                Payment = new PaymentDetails()
                {
                    Status = PaymentStatus.Settled,
                    Method = PaymentMethod.CreditCard
                }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.Issued, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_PaymentStatusSettledRsvStatusNotCompleted_returnPaid()
        {
            ActivityReservation input = new ActivityReservation()
            {
                RsvStatus = RsvStatus.InProcess,
                Payment = new PaymentDetails()
                {
                    Status = PaymentStatus.Settled,
                    Method = PaymentMethod.CreditCard
                }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.Paid, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_PaymentStatusNotSettledPaymentMethodVirtualAccount_returnPendingPayment()
        {
            ActivityReservation input = new ActivityReservation()
            {
                Payment = new PaymentDetails()
                {
                    Status = PaymentStatus.MethodNotSet,
                    Method = PaymentMethod.VirtualAccount
                }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.PendingPayment, actualResult);
        }

        [TestMethod]
        public void MapReservationStatus_PaymentStatusNotSettledPaymentMethodBankTransfer_returnPendingPayment()
        {
            ActivityReservation input = new ActivityReservation()
            {
                Payment = new PaymentDetails()
                {
                    Status = PaymentStatus.MethodNotSet,
                    Method = PaymentMethod.BankTransfer
                }
            };

            var actualResult = ActivityService.MapReservationStatus(input);

            Assert.AreEqual(RsvDisplayStatus.PendingPayment, actualResult);
        }
    }
}
