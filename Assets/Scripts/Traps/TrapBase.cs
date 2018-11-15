using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TrapBase : MonoBehaviour
{
    public enum BehaviourType { FixedTime, Triggered }
    public BehaviourType behaviourType;

    public bool IsFixedTime { get { return behaviourType == BehaviourType.FixedTime; } }
    public bool IsTriggered { get { return behaviourType == BehaviourType.Triggered; } }

    public float delay;

    [ShowIf("IsFixedTime")]
    public float StartOffset;

    float NextStateChangeTime;

    Animator anim;
    Coroutine coroutine;

    [DrawWithUnity]
    public UnityEngine.Events.UnityEvent onTriggered;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        if (behaviourType == BehaviourType.FixedTime)
            coroutine = StartCoroutine(DoFixedTime());
        
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
                NextStateChangeTime = delay + Time.time;
            }

            if (Time.time >= NextStateChangeTime)
            {
                NextStateChangeTime = 0;
                OnTriggered();
                onTriggered?.Invoke();
            }
            yield return null;
        }
    }

    [Button, ShowIf("IsTriggered")]
    public void Trigger()
    {
        if (coroutine != null || behaviourType != BehaviourType.Triggered)
            return;

        coroutine = StartCoroutine(DoTriggered());
    }

    public virtual void OnTriggered() { }

    IEnumerator DoTriggered()
    {
        NextStateChangeTime = Time.time + delay;
        yield return new WaitUntil(() => { return Time.time >= NextStateChangeTime; });

        OnTriggered();
        onTriggered?.Invoke();

        coroutine = null;
    }
    public void print(string str)
    {
        Debug.Log(str);
    }
}
