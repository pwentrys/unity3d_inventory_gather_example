using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles stats of an item.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))] // Required for trigger.
public class ItemStats : MonoBehaviour, IPointerDownHandler
{
    #region Public + Serialized Variables

    [SerializeField]
    private Inventory _Inventory;       // Inventory object.

    [Space]
    [Header("Object State Handler")]
    [SerializeField]
    private States _States;             // Changes color / text depending on status.

    #endregion Public + Serialized Variables

    #region Start Methods

    /// <summary>
    /// Filling up inventory to maximum capacity so we have something to receive.
    /// Also to States updates.
    /// </summary>
    private void OnEnable()
    {
        _Inventory.Current = _Inventory.Capacity;
        
        _States.UpdateState += UpdateState;
        _States.UpdateState += _Inventory.UpdateInventory;
    }

    /// <summary>
    /// Runs at Start.
    /// </summary>
    private void Start()
    {
        Action(0); //Just to make sure everything is working.
    }

    #endregion Start Methods
    
    #region Public Methods
    /// <summary>
    /// Get pointer data on click.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) { Debug.Log("LMB"); }
        if (eventData.button == PointerEventData.InputButton.Right) { FindObjectOfType<Player>().GetComponent<Action>().SetTarget(gameObject); }
        if (eventData.button == PointerEventData.InputButton.Middle) { Debug.Log("MMB"); }
    }

    /// <summary>
    /// Check for if our inventory is empty.
    /// </summary>
    [HideInInspector]
    public bool IsEmpty { get { UpdateState(); return (_Inventory.IsEmpty); } }

    /// <summary>
    /// Check for if our inventory is full.
    /// </summary>
    [HideInInspector]
    public bool IsFull { get { UpdateState(); return (_Inventory.IsFull); } }

    /// <summary>
    /// This allows us to add or negate amounts. If you pass through a negative number, it'll take away. If you pass a positive, it'll add.
    /// </summary>
    /// <param name="Amount">Value we're taking / adding to current load.</param>
    /// <returns></returns>
    public float Action(float Amount)
    {
        // The fun if statement maths.
        float Deposited = 0;

        if (Amount > 0) Deposited = (Amount + _Inventory.Current > _Inventory.Capacity) ? Amount - ((Amount + _Inventory.Current) - _Inventory.Capacity) : Amount;
        else if (Amount < 0) Deposited = (_Inventory.Current + Amount <= 0) ? Amount - (_Inventory.Current + Amount) : Amount;
        else Deposited = Amount;

        // Remember, if you want to take something from this item, you have to pass through "-Amount".
        _Inventory.Current += Deposited;
        _States.UpdateState();
        return Deposited;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Displays State Color. Didn't want to put any time into splitting this up (ex: to make a backpack for inventory / etc.) so we're returning out of the check once any are true.
    /// </summary>
    private void UpdateState()
    {
        if (!_Inventory.IsEmpty && !_Inventory.IsFull && _States.HasLoad.Show) { _States.InventorystateText.color = _States.HasLoad.TextColor; _States.InventorystateText.text = "Has Load"; return; }
        if (_Inventory.IsFull && _States.Full.Show) { _States.InventorystateText.color = _States.Full.TextColor; _States.InventorystateText.text = "Full"; }
        if (_Inventory.IsEmpty && _States.Empty.Show) { _States.InventorystateText.color = _States.Empty.TextColor; _States.InventorystateText.text = "Empty"; }
    }

    #endregion Private Methods

    #region End Methods

    /// <summary>
    /// Unsubscribe from Update State.
    /// </summary>
    private void OnDisable()
    {
        _States.UpdateState -= UpdateState;
        _States.UpdateState -= _Inventory.UpdateInventory;
    }

    #endregion End Methods
}