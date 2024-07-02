namespace Logger
{
    public interface ILogger
    {
        void LogMessage(string source, string text, LogMessageSeverity severity);
        void LogException(string source, Exception exception, bool isCritical);
    }

    public enum LogMessageSeverity
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public interface ILoggerEngine
    {
        void Debug(string source, string text);
        void Info(string source, string text);
        void Warning(string source, string text);
        void Error(string source, string text);

        void Shutdown();
    }

}

