using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [Header("Variable References")]
    public IntReference currentHealth;
    public IntReference maxHealth;
    public int tileSize = 16;

    [Header("HealthBar References")]
    public Image heartsBackground;
    public Image heartsForeground;

    public void Start()
    {
        UpdateHealthBar();
        Player.Instance.GetComponent<Health>().onHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        if(Player.Instance != null)
            Player.Instance.GetComponent<Health>().onHealthChanged -= UpdateHealthBar;
    }

    public void UpdateHealthBar()
    {
        heartsBackground.rectTransform.sizeDelta = new Vector2(maxHealth.Value * tileSize, tileSize);
        heartsForeground.rectTransform.sizeDelta = new Vector2(currentHealth.Value * tileSize, tileSize);
    }
    public void UpdateHealthBar(int change)
    {
        heartsBackground.rectTransform.sizeDelta = new Vector2(maxHealth.Value * tileSize, tileSize);
        heartsForeground.rectTransform.sizeDelta = new Vector2(currentHealth.Value * tileSize, tileSize);
    }
}
