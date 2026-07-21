namespace SolarApi.Api.DependencyInjection;

using Il2CppReloaded;

using SolarApi.Api.DataModels;
using SolarApi.DependencyInjection;
using SolarApi.Events;

internal static class GameServiceContainerApi
{
    public static readonly OneTimeEvent<GameServiceContainer> Ready = new();

    private static BootDataModels? bootDataModels;

    private static AppDataModels? appDataModels;

    private static bool fired;

    static GameServiceContainerApi()
    {
        BootDataApi.Bound.Subscribe(value => OnResolve(ref bootDataModels, value));
        AppDataApi.Bound.Subscribe(value => OnResolve(ref appDataModels, value));
    }

    private static void OnResolve<T>(ref T field, T value)
    {
        field = value;
        TryFire();
    }

    private static void TryFire()
    {
        if (fired)
        {
            throw new InvalidOperationException(
                "Required models were already resolved.");
        }

        if (bootDataModels is null || appDataModels is null)
        {
            return;
        }

        fired = true;

        var container = new GameServiceContainer(
            AppCore.s_appContainer,
            [.. bootDataModels.Items, .. appDataModels.Items]);

        Ready.Invoke(container);
    }
}
