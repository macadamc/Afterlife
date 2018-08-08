using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Health : MonoBehaviour
{
    [System.Serializable]
    public class Events
    {
        public UnityEvent onHurt;
        public UnityEvent onDeath;
    }

    public delegate void OnHealthChanged(int change);
    public OnHealthChanged onHealthChanged;

    public IntReference currentHealth;
    public IntReference maxHealth;
    public int damageFlashAmount = 3;
    //public GameObject spawnOnDeath;
    public bool deactivateRootObjectOnDeath;
    [DrawWithUnity]
    public Events events;

    //int _currentHealth;
    bool _initialized;
    bool _flashing;
    SpriteRenderer[] _sprites;

    public virtual void ChangeHealth(int change)
    {
        if (_flashing)
            return;

        currentHealth.Value += change;

        if (currentHealth.Value > maxHealth.Value)
            currentHealth.Value = maxHealth.Value;

        if (change < 0)
            events.onHurt.Invoke();

        StartCoroutine(DamageFlash(damageFlashAmount));

        if (onHealthChanged != null)
            onHealthChanged.Invoke(change);
    }

    protected IEnumerator DamageFlash(int flashes)
    {
        _flashing = true;
        for (int i = 0; i < flashes; i++)
        {
            foreach(SpriteRenderer r in _sprites)
            {
                r.enabled = false;
            }
            yield return new WaitForSeconds(0.1f);
            foreach (SpriteRenderer r in _sprites)
            {
                r.enabled = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
        _flashing = false;
        yield return null;
        CheckHealth();
    }

    protected void CheckHealth()
    {
        if (currentHealth.Value <= 0)
        {
            /*
            if (spawnOnDeath != null)
                Instantiate(spawnOnDeath, transform.position, Quaternion.identity);
                */

            events.onDeath.Invoke();

            if (deactivateRootObjectOnDeath)
                gameObject.SetActive(false);
        }
    }

    protected virtual void Initialize()
    {
        currentHealth.Value = maxHealth.Value;
        _initialized = true;
        _sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if (!_initialized)
            Initialize();
    }

    [Button]
    public void Damage()
    {
        ChangeHealth(-1);
    }
}
