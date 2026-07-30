namespace SolarApi.Unity.Extensions;

using System.Diagnostics.CodeAnalysis;

using UnityEngine;
using UnityEngine.AddressableAssets;

using UnityObject = UnityEngine.Object;

/// <summary>
/// Provides extension methods for working with Unity objects.
/// </summary>
public static class UnityExtensions
{
    private const string UnityNullJustification =
        "UnityEngine.Object does not support ?. or ?? for detached objects; explicit null check required.";

    extension(GameObject)
    {
        /// <summary>
        /// Finds a <see cref="GameObject"/> in the active scene by name.
        /// </summary>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>The matching <see cref="GameObject"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no <see cref="GameObject"/> with the specified name exists in the active scene.
        /// </exception>
        public static GameObject FindOrThrow(string name)
            => GameObject.Find(name).Ref()
            ?? throw new InvalidOperationException(
                $"'{name}' was not found in the scene.");
    }

    /// <summary>
    /// Finds a child <see cref="Transform"/> by name.
    /// </summary>
    /// <param name="transform">
    /// The parent <see cref="Transform"/> to search.
    /// </param>
    /// <param name="name">
    /// The name or relative path of the child transform to find.
    /// </param>
    /// <returns>
    /// The matching child <see cref="Transform"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no matching child <see cref="Transform"/> exists.
    /// </exception>
    public static Transform FindOrThrow(this Transform transform, string name)
        => transform.Find(name).Ref()
        ?? throw new InvalidOperationException(
            $"'{name}' was not found in the transform.");

    /// <summary>
    /// Returns <see langword="null"/> for Unity objects that have been destroyed,
    /// allowing standard C# null operators to be used.
    /// </summary>
    /// <typeparam name="T">The Unity object type.</typeparam>
    /// <param name="obj">The object to normalize.</param>
    /// <returns>
    /// The object if it is still valid; otherwise, <see langword="null"/>.
    /// </returns>
    [SuppressMessage(
        "Style",
        "IDE0029:Use coalesce expression",
        Justification = UnityNullJustification)]
    [SuppressMessage(
        "Roslynator",
        "RCS1084:Use coalesce expression instead of conditional expression",
        Justification = UnityNullJustification)]
    public static T? Ref<T>(this T? obj)
        where T : UnityObject
    {
        return obj != null ? obj : null;
    }

    /// <summary>
    /// Creates an <see cref="AssetReferenceGameObject"/> that immediately resolves to the specified
    /// <see cref="GameObject"/> without requiring it to be loaded through Addressables.
    /// </summary>
    /// <param name="obj">The <see cref="GameObject"/> to wrap.</param>
    /// <returns>
    /// An <see cref="AssetReferenceGameObject"/> backed by a completed operation containing the specified object.
    /// </returns>
    public static AssetReferenceGameObject ToAssetReference(this GameObject obj)
    {
        var handle = Addressables.ResourceManager.CreateCompletedOperation(obj, string.Empty);
        return new AssetReferenceGameObject(string.Empty)
        {
            OperationHandle = handle,
        };
    }
}
