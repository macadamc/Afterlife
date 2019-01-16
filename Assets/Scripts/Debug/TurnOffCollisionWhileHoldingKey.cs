using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffCollisionWhileHoldingKey : MonoBehaviour {

    public KeyCode debugKey;
    public List<KeyCode> keys;
    List<bool> flags;
    Collider2D col;


	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();

        flags = new List<bool>();
        for (int i = 0; i < keys.Count; i++)
        {
            flags.Add(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            flags[i] = Input.GetKey(keys[i]);
        }

        col.enabled = !flags.Contains(true);
    }
}
