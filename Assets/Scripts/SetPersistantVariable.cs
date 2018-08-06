using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPersistantVariable : MonoBehaviour {

    public string key;
    public bool value;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValue()
    {
        Yarn.Value v = new Yarn.Value(value);


        SaveLoadManager.Instance.savedVariables.SetValue(key, v);
    }
}
