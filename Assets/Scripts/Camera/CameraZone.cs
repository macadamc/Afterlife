using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraZone : InteractOnTrigger2D
{
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
    public List<GameObject> GameObjectsToChangeStateWhileEnteringAndExitingCameraZone = new List<GameObject>();

    protected override void Reset()
    {
        base.Reset();
    }

    private void OnEnable()
    {
        UpdateStates(false);
    }

    protected override void ExecuteOnEnter(Collider2D other)
    {
        OnEnter.Invoke();
        DoInventoryChecks(other);
        CameraFollow.Instance.SetBounds(this);
        m_InBounds = true;
        UpdateStates(true);
    }

    void Update()
    {
        if(m_InBounds)
        {
            if (CameraFollow.Instance._cameraZone != this)
            {
                CameraFollow.Instance.SetBounds(this);
            }
        }
    }

    protected override void ExecuteOnExit(Collider2D other)
    {
        OnExit.Invoke();
        m_InBounds = false;
        UpdateStates(false);
    }

    void UpdateStates(bool state)
    {
        if (GameObjectsToChangeStateWhileEnteringAndExitingCameraZone == null)
            return;
        foreach(GameObject go in GameObjectsToChangeStateWhileEnteringAndExitingCameraZone)
        {
            go.SetActive(state);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(BoxCollider2D.bounds.center, BoxCollider2D.bounds.size);
    }
}
