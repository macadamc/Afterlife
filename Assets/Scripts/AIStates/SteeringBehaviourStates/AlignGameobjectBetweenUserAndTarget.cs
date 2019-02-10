using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ShadyPixel.StateMachine;

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
    protected ProjectileRef pRef;

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

        if(pRef == null)
            pRef = GetComponentInParent<ProjectileRef>();

    }

    public override void Tick()
    {
        if (pRef != null && pRef.value != null)
            goToAlign = pRef.value.gameObject;


        if (target_InputController != null && goToAlign != null)
        {
            var targetPos = target_InputController.transform.position;
            targetPos.y += .25f;

            lookDir = (goToAlign.transform.position - targetPos).normalized;
            lineStart = goToAlign.transform.position + (lookDir * 1f);
            lineEnd = goToAlign.transform.position + (lookDir * 17f);
            desiredPos = ShadyPixel.ShadyMath.NearestPointOnFiniteLine(lineStart, lineEnd, itemController.itemSpawnTransform.position);
            distanceToDesiredPos = Vector2.Distance(itemController.itemSpawnTransform.position, desiredPos);

            if (debugObj != null)
                debugObj.position = desiredPos;

            if (distanceToDesiredPos <= trackingDistance && distanceToDesiredPos > errorThreshold)
            {
                agent.AddSteeringForce(agent.Seek(desiredPos), priority);
            }
        }
    }

    private void Update()
    {
        if(CallEventsWhileAligned && IsAligned)
        {
            uEvent.Invoke();
        }
    }

    public void SetTarget(GameObject go)
    {
        goToAlign = go;
    }

}