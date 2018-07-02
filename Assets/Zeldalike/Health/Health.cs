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

    public IntReference startingHealth;
    public GameObject spawnOnDeath;
    public bool deactivateRootObjectOnDeath;
    [DrawWithUnity]
    public Events events;

    int _currentHealth;
    bool _initialized;

    protected virtual void Initialize()
    {
        _currentHealth = startingHealth;
        _initialized = true;
    }

    public virtual void ChangeHealth(int change)
    {
        _currentHealth += change;

        if (change < 0)
            events.onHurt.Invoke();

        if (_currentHealth <= 0)
        {
            if (spawnOnDeath != null)
                Instantiate(spawnOnDeath, transform.position, Quaternion.identity);

            events.onDeath.Invoke();

            if (deactivateRootObjectOnDeath)
                gameObject.SetActive(false);
        }
    }

    protected virtual void Start()
    {
        if (!_initialized)
            Initialize();
    }
}
