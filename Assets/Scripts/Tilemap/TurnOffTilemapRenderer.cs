using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TurnOffTilemapRenderer : MonoBehaviour
{
    TilemapRenderer tilemapRenderer;


	// Use this for initialization
	void Start () {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapRenderer.enabled = false;
	}


}
