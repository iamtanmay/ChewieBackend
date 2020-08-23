using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    public float speed = 5.0f;

    public float m_minX = -90.0f, m_maxX = 90.0f;
    public float m_minY = -90.0f, m_maxY = 90.0f;
    public float m_minZ = -90.0f, m_maxZ = 90.0f;

    float m_rotX = 0.0f, m_rotY = 0.0f, m_rotZ = 0.0f;

    public bool Enabled { get; set; }

    public void Rotate(float x, float y, float z)
    {
        if (!Enabled)
            return;

        m_rotX = Mathf.Clamp(x, m_minX, m_maxX);
        m_rotY = Mathf.Clamp(y, m_minY, m_maxY);
        m_rotZ = Mathf.Clamp(z, m_minZ, m_maxZ);

        transform.Rotate(new Vector3(m_rotX, m_rotY, m_rotZ) * Time.deltaTime * speed);
    }
}
