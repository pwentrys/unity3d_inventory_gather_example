using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to visually display all object states.
/// </summary>
[System.Serializable]
internal struct States
{
    // All of the possible states
    [Header("Manually Defined Object State Params")]
    [Header("Action State")]
    public State Idle;

    public State Busy;

    [Tooltip("Display Text for State")]
    public Text ActionStateText;

    [Header("Inventory State")]
    public State Empty;

    public State HasLoad;
    public State Full;

    [Tooltip("Display Text for State")]
    public Text InventorystateText;

    public delegate void UpdateStateDelegate(); // Delegate that is used to fire multiple methods in an async manner.
    public UpdateStateDelegate UpdateState;
}