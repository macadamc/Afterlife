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
    public Transform spawnerTransform;
    CameraZoneEvent[] events;

    private void Awake()
    {
        if(targetCameraZone == null)
            targetCameraZone = GetComponent<CameraZone>();
        if (spawnerTransform != null)
            events = spawnerTransform.GetComponentsInChildren<CameraZoneEvent>();
    }


    public void Update()
    {
        if(activeEnemys.Items.Count == 0 && triggered == false && CameraFollow.Instance?._bounds == targetCameraZone?.bounds)
        {
            onTriggered.ev.Invoke();
            triggered = true;
            TriggerEnterEvents(events);
        }
    }

    private bool TriggerEnterEvents(CameraZoneEvent[] events)
    {
        if (events == null || events.Length == 0)
            return false;

        foreach (CameraZoneEvent e in events)
        {
            e.CheckTriggerEventKeys();
        }
        return true;
    }
}
