using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int type = 0;    // Weapon type 
                            // 0 - unarmed
                            // 1 - basic punch
                            // 2 - throw
                            // 3 - melee one handed
                            // 4 - melee two handed
                            // 5 - pistol
                            // 6 - rifle

    public float throwForce;
    public float speed;
    public Rigidbody rigid;

    public void Awake()
    {
        rigid = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Fire()
    {
        switch (type)
        {
            case 2:     Throw(transform.right*-1, throwForce);break;
            default:    return;
        }
    }

    public void Throw(Vector3 direction, float force)
    {
        transform.parent = null;
        rigid.AddForce(force * direction);
        speed = rigid.velocity.magnitude;
    }
}
