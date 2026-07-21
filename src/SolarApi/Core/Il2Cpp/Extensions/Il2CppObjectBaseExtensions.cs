namespace SolarApi.Il2Cpp.Extensions;

using Il2CppInterop.Runtime.InteropTypes;

internal static class Il2CppObjectBaseExtensions
{
    public static Il2CppObjectBase Cast(this Il2CppObjectBase obj, Type type)
    {
        var method = typeof(Il2CppObjectBase)
            .GetMethod(nameof(Il2CppObjectBase.Cast))!
            .MakeGenericMethod(type);

        return (Il2CppObjectBase)method.Invoke(obj, parameters: null)!;
    }
}
