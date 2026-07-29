namespace SolarApi.Collections.Extensions;

public static class KeyValuePairExtensions
{
    public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(
        this IEnumerable<KeyValuePair<TKey, TElement>> pairs)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(pairs);

        return new(pairs);
    }

    public static IEnumerable<KeyValuePair<TKey, TValueNew>> SelectValue<TKey, TValue, TValueNew>(
        this IEnumerable<KeyValuePair<TKey, TValue>> pairs,
        Func<TValue, TValueNew> selector)
    {
        ArgumentNullException.ThrowIfNull(pairs);
        ArgumentNullException.ThrowIfNull(selector);

        return pairs.Select(pair => pair.WithValue(selector));
    }

    public static KeyValuePair<TKey, TValueNew> WithValue<TKey, TValue, TValueNew>(
        this KeyValuePair<TKey, TValue> pair,
        Func<TValue, TValueNew> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return KeyValuePair.Create(pair.Key, selector(pair.Value));
    }
}
