using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetJoystickToTarget : MonoBehaviour {

    public Targets targets;
    public InputController inputController;
    public bool continuousUpdate;

    Transform _target;
    Vector2 _dir;

    private void OnEnable()
    {
        // if no targets return out of statement
        if (targets.transforms.Count == 0)
            return;

        // picks a random target out of possible targets in list.
        _target = targets.transforms[Random.Range(0, targets.transforms.Count)];

        // gets direction vector between two points
        _dir = (_target.position - transform.position).normalized;

        // sets joystick on input controller
        inputController.joystick = _dir;
    }

    private void Update()
    {
        if(_target == null && targets.transforms.Count > 0)
        {
            _target = targets.transforms[Random.Range(0, targets.transforms.Count)];
        }

        if (targets.transforms.Count > 0 && continuousUpdate)
        {
            _dir = (_target.position - transform.position).normalized;
            inputController.joystick = _dir;
        }        
    }

    private void OnDisable()
    {
        inputController.joystick = Vector2.zero;
    }
}
