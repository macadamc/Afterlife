using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnGameObject : MonoBehaviour
{
    public GameObject prefabToSpawn;
    [Range(0,20)]
    public float randomSpawnRange = 0;
    public float initialDelay = 0.0f;
    public int amount = 1;

    List<GameObject> m_spawnedObjects = new List<GameObject>();
    float m_timer;
    bool m_spawned;

    private void OnEnable()
    {
        m_timer = initialDelay;
        m_spawned = false;
    }

    private void OnDisable()
    {
        DeSpawn();
    }

    private void Update()
    {
        if(m_timer > 0)
        {
            m_timer -= Time.deltaTime;
        }
        else if(!m_spawned)
        {
            for (int i = 0; i < amount; i++)
            {
                Spawn(prefabToSpawn, (Vector2)transform.position + Random.insideUnitCircle * randomSpawnRange);
            }
            m_spawned = true;
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

        Gizmos.DrawWireSphere(transform.position, randomSpawnRange);
        
    }
}
