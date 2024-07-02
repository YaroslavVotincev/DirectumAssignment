using System.Diagnostics;

namespace Logger
{
    public class LoggerFactory
    {
        private ILoggerEngine? engine;
        private LogMessageSeverity logLevel = LogMessageSeverity.Info;

        public LoggerFactory(ILoggerEngine? engine = null, LogMessageSeverity? logLevel = null)
        {
            this.engine = engine;
            this.logLevel = logLevel ?? LogMessageSeverity.Debug;
        }

        public Logger Create()
        {
            if (engine == null)
            {
                engine = new Log4NetEngine();
            }
            return new Logger(engine, logLevel);
        }
        public LoggerFactory WithLogLevel(LogMessageSeverity logLevel)
        {
            this.logLevel = logLevel;
            return this;
        }
        public LoggerFactory WithEngine(ILoggerEngine engine)
        {
            this.engine = engine;
            return this;
        }
    }

    public class Logger : ILogger
    {
        private ILoggerEngine engine;
        private LogMessageSeverity logLevel;
        private string? source;

        public Logger(ILoggerEngine engine, LogMessageSeverity logLevel = LogMessageSeverity.Info)
        {
            this.engine = engine;
            this.logLevel = logLevel;
            bool isRelease = true;
#if DEBUG
            isRelease = false;
#endif
            if (isRelease && logLevel == LogMessageSeverity.Debug)
            {
                this.logLevel = LogMessageSeverity.Info;
            }
        }

        public void LogException(string source, Exception exception, bool isCritical)
        {
            var stack = new System.Diagnostics.StackTrace();
            if (isCritical)
            {
                this.engine.Error(source ?? "Unknown", $"[CRITICAL]\n{stack}{exception}");
            }
            else
            {
                this.engine.Error(source ?? "Unknown", $"[EXCEPTION]\n{stack}{exception}");
            }
        }

        public void LogMessage(string source, string text, LogMessageSeverity severity)
        {
            if (severity < logLevel) return;
            switch (severity)
            {
                case LogMessageSeverity.Debug:
                    this.engine.Debug(source, text);
                    break;
                case LogMessageSeverity.Info:
                    this.engine.Info(source, text);
                    break;
                case LogMessageSeverity.Warning:
                    this.engine.Warning(source, text);
                    break;
                case LogMessageSeverity.Error:
                    this.engine.Error(source, text);
                    break;
            }
        }

        public void Debug(string message)
        {
            if (this.logLevel == LogMessageSeverity.Debug)
            {
                ClassSource();
                LogMessage(this.source ?? "Unknown", message, LogMessageSeverity.Debug);
            }

        }

        public void Info(string message)
        {
            if (this.logLevel <= LogMessageSeverity.Info)
            {
                ClassSource();
                LogMessage(this.source ?? "Unknown", message, LogMessageSeverity.Info);
            }
        }

        public void Warning(string message)
        {
            if (this.logLevel <= LogMessageSeverity.Warning)
            {
                ClassSource();
                LogMessage(this.source ?? "Unknown", message, LogMessageSeverity.Warning);
            }
        }

        public void Error(string message)
        {
            ClassSource();
            LogMessage(this.source ?? "Unknown", message, LogMessageSeverity.Error);
        }

        public void Exception(Exception exception, bool isCritical = false)
        {
            ClassSource();
            var stack = new System.Diagnostics.StackTrace();
            if (isCritical)
            {
                this.engine.Error(this.source ?? "Unknown", $"[CRITICAL EXCEPTION]\n{stack}{exception}");
            }
            else
            {
                this.engine.Error(this.source ?? "Unknown", $"[EXCEPTION]\n{stack}{exception}");
            }
        }

        private void ClassSource()
        {
            if (this.source == null)
            {
                StackTrace stackTrace = new StackTrace();
                var frame = stackTrace.GetFrame(2);
                if (frame == null)
                {
                    this.source = "Unknown";
                    return;
                }
                var method = frame.GetMethod();
                if (method == null)
                {
                    this.source = "Unknown";
                    return;
                }
                var type = method.DeclaringType;
                if (type == null)
                {
                    this.source = "Unknown";
                    return;
                }
                this.source = type.Name;
            }
        }
    }
}
