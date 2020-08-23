using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBoneMoveToBoneMoveToBone : MonoBehaviour
{
    public string boneName;
    public Transform boneParent;

    public void MoveToBone()
    {
        Transform bone = FindRecursiveChild(boneParent, boneName);

        if (bone != null)
        {
            transform.parent = bone;
            transform.localPosition = new Vector3();
            transform.localRotation = new Quaternion();
        }
    }

    public static Transform FindRecursiveChild(Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }
}
