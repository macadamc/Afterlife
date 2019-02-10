using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleComponentWhileTargetJoystickIsPressed : MonoBehaviour {

    public MonoBehaviour whilePressed;
    public MonoBehaviour whileReleased;
    Vision _vision;

    private void OnEnable()
    {
        if(_vision == null)
            _vision = GetComponentInParent<Vision>();
    }

    private void Update()
    {
        if(_vision.targets.transforms.Count <= 0)
        {
            return;
        }

        InputController ic = _vision.targets.transforms[0].GetComponent<InputController>();
        if (ic.joystick.magnitude > .1f && whilePressed.enabled == false)
        {
            whilePressed.enabled = true;
            whileReleased.enabled = false;
        }
        else if (ic.joystick.magnitude <= .1f && whileReleased.enabled == false)
        {
            whilePressed.enabled = false;
            whileReleased.enabled = true;
        }
    }
}
