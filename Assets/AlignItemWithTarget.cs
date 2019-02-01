using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel;

public class AlignItemWithTarget : SteeringBehaviourState
{
    public bool exitStateOnAlign;
    public Vector2 targetDistance;
    public float errorThreshold = .5f;
    public Transform debugObj;

    protected InputController inputController;
    protected InputController target_InputController;
    protected ItemController itemController;
    protected Vision _vision;
    protected ItemSpawnPrefabWithCharge itemWithSnap;
    protected Vector2 targetposLast;
    protected Vector3 desiredPos;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (inputController == null)
            inputController = agent.GetComponent<InputController>();

        if (itemController == null)
            itemController = agent.GetComponent<ItemController>();

        itemWithSnap = itemController.currentItem as ItemSpawnPrefabWithCharge;
        Debug.Assert(itemWithSnap != null, "itemController.currentItem is not a ItemSpawnPrefabWithCharge.");

        if (_vision == null)
            _vision = agent.GetComponent<Vision>();

        if (_vision.targets.transforms != null && _vision.targets.transforms.Count > 0)
            target_InputController = _vision.targets.transforms[0].GetComponent<InputController>();


    }

    public override void Tick()
    {
        if (target_InputController != null)
        {
            int snap = itemWithSnap != null ? itemWithSnap.angleSnap : 1;

            Vector3 lookdir = ShadyMath.AngleSnap(target_InputController.transform.position - inputController.transform.position, snap);
            Vector2 lineStart = target_InputController.transform.position + (-lookdir * targetDistance.y);
            Vector2 lineEnd = target_InputController.transform.position + (-lookdir * targetDistance.x);

            desiredPos = ShadyMath.NearestPointOnFiniteLine(lineStart, lineEnd, inputController.transform.position);

            if (debugObj != null)
                debugObj.position = desiredPos;

            float dst = Vector2.Distance(inputController.transform.position, desiredPos);
            if (dst > errorThreshold)
            {
                agent.AddSteeringForce(agent.Seek(desiredPos), priority);
            }

            else if (exitStateOnAlign)
                Next();
        }
    }
}