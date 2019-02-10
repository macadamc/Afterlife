using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTargetAgentJoyStick : SteeringBehaviour {

    InputController target_InputController;
    Vision _vision;
    public bool invert;

    public override void OnEnable()
    {
        base.OnEnable();

        if (_vision == null)
            _vision = GetComponentInParent<Vision>();

        if (_vision.targets.transforms != null && _vision.targets.transforms.Count > 0)
            target_InputController = _vision.targets.transforms[0].GetComponent<InputController>();
    }

    public override void Tick()
    {
        if (target_InputController == null)
            return;

        var vel = target_InputController.joystick.normalized * agent.moveSpeed;

        if (invert)
            vel = -vel;

        agent.AddSteeringForce(vel, priority);

    }

}
