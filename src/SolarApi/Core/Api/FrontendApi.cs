namespace SolarApi.Api;

using HarmonyLib;

using Il2CppReloaded.TreeStateActivities;

using SolarApi.Events;

[HarmonyPatch]
internal static class FrontendApi
{
    public static readonly OneTimeEvent OnFirstActivation
        = OneTimeEvent.Create(nameof(OnFirstActivation), typeof(FrontendApi));

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FrontendActivity), nameof(FrontendActivity.ActiveStarted))]
    private static void ActiveStarted()
    {
        if (!OnFirstActivation.Invoked)
        {
            OnFirstActivation.Invoke();
        }
    }
}
