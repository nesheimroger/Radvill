using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace Radvill.Logger
{
    public static class Log
    {
        private static readonly ILog _log;
        static Log()
        {
            _log = LogManager.GetLogger(Configuration.Logging.LoggerName);
        }

        /* Log a message object */
        public static void Debug(object message)
        {
            _log.Debug(message);
        }
        public static void Info(object message)
        {
            _log.Info(message);
        }
        public static void Warn(object message)
        {
            _log.Warn(message);
        }
        public static void Error(object message)
        {
            _log.Error(message);
        }
        public static void Fatal(object message)
        {
            _log.Fatal(message);
        }

        /* Log a message object and exception */
        public static void Debug(object message, Exception t)
        {
            _log.Debug(message, t);
        }
        public static void Info(object message, Exception t)
        {
            _log.Info(message, t);
        }
        public static void Warn(object message, Exception t)
        {
            _log.Warn(message, t);
        }
        public static void Error(object message, Exception t)
        {
            _log.Error(message, t);
        }
        public static void Fatal(object message, Exception t)
        {
            _log.Error(message, t);
        }

        /* Log a message string using the System.String.Format syntax */
        public static void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }
        public static void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }
        public static void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }
        public static void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }
        public static void FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }

        /* Log a message string using the System.String.Format syntax */
        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.DebugFormat(provider, format, args);
        }
        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.InfoFormat(provider, format, args);
        }
        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.WarnFormat(provider, format, args);
        }
        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.ErrorFormat(provider, format, args);
        }
        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.FatalFormat(provider, format, args);
        }
    }
}
