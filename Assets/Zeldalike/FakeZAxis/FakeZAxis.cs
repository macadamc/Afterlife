using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FakeZAxis : MonoBehaviour {

    public Transform objectToMove;
    public float height;
    public float velocity;
    public float gravity = 7.0f;
    public string onGroundLayerName;
    public string inAirLayerName;

    Vector3 targetPos;
    public Vector3 baseOffset;

    private void LateUpdate()
    {
        targetPos.y = height + baseOffset.y;
        targetPos.x = 0.0f;
        objectToMove.localPosition = targetPos;

        if(Application.isPlaying)
            UpdateHeight();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            velocity -= 5f;
        }
    }

    private void UpdateHeight()
    {
        if(height != 0f || velocity != 0f)
        {
            gameObject.layer = LayerMask.NameToLayer(inAirLayerName);
            velocity += gravity * Time.deltaTime;

            height -= velocity * Time.deltaTime;

            if (height < 0f)
            {
                height = 0f;
                velocity = 0f;
                gameObject.layer = LayerMask.NameToLayer(onGroundLayerName);
            }
        }
    }
}
