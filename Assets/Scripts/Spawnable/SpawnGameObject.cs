using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class SpawnGameObject : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnRange;
    public float initialDelay = 0.0f;
    public int amount = 1;

    UnityAction CameraZoneEnter;
    UnityAction CameraZoneExit;

    List<GameObject> m_spawnedObjects = new List<GameObject>();
    float m_timer;
    bool m_spawned;
    CameraZone m_cameraZone;

    private void Awake()
    {
        m_cameraZone = transform.parent.GetComponentInChildren<CameraZone>();

        CameraZoneEnter += SpawnPrefabs;
        CameraZoneExit += DeSpawn;
    }

    private void OnEnable()
    {
        m_cameraZone.OnEnter.AddListener(CameraZoneEnter);
        m_cameraZone.OnExit.AddListener(CameraZoneExit);

        m_timer = initialDelay;
        m_spawned = false;
    }

    private void OnDisable()
    {
        m_cameraZone.OnEnter.RemoveListener(CameraZoneEnter);
        m_cameraZone.OnExit.RemoveListener(CameraZoneExit);

        DeSpawn();
    }

    private void Update()
    {
        /*
        if(m_timer > 0)
        {
            m_timer -= Time.deltaTime;
        }
        else if(!m_spawned)
        {
            SpawnPrefabs();
            m_spawned = true;
        }
        */
    }

    public void SpawnPrefabs()
    {
        for (int i = 0; i < amount; i++)
        {
            Spawn(prefabToSpawn, (Vector2)transform.position + Random.insideUnitCircle * spawnRange);
        }
    }

    protected void Spawn(GameObject prefab, Vector2 position)
    {
        GameObject go = Instantiate(prefab, position, Quaternion.identity);
        m_spawnedObjects.Add(go);
    }

    protected void DeSpawn()
    {
        for (int i = m_spawnedObjects.Count - 1; i >= 0; i--)
        {
            Destroy(m_spawnedObjects[i]);
        }
        m_spawnedObjects.Clear();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
