using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour {

    BossHealth health;
    public Slider sliderComponent;
    public Text textComponent;

    public void OnEnable()
    {
        Disable();
    }

    public void UpdateHealth()
    {
        if(health != null)
        {
            sliderComponent.value = health.currentHealth;
            sliderComponent.maxValue = health.maxHealth;
            textComponent.text = health.healthBarDisplayText;
        }
    }

    public void SetTargetHealthComponent(BossHealth target)
    {
        Enable();
        health = target;
        UpdateHealth();
    }

    public void Enable()
    {
        sliderComponent.gameObject.SetActive(true);
        textComponent.gameObject.SetActive(true);
    }

    public void Disable()
    {
        sliderComponent.gameObject.SetActive(false);
        textComponent.gameObject.SetActive(false);
    }
}
