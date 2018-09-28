using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [Header("Variable References")]
    public IntVariable currentHealth;
    public IntVariable maxHealth;
    public int tileSize = 16;

    [Header("HealthBar References")]
    public Image heartsBackground;
    public Image heartsForeground;

    public void OnEnable()
    {
        currentHealth.onValueChanged += UpdateCurrentHealth;
        maxHealth.onValueChanged += UpdateMaxHealth;
        UpdateHealth();
    }

    public void OnDisable()
    {
        currentHealth.onValueChanged -= UpdateCurrentHealth;
        maxHealth.onValueChanged -= UpdateMaxHealth;
    }

    public void UpdateCurrentHealth()
    {
        heartsForeground.rectTransform.sizeDelta = new Vector2(currentHealth.GetValue() * tileSize, tileSize);
    }

    public void UpdateMaxHealth()
    {
        heartsBackground.rectTransform.sizeDelta = new Vector2(maxHealth.GetValue() * tileSize, tileSize);
    }

    public void UpdateHealth()
    {
        UpdateCurrentHealth();
        UpdateMaxHealth();
    }
}
