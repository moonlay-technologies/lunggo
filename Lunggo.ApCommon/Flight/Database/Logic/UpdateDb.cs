using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Sequence;
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
            internal static void Details(GetTripDetailsResult details)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    foreach (var segment in details.Itinerary.Trips.SelectMany(trip => trip.Segments))
                    {
                        UpdateDetailsQuery.GetInstance().Execute(conn, new
                        {
                            details.BookingId,
                            segment.DepartureAirport,
                            segment.ArrivalAirport,
                            segment.DepartureTime,
                            segment.ArrivalTime,
                            segment.Duration,
                            segment.Pnr,
                            segment.DepartureTerminal,
                            segment.ArrivalTerminal,
                            segment.Baggage
                        });
                    }
                }
            }

            internal static void CancelReservation(string rsvNo, CancellationType type)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var cancellationType = CancellationTypeCd.Mnemonic(type);
                    CancelReservationsQuery.GetInstance()
                        .Execute(conn, new { RsvNo = rsvNo, CancellationType = cancellationType });
                }
            }

            internal static void ExpireReservations()
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var minuteTimeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout"));
                    ExpireReservationsQuery.GetInstance().Execute(conn, new { MinuteTimeout = minuteTimeout });
                }
            }

            internal static bool Payment(string rsvNo, PaymentData payment)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var prevStatus = GetDb.PaymentStatus(rsvNo);
                    if (payment.Status != prevStatus)
                    {
                        var queryParam = new FlightReservationTableRecord();
                        if (rsvNo != null)
                            queryParam.RsvNo = rsvNo;
                        if (payment.Medium != PaymentMedium.Undefined)
                            queryParam.PaymentMediumCd = PaymentMediumCd.Mnemonic(payment.Medium);
                        if (payment.Method != PaymentMethod.Undefined)
                            queryParam.PaymentMethodCd = PaymentMethodCd.Mnemonic(payment.Method);
                        if (payment.Status != PaymentStatus.Undefined)
                            queryParam.PaymentStatusCd = PaymentStatusCd.Mnemonic(payment.Status);
                        if (payment.Time.HasValue)
                            queryParam.PaymentTime = payment.Time.Value.ToUniversalTime();
                        if (payment.Id != null)
                            queryParam.PaymentId = payment.Id;
                        if (payment.TargetAccount != null)
                            queryParam.PaymentTargetAccount = payment.TargetAccount;
                        if (payment.TimeLimit.HasValue)
                            queryParam.PaymentTimeLimit = payment.TimeLimit.Value.ToUniversalTime();
                        if (payment.Url != null)
                            queryParam.PaymentUrl = payment.Url;
                        if (payment.PaidAmount != null)
                            queryParam.PaidAmount = payment.PaidAmount;
                        if (payment.FinalPrice != null)
                            queryParam.FinalPrice = payment.FinalPrice;
                        UpdatePaymentQuery.GetInstance().Execute(conn, queryParam, queryParam);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            internal static void ConfirmRefund(string rsvNo, Refund refund)
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

            internal static void IssueProgress(string rsvNo, string progressMessage)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    FlightReservationTableRepo.GetInstance().Update(conn, new FlightReservationTableRecord
                    {
                        RsvNo = rsvNo,
                        IssueProgress = progressMessage
                    });
                }
            }
        }
    }
}