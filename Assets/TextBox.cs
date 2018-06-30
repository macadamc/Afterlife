using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class TextBox : MonoBehaviour
{
    public GameObject choiceContainer;
    public Text textComp;
    public float delay;
    public bool running;

    public bool autoSize;
    [ShowIf("autoSize"), Indent]
    public Vector2 maxSize;
    [ShowIf("autoSize"), Indent]
    public Vector2 padding;

    public enum InputType { Player, Passive }
    public InputType inputType;

    [ShowIf("IsPassive")]
    public float passiveLineDelay;
    bool IsPassive() { return inputType == InputType.Passive; }

}
