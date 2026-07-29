namespace SolarApi.DependencyInjection;

using HarmonyLib;

using Il2CppInterop.Runtime.InteropTypes;

using Il2CppTekly.DataModels.Models;
using Il2CppTekly.Injectors;
using Il2CppTekly.Localizations;

using SolarApi.Collections.Extensions;
using SolarApi.Il2Cpp.Extensions;

[HarmonyPatch]
internal sealed class GameServiceContainer : IGameServiceContainer
{
    public GameServiceContainer(InjectorContainer container, IEnumerable<ModelBase> models)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(models);

        this.Services = CreateServiceMap();
        this.Models = models.ToDictionary(m => m.GetType());

        IReadOnlyDictionary<Type, Il2CppObjectBase> CreateServiceMap()
        {
            var serviceMap = container.m_instances.AsEnumerable()
                .Where(kvp => kvp.Value.TryCast<SingletonProvider>() != null)
                .Select(kvp =>
                {
                    var interopType = kvp.Key.GetInteropType();
                    var instance = kvp.Value.Instance.Cast(interopType);
                    return KeyValuePair.Create(interopType, instance);
                })
                .ToDictionary();

            serviceMap[typeof(ILocalizer)] = GetLocalizer();

            return serviceMap;
        }
    }

    public IReadOnlyDictionary<Type, Il2CppObjectBase> Services { get; }

    public IReadOnlyDictionary<Type, ModelBase> Models { get; }

    private static ILocalizer GetLocalizer()
    {
        var localizerType = Il2CppSystem.Type.GetType("Tekly.Localizations.Localizer, Tekly.Localizations.Runtime")
            ?? throw new TypeLoadException("Failed to get Localizer type.");

        const string InstancePropertyName = nameof(Localizer.Instance);
        var instanceProperty = localizerType.BaseType.GetProperty(InstancePropertyName)
            ?? throw new MissingMemberException(localizerType.BaseType.FullName, InstancePropertyName);

        return instanceProperty.GetValue(obj: null).Cast<ILocalizer>();
    }
}
