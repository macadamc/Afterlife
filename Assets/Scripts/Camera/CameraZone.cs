using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraZone : InteractOnTrigger2D
{
    /*
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
*/
    public BoxCollider2D BoxCollider2D
    {
        get
        {
            if (col == null)
                col = GetComponent<BoxCollider2D>();
            return col;
        }
    }
    BoxCollider2D col;
    bool m_InBounds;

    protected override void Reset()
    {
        base.Reset();
    }

    protected override void ExecuteOnEnter(Collider2D other)
    {
        OnEnter.Invoke();
        DoInventoryChecks(other);
        CameraFollow.Instance.SetBounds(BoxCollider2D.bounds);
        m_InBounds = true;
    }

    void Update()
    {
        if(m_InBounds)
        {
            if (CameraFollow.Instance._bounds != BoxCollider2D.bounds)
            {
                CameraFollow.Instance.SetBounds(BoxCollider2D.bounds);
            }
        }
    }

    protected override void ExecuteOnExit(Collider2D other)
    {

        OnExit.Invoke();
        m_InBounds = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(BoxCollider2D.bounds.center, BoxCollider2D.bounds.size);
    }
}
