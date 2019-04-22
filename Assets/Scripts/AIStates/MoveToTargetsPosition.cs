using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class MoveToTargetsPosition : State {

    public float targetDistance = 2.0f;
    public InputController inputController;
    Vision _vision;
    bool _running;
    Vector2 _target;
    Coroutine coroutine;
    public bool changeStateOnFinish = true;

    protected override void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

        _vision = GetComponentInParent<Vision>();

        if(_vision.HasTargets)//_vision.targets.transforms.Count > 0)
            _target = _vision.targets[0].position;//_vision.targets.transforms[0].position;

        base.OnEnable();

        if (coroutine == null)
            coroutine = StartCoroutine(MoveTowardTarget());
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_running)
            StopCoroutine(coroutine);
        coroutine = null;

        inputController.joystick = Vector2.zero;
    }

    private IEnumerator MoveTowardTarget()
    {
        _running = true;

        while (Vector2.Distance(transform.position, _target) > targetDistance)
        {
            inputController.joystick = ((Vector3)_target - transform.position).normalized;
            yield return null;
        }

        _running = false;
        yield return null;
        if (changeStateOnFinish)
            Next();
    }
}
