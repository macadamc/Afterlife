using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldSword : HeldItem
{
    public ParticleSystem ps;
    public float MaxParticlesAtFullCharge;

    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule emission;

    private ParticleSystem.MinMaxCurve lastSpawnOvertime;
    private ParticleSystem.MinMaxCurve lastSpeed;

    private float startTime;
    private float minChargeTime;
    ItemSpawnPrefabWithCharge item;

    public override void Begin(ItemController user)
    {
        item = user.currentItem as ItemSpawnPrefabWithCharge;

        emission = ps.emission;
        main = ps.main;

        lastSpawnOvertime = emission.rateOverTime;
        lastSpeed = main.startSpeed;

        startTime = Time.time;
        if(item != null)
        {
            minChargeTime = Time.time + item.chargeTime;
        }

        main.maxParticles = 0;// Mathf.RoundToInt(MaxParticlesAtFullCharge);
    }

    public override void Hold(ItemController user)
    {
        main.maxParticles = Mathf.RoundToInt(MaxParticlesAtFullCharge);
        if(Time.time >= minChargeTime)
        {
            float chargePercentage = Time.time / minChargeTime;

            emission.rateOverTime = MaxParticlesAtFullCharge * chargePercentage;
            main.startSpeed = Mathf.Clamp(1f * chargePercentage, .25f, 1f); 
        }
        
    }

    public override void End(ItemController user)
    {
        emission.rateOverTime = 0;
        main.startSpeed = lastSpeed;
        ps.transform.parent = null;
    }
}