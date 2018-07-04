using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGameObject : CameraZoneEvent
{
    public GameObject prefabToSpawn;
    public bool isLocal;
    public bool persistant;
    GameObject spawnedObject;

    protected override void OnTrigger()
    {
        spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
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
}
