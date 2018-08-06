using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterXSeconds : MonoBehaviour {

    public float time = 0.5f;

    float _destroyTime;

    private void OnEnable()
    {
        _destroyTime = Time.time + time;
    }

    // Update is called once per frame
    void Update () {

        if (Time.time > _destroyTime)
            Destroy(gameObject);
		
	}
}
