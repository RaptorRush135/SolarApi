namespace SolarApi.Api.DataModels;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.DataModels;

using SolarApi.Events;

[HarmonyPatch]
internal static class BootDataApi
{
    public static readonly OneTimeEvent<BootDataModels> Bound = new();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BootDataProvider), nameof(BootDataProvider.OnBind))]
    private static void OnBindBootDataProvider(BootDataProvider __instance)
    {
        var model = new BootDataModels(
            __instance.m_settingsDataModel,
            __instance.m_controllerDataModel,
            __instance.m_loadingDataModel,
            __instance.m_inputDataModel);

        Bound.Invoke(model);
    }
}
