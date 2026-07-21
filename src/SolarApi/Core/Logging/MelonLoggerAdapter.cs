namespace SolarApi.Logging;

using MelonLoader;

using Microsoft.Extensions.Logging;

public sealed class MelonLoggerAdapter(
    MelonLogger.Instance logger) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Debug or LogLevel.Trace => MelonDebug.IsEnabled(),
            _ => true,
        };
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!this.IsEnabled(logLevel))
        {
            return;
        }

        if (logLevel == LogLevel.None && state is null)
        {
            logger.WriteSpacer();
            return;
        }

        string message = formatter(state, exception);
        if (exception != null)
        {
            message += "\n" + exception;
        }

        switch (logLevel)
        {
            case LogLevel.Trace:
                logger.Msg($"[TRACE] {message}");
                break;
            case LogLevel.Debug:
                logger.Msg($"[DEBUG] {message}");
                break;
            case LogLevel.Information:
                logger.Msg(message);
                break;
            case LogLevel.Warning:
                logger.Warning(message);
                break;
            case LogLevel.Error:
                logger.Error(message);
                break;
            case LogLevel.Critical:
                logger.BigError(message);
                break;
        }
    }
}
