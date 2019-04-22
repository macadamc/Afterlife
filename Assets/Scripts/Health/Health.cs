using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Variables;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Health : MonoBehaviour, IDataPersister
{
    [System.Serializable]
    public class Events
    {
        public UnityEvent onHurt;
        public UnityEvent onDeath;
    }

    public delegate void OnHealthChanged(int change);
    public OnHealthChanged onHealthChanged;

    public delegate void OnGameObjectDeath();
    public OnGameObjectDeath onGameObjectDeath;

    public IntReference currentHealth;
    public IntReference maxHealth;
    public int damageFlashAmount = 3;
    public Transform SpriteRoot;
    //public GameObject spawnOnDeath;
    public bool deactivateRootObjectOnDeath;
    [DrawWithUnity]
    public Events events;

    public DataSettings dataSettings;

    //int _currentHealth;
    bool _initialized;
    public bool invincible;
    SpriteRenderer[] _sprites;


    public virtual void ChangeHealth(int change)
    {
        if (invincible)
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
        invincible = true;
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
        invincible = false;
        yield return null;
        CheckHealth();
    }

    protected virtual void CheckHealth()
    {
        if (currentHealth.Value <= 0)
        {
            events.onDeath.Invoke();

            if(onGameObjectDeath != null)
                onGameObjectDeath.Invoke();

            if (deactivateRootObjectOnDeath)
                gameObject.SetActive(false);
        }
    }

    protected virtual void OnEnable()
    {
        _sprites = SpriteRoot.GetComponentsInChildren<SpriteRenderer>();
        PersistentDataManager.RegisterPersister(this);
    }

    protected virtual void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    [Button]
    public void Damage()
    {
        ChangeHealth(-1);
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    public Data SaveData()
    {
        return new Data<int, int>(currentHealth.Value, maxHealth.Value);
    }

    public void LoadData(Data data)
    {
        Data<int, int> loadedData = data as Data<int,int>;

        if (loadedData == null)
            return;

        currentHealth.Value = loadedData.value0;
        maxHealth.Value = loadedData.value1;
    }
}
