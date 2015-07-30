using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles user keyboard input for Movement and Target Selection.
/// </summary>
[System.Serializable]
internal struct HardCodedInput
{
    //Hardcoding to make it easy to share on GH. NEVER DO THIS.
    [Header("Manually Defined Movement Keys")]
    [SerializeField]
    public KeyCode Up;

    [SerializeField]
    public KeyCode Left;

    [SerializeField]
    public KeyCode Down;

    [SerializeField]
    public KeyCode Right;

    [Space,Header("Manually Defined Target Keys")]
    public PointerEventData.InputButton SelectTarget;
    public KeyCode DeselectTarget;
}