using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SimpleRotate : MonoBehaviour
{
    public float speed;
    public float acceleration;

    public bool clampSpeed;
    [ShowIf("clampSpeed")]
    public Vector2 speedBounds;

    float currentSpeed;
    Quaternion startRotation;

    private void OnEnable()
    {
        startRotation = transform.rotation;
        currentSpeed = speed;
    }

    private void OnDisable()
    {
        transform.rotation = startRotation;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, currentSpeed * Time.deltaTime));
        float accelerationDelta = acceleration * Time.deltaTime;
        currentSpeed = clampSpeed ? Mathf.Clamp(currentSpeed + accelerationDelta, speedBounds.x, speedBounds.y) : currentSpeed + accelerationDelta;
    }
}
