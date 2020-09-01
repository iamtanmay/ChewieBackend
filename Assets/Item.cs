using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Frontend Item instance
/// </summary>
[System.Serializable]
public class Item : Interactable
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

    public ItemManager mgr;

    public void Start()
    {
        rigid = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
        this.enabled = false;
    }

    //Pick it up if its dropped, try to steal it if its not owned by character
    public override void Interact(uint instanceID)
    {
    }

    public bool Equals(Item item)
    {
        return (item.ID_Template == ID_Template && item.ID_Mutable == ID_Mutable);
    }
}