using System;
using NLog;
using Pactor.Infra.Crosscutting.Log;

namespace Pactor.Infra.Crosscutting.LogCore
{
    public class NLogLogger : ILog
    {
        private readonly ILogger _logger;

        public NLogLogger(ILogger logger)
        {
            _logger = logger;
        }

        public ILogger Logger => _logger;

        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        public bool IsWarnEnabled => _logger.IsWarnEnabled;

        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        public void Debug(Func<object> callback)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(callback());
        }

        public void Debug(Func<object> callback, Exception exception)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(exception, callback().ToString());
        }

        public void Debug(object message)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(exception, message.ToString());
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(format, args);
        }

        public void DebugFormat(string format, object arg0)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(format, arg0);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(format, arg0, arg1, arg2);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(provider, format, args);
        }

        public void Info(Func<object> callback)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(callback());
        }

        public void Info(Func<object> callback, Exception exception)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(exception, callback().ToString());
        }

        public void Info(object message)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(exception, message.ToString());
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(format, args);
        }

        public void InfoFormat(string format, object arg0)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(format, arg0, arg1, arg2);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(provider, format, args);
        }

        public void Warn(Func<object> callback)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(callback());
        }

        public void Warn(Func<object> callback, Exception exception)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(exception, callback().ToString());
        }

        public void Warn(object message)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(exception, message.ToString());
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(format, args);
        }

        public void WarnFormat(string format, object arg0)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(format, arg0, arg1, arg2);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(provider, format, args);
        }

        public void Error(Func<object> callback)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(callback());
        }

        public void Error(Func<object> callback, Exception exception)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(exception, callback().ToString());
        }

        public void Error(object message)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(exception, message.ToString());
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(format, args);
        }

        public void ErrorFormat(string format, object arg0)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(provider, format, args);
        }

        public void Fatal(Func<object> callback)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(callback());
        }

        public void Fatal(Func<object> callback, Exception exception)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(exception, callback().ToString());
        }

        public void Fatal(object message)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(exception, message.ToString());
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(format, args);
        }

        public void FatalFormat(string format, object arg0)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(format, arg0);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(format, arg0, arg1, arg2);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(provider, format, args);
        }
    }
}