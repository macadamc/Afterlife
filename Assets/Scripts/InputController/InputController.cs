﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [System.Serializable]
    public class Button
    {
        public bool pressed;
        public bool held;
        public bool released;

        public void Evaluate(string buttonName)
        {
            pressed = SimpleInput.GetButtonDown(buttonName);
            held = SimpleInput.GetButton(buttonName);
            released = SimpleInput.GetButtonUp(buttonName);
        }
        public void SetValue(bool value)
        {
            pressed = value;
            held = value;
            released = value;
        }
    }

    public Vector2 joystick = Vector2.zero;
    public Vector2 lookDirection = Vector2.zero;
    public bool strafe;
    public Button input;
    public Button interact;
    public Button dodge;

    public void SetLookDirection()
    {
        if (joystick.magnitude == 0.0f || strafe)
            return;

        lookDirection = joystick.normalized;

    }

    public void SetLookDirection(Vector2 dir)
    {
        if (strafe || dir.magnitude == 0.0f)
            return;

        lookDirection = dir.normalized;

    }



}