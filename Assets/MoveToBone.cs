using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class MoveToBone : MonoBehaviour
{
    public string boneName;
    public Transform boneParent;
    public Vector3 posOffset, angleOffset;
    public UnityEvent onMoved;  

    public void Move()
    {
        Transform bone = FindRecursiveChild(boneParent, boneName);

        if (bone != null)
        {
            transform.parent = bone;
            transform.localPosition = posOffset;
            transform.localRotation = Quaternion.Euler(angleOffset);
            onMoved.Invoke();
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
