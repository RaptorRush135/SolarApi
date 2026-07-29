namespace SolarApi.Logging;

using global::MelonLoader;
using global::MelonLoader.Logging;

using Microsoft.Extensions.Logging;

public sealed class MelonLoggerProvider(
    ColorARGB color)
    : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        string displayName = GetCategoryDisplay(categoryName);
        var logger = new MelonLogger.Instance(displayName, color);
        return new MelonLoggerAdapter(logger);
    }

    public void Dispose()
    {
    }

    private static string GetCategoryDisplay(string categoryName)
    {
        int firstDot = categoryName.IndexOf('.');
        if (firstDot < 0)
        {
            return categoryName;
        }

        int lastDot = categoryName.LastIndexOf('.');
        if (lastDot <= firstDot)
        {
            return categoryName;
        }

        return string.Concat(
            categoryName.AsSpan(0, firstDot),
            ":",
            categoryName.AsSpan(lastDot + 1));
    }
}
