﻿using System.Diagnostics;
using log4net.Appender;
using log4net.Core;

namespace Lunggo.Framework.SharedModel
{
    public class AzureTraceAppender : TraceAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var level = loggingEvent.Level;

            var message = RenderLoggingEvent(loggingEvent);

            if (level >= Level.Error)
            {
                Trace.TraceError(message);
            }
            else if (level >= Level.Warn)
            {
                Trace.TraceWarning(message);
            }
            else if (level >= Level.Info)
            {
                Trace.TraceInformation(message);
            }
            else
            {
                Trace.Write(message);
            }

            if (ImmediateFlush)
            {
                Trace.Flush();
            }
        }
    }
}
