namespace SolarApi.Unity;

using global::MelonLoader;

using UnityEngine;

/// <summary>
/// Marks a <see cref="GameObject"/> that should be activated once after it has been loaded.
/// The marker component removes itself after activation.
/// </summary>
/// <param name="ptr">The native IL2CPP object pointer.</param>
[RegisterTypeInIl2Cpp]
public sealed class RequiresActivationMarker(IntPtr ptr)
    : MonoBehaviour(ptr)
{
    /// <summary>
    /// Activates the specified <see cref="GameObject"/>
    /// if it contains a <see cref="RequiresActivationMarker"/> component.
    /// <para>
    /// If the marker is not present, this method does nothing.
    /// </para>
    /// </summary>
    /// <param name="gameObject">The <see cref="GameObject"/> to activate if required.</param>
    public static void ActivateIfRequired(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<RequiresActivationMarker>(out var marker))
        {
            marker.Activate();
        }
    }

    /// <summary>
    /// Activates the associated <see cref="GameObject"/> and removes this marker.
    /// </summary>
    public void Activate()
    {
        this.gameObject.SetActive(true);
        Destroy(this);
    }
}
