using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TeleportToTarget : MonoBehaviour
{

    public Transform transformToTeleport;
    public Targets targets;

    [MinMaxSlider(0f, 20f, true)]
    public Vector2 minMaxDistance = new Vector2(2, 4f);

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

        Vector2 potentialPos = targetPos + Random.insideUnitCircle.normalized * Random.Range(minMaxDistance.x, minMaxDistance.y);

        RaycastHit2D hit = Physics2D.Raycast(potentialPos, (targetPos - potentialPos).normalized, Vector2.Distance(potentialPos, targetPos), obstacleLayers);

        if (hit.collider == null)
            return potentialPos;
        else
            return ValidTeleportPosition();
    }
}