using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TileRef : MonoBehaviour
{
    Tilemap _tilemap;

    public TileBase GetTile(Vector2 position)
    {
        if (_tilemap == null)
            return null;

        return _tilemap.GetTile(_tilemap.WorldToCell(position));
    }

    private void Awake()
    {
        _tilemap = GameObject.FindGameObjectWithTag("Tilemap_BG").GetComponent<Tilemap>();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _tilemap = GameObject.FindGameObjectWithTag("Tilemap_BG").GetComponent<Tilemap>();
    }
}
