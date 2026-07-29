namespace SolarApi.Hooks;

public interface IFunctionHook : IDisposable
{
    bool IsHooked { get; }

    void Attach();

    void Detach();
}
