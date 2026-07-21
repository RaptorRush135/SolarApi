namespace SolarApi;

using MelonLoader;
using MelonLoader.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SolarApi.DependencyInjection;
using SolarApi.Logging;

public class SolarModBuilder<T>
    : ISolarModBuilder
    where T : SolarMod
{
    public ColorARGB ConsoleColor { get; set; } = MelonLogger.DefaultMelonColor;

    public IGameServiceContainer? GameServiceContainer { get; set; }

    public string ModName { get; } = typeof(T).Name;

    public SolarModBuilder<T> WithConsoleColor(ColorARGB color)
    {
        this.ConsoleColor = color;
        return this;
    }

    public SolarModBuilder<T> WithGameServiceContainer(IGameServiceContainer? container)
    {
        this.GameServiceContainer = container;
        return this;
    }

    SolarMod ISolarModBuilder.Build()
        => this.Build();

    ISolarModBuilder ISolarModBuilder.WithConsoleColor(ColorARGB color)
        => this.WithConsoleColor(color);

    ISolarModBuilder ISolarModBuilder.WithGameServiceContainer(IGameServiceContainer? container)
        => this.WithGameServiceContainer(container);

    public T Build()
    {
        var services = new ServiceCollection();

        this.ConfigureLogging(services);
        this.ConfigureServices(services);

        var provider = this.BuildServiceProvider(services);

        var logger = provider.GetRequiredService<ILogger<T>>();

        var instance = ActivatorUtilities.CreateInstance<T>(provider);

        instance.PreInitialize(logger);

        return instance;
    }

    protected virtual void ConfigureServices(
        IServiceCollection services)
    {
        this.ConfigureHarmonyServices(services);
        this.ConfigureGameServices(services);
    }

    protected virtual void ConfigureHarmonyServices(
        IServiceCollection services)
    {
        // TODO
        services.AddSingleton((_) => new HarmonyLib.Harmony($"{typeof(SolarMod).Assembly.FullName}:Mod"));
    }

    protected virtual void ConfigureGameServices(
        IServiceCollection services)
    {
        var container = this.GameServiceContainer;
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
    }

    protected virtual void ConfigureLogging(
        IServiceCollection services)
    {
        services.AddLogging(this.ConfigureLogging);
    }

    protected virtual void ConfigureLogging(
        ILoggingBuilder builder)
    {
        builder.AddProvider(
            new MelonLoggerProvider(this.ConsoleColor));
    }

    protected virtual IServiceProvider BuildServiceProvider(
        IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }
}
