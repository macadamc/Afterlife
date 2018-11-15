using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoTest : MonoBehaviour {

    public Vector3 gizmoSize;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gizmoSize);

    }
}
