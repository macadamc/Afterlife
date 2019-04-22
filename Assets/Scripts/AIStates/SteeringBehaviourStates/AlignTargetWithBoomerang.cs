using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;
using ShadyPixel;

public class AlignTargetWithBoomerang : SteeringBehaviourState
{
    protected InputController inputController;
    protected InputController target_InputController;
    protected Vision _vision;
    protected HeldItem projectileRef;

    public bool exitStateProjectileDestroyed;
    public float errorThreshold = .5f;
    public Transform debugObj;
    public float trackingDistance = 6f;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

        if (projectileRef == null)
            projectileRef = GetComponentInParent<HeldItemRef>().value;

        if (_vision == null)
            _vision = GetComponentInParent<Vision>();

        if (_vision.HasTargets)//_vision.targets.transforms != null && _vision.targets.transforms.Count > 0)
            target_InputController = _vision.targets[0].GetComponent<InputController>();//_vision.targets.transforms[0].GetComponent<InputController>();


    }

    public override void Tick()
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
            var desiredPos = ShadyMath.NearestPointOnFiniteLine(target_InputController.transform.position, lineEnd, inputController.transform.position);

            if (debugObj != null)
                debugObj.position = desiredPos;

            float dst = Vector2.Distance(lineStart, desiredPos);
            if (dst <= trackingDistance && dst > errorThreshold)
            {
                agent.AddSteeringForce(agent.Seek(desiredPos), priority);
            }
        }
    }
}

