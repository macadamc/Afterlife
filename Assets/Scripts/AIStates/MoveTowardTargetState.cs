using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class MoveTowardTargetState : State
{
    public bool invert;
    public float targetDistance = 2.0f;
    public InputController inputController;
    Vision _vision;
    bool _running;
    Transform _target;
    Coroutine coroutine;
    public bool changeStateOnFinish = true;

    protected override void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

        _vision = GetComponentInParent<Vision>();

        if(_vision.targets.transforms.Count > 0)
            _target = _vision.targets.transforms[0];

        base.OnEnable();

        if(_target != null && coroutine == null)
            coroutine = StartCoroutine(MoveTowardTarget());
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if(_running)
            StopCoroutine(coroutine);
        coroutine = null;

        inputController.joystick = Vector2.zero;
    }

    bool TestState
    {
        get
        {
            if (invert)
            {
                return Vector2.Distance(transform.position, _target.position) < targetDistance && _vision.targets.transforms.Contains(_target);
            }
            else
            {
                return Vector2.Distance(transform.position, _target.position) > targetDistance && _vision.targets.transforms.Contains(_target);
            }
        }
    }
    private IEnumerator MoveTowardTarget()
    {
        _running = true;        

        while (TestState)
        {
            inputController.joystick = (_target.position - transform.position).normalized;
            if (invert)
                inputController.joystick = -inputController.joystick;

            yield return null;
        }

        _running = false;
        yield return null;
        if(changeStateOnFinish)
            Next();
    }
}
