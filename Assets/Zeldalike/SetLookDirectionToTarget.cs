using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLookDirectionToTarget : MonoBehaviour {

    public InputController inputController;
    Vision _vision;

    private void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

        _vision = GetComponentInParent<Vision>();
    }

    private void Update()
    {
        if (_vision.targets.transforms == null || _vision.targets.transforms.Count == 0)
            return;

        Vector2 dir = (_vision.targets.transforms[0].position - transform.position).normalized;

        inputController.SetLookDirection(dir);
    }
}
