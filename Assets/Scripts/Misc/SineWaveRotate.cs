using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveRotate : MonoBehaviour {

    public Targets targets;
    public bool snapToTargetOnEnable = false;
    public bool followTarget = false;

    public float rotateTime = 2f;
    public float maxRotation = 45f;

    float m_Time;
    Quaternion m_StartRotation;
    float m_ForwardAngle;

    Vector3 vectorToTarget = Vector3.zero;
    float angleToTarget = 0;
    Quaternion quaternionToTarget = Quaternion.identity;

    private void OnEnable()
    {
        m_ForwardAngle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
        m_StartRotation = transform.rotation;
        m_Time = 0f;

        if (snapToTargetOnEnable)
        {
            if (targets == null || targets.transforms.Count == 0)
                return;

            vectorToTarget = targets.transforms[0].position - transform.position;
            angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            quaternionToTarget = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
            transform.localRotation = quaternionToTarget;
            m_ForwardAngle = angleToTarget;
        }
    }

    private void OnDisable()
    {
        transform.rotation = m_StartRotation;
        m_Time = 0f;
    }

    void Update()
    {
        m_Time += Time.deltaTime;

        if (followTarget && targets != null && targets.transforms.Count >= 0)
        {
            vectorToTarget = targets.transforms[0].position - transform.position;
            angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            m_ForwardAngle = angleToTarget;
        }



        transform.rotation = Quaternion.Euler(0f, 0f, m_ForwardAngle + (maxRotation * Mathf.Sin(m_Time * rotateTime)));
    }
}
