using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnGameObject : CameraZoneEvent
{
    public GameObject prefabToSpawn;

    public bool isLocal;

    public bool persistant;
    public bool IsPersistant { get { return persistant; } }

    GameObject spawnedObject;

    [Range(0,20)]
    public float randomSpawnRange;

    public bool rotateSpawnedObject = true;

    protected override void OnTrigger()
    {
        spawnedObject = Instantiate(prefabToSpawn, transform.position + (Vector3)Random.insideUnitCircle*randomSpawnRange, rotateSpawnedObject ? Quaternion.identity : prefabToSpawn.transform.rotation);
        Spawnable spawnable = spawnedObject.GetComponent<Spawnable>();
        spawnable.spawner = this;
        spawnable.isLocal = isLocal;
        spawnable.persistant = persistant;
    }

    protected override void OnExit()
    {
        if(spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(randomSpawnRange == 0.0f)
        {
            Gizmos.DrawLine(transform.position - Vector3.right / 2, transform.position + Vector3.right / 2);
            Gizmos.DrawLine(transform.position - Vector3.up / 2, transform.position + Vector3.up / 2);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, randomSpawnRange);
        }
    }
}
