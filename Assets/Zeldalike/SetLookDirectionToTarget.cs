using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLookDirectionToTarget : MonoBehaviour {

    public InputController inputController;
    Vision _vision;
    Transform _target;

    private void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

        _target = GetTarget();
        _vision = GetComponentInParent<Vision>();
    }

    private void Update()
    {
        if (_target == null)
        {
            if (_vision.targets.transforms.Count == 0)
            {
                return;
            }
            _target = GetTarget();
            if (_target == null)
                return;
        }

        Vector2 dir = (_target.position - transform.position).normalized;

        inputController.SetLookDirection(dir);
    }

    private Transform GetTarget()
    {
        return _vision.targets.transforms[Random.Range(0,_vision.targets.transforms.Count)];
    }
}
