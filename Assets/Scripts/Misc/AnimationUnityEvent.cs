using UnityEngine;
using UnityEngine.Events;

public class AnimationUnityEvent : MonoBehaviour {

    public UnityEvent onTriggerEvent;
    public void TriggerEvent()
    {
        if(onTriggerEvent != null)
            onTriggerEvent.Invoke();
    }
}
