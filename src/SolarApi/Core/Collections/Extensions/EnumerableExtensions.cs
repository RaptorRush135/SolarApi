namespace SolarApi.Collections.Extensions;

public static class EnumerableExtensions
{
    public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(
        this IEnumerable<KeyValuePair<TKey, TElement>> pairs)
        where TKey : notnull
    {
        return new(pairs);
    }
}
