using log4net.Config;
using log4net;
using NLog;

namespace Logger
{
    public class NLogEngine : ILoggerEngine
    {
        public NLogEngine()
        {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("App.config");
        }

        public void Debug(string source, string text)
        {
            NLog.LogManager.GetLogger(source).Debug(text);
        }
        public void Info(string source, string text)
        {
            NLog.LogManager.GetLogger(source).Info(text);
        }
        public void Warning(string source, string text)
        {
            NLog.LogManager.GetLogger(source).Warn(text);
        }
        public void Error(string source, string text)
        {
            NLog.LogManager.GetLogger(source).Error(text);
        }
        public void Shutdown()
        {
            NLog.LogManager.Shutdown();
        }
    }
    public class Log4NetEngine : ILoggerEngine
    {
        public Log4NetEngine()
        {
            XmlConfigurator.Configure();
        }

        public void Debug(string source, string text)
        {
            log4net.LogManager.GetLogger(source).Debug(text);
        }
        public void Info(string source, string text)
        {
            log4net.LogManager.GetLogger(source).Info(text);
        }
        public void Warning(string source, string text)
        {
            log4net.LogManager.GetLogger(source).Warn(text);
        }
        public void Error(string source, string text)
        {
            log4net.LogManager.GetLogger(source).Error(text);
        }
        public void Shutdown()
        {
            log4net.LogManager.Shutdown();
        }
    }
}