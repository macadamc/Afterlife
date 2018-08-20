using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLookDirectionToJoystick : MonoBehaviour {

    public InputController inputController;

    public bool useOrthognial;

    private void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();
    }

    private void Update()
    {
        if(useOrthognial)
        {
            if (Mathf.Abs(inputController.joystick.x) > Mathf.Abs(inputController.joystick.y))
            {
                inputController.joystick.y = 0;
            }
            else
            {
                inputController.joystick.x = 0;
            }
        }
       
        inputController.SetLookDirection(inputController.joystick.normalized);
    }
}
