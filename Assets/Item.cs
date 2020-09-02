using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Frontend Item base. Child classes must define the mutable method to create unique items
/// </summary>
[System.Serializable]
public abstract class Item : Interactable
{
    /// <summary>
    /// Core instance data, synchronized with Backend
    /// Its kept here instead of being wrapped in a structure so it can be edited in editor
    /// </summary>
    #region Core
    public uint ID_Instance;
    public uint ID_Template;
    public uint ID_Mutable;
    public uint ID_Inventory;
    public Vector3 rel_pos, rot;
    #endregion

    /// <summary>
    /// Instance specific data - merged from template and mutable data
    /// </summary>
    public Attributes data;

    /// <summary>
    /// Ingame items are physical objects
    /// </summary>
    public Rigidbody rigid;

    /// <summary>
    /// Trigger or physical Collider
    /// </summary>
    public Collider itemCollider;

    /// <summary>
    /// Container that the item belongs to
    /// </summary>
    public Inventory inventory;

    /// <summary>
    /// Container slot that the item is currently mounted in
    /// </summary>
    public MountSlot inventorySlot;

    public ObjectManager mgr;

    public void Start()
    {
        rigid = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
        this.enabled = false;
    }

    //Allows for creation of Mutable data
    public abstract void ApplyMutable();

    public bool Equals(Item item)
    {
        return (item.ID_Template == ID_Template && item.ID_Mutable == ID_Mutable);
    }
}