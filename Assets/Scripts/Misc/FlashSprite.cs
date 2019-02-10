using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlashSprite : MonoBehaviour {

    public Transform T
    {
        get
        {
            if (flashTransform != null)
                return flashTransform;
            else
                return transform;
        }
    }

    public enum SpriteState { On, Off}
    public SpriteState stateWhenFinished;

    public Transform flashTransform;

    public float offTime = 0.2f;
    public float onTime = 0.2f;
    public int flashes = 4;

    public UnityEvent onStartEvent;
    public UnityEvent onFinishEvent;

    //bool _flashing;
    SpriteRenderer[] _renderers;

    private void OnEnable()
    {
        _renderers = T.GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(Flash(flashes));
    }

    protected IEnumerator Flash(int flashes)
    {
        onStartEvent.Invoke();
        //_flashing = true;
        for (int i = 0; i < flashes; i++)
        {
            foreach (SpriteRenderer r in _renderers)
            {
                r.enabled = false;
            }
            yield return new WaitForSeconds(offTime);
            foreach (SpriteRenderer r in _renderers)
            {
                r.enabled = true;
            }
            yield return new WaitForSeconds(onTime);
        }
        //_flashing = false;

        if(stateWhenFinished == SpriteState.On)
        {
            foreach (SpriteRenderer r in _renderers)
            {
                r.enabled = true;
            }
        }
        else
        {
            foreach (SpriteRenderer r in _renderers)
            {
                r.enabled = false;
            }
        }

        onFinishEvent.Invoke();
        yield return null;
    }
}
