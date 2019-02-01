using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitProjectile : MonoBehaviour
{
    public string ItemSpawnPosGOName = "ItemSpawnTransform";
    private void OnTriggerEnter2D(Collider2D other)
    {
        var projectile = other.GetComponent<Projectile>();
        var heldItem = GetComponent<HeldItem>();
        var targets = heldItem.User.GetComponent<TargetTags>();

        if (projectile != null && heldItem != null && projectile.isHittable)
        {
            var targetlist = targets.GetTargets();

            var projTarg = projectile.GetComponent<TargetTags>();
            projTarg.SetTargets(targetlist);

            var dir = projectile.transform.position - heldItem.User.SpawnTransform.position;
            dir.Normalize();
            var angleToTarget = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            projectile.speed *= 1.35f;
            projectile.transform.rotation = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
        }
    }
}
