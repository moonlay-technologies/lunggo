using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        internal class UpdateDb
        {
            internal static void CancelReservation(string rsvNo, CancellationType type)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var cancellationType = CancellationTypeCd.Mnemonic(type);
                    CancelReservationsQuery.GetInstance()
                        .Execute(conn, new {RsvNo = rsvNo, CancellationType = cancellationType});
                }
            }

            internal static void ExpireReservations()
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var minuteTimeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout"));
                    ExpireReservationsQuery.GetInstance().Execute(conn, new {MinuteTimeout = minuteTimeout});
                }
            }

            internal static bool Payment(string rsvNo, PaymentInfo payment)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var prevStatus = GetDb.PaymentStatus(rsvNo);
                    if (payment.Status != prevStatus)
                    {
                        var queryParam = new FlightReservationTableRecord
                        {
                            RsvNo = rsvNo,
                            PaymentMediumCd = PaymentMediumCd.Mnemonic(payment.Medium),
                            PaymentMethodCd = PaymentMethodCd.Mnemonic(payment.Method),
                            PaymentStatusCd = PaymentStatusCd.Mnemonic(payment.Status),
                            PaymentTime = payment.Time.HasValue ? payment.Time.Value.ToUniversalTime() : (DateTime?) null,
                            PaymentId = payment.Id,
                            PaymentTargetAccount = payment.TargetAccount,
                            PaymentTimeLimit = payment.TimeLimit.HasValue ? payment.TimeLimit.Value.ToUniversalTime() : (DateTime?) null,
                            PaymentUrl = payment.Url,
                            PaidAmount = payment.PaidAmount,
                            FinalPrice = payment.FinalPrice,
                        };
                        UpdatePaymentQuery.GetInstance().Execute(conn, queryParam, queryParam);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            internal static void ConfirmPayment(string rsvNo, decimal paidAmount)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    FlightReservationTableRepo.GetInstance().Update(conn, new FlightReservationTableRecord
                    {
                        PaidAmount = paidAmount,
                        RsvNo = rsvNo
                    });
                }
            }

            internal static void ConfirmRefund(string rsvNo, RefundInfo refund)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    FlightReservationTableRepo.GetInstance().Update(conn, new FlightReservationTableRecord
                    {
                        RefundAmount = refund.Amount,
                        RefundTargetBank = refund.TargetBank,
                        RefundTargetAccount = refund.TargetAccount,
                        RefundTime = refund.Time.ToUniversalTime(),
                        RsvNo = rsvNo
                    });
                }
            }

            internal static void BookingStatus(List<BookingStatusInfo> statusData)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var query = UpdateBookingStatusQuery.GetInstance();
                    var dbBookingStatusInfo = statusData.Select(info => new
                    {
                        info.BookingId,
                        BookingStatusCd = BookingStatusCd.Mnemonic(info.BookingStatus)
                    }).ToArray();
                    query.Execute(conn, dbBookingStatusInfo);
                }
            }
        }
    }
}