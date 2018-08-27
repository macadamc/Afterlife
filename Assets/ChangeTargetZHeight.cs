using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ChangeTargetZHeight : MonoBehaviour {

    public FlyingMovementController mc;
    public bool useSpeedPercentage = false;

    [HideIf("useSpeedPercentage")]
    public float newHeight;
    float _lastHeight;

    private void OnEnable()
    {
        _lastHeight = mc.targetFlyingHeight;
        mc.targetFlyingHeight = newHeight;
    }

    private void OnDisable()
    {
        mc.targetFlyingHeight = _lastHeight;
    }
    public void Update()
    {
        if (useSpeedPercentage)
            mc.targetFlyingHeight = mc.SpeedPercentage * _lastHeight;
    }

}
