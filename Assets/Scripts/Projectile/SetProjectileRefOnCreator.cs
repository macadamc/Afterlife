using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetProjectileRefOnCreator : MonoBehaviour
{
    public Projectile projectile;

    private void Start()
    {
        var pRef = projectile.creator.GetComponentInParent<ProjectileRef>();

        if (pRef != null)
            pRef.value = projectile;
    }
}