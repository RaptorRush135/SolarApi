namespace SolarApi.Collections;

public sealed class DisposeGroup(
    int capacity)
    : IDisposable
{
    private readonly List<IDisposable> disposables = new(capacity);

    public DisposeGroup()
        : this(0)
    {
    }

    public T Collect<T>(T disposable)
        where T : IDisposable
    {
        this.disposables.Add(disposable);
        return disposable;
    }

    public void Dispose()
    {
        foreach (var disposable in this.disposables)
        {
            disposable.Dispose();
        }
    }
}
