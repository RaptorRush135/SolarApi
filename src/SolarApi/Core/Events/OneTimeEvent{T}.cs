namespace SolarApi.Events;

using System.Diagnostics.CodeAnalysis;

using global::MelonLoader;

using Microsoft.Extensions.Logging;

public sealed class OneTimeEvent<T>
{
    private readonly MelonEvent<T> @event = new();

    private readonly ILogger logger;

    public OneTimeEvent(string name, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        this.Name = name;
        this.logger = logger;
    }

    public string Name { get; }

    public T? Value { get; private set; }

    public bool Disposed => this.@event.Disposed;

    [MemberNotNullWhen(true, nameof(Value))]
    public bool Invoked { get; private set; }

    public void Subscribe(Action<T> action)
    {
        if (this.Disposed)
        {
            return;
        }

        if (this.Invoked)
        {
            action.Invoke(this.Value);
            return;
        }

        this.@event.Subscribe(new(action));
    }

    public void Invoke(T value)
    {
        if (this.Invoked)
        {
            this.logger.LogWarning(
                "One-time event '{EventName}' was already invoked ({TypeName})",
                this.Name,
                typeof(T).Name);

            return;
        }

        if (this.Disposed)
        {
            return;
        }

        this.Value = value;
        this.Invoked = true;
        this.@event.Invoke(value);
    }

    internal static OneTimeEvent<T> Create(string name, Type type)
        => new(name, SolarLogger.Factory.CreateLogger(type));
}
