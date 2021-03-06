﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replace_Box_Collider_With_Mesh_Collider : MonoBehaviour
{
    private List<GameObject> listOfChildren;

    public void GetChildRecursive(GameObject obj)
    {

        if (null == obj)
            return;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            listOfChildren.Add(child.gameObject);
            GetChildRecursive(child.gameObject);
        }

        Debug.Log("Number of children " + listOfChildren.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        listOfChildren = new List<GameObject>();
        GetChildRecursive(transform.gameObject);

        foreach (GameObject t in listOfChildren)
        {
            if (t.transform == transform) continue; // Skip the transform itself

            if (t.GetComponent<BoxCollider>() != null)
            {
                BoxCollider box = t.GetComponent<BoxCollider>();
                GameObject.Destroy(box);
                t.AddComponent<MeshCollider>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
