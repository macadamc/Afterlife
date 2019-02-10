using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLookDirectionToTarget : MonoBehaviour {

    public InputController inputController;
    public Targets targets;
    Transform _target;

    private void OnEnable()
    {
        _target = GetTarget();
    }

    private void Update()
    {
        if (_target == null)
        {
            _target = GetTarget();
            if (_target == null)
                return;
        }

        Vector2 dir = (_target.position - transform.position).normalized;

        inputController.SetLookDirection(dir);
    }

    private Transform GetTarget()
    {
        if (targets == null || targets.transforms.Count == 0)
        {
            return null;
        }
        return targets.transforms[0];
    }
}
