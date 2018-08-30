using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class SpawnGameObject : MonoBehaviour
{
    public Color GizmoColor = Color.yellow;
    public GameObject prefabToSpawn;
    public float spawnRange;
    public int amount = 1;

    UnityAction CameraZoneEnter;
    UnityAction CameraZoneExit;

    List<GameObject> m_spawnedObjects = new List<GameObject>();
    float m_timer;
    bool m_spawned;

    private void Awake()
    {
        CameraZoneEnter += SpawnPrefabs;
        CameraZoneExit += DeSpawn;
    }

    private void OnEnable()
    {
        m_spawned = false;
        SpawnPrefabs();
    }

    private void OnDisable()
    {
        DeSpawn();
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
        Gizmos.color = GizmoColor;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
