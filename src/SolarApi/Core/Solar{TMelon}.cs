namespace SolarApi;

using MelonLoader;

using SolarApi.Api.DependencyInjection;
using SolarApi.DependencyInjection;

public static class Solar<TMelon>
    where TMelon : MelonMod
{
    public static void RegisterMod<TBuilder>()
        where TBuilder : ISolarModBuilder, new()
    {
        RegisterMod(new TBuilder());
    }

    public static void RegisterMod(ISolarModBuilder builder)
    {
        Melon<Core>.Logger.Msg($"Registered solar mod: '{builder.ModName}'");

        GameServiceContainerApi.Ready.Subscribe(
            container => Build(builder, container));
    }

    private static void Build(
        ISolarModBuilder builder,
        IGameServiceContainer container)
    {
        var melon = Melon<TMelon>.Instance;

        var mod = builder
            .WithGameServiceContainer(container)
            .WithConsoleColor(melon.ConsoleColor)
            .Build();

        try
        {
            mod.Initialize();
        }
        catch (Exception ex)
        {
            mod.Deinitialize(ex);
        }
    }
}
