namespace SolarApi.Unity;

using SolarApi.Unity.Extensions;

using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// Creates inactive clones of prefab assets while preventing their lifecycle
/// callbacks (such as <c>Awake</c> and <c>Start</c>) from executing during instantiation.
/// <para>
/// This emulates Unity's behavior when instantiating from a prefab asset rather than a scene object.
/// </para>
/// </summary>
public static class AssetPrefabCloner
{
    /// <summary>
    /// Loads a prefab asset from an <see cref="AssetReferenceGameObject"/> and
    /// creates a clone while suppressing lifecycle callbacks during instantiation.
    /// <para>
    /// The returned clone is inactive and is marked for deferred activation via <see cref="RequiresActivationMarker"/>.
    /// </para>
    /// </summary>
    /// <param name="reference">
    /// The asset reference that identifies the prefab asset.
    /// </param>
    /// <param name="expectLoaded">
    /// <see langword="true"/> to log a warning if the prefab asset was not already loaded;
    /// otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>
    /// A clone of the referenced prefab asset with activation deferred.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reference"/> is <see langword="null"/>.
    /// </exception>
    public static GameObject Clone(
        AssetReferenceGameObject reference,
        bool expectLoaded = false)
    {
        ArgumentNullException.ThrowIfNull(reference);

        GameObject prefab = PrefabLoader.LoadPrefabReference(reference, expectLoaded);

        return Clone(prefab);
    }

    /// <summary>
    /// Creates a clone of a prefab asset while suppressing lifecycle callbacks during instantiation.
    /// <para>
    /// The returned clone is inactive and is marked for deferred activation via <see cref="RequiresActivationMarker"/>.
    /// </para>
    /// </summary>
    /// <param name="prefab">The prefab asset to clone.</param>
    /// <returns>An inactive clone of the prefab asset.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="prefab"/> is <see langword="null"/>.
    /// </exception>
    public static GameObject Clone(
        GameObject prefab)
    {
        ArgumentNullException.ThrowIfNull(prefab.Ref());

        prefab.SetActive(false);

        var clone = Object.Instantiate(prefab);

        try
        {
            clone.AddComponent<RequiresActivationMarker>();
            Object.DontDestroyOnLoad(clone);
        }
        finally
        {
            prefab.SetActive(true);
        }

        return clone;
    }
}
