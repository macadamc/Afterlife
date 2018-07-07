using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FakeZAxis : MonoBehaviour {

    public Tilemap BackgroundTilemap
    {
        get
        {
            if (_tilemap == null)
                _tilemap = GameObject.FindGameObjectWithTag("Tilemap_BG").GetComponent<Tilemap>();

            return _tilemap;
        }
    }

    public Transform objectToMove;
    public float height;
    public float velocity;
    public float gravity = 7.0f;
    public string onGroundLayerName;
    public string inAirLayerName;
    public float waterOffset = 0.2f;
    public float baseOffset = 0.0f;
    public TileList waterTiles;


    Vector3 targetPos;
    Tilemap _tilemap;
    float _currentOffset;

    private void LateUpdate()
    {
        if (waterTiles != null && InWater())
            _currentOffset = waterOffset;
        else
            _currentOffset = baseOffset;

        targetPos.y = height + _currentOffset;
        targetPos.x = 0.0f;
        objectToMove.localPosition = targetPos;

        if(Application.isPlaying)
            UpdateHeight();
    }


    private void UpdateHeight()
    {
        if(height != 0f || velocity != 0f)
        {
            gameObject.layer = LayerMask.NameToLayer(inAirLayerName);
            velocity += gravity * Time.deltaTime;

            height -= velocity * Time.deltaTime;

            if (height < 0f)
            {
                height = 0f;
                velocity = 0f;
                gameObject.layer = LayerMask.NameToLayer(onGroundLayerName);
            }
        }

            
    }

    public bool InWater()
    {
        TileBase tile = BackgroundTilemap.GetTile(BackgroundTilemap.WorldToCell(transform.position));
        return waterTiles.tiles.Contains(tile);
    }
}
