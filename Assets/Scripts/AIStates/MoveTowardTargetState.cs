using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class MoveTowardTargetState : State
{
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

    private IEnumerator MoveTowardTarget()
    {
        _running = true;

        while(Vector2.Distance(transform.position, _target.position) > targetDistance && _vision.targets.Contains(_target))
        {
            inputController.joystick = (_target.position - transform.position).normalized;
            yield return null;
        }

        _running = false;
        yield return null;
        if(changeStateOnFinish)
            Next();
    }
}
