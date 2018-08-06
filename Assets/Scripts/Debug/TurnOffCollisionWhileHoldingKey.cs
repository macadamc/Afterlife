using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffCollisionWhileHoldingKey : MonoBehaviour {

    public KeyCode debugKey;
    Collider2D col;


	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(debugKey))
        {
            if(col.enabled == true)
                col.enabled = false;
        }
        else
        {
            if (col.enabled == false)
                col.enabled = true;
        }
		
	}
}
