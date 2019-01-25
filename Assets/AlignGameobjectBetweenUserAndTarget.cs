using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ShadyPixel.StateMachine;
/*
public class AlignGameobjectBetweenUserAndTarget : State
{
    public GameObject goToAlign;

    public InputController inputController;
    public InputController target_InputController;
    private Vision _vision;
    public float errorThreshold = .5f;
    public Transform debugObj;
    public float trackingDistance = 6f;

    protected override void OnEnable()
    {
        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

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
        if (target_InputController != null && goToAlign != null)
        {
            Vector3 lookdir = goToAlign.transform.position - target_InputController.transform.position;
            lookdir.Normalize();


            Vector2 lineStart = goToAlign.transform.position + (lookdir * 1f);
            Vector2 lineEnd = goToAlign.transform.position + (lookdir * 17f);
            var desiredPos = NearestPointOnFiniteLine(lineStart, lineEnd, inputController.transform.position);

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
*/
public class AlignGameobjectBetweenUserAndTarget : SteeringBehaviour
{
    public GameObject goToAlign;
    public float errorThreshold = .5f;
    public Transform debugObj;
    public float trackingDistance = 6f;
    public bool CallEventsWhileAligned;
    public UnityEvent uEvent;

    protected InputController inputController;
    protected ItemController itemController;
    protected InputController target_InputController;

    protected Vector3 lookDir;
    protected Vector2 lineStart;
    protected Vector2 lineEnd;
    protected Vector3 desiredPos;
    protected float distanceToDesiredPos;

    private Vision _vision;

    public bool IsAligned
    {
        get
        {
            return target_InputController != null && goToAlign != null && distanceToDesiredPos <= errorThreshold;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();

        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

        if (itemController == null)
            itemController = GetComponentInParent<ItemController>();

        if (_vision == null)
            _vision = GetComponentInParent<Vision>();

        if (_vision.targets.transforms != null && _vision.targets.transforms.Count > 0)
            target_InputController = _vision.targets.transforms[0].GetComponent<InputController>();
    }

    public override void Tick()
    {
        if (target_InputController != null && goToAlign != null)
        {
            var targetPos = target_InputController.transform.position;
            targetPos.y += .25f;

            lookDir = (goToAlign.transform.position - targetPos).normalized;
            lineStart = goToAlign.transform.position + (lookDir * 1f);
            lineEnd = goToAlign.transform.position + (lookDir * 17f);
            desiredPos = NearestPointOnFiniteLine(lineStart, lineEnd, itemController.itemSpawnTransform.position);
            distanceToDesiredPos = Vector2.Distance(itemController.itemSpawnTransform.position, desiredPos);

            if (debugObj != null)
                debugObj.position = desiredPos;

            if (distanceToDesiredPos <= trackingDistance && distanceToDesiredPos > errorThreshold)
            {
                Vector2 neededVelocity = (desiredPos - itemController.itemSpawnTransform.position).normalized * agent.moveSpeed;
                neededVelocity = neededVelocity - agent.velocity;

                agent.AddSteeringForce(neededVelocity, priority);
            }
        }
    }

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

    private void Update()
    {
        if(CallEventsWhileAligned && IsAligned)
        {
            uEvent.Invoke();
        }
    }

}