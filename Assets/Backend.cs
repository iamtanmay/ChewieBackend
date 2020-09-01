using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate BackendInventory Delegate_Backend_GetInventory(uint containerID);
public delegate bool Delegate_Backend_PutInventory(uint containerID, BackendInventory icontainer);
public delegate uint Delegate_Backend_CreateInventory();
public delegate void Delegate_Backend_DeleteInventory(uint containerID);

public delegate uint Delegate_Backend_CreateItem(uint templateID, uint containerID);
public delegate void Delegate_Backend_DestroyItem(uint instanceID);
public delegate BackendItem Delegate_Backend_GetItem(uint instanceID);
public delegate bool Delegate_Backend_MoveItem(uint instanceID, uint newContainerID);

#region Backend_Definitions
/// <summary>
/// Serializable Backend Inventory
/// </summary>
public struct BackendInventory
{
    public uint containerID;
    public uint[] gameItemInstanceIDs;
    public bool IsActiveInventory;
}

/// <summary>
/// Serializable Backend Item
/// </summary>
[System.Serializable]
public struct BackendItem
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

    public BackendItem(uint iInstance, uint iTemplate, uint iMutable, uint iInventory, Vector3 irel_pos, Vector3 irot)
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
public interface ItemAttributes : Attributes
{
    Dictionary<uint, uint> recipeIngredients { get; set; }
}
#endregion

public class Backend : MonoBehaviour
{
    /// <summary>
    /// uint instanceID
    /// ItemBackend backend Item representation
    /// </summary>
    public Dictionary<uint, BackendItem> items;

    /// <summary>
    /// uint containerID
    /// All Inventories in game
    /// </summary>
    public Dictionary<uint, BackendInventory> containers;

    /// <summary>
    /// uint TemplateID
    /// All Templates in game
    /// </summary>
    public Dictionary<uint, Attributes> templates;

    /// <summary>
    /// Counter for creating new Instance IDs
    /// </summary>
    public uint instanceIDCounter = 1;

    /// <summary>
    /// Counter for creating new Container IDs
    /// </summary>
    public uint containerIDCounter = 1;

    // Start is called before the first frame update
    void Start()
    {
        items = new Dictionary<uint, BackendItem>();
        templates = new Dictionary<uint, Attributes>();
        containers = new Dictionary<uint, BackendInventory>();
    }

    public bool PutContainer(uint containerID, BackendInventory icontainer)
    {
        if (containers.ContainsKey(containerID))
            containers[containerID] = icontainer;
        else
            return false;
        return true;
    }

    public BackendInventory GetContainer(uint containerID)
    {
        return containers[containerID];
    }

    public uint CreateContainer()
    {
        containerIDCounter++;
        BackendInventory newInventory = new BackendInventory();
        newInventory.containerID = containerIDCounter;
        return containerIDCounter;
    }

    public void DeleteContainer(uint containerID)
    {
        if (containers.ContainsKey(containerID))
            containers.Remove(containerID);
    }

    public uint CreateItem(uint templateID, uint containerID)
    {
        BackendItem newItem = new BackendItem();
        newItem.ID_Template = templateID;
        newItem.ID_Inventory = containerID;
        instanceIDCounter++;
        newItem.ID_Instance = instanceIDCounter;
        items.Add(instanceIDCounter, newItem);
        return instanceIDCounter;
    }

    public void DeleteItem(uint instanceID)
    {
        if (items.ContainsKey(instanceID))
            items.Remove(instanceID);
    }

    public bool MoveItem(uint instanceID, uint newContainerID)
    {
        if (items.ContainsKey(instanceID))
        {
            BackendItem movedItem = items[instanceID];
            movedItem.ID_Inventory = newContainerID;
            items[instanceID] = movedItem;
        }
        else
            return false;
        return true;
    }

    public void ModifyItem(uint instanceID, uint mutableID)
    {
        BackendItem modifiedItem = items[instanceID];
        modifiedItem.ID_Mutable = mutableID;
        items[instanceID] = modifiedItem;
    }

    public bool ContainsItem(uint instanceID)
    {
        return items.ContainsKey(instanceID);
    }

    public BackendItem GetItem(uint instanceID)
    {
        return items[instanceID];
    }
}