using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAreaName : MonoBehaviour {

    public string areaName;

	// Use this for initialization
	void Start () {
        AreaNameManager.Instance.SetAreaText(areaName);
	}
}
