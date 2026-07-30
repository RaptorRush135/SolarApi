namespace SolarApi.Api.DependencyInjection;

using Il2CppReloaded;

using SolarApi.Api.Activities;
using SolarApi.Api.DataModels;
using SolarApi.DependencyInjection;
using SolarApi.Events;

internal static class GameServiceContainerApi
{
    public static readonly OneTimeEvent<GameServiceContainer> Ready
        = OneTimeEvent<GameServiceContainer>.Create(nameof(Ready), typeof(GameServiceContainerApi));

    static GameServiceContainerApi()
    {
        FrontendApi.OnFirstActivation.Subscribe(TryFire);
    }

    private static void TryFire()
    {
        if (Ready.Invoked)
        {
            throw new InvalidOperationException(
                "Required services were already resolved.");
        }

        var bootDataModels = BootDataApi.Bound.Value;
        var appDataModels = AppDataApi.Bound.Value;
        var gameplayActivity = GameplayActivityApi.Created.Value;

        if (bootDataModels is null || appDataModels is null || gameplayActivity is null)
        {
            throw new InvalidOperationException(
                "Missing required services.");
        }

        var container = new GameServiceContainer(
            AppCore.s_appContainer,
            [.. bootDataModels.Items, .. appDataModels.Items],
            [gameplayActivity]);

        Ready.Invoke(container);
    }
}
