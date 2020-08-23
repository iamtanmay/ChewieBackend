
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Seeks bone in parent, and positions itself, to allow Objects to be attached to it
public class MountSlot : MonoBehaviour
{
    public string InventorySlotName = "";
    public Vector3 pos, rot;
    public Transform slotParent;
    public Transform item;
    public bool mounted = false;

    public void Init()
    {
        transform.parent = slotParent;

        //Align it in the right spot
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(rot);

        this.enabled = false;
    }

    /// <summary>
    /// Will drop old item
    /// </summary>
    /// <param name="newItem"></param>
    public void Mount(Transform newItem)
    {
        if (newItem == null)
            return;

        if (!mounted)
            item = newItem;
        else
        {
            Drop();
            Mount(newItem);
        }
        item.transform.parent = transform;
        item.transform.localPosition = new Vector3();
        item.transform.localRotation = new Quaternion();

        mounted = true;
    }

    public void Drop()
    {
        item.transform.parent = null;
        mounted = false;
    }
}
