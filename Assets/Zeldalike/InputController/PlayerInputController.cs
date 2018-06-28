using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : InputController
{
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";
    public string inputButtonName = "Fire1";

    private void Update()
    {
        // creats new vector 2 out of input axis
        joystick = new Vector2(SimpleInput.GetAxisRaw(horizontalAxisName), SimpleInput.GetAxisRaw(verticalAxisName));

        // if magnitude greater than 1, the input needs normalized so you can't move faster diagnal.
        if (joystick.magnitude > 1)
            joystick.Normalize();

        // check for input button presses.
        input.Evaluate(inputButtonName);
    }
}
