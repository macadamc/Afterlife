using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour {

    public Targets targets;
    //public Transform targetTransform;
    public float speed = 90f;
    public bool snapToTargetOnEnable = true;

    Quaternion startRotation;
    Vector3 vectorToTarget = Vector3.zero;
    float angleToTarget = 0;
    Quaternion quaternionToTarget = Quaternion.identity;

    void OnEnable ()
    {
        startRotation = transform.rotation;


        vectorToTarget = Vector3.zero;
        angleToTarget = 0f;
        quaternionToTarget = startRotation;

        if (snapToTargetOnEnable)
        {
            if (targets == null || targets.transforms.Count == 0)
                return;

            vectorToTarget = targets.transforms[0].position - transform.position;
            angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            quaternionToTarget = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
            transform.localRotation = quaternionToTarget;
        }
    }

    private void OnDisable()
    {
        transform.localRotation = startRotation;
    }

    // Update is called once per frame
    void Update ()
    {
        if (targets == null || targets.transforms.Count == 0)
            return;

        vectorToTarget = targets.transforms[0].position - transform.position;
        angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        quaternionToTarget = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
        transform.localRotation = Quaternion.Slerp(transform.rotation, quaternionToTarget, Time.deltaTime * speed);
    }
}
