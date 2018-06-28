using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : TriggerZone
{
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

    BoxCollider2D col;
    BoxCollider2D Collider
    {
        get
        {
            if (col == null)
                col = GetComponent<BoxCollider2D>();

            return col;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

        base.OnTriggerEnter2D(collision);
        if (triggerTags.Count > 0)
        {
            if (HasTag(collision.gameObject) == false)
                return;
        }
        CameraFollow.SetBounds(Collider.bounds);
    }

    private void OnTriggerStay(Collider other)
    {
        if(CameraFollow._bounds != Collider.bounds)
        {
            CameraFollow.SetBounds(Collider.bounds);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(Collider.bounds.center, Collider.bounds.size);
    }
}
