using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace Lunggo.Framework.Core
{
    public class LunggoLogger
    {
        private ILog Log;
        private static LunggoLogger instance = new LunggoLogger();


        private LunggoLogger()
        {
            
        }
        public void init(ILog log)
        {
            Log = log;
        }
        public static LunggoLogger GetInstance()
        {
            return instance;
        }


        public static void Debug(object message)
        {
            instance.Log.Debug(message);
        }

        public static void Debug(object message, Exception exception)
        {
            instance.Log.Debug(message, exception);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            instance.Log.DebugFormat(format, args);
        }

        public static void DebugFormat(string format, object arg0)
        {
            instance.Log.DebugFormat(format, arg0);
        }

        public static void DebugFormat(string format, object arg0, object arg1)
        {
            instance.Log.DebugFormat(format, arg0, arg1);
        }

        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            instance.Log.DebugFormat(format, arg0, arg1, arg2);
        }

        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            instance.Log.DebugFormat(provider, format, args);
        }

        public static void Info(object message)
        {
            instance.Log.Info(message);
        }

        public static void Info(object message, Exception exception)
        {
            instance.Log.Info(message, exception);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            instance.Log.InfoFormat(format, args);
        }

        public static void InfoFormat(string format, object arg0)
        {
            instance.Log.InfoFormat(format, arg0);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            instance.Log.InfoFormat(format, arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            instance.Log.InfoFormat(format, arg0, arg1, arg2);
        }

        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            instance.Log.InfoFormat(provider, format, args);
        }

        public static void Warn(object message)
        {
            instance.Log.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
            instance.Log.Warn(message, exception);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            instance.Log.WarnFormat(format, args);
        }

        public static void WarnFormat(string format, object arg0)
        {
            instance.Log.WarnFormat(format, arg0);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            instance.Log.WarnFormat(format, arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            instance.Log.WarnFormat(format, arg0, arg1, arg2);
        }

        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            instance.Log.WarnFormat(provider, format, args);
        }

        public static void Error(object message)
        {
            instance.Log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            instance.Log.Error(message, exception);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            instance.Log.ErrorFormat(format, args);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            instance.Log.ErrorFormat(format, arg0);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            instance.Log.ErrorFormat(format, arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            instance.Log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            instance.Log.ErrorFormat(format, format, args);
        }

        public static void Fatal(object message)
        {
            instance.Log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            instance.Log.Fatal(message, exception);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            instance.Log.FatalFormat(format, args);
        }

        public static void FatalFormat(string format, object arg0)
        {
            instance.Log.FatalFormat(format, arg0);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            instance.Log.FatalFormat(format, arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            instance.Log.FatalFormat(format, arg0, arg1, arg2);
        }

        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            instance.Log.FatalFormat(provider, format, args);
        }

        public static bool IsDebugEnabled {
            get { return instance.Log.IsDebugEnabled; }
        }
        public static bool IsInfoEnabled
        {
            get { return instance.Log.IsInfoEnabled; }
        }
        public static bool IsWarnEnabled
        {
            get { return instance.Log.IsWarnEnabled; }
        }
        public static bool IsErrorEnabled
        {
            get { return instance.Log.IsErrorEnabled; }
        }
        public static bool IsFatalEnabled
        {
            get { return instance.Log.IsFatalEnabled; }
        }
    }
}
