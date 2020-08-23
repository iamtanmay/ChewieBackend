using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate uint Delegate_Backend_CreateItem(uint templateID, uint containerID);
public delegate void Delegate_Backend_DestroyItem(uint instanceID);
public delegate ItemBackend Delegate_Backend_GetItem(uint instanceID);
public delegate bool Delegate_Backend_MoveItem(uint instanceID, uint newContainerID);

public class Backend_GameItems : MonoBehaviour
{
    /// <summary>
    /// uint instanceID
    /// ItemBackend backend Item representation
    /// </summary>
    public Dictionary<uint, ItemBackend> items;

    public Dictionary<uint, Attributes> templates;

    public Backend_Inventories inventoriesBackend;

    /// <summary>
    /// Counter for creating new Instance IDs
    /// </summary>
    public uint instanceIDCounter = 1;

    // Start is called before the first frame update
    void Start()
    {
        items = new Dictionary<uint, ItemBackend>();
        templates = new Dictionary<uint, Attributes>();
    }

    public uint CreateItem(uint templateID, uint containerID)
    {
        ItemBackend newItem = new ItemBackend();
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
            ItemBackend movedItem = items[instanceID];
            movedItem.ID_Inventory = newContainerID;
            items[instanceID] = movedItem;
        }
        else
            return false;
        return true;
    }

    public void ModifyItem(uint instanceID, uint mutableID)
    {
        ItemBackend modifiedItem = items[instanceID];
        modifiedItem.ID_Mutable = mutableID;
        items[instanceID] = modifiedItem;
    }

    public bool ContainsItem(uint instanceID)
    {
        return items.ContainsKey(instanceID);
    }

    public ItemBackend GetItem(uint instanceID)
    {
        return items[instanceID];
    }
}
