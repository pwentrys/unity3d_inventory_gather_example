using UnityEngine;

/// <summary>
/// Used to create a global singleton.
/// Slightly modified for personal preference.
/// From http://wiki.unity3d.com/index.php?title=Singleton
/// </summary>
/// <typeparam name="T">The Type we're going to instantiate.</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Public Variables
    /// <summary>
    /// Creates instance of object if one doesn't exist already and our application is not exiting.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_IsQuitting) return null;

            lock (_Lock)
            {
                if (_Instance == null)
                {
                    _Instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1) return _Instance;

                    if (_Instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _Instance = singleton.AddComponent<T>();
                        DontDestroyOnLoad(singleton);
                    }
                }

                return _Instance;
            }
        }
    }

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Ensures no more than one thread is accessing the object simultaneously.
    /// </summary>
    private static object _Lock = new object();
    /// <summary>
    /// Flag that ensures object is deleted on destroy as to not cause the world's weirdest and most frustrating invisible object issues.
    /// </summary>
    private static bool _IsQuitting = false;
    /// <summary>
    /// Actual object instance.
    /// </summary>
    private static T _Instance;

    #endregion Private Variables

    #region End Methods
    /// <summary>
    /// Runs when application is quitting.
    /// </summary>
    public void OnDestroy()
    {
        _IsQuitting = true;
    }

    #endregion End Methods
}