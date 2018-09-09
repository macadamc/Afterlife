using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class WindSway : MonoBehaviour {

    public float swayAmt = 0.25f;
    public float randomDelay = 0.5f;
    public float time = 1f;

	// Use this for initialization
	void Start () {
        Tween.LocalRotation(transform, new Vector3(0f, 0f, -swayAmt), new Vector3(0f, 0f, swayAmt), time, Random.Range(0, randomDelay),Tween.EaseInOut,Tween.LoopType.PingPong);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
