using System.Collections;
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

    public Vector2 joystick;
    public Button input;

}
