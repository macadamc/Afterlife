using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLookDirectionToJoystick : MonoBehaviour {

    public InputController inputController;

    private void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();
    }

    private void Update()
    {
        inputController.SetLookDirection();
    }
}
