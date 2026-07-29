namespace SolarApi.Events;

using global::MelonLoader;

public sealed class OneTimeEvent
{
    private readonly MelonEvent @event = new();

    // TODO
    private readonly MelonLogger.Instance logger;

    public OneTimeEvent()
        : this(Melon<Core>.Logger)
    {
    }

    public OneTimeEvent(MelonLogger.Instance logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        this.logger = logger;
    }

    public bool Disposed => this.@event.Disposed;

    public bool Invoked { get; private set; }

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
            this.logger.Warning("One-time event was already invoked");
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
