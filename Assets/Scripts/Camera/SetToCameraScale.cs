using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToCameraScale : MonoBehaviour {

    public Camera cam;
    private Camera thisCam;

    private void Start()
    {
        thisCam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (cam == null || thisCam == null)
            return;

        thisCam.orthographicSize = cam.orthographicSize;
        thisCam.pixelRect = cam.pixelRect;
    }

}
