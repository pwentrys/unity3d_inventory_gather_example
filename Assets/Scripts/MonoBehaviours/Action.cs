using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles any action which allows an object to take or give something to another object.
/// Calling this "Action" for the sake of easily reusing this for any other type of action you want to do that has the same type of template.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))] // We need these for OnTriggerEnter2D to work.
public class Action: MonoBehaviour
{
    #region Private Variables
    private Target _Target = new Target();    // Our target object.
    #endregion

    #region Public + Serialized Variables
    public bool IsRunning { get; private set; } // Outside classes can check to see if action is running, but can only be set by this instance of this class. Just comment out [HideInInspector] or check it out in Debug if you want to see it.

    [SerializeField, Range(0.25f, 60f), Header("Manually Defined Action Parameters")]
    private float _ActionRepeatDelay = 1f;     // This allows you to set it in the Inspector using a value slider, while keeping the variable private.

    [SerializeField, Tooltip("Amount that is taken / given each time action runs."), Range(0.25f, 60f)]
    private float _AmountPerAction = 1f;       // How much you get each time the action runs. This should be done in a different way, but this is the easiest for you to get a handle of to start off with.

    [Space, SerializeField]
    private Inventory _Inventory; // Our Inventory. Serializable since you're going to tinker with it in the Editor.
    [Space, Header("Object State Handler"), SerializeField]
    private States _States;     // Change color and text depending on status.
    [SerializeField]
    private Text _TargetText;
    #endregion

    #region Listener Sub + Unsub
    // IT IS SUPER IMPORTANT YOU ALWAYS SUB AND UNSUB.
    /// <summary>
    /// Sub.
    /// </summary>
    void OnEnable() {
        _States.UpdateState += UpdateState;
        _States.UpdateState += _Inventory.UpdateInventory;
    }

    /// <summary>
    /// Unsub.
    /// </summary>
    void OnDisable()
    {
        _States.UpdateState -= UpdateState;
        _States.UpdateState -= _Inventory.UpdateInventory;
    }
    #endregion

    #region Start Methods
    /// <summary>
    /// Runs on Start.
    /// </summary>
    void Start()
    {
        SetTarget(null);
    }
    #endregion

    #region Trigger Listener Methods
    /// <summary>
    /// Now we do the trigger magic.
    /// </summary>
    /// <param name="collider">Collider object.</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        // GTFO if we're already running or we have no target.
        if (IsRunning || !_Target.GO) return;
        
        //TODO Take this out. Debugging purposes to make sure this works.
        bool m_Matches = (collider.gameObject == _Target.GO);
        #if UNITY_EDITOR
        Debug.LogFormat(string.Format("<b><color=#{0}>OnTriggerEnter2D</color></b> - <i><color=#{1}>{2}</color></i>", ColorUtility.ToHtmlStringRGBA(Color.white), m_Matches ? ColorUtility.ToHtmlStringRGBA(Color.green) : ColorUtility.ToHtmlStringRGBA(Color.red), m_Matches ? "Target DOES Match Collider" : "Target DOES NOT Matches Collider"));
        #endif

        // If our target is empty, we reset our taget.
        if (_Target.Stats.IsEmpty) { _Target = new Target(); }

        // If the collider we hit matches our taget, let 'er rip!
        if (collider.gameObject == _Target.GO)
        {
            // Tick that we're now running.
            IsRunning = true;
            _States.UpdateState();
            // Start the invoke. We will start it in _ActionRepeatDelay seconds and repeat every _ActionRepeatDelay seconds.
            InvokeRepeating("RunAction", _ActionRepeatDelay, _ActionRepeatDelay);
        }
    }

    /// <summary>
    /// Runs on exit trigger.
    /// </summary>
    /// <param name="collider">Collider object.</param>
    // Fail safe for leaving while gathering.
    void OnTriggerExit2D(Collider2D collider)
    {
        // If we don't have a target and we're running OR if we're running and we're exiting the target, leave.
        if ((!_Target.GO && IsRunning) || (IsRunning && collider.gameObject == _Target.GO))
        {
            IsRunning = false;
            CancelInvoke();
            _States.UpdateState();
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Set target to game object.
    /// </summary>
    /// <param name="GO">Target Game Object.</param>
    public void SetTarget(GameObject GO)
    {
        if (_TargetText)
        {
            if (GO) _TargetText.text = GO.name;
            else _TargetText.text = "None";
        }

        if (GO)
        {
            _Target.GO = GO;
            _Target.Stats = GO.GetComponent<ItemStats>();
        }

        _States.UpdateState();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Run Action Invoke for "Gathering", or whatever action may be.
    /// </summary>
    void RunAction()
    {
        // I'm tired at this point, so enjoy the lazy double check. This ensures that the object still has goodies for us since the last time we ran this AND we don't have to wait _ActionRepeatDelay before finding out that the object is empty.
        InvokeContinueCheck();
        // 
        //
        // REMEMBER, IT NEEDS TO BE NEGATIVE SINCE WE'RE TAKING AWAY FROM THEIR INVENTORY. SINCE WE'RE ADDING TO OUR OWN, WE HAVE TO NEGATE THAT AS WELL
        //
        //
        _Inventory.Current += -_Target.Stats.Action(-_AmountPerAction);
        InvokeContinueCheck();
    }

    /// <summary>
    /// Here we check to see if we should cancel the invoke.
    /// </summary>
    private void InvokeContinueCheck()
    {
        // If target doesn't have any good left for us OR we're full, we stop in the invoke and set isRunning to false;
        // The way I use CancelInvoke cancels every invoke running on this current instance of this class, which shouldn't matter because you shouldn't be gathering two things at once.
        if (_Target.Stats.IsEmpty || _Inventory.IsFull) { IsRunning = false; CancelInvoke(); }

        _States.UpdateState();
    }

    /// <summary>
    /// Displays State Colors and labels.
    /// </summary>
    private void UpdateState()
    {
        if (!IsRunning && _States.Idle.Show) { _States.ActionStateText.color = _States.Idle.TextColor; _States.ActionStateText.text = "Idle"; }
        if (IsRunning && _States.Busy.Show) { _States.ActionStateText.color = _States.Busy.TextColor; _States.ActionStateText.text = "Busy"; }
        if (!_Inventory.IsEmpty && !_Inventory.IsEmpty && _States.HasLoad.Show) { _States.InventorystateText.color = _States.HasLoad.TextColor; _States.InventorystateText.text = "Has Load"; return; }
        if (_Inventory.IsFull && _States.Full.Show) { _States.InventorystateText.color = _States.Full.TextColor; _States.InventorystateText.text = "Full"; }
        if (_Inventory.IsEmpty && _States.Empty.Show) { _States.InventorystateText.color = _States.Empty.TextColor; _States.InventorystateText.text = "Empty"; }
    }
    #endregion
}