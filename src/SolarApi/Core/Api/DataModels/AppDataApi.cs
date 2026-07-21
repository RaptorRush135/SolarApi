namespace SolarApi.Api.DataModels;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using HarmonyLib;

using Il2CppReloaded.DataModels;

using SolarApi.Events;

[HarmonyPatch]
public static class AppDataApi
{
    public static readonly OneTimeEvent<AppDataModels> Bound = new();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AppDataProvider), nameof(AppDataProvider.OnBind))]
    private static void OnBindAppDataProvider(AppDataProvider __instance)
    {
        var model = new AppDataModels(
            __instance.m_usersModel,
            __instance.m_almanacModel,
            __instance.m_levelDataModel,
            __instance.m_platformDataModel,
            __instance.m_dialogDataModel,
            __instance.m_feDataModel,
            __instance.m_runHistoryModel,
            __instance.m_achievementsModel);

        Bound.Invoke(model);
    }
}
