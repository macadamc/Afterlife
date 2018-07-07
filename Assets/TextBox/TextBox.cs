using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Pixelplacement;
using Pixelplacement.TweenSystem;

public class TextBox : MonoBehaviour
{
    public GameObject choiceContainer;
    public Text textComp;
    public float delay;

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

    public TweenBase cTween = null;

    public virtual void EnabledTextBox()
    {
        gameObject.SetActive(true);
        cTween = Tween.LocalScale(GetComponent<RectTransform>(), Vector2.zero, Vector2.one, 0.5f, 0f, Tween.EaseSpring);
    }
    public virtual void DisableTextbox()
    {
        var rt = GetComponent<RectTransform>();
        if(rt != null)
            cTween = Tween.LocalScale(rt, Vector2.one, Vector2.zero, 0.5f, 0f, Tween.EaseIn, Tween.LoopType.None, null, () => { gameObject.SetActive(false); });
    } 

}
