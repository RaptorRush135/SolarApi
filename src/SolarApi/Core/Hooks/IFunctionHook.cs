namespace SolarApi.Hooks;

public interface IFunctionHook
{
    bool IsHooked { get; }

    void Attach();

    void Detach();
}
