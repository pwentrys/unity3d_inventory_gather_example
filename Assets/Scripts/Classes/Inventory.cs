using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines an object's inventory.
/// </summary>
[System.Serializable]
internal class Inventory
{
    #region Public + Serialized Variables
    [Tooltip("What is the most we can have in our inventory."), Range(0.1f, 100f)]
    public float Capacity = 1; // Inventory capacity is 1 by default if it is not changed in the editor.

    [HideInInspector]
    public float Current = 0;  // You don't really need to see this in the inspector. If you do for debugging, either comment out the line below or open the look at it in debug mode.

    [SerializeField]
    public Text _Text;         // Used to visually show current load.
    #endregion

    #region Event Handlers

    /// <summary>
    /// Runs each time State.UpdateStates runs via a delegate that's assigned in Object & Player.
    /// </summary>
    public void UpdateInventory()
    {
        _Text.text = string.Format("Load: {0}", Current);
    }

    #endregion

    #region Inspector Methods
    /// <summary>
    /// Checks if this inventory is empty.
    /// </summary>
    public bool IsEmpty { get { return (Current <= 0); } }

    /// <summary>
    /// Checks if this inventory is full.
    /// </summary>
    public bool IsFull { get { return (Current >= Capacity); } }

    #endregion
}