using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class MatchTargetMovement : State
{
    InputController inputController;
    InputController target_InputController;
    Vision _vision;
    public bool invert;

    protected override void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

        if(_vision == null)
            _vision = GetComponentInParent<Vision>();

        if(_vision.HasTargets)//_vision.targets.transforms != null && _vision.targets.transforms.Count > 0)
            target_InputController = _vision.targets[0].GetComponent<InputController>();//_vision.targets.transforms[0].GetComponent<InputController>();

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inputController.joystick = Vector2.zero;
    }

    private void Update()
    {
        if (target_InputController == null)
            return;

        if(!invert)
            inputController.joystick = target_InputController.joystick;
        else
            inputController.joystick = -target_InputController.joystick;
    }
}



