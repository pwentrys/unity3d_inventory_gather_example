using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This is strictly to easily require all components on a player. Make sure to assign everything manually.
/// You can also use this to extend everything out nicely and assign other things to the player object.
/// </summary>
[RequireComponent(typeof(Action))]
[RequireComponent(typeof(Movement))]
// This means the script executes in Edit mode. READ UP THOROUGLY ABOUT THIS BEFORE EVER USING IT.
[ExecuteInEditMode]
public class Player : MonoBehaviour
{
    #region Private Variables
    private Action _Action;
    private Movement _Movement;
    #endregion

    #region Public + Serialized Variables
    /// <summary>
    /// This is used purely for the fun of creating a random test subject name each time.
    /// </summary>
    [Header("Optional Fun")]
    [Tooltip("Used to randomly assign a name on each run.")]
    [SerializeField]
    private Text _Name;
    #endregion

    #region Start Methods
    /// <summary>
    /// Runs on Awake.
    /// </summary>
    private void Awake()
    {
        AssignObjects();
    }

    /// <summary>
    /// Runs on Start.
    /// </summary>
    private void Start()
    {
        RandomName();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Assigns objects if they don't exist.
    /// </summary>
    private void AssignObjects()
    {
        if (!_Action) _Action = GetComponent<Action>();
        if (!_Movement) _Movement = GetComponent<Movement>();
    }

    /// <summary>
    /// Assigns random number to name each time script is run.
    /// </summary>
    private void RandomName()
    {
        if (_Name) _Name.text = string.Format("Test Subject #{0}", Random.Range(1000, 9999));
    }
    #endregion
}