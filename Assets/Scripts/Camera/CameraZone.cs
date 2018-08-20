using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraZone : InteractOnTrigger2D
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

    public Bounds bounds;

    bool m_InBounds;

    protected override void Reset()
    {
        base.Reset();
        bounds = GetComponent<BoxCollider2D>().bounds;
    }

    protected override void ExecuteOnEnter(Collider2D other)
    {
        //base.ExecuteOnEnter(other);
        if(other.gameObject.CompareTag("Player"))
        {
            OnEnter.Invoke();
            DoInventoryChecks(other);
            CameraFollow.SetBounds(bounds);
            m_InBounds = true;
        }
    }

    void Update()
    {
        if(m_InBounds)
        {
            if (CameraFollow._bounds != bounds)
            {
                CameraFollow.SetBounds(bounds);
            }
        }
    }

    protected override void ExecuteOnExit(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnExit.Invoke();
            m_InBounds = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
