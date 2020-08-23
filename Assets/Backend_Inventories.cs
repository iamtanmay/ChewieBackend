using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate BackendInventory Delegate_Backend_GetInventory(uint containerID);
public delegate bool Delegate_Backend_PutInventory(uint containerID, BackendInventory icontainer);
public delegate uint Delegate_Backend_CreateInventory();
public delegate void Delegate_Backend_DeleteInventory(uint containerID);

/// <summary>
/// Serializable Backend for Inventories
/// </summary>
public class BackendInventory
{
    public uint containerID;
    public uint[] gameItemInstanceIDs;
    public bool IsActiveInventory = false;
}

public class Backend_Inventories : MonoBehaviour
{
    /// <summary>
    /// uint containerID
    /// All Inventories in game
    /// </summary>
    public Dictionary<uint, BackendInventory> containers;
    public Backend_GameItems gameItemBackend;
    public uint containerIDCounter = 1;

    void Start()
    {
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
}
