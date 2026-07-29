namespace SolarApi.Logging;

using global::MelonLoader;
using global::MelonLoader.Logging;

using Microsoft.Extensions.Logging;

public sealed class MelonLoggerProvider(
    ColorARGB color,
    IReadOnlyDictionary<string, string>? aliases = null)
    : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        string displayName = this.GetCategoryDisplay(categoryName);
        var logger = new MelonLogger.Instance(displayName, color);
        return new MelonLoggerAdapter(logger);
    }

    public void Dispose()
    {
    }

    private string GetCategoryDisplay(string categoryName)
    {
        int firstDot = categoryName.IndexOf('.');
        if (firstDot < 0)
        {
            return aliases?.GetValueOrDefault(categoryName) ?? categoryName;
        }

        int lastDot = categoryName.LastIndexOf('.');

        string first = categoryName[..firstDot];
        string last = categoryName[(lastDot + 1)..];

        first = aliases?.GetValueOrDefault(first) ?? first;
        last = aliases?.GetValueOrDefault(last) ?? last;

        return $"{first}:{last}";
    }
}
