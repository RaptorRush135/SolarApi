namespace SolarApi.Logging.Extensions;

using Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    public static void LogSpacer(this ILogger logger)
        => logger.Log(LogLevel.None, default, (object?)null, null, static (_, _) => string.Empty);

    public static void LogLine(this ILogger logger, int length = 30)
        => logger.LogInformation("{Line}", new string('-', length));
}
