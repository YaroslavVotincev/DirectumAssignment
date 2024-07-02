using log4net;
using log4net.Config;
using Logger;

namespace Assignment
{
    internal class Program
    {
        public class SomeClass
        {
            private Logger.Logger logger;

            public SomeClass(Logger.Logger logger)
            {
                this.logger = logger;
            }

            public void DoSomethingDebug()
            {
                logger.Debug("Doing something, debug message");
            }

            public void DoSomethingInfo()
            {
                logger.Info("Doing something, info message");
            }

            public void DoSomethingWarning()
            {
                logger.Warning("Doing something, warning message");
            }

            public void DoSomethingError()
            {
                logger.Error("Doing something, error message");
            }

            public void DoSomethingException()
            {
                try
                {
                    throw new Exception("Test Exception Message");
                }
                catch (Exception e)
                {
                    logger.Exception(e, true);
                }
            }

        }

        static void Main(string[] args)
        {
            Console.WriteLine("NLog Engine/Core:");
            var NLogLogger = new Logger.LoggerFactory()
                .WithEngine(new NLogEngine())
                .WithLogLevel(LogMessageSeverity.Debug)
                .Create();

            NLogLogger.Debug("NLog debug message");
            NLogLogger.Info("NLog info message");
            NLogLogger.Warning("NLog warning message");
            NLogLogger.Error("NLog error message");
            try
            {
                var a = 1;
                var b = 0;
                var c = a / b;
            }
            catch (Exception e)
            {
                NLogLogger.Exception(e);
            }

            Console.WriteLine("");
            Console.WriteLine("Log4Net Engine/Core:");
            var Log4NetLogger = new Logger.LoggerFactory()
                .WithEngine(new Log4NetEngine())
                .WithLogLevel(LogMessageSeverity.Debug)
                .Create();

            var tempClass = new SomeClass(Log4NetLogger);
            tempClass.DoSomethingDebug();
            tempClass.DoSomethingInfo();
            tempClass.DoSomethingWarning();
            tempClass.DoSomethingError();
            tempClass.DoSomethingException();

        }
    }
}
