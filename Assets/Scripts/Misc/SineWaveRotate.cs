using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveRotate : MonoBehaviour {

    public float rotateTime = 2f;
    public float maxRotation = 45f;

    float m_Time;
    Quaternion m_StartRotation;
    float m_ForwardAngle;

    private void OnEnable()
    {
        m_ForwardAngle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
        m_StartRotation = transform.rotation;
        m_Time = 0f;
    }

    private void OnDisable()
    {
        transform.rotation = m_StartRotation;
        m_Time = 0f;
    }

    void Update()
    {
        m_Time += Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, 0f, m_ForwardAngle + (maxRotation * Mathf.Sin(m_Time * rotateTime)));
    }
}
