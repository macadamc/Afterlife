using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class GameobjectSpawnerInfo : MonoBehaviour
{
    public GameObject gameObjectPrefab;
    [MinValue(1)]
    public int amountToSpawn = 1;
    [MinValue(0f)]
    public float spawnRadius = 1f;
    public Color gizmoColor = Color.yellow;

    GameObject SpawnObject(GameObject prefab, Vector2 pos)
    {
        GameObject go = Instantiate(prefab,pos,Quaternion.identity);
        return go;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}


