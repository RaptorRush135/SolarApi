namespace SolarApi.Events;

using global::MelonLoader;

using Microsoft.Extensions.Logging;

public sealed class OneTimeEvent
{
    private readonly MelonEvent @event = new();

    private readonly ILogger logger;

    public OneTimeEvent(string name, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(logger);

        this.Name = name;
        this.logger = logger;
    }

    public string Name { get; }

    public bool Disposed => this.@event.Disposed;

    public bool Invoked { get; private set; }

    public static OneTimeEvent Create(string name, Type type)
        => new(name, SolarLogger.Factory.CreateLogger(type));

    public void Subscribe(Action action)
    {
        if (this.Disposed)
        {
            return;
        }

        if (this.Invoked)
        {
            action.Invoke();
            return;
        }

        this.@event.Subscribe(new(action));
    }

    public void Invoke()
    {
        if (this.Invoked)
        {
            this.logger.LogWarning(
                "One-time event '{EventName}' was already invoked",
                this.Name);

            return;
        }

        if (this.Disposed)
        {
            return;
        }

        this.Invoked = true;
        this.@event.Invoke();
    }
}
