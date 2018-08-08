using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InitializeCanvasScreenSpaceCamera : MonoBehaviour
{
    public string sortingLayer = "FG";
    public int orderInLayer;

	void Awake ()
    {
        Camera cam = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        canvas.sortingLayerID = SortingLayer.NameToID(sortingLayer);
        canvas.sortingOrder = orderInLayer;
		
	}

}
