using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabShootingTrap : TrapBase
{
    public GameObject projectile;
    public List<Transform> targets;
    public bool targetPlayer;

    private void Awake()
    {
        if (targets == null)
            targets = new List<Transform>();
        if (targetPlayer)
            targets.Add(GameObject.FindGameObjectWithTag("Player").transform);
    }

    public override void OnTriggered()
    {
        foreach(Transform t in targets)
        {
            if (t.gameObject.activeSelf == false)
                continue;

            var lookDir = (t.position - transform.position).normalized;

            //  spawns object and sets parent
            var heldItem = Instantiate(projectile);

            //  sets transform of spawned object
            heldItem.transform.position = (Vector2)transform.position;

            //  gets the angle from the look direction
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            //  rotates object to face the new angle
            heldItem.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

}
