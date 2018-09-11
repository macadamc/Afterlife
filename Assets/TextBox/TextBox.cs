using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using TMPro;

public class TextBox : MonoBehaviour
{
    public bool useTimeScale = true;
    public GameObject choiceContainer;
    public TextMeshProUGUI text;
    public RectTransform bg;

    public TweenBase cTween = null;

    public virtual void EnabledTextBox(bool tween= true)
    {
        gameObject.SetActive(true);
        if(tween)
            cTween = Tween.LocalScale(GetComponent<RectTransform>(), Vector2.zero, Vector2.one, 0.5f, 0f, Tween.EaseInOut);
        else
        {
            transform.localScale = Vector3.one;
        }
    }
    public virtual void DisableTextbox(bool tween = true)
    {
        var rt = GetComponent<RectTransform>();
        if(rt != null)
        {
            if(tween)
            {
                cTween = Tween.LocalScale(rt, Vector2.one, Vector2.zero, 0.5f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, () => { gameObject.SetActive(false); });
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
            
    } 

}
