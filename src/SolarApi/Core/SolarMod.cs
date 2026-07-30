namespace SolarApi;

using global::MelonLoader;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public abstract class SolarMod
{
    public IServiceProvider Provider { get; private set; } = null!;

    public ILogger Logger { get; private set; } = null!;

    public bool Disposed { get; private set; }

    private MelonMod Melon { get; set; } = null!;

    public void Deinitialize()
        => this.OnDeinitialize();

    internal void PreInitialize<TMod>(MelonMod melon, IServiceProvider provider)
        where TMod : SolarMod
    {
        this.Melon = melon;
        this.Provider = provider;
        this.Logger = provider.GetRequiredService<ILogger<TMod>>();
    }

    internal void Initialize()
    {
        this.HarmonyInit();
        this.OnInitialize();
    }

    internal virtual void OnInitializationFailure(Exception exception)
    {
        this.Logger.LogError(exception, "Failed to initialize");
        this.Deinitialize();
    }

    protected abstract void OnInitialize();

    protected virtual void OnDeinitialize()
    {
        if (this.Disposed)
        {
            return;
        }

        this.Disposed = true;

        this.Melon.Unregister();

        if (this.Provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void HarmonyInit()
    {
        foreach (var type in this.Melon.MelonAssembly.Assembly.GetValidTypes())
        {
            try
            {
                this.Melon.HarmonyInstance.CreateClassProcessor(type, false).Patch();
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Failed to HarmonyInit PatchAll: {TypeName}", type.FullName);
            }
        }
    }
}
