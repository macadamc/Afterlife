using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;


public class GameobjectSpawnManager : MonoBehaviour
{
    //public List<GameobjectSpawnerInfo> spawners = new List<GameobjectSpawnerInfo>();
    public bool spawnOnEnable = true;
    public float delayBeforeSpawn = 0f;
    public GameObject spawnParticlePrefab;
    public UnityEvent onSpawnObjects;
    public UnityEvent onNoEnemies;

    bool spawned;
    public int spawnedObjects = 0;

    Coroutine runningCoroutine;

    bool triggered;

    void OnEnable()
    {
        if (spawnOnEnable)
            Spawn();
    }

    void Update()
    {
        if (PauseManager.Instance.Paused || !spawned)
            return;


        if (spawnedObjects <= 0)
        {
            onNoEnemies.Invoke();
            Debug.Log("No Enemies Left");
            spawned = false;
        }
    }

    public void Spawn()
    {
        if(runningCoroutine == null && triggered == false)
            runningCoroutine = StartCoroutine(SpawnObjects());

        triggered = true;
    }

    IEnumerator SpawnObjects()
    {
        onSpawnObjects.Invoke();
        yield return new WaitForSeconds(delayBeforeSpawn);

        foreach (Transform child in transform)
        {
            GameobjectSpawnerInfo spawnInfo = child.GetComponent<GameobjectSpawnerInfo>();
            if (spawnInfo != null)
            {
                for (int i = 0; i < spawnInfo.amountToSpawn; i++)
                {
                    Vector2 randSpawnPos = child.position + Random.insideUnitSphere * spawnInfo.spawnRadius;
                    GameObject spawnedObj = SpawnObject(spawnInfo.gameObjectPrefab, randSpawnPos);

                    if(spawnParticlePrefab != null)
                        Instantiate(spawnParticlePrefab, randSpawnPos, Quaternion.identity);

                    Health hp = spawnedObj.GetComponent<Health>();
                    if (hp != null)
                    {
                        hp.onGameObjectDeath += OnHealthComponentDeath;
                        spawnedObjects++;
                    }
                }
            }
        }
        runningCoroutine = null;
        spawned = true;
    }

    void OnHealthComponentDeath()
    {
        spawnedObjects--;
    }

    GameObject SpawnObject(GameObject prefab, Vector2 pos)
    {
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        return go;
    }

    [Button]
    void CreateNewSpawner()
    {
        GameObject go = new GameObject("Spawner Instance");
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.AddComponent<GameobjectSpawnerInfo>();
    }
}
