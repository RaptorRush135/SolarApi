namespace SolarApi;

using global::MelonLoader;

using Microsoft.Extensions.Logging;

using SolarApi.Logging;

internal static class SolarLogger
{
    public static ILoggerFactory Factory => field ??= CreateFactory();

    public static ILoggerFactory CreateFactory()
    {
        var solarMelon = Melon<Core>.Instance;

        var provider = new MelonLoggerProvider(solarMelon.ConsoleColor);

        var factory = LoggerFactory.Create(
            builder => builder.AddProvider(provider));

        solarMelon.OnUnregister.Subscribe(
            factory.Dispose,
            unsubscribeOnFirstInvocation: true);

        return factory;
    }
}
