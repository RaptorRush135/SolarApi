namespace SolarApi.MelonLoader;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using System.Reflection;

using global::MelonLoader;

using HarmonyLib;

using Microsoft.Extensions.Logging;

using SolarApi;

public sealed class SanityCheckDetourBypass : IDisposable
{
    private static readonly ILogger<SanityCheckDetourBypass> Logger
        = SolarLogger.Factory.CreateLogger<SanityCheckDetourBypass>();

    private static readonly MethodInfo? Target = AccessTools.Method(
       "MelonLoader.CoreClrUtils.CoreClrDelegateFixer:SanityCheckDetour");

    private static readonly HarmonyMethod PatchMethod = AccessTools.Method(
        typeof(SanityCheckDetourBypass), nameof(CheckBypassPatch))
            .ToNewHarmonyMethod();

    private readonly Harmony harmony;

    public SanityCheckDetourBypass(Harmony harmony)
    {
        ArgumentNullException.ThrowIfNull(harmony);

        this.harmony = harmony;

        if (Target == null)
        {
            Logger.LogWarning("Failed to locate SanityCheckDetour");
            return;
        }

        PatchShieldBypass.ExecuteWithBypass(
            () => this.harmony.Patch(Target, prefix: PatchMethod));
    }

    public void Dispose()
    {
        if (Target != null)
        {
            PatchShieldBypass.ExecuteWithBypass(
                () => this.harmony.Unpatch(Target, PatchMethod.method));
        }
    }

    private static bool CheckBypassPatch(ref bool __result)
    {
        __result = true;
        return false;
    }
}
