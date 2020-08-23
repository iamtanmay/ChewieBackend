using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable class can be used for objects that don't have an item - for example a screen belonging to an item or a button etc
/// </summary>
public class Interactable : MonoBehaviour
{
    /// <summary>
    /// Object description
    /// </summary>
    public string description;

    /// <summary>
    /// The Interaction of the object with character with instanceID
    /// </summary>
    /// <param name="instanceID"></param>
    public virtual void Interact(uint instanceID) { }
}