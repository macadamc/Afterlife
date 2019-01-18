using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.StateMachine;
using Sirenix.OdinInspector;

public class UseItemState : State
{
    [MinMaxSlider(0.0f,5.0f)]
    public Vector2 holdTime;
    public bool changeStateOnFinish;

    private Coroutine coroutine;
    private InputController inputController;
    private ItemController itemController;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (inputController == null)
            inputController = GetComponentInParent<InputController>();

        if (itemController == null)
            itemController = GetComponentInParent<ItemController>();
    }

    protected override void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        inputController.input.SetValue(false);
    }

    private void Update()
    {
        if (Time.time >= itemController._nextPossibleUseTime && coroutine == null)
        {
            coroutine = StartCoroutine(UseItem());
        }
        else if (coroutine == null && changeStateOnFinish)
        {
            Next();
        }
    }


    public IEnumerator UseItem()
    {
        inputController.input.pressed = true;
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
