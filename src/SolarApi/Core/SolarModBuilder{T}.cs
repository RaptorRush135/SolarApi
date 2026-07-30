namespace SolarApi;

using global::MelonLoader;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SolarApi.DependencyInjection;
using SolarApi.Logging;

public class SolarModBuilder<T>
    where T : SolarMod
{
    public string ModName { get; } = typeof(T).Name;

    public virtual string? ShortName => null;

    public T Build(
        MelonMod melon,
        IGameServiceContainer gameServiceContainer)
    {
        var services = new ServiceCollection();

        this.ConfigureLogging(services, melon);
        this.ConfigureServices(services, melon, gameServiceContainer);

        var provider = this.BuildServiceProvider(services);

        var instance = ActivatorUtilities.CreateInstance<T>(provider);

        instance.PreInitialize<T>(melon, provider);

        return instance;
    }

    protected virtual void ConfigureLogging(
        IServiceCollection services,
        MelonMod melon)
    {
        services.AddLogging(
            b => this.ConfigureLogging(b, melon));
    }

    protected virtual void ConfigureLogging(
        ILoggingBuilder builder,
        MelonMod melon)
    {
        Dictionary<string, string> aliases = [];

        this.ConfigureLoggingAliases(builder, aliases);

        builder.AddProvider(
            new MelonLoggerProvider(melon.ConsoleColor, aliases));
    }

    protected virtual void ConfigureLoggingAliases(
        ILoggingBuilder builder,
        Dictionary<string, string> aliases)
    {
        if (this.ShortName != null)
        {
            aliases.Add(typeof(T).Assembly.GetName().Name!, this.ShortName);
        }
    }

    protected virtual void ConfigureServices(
        IServiceCollection services,
        MelonMod melon,
        IGameServiceContainer gameServiceContainer)
    {
        this.ConfigureHarmonyServices(services, melon);
        this.ConfigureGameServices(services, gameServiceContainer);
        this.ConfigureModServices(services);
    }

    protected virtual void ConfigureHarmonyServices(
        IServiceCollection services,
        MelonMod melon)
    {
        services.AddSingleton((_) => melon.HarmonyInstance);
    }

    protected virtual void ConfigureGameServices(
        IServiceCollection services,
        IGameServiceContainer gameServiceContainer)
    {
        var container = gameServiceContainer;
        if (container == null)
        {
            return;
        }

        foreach (var (type, service) in container.Services)
        {
            services.AddSingleton(type, service);
        }

        foreach (var (type, model) in container.Models)
        {
            services.AddSingleton(type, model);
        }

        foreach (var (type, activity) in container.Activities)
        {
            services.AddSingleton(type, activity);
        }
    }

    protected virtual void ConfigureModServices(
        IServiceCollection services)
    {
    }

    protected virtual IServiceProvider BuildServiceProvider(
        IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }
}
