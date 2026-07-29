namespace SolarApi.Api.DependencyInjection;

using System.Diagnostics.CodeAnalysis;

using Il2CppReloaded;

using SolarApi.Api.DataModels;
using SolarApi.DependencyInjection;
using SolarApi.Events;

internal static class GameServiceContainerApi
{
    public static readonly OneTimeEvent<GameServiceContainer> Ready = new();

    private static BootDataModels? bootDataModels;

    private static AppDataModels? appDataModels;

    [SuppressMessage(
        "Minor Code Smell",
        "S3963:\"static\" fields should be initialized inline",
        Justification = "False positive (https://github.com/SonarSource/sonar-dotnet/issues/9656).")]
    static GameServiceContainerApi()
    {
        BootDataApi.Bound.Subscribe(value => bootDataModels = value);
        AppDataApi.Bound.Subscribe(value => appDataModels = value);
        FrontendApi.OnFirstActivation.Subscribe(TryFire);
    }

    private static void TryFire()
    {
        if (Ready.Invoked)
        {
            throw new InvalidOperationException(
                "Required models were already resolved.");
        }

        if (bootDataModels is null || appDataModels is null)
        {
            throw new InvalidOperationException(
                "Missing required models.");
        }

        var container = new GameServiceContainer(
            AppCore.s_appContainer,
            [.. bootDataModels.Items, .. appDataModels.Items]);

        Ready.Invoke(container);
    }
}
