namespace SolarApi;

using global::MelonLoader;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SolarApi.Api.DependencyInjection;
using SolarApi.DependencyInjection;

public static class Solar<TMod>
    where TMod : SolarMod
{
    private static readonly ILogger Logger = SolarLogger.Factory.CreateLogger(typeof(Solar<TMod>));

    public static TMod Instance
    {
        get
        {
            return field
                ?? throw new InvalidOperationException(
                    $"No registered mod of type {typeof(TMod).FullName} was found.");
        }
        private set;
    }

    public static IServiceProvider Provider => Instance.Provider;

    public static ILogger<TCategoryName> GetLogger<TCategoryName>()
        => Provider.GetRequiredService<ILogger<TCategoryName>>();

    public static void RegisterMod<TBuilder>(MelonMod melon)
        where TBuilder : SolarModBuilder<TMod>, new()
    {
        RegisterMod(melon, new TBuilder());
    }

    public static void RegisterMod(MelonMod melon, SolarModBuilder<TMod> builder)
    {
        if (!melon.MelonAssembly.HarmonyDontPatchAll)
        {
            throw new InvalidOperationException(
                "HarmonyDontPatchAll must be true." +
                " Solar applies Harmony patches after the game's services become available.");
        }

        Logger.LogInformation("Registered solar mod: '{ModName}'", builder.ModName);

        GameServiceContainerApi.Ready.Subscribe(
            container => Build(melon, builder, container));
    }

    private static void Build(
        MelonMod melon,
        SolarModBuilder<TMod> builder,
        IGameServiceContainer container)
    {
        Logger.LogInformation("Building solar mod: '{ModName}'", builder.ModName);

        if (!melon.Registered)
        {
            Logger.LogWarning(
                "Cannot build mod because {MelonTypeName}" +
                " is either not registered or has already been unloaded.",
                melon.MelonTypeName);

            return;
        }

        TMod mod = builder.Build(melon, container);

        Instance = mod;

        try
        {
            mod.Initialize();
        }
        catch (Exception ex)
        {
            mod.OnInitializationFailure(ex);
            return;
        }

        melon.OnUnregister.Subscribe(
            () => mod.Deinitialize(),
            unsubscribeOnFirstInvocation: true);
    }
}
