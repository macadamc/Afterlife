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
            events.onDeath.Invoke();
    }

    protected virtual void Start()
    {
        if (!_initialized)
            Initialize();
    }
}
