using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;

public class AlignTargetWithBoomerang : State
{
    public InputController inputController;
    public InputController target_InputController;
    private Vision _vision;
    private HeldItem projectileRef;

    public bool exitStateProjectileDestroyed;
    public float errorThreshold = .5f;
    public Transform debugObj;
    public float trackingDistance = 6f;

    protected override void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

        if (projectileRef == null)
            projectileRef = GetComponentInParent<HeldItemRef>().value;

        if (_vision == null)
            _vision = GetComponentInParent<Vision>();

        if (_vision.targets.transforms != null && _vision.targets.transforms.Count > 0)
            target_InputController = _vision.targets.transforms[0].GetComponent<InputController>();

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inputController.joystick = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (exitStateProjectileDestroyed && projectileRef == null)
        {
            Next();
            return;
        }

        if (target_InputController != null)
        {
            Vector3 lookdir = projectileRef.transform.position - target_InputController.transform.position;
            lookdir.Normalize();


            Vector2 lineStart = target_InputController.transform.position + (-lookdir * .5f);
            Vector2 lineEnd = target_InputController.transform.position + (-lookdir * 10f);
            var desiredPos = NearestPointOnFiniteLine(target_InputController.transform.position, lineEnd, inputController.transform.position);

            if (debugObj != null)
                debugObj.position = desiredPos;

            float dst = Vector2.Distance(inputController.transform.position, desiredPos);
            if (dst <= trackingDistance && dst > errorThreshold)
            {
                inputController.joystick = desiredPos - inputController.transform.position;

                if (inputController.joystick.magnitude > 1f)
                    inputController.joystick.Normalize();
            }
        }
    }
    //linePnt - point the line passes through
    //lineDir - unit vector in direction of line, either direction works
    //pnt - the point to find nearest on line for
    public Vector3 NearestPointOnLine(Vector2 linePnt, Vector2 lineDir, Vector2 pnt)
    {
        lineDir.Normalize();//this needs to be a unit vector
        var v = pnt - linePnt;
        var d = Vector2.Dot(v, lineDir);
        return linePnt + lineDir * d;
    }

    public Vector3 NearestPointOnFiniteLine(Vector3 start, Vector3 end, Vector3 pnt)
    {
        var line = (end - start);
        var len = line.magnitude;
        line.Normalize();

        var v = pnt - start;
        var d = Vector3.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);
        return start + line * d;
    }
}

