namespace SolarApi.Api.Activities;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.TreeStateActivities;

using SolarApi.Events;

[HarmonyPatch]
internal static class GameplayActivityApi
{
    public static readonly OneTimeEvent<GameplayActivity> Created
        = OneTimeEvent<GameplayActivity>.Create(nameof(Created), typeof(GameplayActivityApi));

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameplayActivity), nameof(GameplayActivity.Awake))]
    private static void Awake(GameplayActivity __instance)
    {
        Created.Invoke(__instance);
    }
}
