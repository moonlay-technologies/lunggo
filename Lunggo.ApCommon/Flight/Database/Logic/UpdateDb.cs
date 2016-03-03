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

            internal static bool Payment(string rsvNo, Payment.Model.PaymentData paymentData)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var prevStatus = GetDb.PaymentStatus(rsvNo);
                    if (paymentData.Status != prevStatus)
                    {
                        var queryParam = new FlightReservationTableRecord
                        {
                            RsvNo = rsvNo,
                            PaymentMediumCd = PaymentMediumCd.Mnemonic(paymentData.Medium),
                            PaymentMethodCd = PaymentMethodCd.Mnemonic(paymentData.Method),
                            PaymentStatusCd = PaymentStatusCd.Mnemonic(paymentData.Status),
                            PaymentTime = paymentData.Time.HasValue ? paymentData.Time.Value.ToUniversalTime() : (DateTime?) null,
                            PaymentId = paymentData.Id,
                            PaymentTargetAccount = paymentData.TargetAccount,
                            PaymentTimeLimit = paymentData.TimeLimit.HasValue ? paymentData.TimeLimit.Value.ToUniversalTime() : (DateTime?) null,
                            PaymentUrl = paymentData.Url,
                            PaidAmount = paymentData.PaidAmount,
                            FinalPrice = paymentData.FinalPrice,
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