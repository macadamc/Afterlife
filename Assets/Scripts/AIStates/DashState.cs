using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ShadyPixel.StateMachine;

public class DashState : State
{
    public Vision vision;
    [MinMaxSlider(0, 15, true)]
    public Vector2 dashForce;
    [MinMaxSlider(0f, .5f, true)]
    public Vector2 dashDelay;

    MovementController movementController;
    InputController ic;

    // Use this for initialization
    protected override void OnEnable()
    {
        base.OnEnable();

        if (movementController == null)
            movementController = GetComponentInParent<MovementController>();

        //get first target.
        InputController target_IC = null;
        if (vision.targets.transforms != null && vision.targets.transforms.Count > 0)
            target_IC = vision.targets.transforms[0].GetComponent<InputController>();

        StartCoroutine(Dash(target_IC));
    }

    IEnumerator Dash(InputController target_IC)
    {
        float dodgeTime = Time.time + Random.Range(dashDelay.x, dashDelay.y);

        yield return new WaitUntil(() => { return Time.time >= dodgeTime; });

        Vector2 dir = (target_IC.transform.position - transform.position).normalized;

        movementController.SetKnockback(dir * Random.Range(dashForce.x, dashForce.y));
        StateMachine.Next();



    }
}