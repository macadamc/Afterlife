using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTransformOnReEnable : MonoBehaviour {

    Vector2 m_StartPosition;
    bool m_Initialized;

    public void OnEnable()
    {
        if(!m_Initialized)
        {
            m_StartPosition = transform.position;
            m_Initialized = true;
            return;
        }
        transform.position = m_StartPosition;
    }
}
