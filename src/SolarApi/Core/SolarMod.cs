namespace SolarApi;

using Microsoft.Extensions.Logging;

public abstract class SolarMod
{
    public ILogger Logger { get; private set; } = null!;

    public void Deinitialize(Exception? exception)
        => this.OnDeinitialize(exception);

    internal void PreInitialize(ILogger logger)
        => this.Logger = logger;

    internal void Initialize()
        => this.OnInitialize();

    protected abstract void OnInitialize();

    protected virtual void OnDeinitialize(Exception? exception)
    {
        this.Logger.LogError(exception, "Failed to initialize");
    }
}
