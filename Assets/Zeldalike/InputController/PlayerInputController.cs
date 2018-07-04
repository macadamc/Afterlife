﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";
    public string inputButtonName = "Fire1";

    public InputController inputController;

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        Init();
    }

    private void Init()
    {
        if (inputController == null)
            inputController = GetComponentInChildren<InputController>();

        inputController.joystick = Vector2.zero;
        inputController.input.SetValue(false);
    }

    private void Update()
    {
        // creats new vector 2 out of input axis
        inputController.joystick = new Vector2(SimpleInput.GetAxisRaw(horizontalAxisName), SimpleInput.GetAxisRaw(verticalAxisName));

        // if magnitude greater than 1, the input needs normalized so you can't move faster diagnal.
        if (inputController.joystick.magnitude > 1)
            inputController.joystick.Normalize();

        // check for input button presses.
        inputController.input.Evaluate(inputButtonName);
    }
}