using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;
using Sirenix.OdinInspector;

public class DodgeState : State
{
    public Vision vision;
    public GameObject onDodge;
    [MinMaxSlider(0,15,true)]
    public Vector2 dodgeForce;
    [MinMaxSlider(0f, .5f, true)]
    public Vector2 dodgeDelay;

    MovementController movementController;
    InputController ic;
    Coroutine coroutine;

    
	// Use this for initialization
	protected override void OnEnable()
    {
        base.OnEnable();

        if (movementController == null)
            movementController = GetComponentInParent<MovementController>();
    }

    private void Update()
    {
        //get first target.
        InputController target_IC = null;
        if (vision.targets.transforms != null && vision.targets.transforms.Count > 0)
            target_IC = vision.targets.transforms[0].GetComponent<InputController>();

        if ((coroutine == null && target_IC != null && (target_IC.input.held || target_IC.input.pressed || target_IC.input.released)))
        {

            coroutine = StartCoroutine(Dodge(target_IC));
        }
        else
        {
            //StateMachine.Next();
        }
    }

    IEnumerator Dodge(InputController target_IC)
    {
        float dodgeTime = Time.time + Random.Range(dodgeDelay.x, dodgeDelay.y);
        yield return new WaitUntil(() => { return Time.time >= dodgeTime; });

        Vector2 dir = -(target_IC.transform.position - transform.position).normalized;
        movementController.SetKnockback(dir * Random.Range(dodgeForce.x, dodgeForce.y));
        yield return new WaitUntil(() => { return movementController.Knockedback == false; });

        coroutine = null;
        StateMachine.ChangeState(onDodge);
    }
}