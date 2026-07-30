namespace SolarApi.Unity;

using Microsoft.Extensions.Logging;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using ILogger = Microsoft.Extensions.Logging.ILogger;

/// <summary>
/// Provides helpers for loading prefab assets from Addressables.
/// </summary>
public static class PrefabLoader
{
    private static readonly ILogger Logger = SolarLogger.Factory.CreateLogger(typeof(PrefabLoader));

    /// <summary>
    /// Loads the prefab asset referenced by an <see cref="AssetReferenceGameObject"/>.
    /// <para>
    /// The returned <see cref="GameObject"/> is the prefab asset itself, not an instantiated prefab instance.
    /// </para>
    /// </summary>
    /// <param name="reference">
    /// The asset reference that points to the prefab.
    /// </param>
    /// <param name="expectLoaded">
    /// <see langword="true"/> to log a warning if the prefab asset was not already loaded;
    /// otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>The loaded prefab asset.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reference"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The prefab asset could not be loaded.
    /// </exception>
    public static GameObject LoadPrefabReference(
        AssetReferenceGameObject reference,
        bool expectLoaded)
    {
        ArgumentNullException.ThrowIfNull(reference);

        AsyncOperationHandle handle = reference.OperationHandle.IsValid()
           ? reference.OperationHandle
           : reference.LoadAssetAsync<GameObject>();

        bool isLoaded = handle.IsDone;
        if (!isLoaded)
        {
            handle.WaitForCompletion();
        }

        if (handle.Result == null)
        {
            throw new InvalidOperationException(
                $"Failed to load prefab from AssetReference. AssetGUID: '{reference.AssetGUID}'.");
        }

        var result = handle.Result.Cast<GameObject>();

        if (expectLoaded && !isLoaded)
        {
            Logger.LogWarning("Expected prefab '{ResultName}' to be loaded", result.name);
        }

        return result;
    }
}
