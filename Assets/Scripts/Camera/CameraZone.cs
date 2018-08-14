using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : TriggerZone
{
    public Transform cameraZoneTriggerEvents;

    CameraFollow _camFollow;
    CameraFollow CameraFollow
    {
        get
        {
            if (_camFollow == null)
                _camFollow = Camera.main.GetComponent<CameraFollow>();

            return _camFollow;
        }
    }

    public BoxCollider2D col;
    BoxCollider2D Collider
    {
        get
        {
            if (col == null)
                col = GetComponent<BoxCollider2D>();

            return col;
        }
    }

    CameraZoneEvent[] zoneEvents;

    private void OnEnable()
    {
        if(cameraZoneTriggerEvents !=null)
            zoneEvents = cameraZoneTriggerEvents.GetComponentsInChildren<CameraZoneEvent>();
    }

    protected override void OnEnter(Collider2D collision)
    {
        CameraFollow.SetBounds(Collider.bounds);
        TriggerEnterEvents();
    }

    protected override void OnStay(Collider2D collision)
    {
        if(CameraFollow._bounds != Collider.bounds)
        {
            CameraFollow.SetBounds(Collider.bounds);
        }
    }

    protected override void OnExit(Collider2D collision)
    {
        TriggerExitEvents();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(Collider.bounds.center, Collider.bounds.size);
    }


    private void TriggerEnterEvents()
    {
        if (zoneEvents == null || zoneEvents.Length == 0)
            return;

        foreach (CameraZoneEvent e in zoneEvents)
        {
            e.CheckTriggerEventKeys();
        }
    }

    private void TriggerExitEvents()
    {
        if (zoneEvents == null || zoneEvents.Length == 0)
            return;

        foreach (CameraZoneEvent e in zoneEvents)
        {
            e.TriggerExitEvent();
        }
    }
}
