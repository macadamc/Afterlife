using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : MonoBehaviour
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
    }

    public Vector2 joystick;
    public Button input;

}
