using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class TriggerOnNoEnemys : MonoBehaviour
{
    [System.Serializable]
    public class NoEnemyEvent
    {
        public UnityEvent ev;
    }

    public ShadyPixel.RuntimeSets.TransformRuntimeSet activeEnemys;

    [DrawWithUnity]
    public NoEnemyEvent onTriggered;

    bool triggered = false;
    public CameraZone targetCameraZone;

    private void Awake()
    {
        if(targetCameraZone == null)
            targetCameraZone = GetComponent<CameraZone>();
    }


    public void Update()
    {
        if(activeEnemys.Items.Count == 0 && triggered == false)
        {
            onTriggered.ev.Invoke();
            triggered = true;
        }
    }
}
