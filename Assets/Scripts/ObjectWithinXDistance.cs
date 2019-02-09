using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectWithinXDistance : MonoBehaviour
{
    public GameObject ObjToTest;
    public float dist;
    public AlignGameobjectBetweenUserAndTarget align;
    public UnityEvent uEvent;

	// Update is called once per frame
	void Update ()
    {
        if (Vector2.Distance(ObjToTest.transform.position, transform.position) <= dist && align.IsAligned)
            uEvent.Invoke();
	}
}
