namespace SolarApi;

using MelonLoader.Logging;

using SolarApi.DependencyInjection;

public interface ISolarModBuilder
{
    string ModName { get; }

    // TODO: Refactor?
    ISolarModBuilder WithConsoleColor(ColorARGB color);

    ISolarModBuilder WithGameServiceContainer(IGameServiceContainer? container);

    SolarMod Build();
}
