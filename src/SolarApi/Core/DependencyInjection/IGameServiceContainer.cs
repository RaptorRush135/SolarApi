namespace SolarApi.DependencyInjection;

using Il2CppInterop.Runtime.InteropTypes;

using Il2CppTekly.DataModels.Models;

public interface IGameServiceContainer
{
    IReadOnlyDictionary<Type, Il2CppObjectBase> Services { get; }

    IReadOnlyDictionary<Type, ModelBase> Models { get; }
}
