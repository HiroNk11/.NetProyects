namespace LoggerArchivos.Logging
{
    internal interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
