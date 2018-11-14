using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToTarget : MonoBehaviour {

    public Transform transformToTeleport;
    public Targets targets;
    public float maxDistanceFromTarget = 4f;
    public LayerMask obstacleLayers;
    public bool teleportOnEnable = true;

    Vector2 teleportPosition;

    private void OnEnable()
    {
        if (teleportOnEnable)
        {
            Teleport();
        }
    }

    public void Teleport()
    {
        if (targets == null || targets.transforms.Count == 0)
            return;

        transformToTeleport.position = ValidTeleportPosition();
    }


    public Vector2 ValidTeleportPosition()
    {
        Vector2 targetPos = targets.transforms[Random.Range(0, targets.transforms.Count)].position;
        Vector2 potentialPos = targetPos + Random.insideUnitCircle * maxDistanceFromTarget;

        RaycastHit2D hit = Physics2D.Raycast(potentialPos, (targetPos - potentialPos).normalized, Vector2.Distance(potentialPos, targetPos), obstacleLayers);

        if (hit.collider == null)
            return potentialPos;
        else
            return ValidTeleportPosition();
    }
}
