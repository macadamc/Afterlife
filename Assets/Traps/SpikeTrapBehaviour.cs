using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ShadyPixel.Audio;


public class SpikeTrapBehaviour : MonoBehaviour
{
    public enum BehaviourType { FixedTime, Triggered }
    public BehaviourType behaviourType;

    public bool IsFixedTime { get { return behaviourType == BehaviourType.FixedTime; } }

    public float upDelay;
    public float downDelay;

    [ShowIf("IsFixedTime")]
    public float StartOffset;

    bool isUp;
    float NextStateChangeTime;

    Animator anim;
    public BoxCollider2D hurtbox;
    Coroutine coroutine;

    private void OnEnable()
    {
        if (behaviourType == BehaviourType.FixedTime)
            coroutine = StartCoroutine(DoFixedTime());
        anim = GetComponent<Animator>();
    }

    IEnumerator DoFixedTime()
    {
        if (StartOffset > 0)
        {
            float waittime = Time.time + StartOffset;
            yield return new WaitUntil(() => { return Time.time >= waittime; });
        }
        else
        {
            yield return null;
        }

        while (coroutine != null)
        {
            if (NextStateChangeTime == 0)
            {
                float delay = isUp ? upDelay : downDelay;
                NextStateChangeTime = delay + Time.time;
            }

            if (Time.time >= NextStateChangeTime)
            {
                isUp = !isUp;
                string trigger = isUp ? "Trigger_Up" : "Trigger_Down";
                anim.SetTrigger(trigger);
                NextStateChangeTime = 0;
                hurtbox.gameObject.SetActive(isUp);
            }
            yield return null;
        }
    }

    public void Trigger()
    {
        if (coroutine != null || behaviourType != BehaviourType.Triggered)
            return;

        coroutine = StartCoroutine(DoTriggered());
    }

    IEnumerator DoTriggered()
    {
        NextStateChangeTime = Time.time + upDelay;
        yield return new WaitUntil(() => { return Time.time >= NextStateChangeTime; });
        anim.SetTrigger("Trigger_Up");
        hurtbox.gameObject.SetActive(true);

        NextStateChangeTime = Time.time + downDelay;
        yield return new WaitUntil(() => { return Time.time >= NextStateChangeTime; });
        anim.SetTrigger("Trigger_Down");
        hurtbox.gameObject.SetActive(false);

        coroutine = null;
    }
}
