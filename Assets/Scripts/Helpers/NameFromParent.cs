using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// If no target is set, sets text to parent name.
/// </summary>
public class NameFromParent : MonoBehaviour {
    #region Public + Serialized Methods
    [Header("Optional - Parent")]
    [Tooltip("If no target is set, sets text to parent name.")]
    [SerializeField]
    private GameObject ParentToInheritNameFrom;
    #endregion

    #region Start Methods
    /// <summary>
    /// Runs on Start.
    /// </summary>
    void Start () {
        SetText();
	}
    #endregion

    #region Private Methods
    /// <summary>
    /// Sets Text object to specified / parent name.
    /// </summary>
    private void SetText()
    {
        GetComponent<Text>().text = (ParentToInheritNameFrom) ? ParentToInheritNameFrom.name.Replace('(', ' ').Replace(')', ' ') : transform.parent.name.Replace('(', ' ').Replace(')', ' ');
    }
    #endregion
}