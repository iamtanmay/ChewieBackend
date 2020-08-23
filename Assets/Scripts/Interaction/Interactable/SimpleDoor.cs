using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDoor : Item 
{
    bool closed = true;

    public override void Interact(uint instanceID)
    {
        if (closed)
        {
            transform.Rotate(Vector3.up, 90.0f);
            itemCollider.isTrigger = true;
            closed = false;
        }
        else
        {
            transform.Rotate(Vector3.up, -90.0f);
            itemCollider.isTrigger = false;
            closed = true;
        }
    }
}
