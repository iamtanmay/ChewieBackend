using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointsMovement : MonoBehaviour
{
    public float speed = 1f;
    public int currentPointIndex = 0;
    public Transform[] cameraPoints;
    public Transform currentPoint;

    void Start()
    {
        currentPoint = cameraPoints[0];
        transform.parent = currentPoint;
        transform.localPosition = new Vector3();
        transform.localRotation = new Quaternion();
    }

    void Update()
    {
        ChangeTarget(currentPointIndex);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(), speed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(), speed * Time.deltaTime);
    }

    public void ChangeTarget(int index)
    {
        currentPoint = cameraPoints[index];
        transform.parent = currentPoint;
    }
}
