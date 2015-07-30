using UnityEngine;

/// <summary>
/// Here we configure out target object. Sticking this into a struct since you're probably going to reuse that little template over and over for other things.
/// </summary>
internal struct Target
{
    public GameObject GO;   // Our target GameObject.
    public ItemStats Stats; // State of Target GameObject.
}