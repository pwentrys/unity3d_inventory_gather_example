using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles object movement.
/// </summary>
public class Movement : MonoBehaviour
{
    #region Public + Serialized Variables
    [SerializeField, Tooltip("Movement Keys. Hardcoding to make it easier to share on GitHub.")]
    private HardCodedInput _HardCodedInput;

    [SerializeField, Tooltip("How far we move when we hold down a movement key"), Range(1, 100)]
    private int _MovementValue = 1;

    [SerializeField]
    private Text HelpText; // Shows input bindings.
    #endregion

    #region Private Variables
    private Vector2 _NewPosition; // So we don't have to redefine the variable each time. Player nicer with the Garbage Collect.
    #endregion

    #region Start Methods
    /// <summary>
    /// Run on Start.
    /// </summary>
    void Start()
    {
        // This should sit in it's own class.
        if (!HelpText) HelpText = transform.root.GetChild(0).GetComponent<Text>();
        if (HelpText) HelpText.text = string.Format("<b>Movement</b>\nUp: {0}\nLeft: {1}\nDown: {2}\nRight: {3}\n\n<b>Targeting</b>\nSelect: {4} Mouse\nDeselect: {5}", _HardCodedInput.Up, _HardCodedInput.Left, _HardCodedInput.Down, _HardCodedInput.Right, _HardCodedInput.SelectTarget, _HardCodedInput.DeselectTarget);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Runs on Update AKA each frame. This is inefficient as crap but another way to handle input.
    /// </summary>
    private void Update()
    {
        // Here we check for keyboard inputs.
        if (Input.GetKey(_HardCodedInput.Up)) Move(MoveDirection.Up);
        if (Input.GetKey(_HardCodedInput.Left)) Move(MoveDirection.Left);
        if (Input.GetKey(_HardCodedInput.Down)) Move(MoveDirection.Down);
        if (Input.GetKey(_HardCodedInput.Right)) Move(MoveDirection.Right);
        // This handles clearing target. This is in here but you should move it out of here.
        if (Input.GetKeyDown(_HardCodedInput.DeselectTarget)) GetComponent<Action>().SetTarget(null);
    }

    /// <summary>
    /// Move Object
    /// </summary>
    /// <param name="Direction"></param>
    private void Move(MoveDirection Direction)
    {
        // Assign appropriate movement base on input key.
        switch (Direction)
        {
            case MoveDirection.Up:
                _NewPosition = new Vector2(transform.position.x, transform.position.y + _MovementValue);
                break;

            case MoveDirection.Left:
                _NewPosition = new Vector2(transform.position.x - _MovementValue, transform.position.y);
                break;

            case MoveDirection.Down:
                _NewPosition = new Vector2(transform.position.x, transform.position.y - _MovementValue);
                break;

            case MoveDirection.Right:
                _NewPosition = new Vector2(transform.position.x + _MovementValue, transform.position.y);
                break;

            default:
                _NewPosition = transform.position;
                break;
        }

        // Finally, update our position.
        transform.position = Vector3.Lerp(transform.position, _NewPosition, Time.deltaTime);
    }
    #endregion
}