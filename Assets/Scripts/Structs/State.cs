using UnityEngine;

/// <summary>
/// Used as an object template that States uses for each state.
/// </summary>
[System.Serializable]
internal struct State
{
    [Tooltip("If not selected, this State does not apply to the object.")]
    public bool Show;

    [Tooltip("Changes color of the image.")]
    public Color TextColor;
}