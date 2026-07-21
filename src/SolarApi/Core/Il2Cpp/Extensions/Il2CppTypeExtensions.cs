namespace SolarApi.Il2Cpp.Extensions;

using Il2CppSystem.Reflection;

internal static class Il2CppTypeExtensions
{
    public static Type GetInteropType(this Il2CppSystem.Type type)
    {
        string interopTypeName = GetInteropTypeName(type);
        string interopAssemblyName = GetInteropAssemblyName(type.Assembly);

        return Type.GetType($"{interopTypeName}, {interopAssemblyName}")
            ?? throw new InvalidOperationException(
                $"Could not find System.Type for Il2Cpp type: {type.AssemblyQualifiedName}");

        static string GetInteropTypeName(Il2CppSystem.Type type)
        {
            string fullName = type.FullName;
            bool isInNamespace = fullName.Contains('.');
            return isInNamespace
                ? $"Il2Cpp{fullName}"
                : $"Il2Cpp.{fullName}";
        }

        static string GetInteropAssemblyName(Assembly assembly)
        {
            string name = assembly.GetName().Name;
            return name == "Assembly-CSharp"
                ? name :
                $"Il2Cpp{name}";
        }
    }
}
