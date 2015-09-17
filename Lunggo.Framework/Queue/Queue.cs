namespace Lunggo.Framework.Queue
{
    public enum Queue
    {
        FlightEticket,
        FlightEticketEmail,
        FlightChangedEticket,
        FlightChangedEticketEmail,
        FlightInstantPaymentNotif,
        FlightPendingPaymentNotif,
        FlightPendingPaymentConfirmedNotif,
        FlightPendingPaymentExpiredNotif,
        FlightCrawl,
        UserConfirmationEmail,
        ForgotPasswordEmail,
        InitialSubscriberEmail,
        VoucherEmail
    }
}