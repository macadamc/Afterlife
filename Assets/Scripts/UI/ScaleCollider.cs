using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ScaleCollider : MonoBehaviour
{

    public float maxR = 3;
    public float stepsize = .01f;
    public float waitTime = .2f;
    public bool loop;
    float m_nextUpdate;
    float m_lastR;

    public CircleCollider2D coll;

    private void OnEnable()
    {
        m_nextUpdate = Time.time + waitTime;
        m_lastR = coll.radius;
        coll.radius = stepsize;
    }

    private void OnDisable()
    {
        coll.radius = m_lastR;
    }

    private void FixedUpdate()
    {
        if(Time.time >= m_nextUpdate)
        {
            float v = coll.radius + stepsize;
            if (v < maxR)
                coll.radius = Mathf.Clamp(v, 0, maxR);
            else if(loop)
            {
                coll.radius = m_lastR;
                coll.enabled = false;
                coll.enabled = true;
            }

            m_nextUpdate = Time.time + waitTime;
        }
    }
}
