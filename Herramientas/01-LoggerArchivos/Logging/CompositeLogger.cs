using System.Collections.Generic;

namespace LoggerArchivos.Logging
{
    internal class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CompositeLogger(IEnumerable<ILogger> loggers)
        {
            _loggers = new List<ILogger>(loggers);
        }

        public void Log(LogLevel level, string message)
        {
            foreach (ILogger logger in _loggers)
            {
                logger.Log(level, message);
            }
        }
    }
}
