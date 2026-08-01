namespace SolarApi.Utilities;

public sealed class DisposableAction(
    Action dispose)
    : IDisposable
{
    private Action? dispose = dispose;

    public void Dispose()
    {
        this.dispose?.Invoke();
        this.dispose = null;
    }
}
