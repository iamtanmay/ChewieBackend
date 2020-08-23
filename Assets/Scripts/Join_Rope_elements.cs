using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Join_Rope_elements : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<HingeJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
