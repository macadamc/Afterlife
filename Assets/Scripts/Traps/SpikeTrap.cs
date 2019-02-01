using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpikeTrap : MonoBehaviour
{
    protected enum state { INIT, BEFOREUP, BEFOREDOWN, DOWN };

    public enum BehaviourType { FixedTime, Triggered }
    public BehaviourType behaviourType;

    public float initalDelay, delayBeforeUp = 0;
    public float delayBeforeDown = 1f;

    public Collider2D hurtbox;

    [ShowIf("behaviourType", BehaviourType.Triggered)]
    public bool overrideDownState;
    [ShowIf("overrideDownState")]
    public bool hold = true;

    float m_nextStateTime;
    Coroutine m_coroutine;
    Animator m_anim;
    protected int currentState;

    WaitUntil WaitForNextState(float time)
    {
        m_nextStateTime = Time.time + time;
        return new WaitUntil(() => { return Time.time >= m_nextStateTime; });
    }
    private void FixedUpdate()
    {
        if (behaviourType == BehaviourType.Triggered)
            return;

        if (Time.time < m_nextStateTime)
            return;

        var coll = hurtbox.GetComponent<BoxCollider2D>();

        
        if (currentState == 4)
            currentState = 0;

        switch (currentState)
        {
            case 0:
                m_nextStateTime = Time.time + initalDelay;
                break;
            case 1:
                m_anim.SetTrigger("TrapSprung");
                m_nextStateTime = Time.time + delayBeforeUp;
                break;
            case 2:
                m_anim.SetTrigger("Trigger_Up");
                hurtbox.gameObject.SetActive(true);
                coll.isTrigger = false;
                m_nextStateTime = Time.time + delayBeforeDown;
                break;
            case 3:
                m_anim.SetTrigger("Trigger_Down");
                hurtbox.gameObject.SetActive(false);
                coll.isTrigger = true;
                break;
        }

        currentState++;
    }

    IEnumerator DoTriggered()
    {
        if (initalDelay > 0)
        {
            yield return WaitForNextState(initalDelay);
        }
        m_anim.SetTrigger("TrapSprung");

        if (delayBeforeUp > 0)
        {
            yield return WaitForNextState(delayBeforeUp);
        }
        m_anim.SetTrigger("Trigger_Up");
        hurtbox.gameObject.SetActive(true);
        yield return null;
        var coll = hurtbox.GetComponent<Collider2D>();
        coll.isTrigger = false;

        if (overrideDownState)
        {
            yield return new WaitUntil(() => { return hold == false; });
            hold = true;
        }

        if (delayBeforeDown > 0)
        {
            yield return WaitForNextState(delayBeforeDown);
        }
        else
        {
            yield return null;
        }
        m_anim.SetTrigger("Trigger_Down");
        hurtbox.gameObject.SetActive(false);
        coll.isTrigger = true;

        m_coroutine = null;
    }

    IEnumerator DoFixedTime()
    {
        while (gameObject.activeSelf)
        {
            if (initalDelay > 0)
            {
                yield return WaitForNextState(initalDelay);
            }
            m_anim.SetTrigger("TrapSprung");

            if (delayBeforeUp > 0)
            {
                yield return WaitForNextState(delayBeforeUp);
            }
            m_anim.SetTrigger("Trigger_Up");
            hurtbox.gameObject.SetActive(true);
            yield return null;
            var coll = hurtbox.GetComponent<BoxCollider2D>();
            coll.isTrigger = false;

            if (delayBeforeDown > 0)
            {
                yield return WaitForNextState(delayBeforeDown);
            }
            else
            {
                yield return null;
            }
            m_anim.SetTrigger("Trigger_Down");
            hurtbox.gameObject.SetActive(false);
            coll.isTrigger = true;
            yield return null;
        }
    }

    [Button]
    public void Trigger()
    {
        if (m_coroutine != null || behaviourType != BehaviourType.Triggered)
            return;

        m_coroutine = StartCoroutine(DoTriggered());
    }

    private void OnEnable()
    {
        if(m_anim == null)
            m_anim = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        if(m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
            m_coroutine = null;
        }
    }
}
