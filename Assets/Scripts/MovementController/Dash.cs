using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

    public bool activateOnEnable = true;

    public float force = 10f;

    public InputController ic;

    MovementController mc;

    private void Awake()
    {
        mc = GetComponentInParent<MovementController>();
        ic = GetComponentInParent<InputController>();
    }

    private void OnEnable()
    {
        if (activateOnEnable)
            ExecuteDash();
    }

    public void ExecuteDash()
    {
        Vector2 dashVector = ic.lookDirection * force;
        mc._dodgeVector = dashVector;
    }
}
