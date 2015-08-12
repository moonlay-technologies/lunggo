﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.HtmlTemplate
{
    public enum HtmlTemplateType
    {
        FlightEticket,
        FlightInvoice,
        FlightEticketEmail,
        FlightChangedEticketEmail,
        FlightInstantPaymentNotifEmail,
        FlightPendingPaymentNotifEmail,
        FlightPendingPaymentConfirmedNotifEmail,
        FlightPendingPaymentExpiredNotifEmail,
        UserConfirmationEmail,
        ForgotPasswordEmail
    }
}
