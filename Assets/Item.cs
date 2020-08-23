using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Backend Item Instance
/// </summary>
[System.Serializable]
public struct ItemBackend
{
    /// <summary>
    /// Core instance data, synchronized with Frontend
    /// </summary>
    #region Core
    public uint ID_Instance;
    public uint ID_Template;
    public uint ID_Mutable;
    public uint ID_Inventory;
    public Vector3 rel_pos, rot;
    #endregion

    public ItemBackend(uint iInstance, uint iTemplate, uint iMutable, uint iInventory, Vector3 irel_pos, Vector3 irot)        
    {
        ID_Instance = iInstance;
        ID_Template = iTemplate;
        ID_Mutable = iMutable;
        ID_Inventory = iInventory;
        rel_pos = irel_pos;
        rot = irot;
    }
}

/// <summary>
/// Base for all Attributes
/// </summary>
public interface Attributes
{
    uint ID { get; set; }
    string name { get; set; }
    Attributes MergeAttributes(Attributes mutable);
}

/// <summary>
/// Attributes for all Items
/// List of ingredient template IDs and amounts to create Item
/// </summary>
public interface ItemAttributes: Attributes
{
    Dictionary<uint, uint> recipeIngredients { get; set; }
}

/// <summary>
/// Frontend Item instance
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

    public ItemManager mgr;

    public void Start()
    {
        rigid = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
        this.enabled = false;
    }

    public bool Equals(Item item)
    {
        return (item.ID_Template == ID_Template && item.ID_Mutable == ID_Mutable);
    }
}