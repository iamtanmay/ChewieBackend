using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// System for simply mounting transforms
/// Does not handle additional logic like activating objects or inventory functions
/// </summary>
public class MountingSystem : MonoBehaviour
{
    //Instantiated slots with correct alignment
    public List<MountSlot> mountSlots;

    //Prefabs for mount slots
    public List<Transform> mountSlotPrefabs;

    public void InitialiseMountPoints()
    {
        mountSlots = new List<MountSlot>();

        foreach (Transform mount_Point_Prefab in mountSlotPrefabs)
        {
            Transform tobj = GameObject.Instantiate(mount_Point_Prefab) as Transform;
            mountSlots.Add(tobj.GetComponent<MountSlot>());
            mountSlots[mountSlots.Count - 1].slotParent = transform.FindDeepChild(mountSlots[mountSlots.Count - 1].InventorySlotName);
            mountSlots[mountSlots.Count - 1].Init();
        }
    }

    public void AddItem(int ID, Transform item)
    {
        mountSlots[ID].Mount(item);
    }

    public void AddItem(string mount_Point_Name, Transform item)
    {
        FindMountPoint(mount_Point_Name).Mount(item);
    }

    public void DropItem(int ID)
    {
        mountSlots[ID].Drop();
    }

    public void DropItem(string mount_Point_Name)
    {
        FindMountPoint(mount_Point_Name).Drop();
    }

    public MountSlot FindMountPoint(string mount_Point_Name)
    {
        foreach (MountSlot mount_Point in mountSlots)
            if (mount_Point.name == mount_Point_Name + "(Clone)")
                return mount_Point;

        Debug.Log("ERROR: Mount point " + mount_Point_Name + " not found !!");
        return mountSlots[0];
    }

    public MountSlot FindEmptySlot()
    {
        //Search for a free slot
        foreach (MountSlot mount_Point in mountSlots)
            //If there is a slot
            if (!mount_Point.mounted)
                return mount_Point;
        return null;
    }
}