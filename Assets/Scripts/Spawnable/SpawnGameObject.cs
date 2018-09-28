using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class SpawnGameObject : MonoBehaviour
{
    [TabGroup("Gizmo")]
    public Color GizmoColor = Color.yellow;

    [TabGroup("References")]
    public GameObject prefabToSpawn;
    [TabGroup("References")]
    public Transform spawnTransform;

    [TabGroup("Spawn Settings")]
    public bool triggerOnce;
    [TabGroup("Spawn Settings"), MinMaxSlider(0f, 10f, true)]
    public Vector2 startDelay;
    [TabGroup("Spawn Settings")]
    public float spawnRange;
    [TabGroup("Spawn Settings")]
    public Vector2Int spawnAmount;
    [TabGroup("Spawn Settings")]
    public bool despawnOnDisable;
    [TabGroup("Spawn Settings")]
    public bool restrictMaxAllowedAtOnce = false;
    [TabGroup("Spawn Settings"), ShowIf("restrictMaxAllowedAtOnce")]
    public int maxSpawnedAtOnce = 1;
    [TabGroup("Spawn Settings")]
    public bool restrictMaxOverLifetime = false;
    [TabGroup("Spawn Settings"), ShowIf("restrictMaxOverLifetime")]
    public int maxSpawnedOverLifetime = 1;
    [TabGroup("Spawn Settings"), MinMaxSlider(0f, 10f, true)]
    public Vector2 timeBetweenSpawning;
    [TabGroup("Spawn Settings")]
    public bool customSpawnEvent;
    [TabGroup("Spawn Settings")]
    public UnityEvent onTriggerSpawnEvent;


    List<GameObject> m_spawnedObjects = new List<GameObject>();
    float m_timer;
    int m_totalSpawned;
    bool m_waiting;
    bool m_triggered;


    public void ForceTriggerNextSpawnWave()
    {
        int randomAmt = Random.Range(spawnAmount.x, spawnAmount.y);

        for (int i = 0; i < randomAmt; i++)
        {
            Spawn(prefabToSpawn, (Vector2)spawnTransform.position + Random.insideUnitCircle * spawnRange);
        }

        m_waiting = false;
    }

    protected void SpawnWave()
    {
        m_triggered = true;

        m_waiting = true;

        if (onTriggerSpawnEvent != null)
            onTriggerSpawnEvent.Invoke();

        if (customSpawnEvent)
            return;

        ForceTriggerNextSpawnWave();
    }

    protected void Spawn(GameObject prefab, Vector2 position)
    {
        GameObject go = Instantiate(prefab, position, Quaternion.identity);
        m_spawnedObjects.Add(go);
        m_totalSpawned++;
    }

    protected void DeSpawn()
    {
        for (int i = m_spawnedObjects.Count - 1; i >= 0; i--)
        {
            Destroy(m_spawnedObjects[i]);
        }
        m_spawnedObjects.Clear();
    }

    private void Update()
    {
        if (m_timer > 0)
        {
            if(!m_waiting)
                m_timer -= Time.deltaTime;
        }
        else
        {
            UpdateSpawnedObjects();
            SetDelayTime(Random.Range(timeBetweenSpawning.x, timeBetweenSpawning.y));

            if (triggerOnce && m_triggered)
                return;

            if (restrictMaxAllowedAtOnce && m_spawnedObjects.Count >= maxSpawnedAtOnce)
                return;

            if (restrictMaxOverLifetime && m_totalSpawned >= maxSpawnedOverLifetime)
                return;

            SpawnWave();
        }
    }

    private void UpdateSpawnedObjects()
    {
        for (int i = m_spawnedObjects.Count-1; i >= 0; i--)
        {
            if (!m_spawnedObjects[i].gameObject.activeInHierarchy)
                m_spawnedObjects.Remove(m_spawnedObjects[i].gameObject);
        }
    }

    private void SetDelayTime(float time)
    {
        m_timer = time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }

    private void OnEnable()
    {
        if (spawnTransform == null)
            spawnTransform = transform;

        SetDelayTime(Random.Range(startDelay.x, startDelay.y));

    }

    private void OnDisable()
    {
        if(despawnOnDisable)
            DeSpawn();

        StopAllCoroutines();
    }
}
