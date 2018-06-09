using System;
using NLog;

namespace Pactor.Infra.Crosscutting.LogCore
{
    public static class LogBootstrap
    {
        public static void Configure(string configFilePath = null)
        {
            LogManager.LoadConfiguration(configFilePath ?? "nlog.config");
        }

        public static Log.ILog GetLogger(string loggerName)
        {
            return new NLogLogger(LogManager.GetLogger(loggerName));
        }

        public static Log.ILog GetLogger(Type type)
        {
            return new NLogLogger(LogManager.GetLogger(type.FullName));
        }
    }
}
