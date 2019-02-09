using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    public string healthBarDisplayText;
    BossHealthBar healthBar;

    public void Awake()
    {
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.SetTargetHealthComponent(this);
    }

    public override void ChangeHealth(int change)
    {
        base.ChangeHealth(change);
        healthBar.UpdateHealth();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.SetTargetHealthComponent(this);
    }

    protected override void OnDisable()
    {
        healthBar.Disable();

        base.OnDisable();
    }

    protected override void CheckHealth()
    {
        if (currentHealth <= 0)
            healthBar.Disable();

        base.CheckHealth();
    }


}
