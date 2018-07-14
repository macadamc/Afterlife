using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;
using Sirenix.OdinInspector;

public class UseItemState : State
{
    public InputController inputController;

    [MinMaxSlider(0.0f,5.0f)]
    public Vector2 holdTime;

    public bool changeStateOnFinish;
    Coroutine coroutine;

    protected override void OnEnable()
    {
        if(inputController == null)
            inputController = GetComponentInParent<InputController>();

        base.OnEnable();

        if(coroutine == null)
            coroutine = StartCoroutine(UseItem());
    }

    protected override void OnDisable()
    {
        StopCoroutine(coroutine);
        coroutine = null;
        inputController.input.SetValue(false);
    }

    public IEnumerator UseItem()
    {

        inputController.input.pressed = true;
        yield return null;
        inputController.input.held = true;
        yield return new WaitForSeconds(Random.Range(holdTime.x, holdTime.y));
        inputController.input.held = false;
        inputController.input.pressed = false;
        inputController.input.released = true;
        yield return null;
        inputController.input.released = false;

        if(changeStateOnFinish)
        {
            Next();
        }
    }
}
